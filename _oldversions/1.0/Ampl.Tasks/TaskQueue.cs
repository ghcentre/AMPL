using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GHC.Utilities;
using System.Threading;
using System.Diagnostics;

namespace Ampl.Tasks
{
  /// <summary>
  /// Task queue synchonized object.
  /// </summary>
  /// <typeparam name="T">Queue item type.</typeparam>
  public class TaskQueue<TQueueItem>
  {
    private static Syslog _log = new Syslog("AMPL.Tasks");

    /// <summary>
    /// Locker object.
    /// </summary>
    private object _locker = new object();

    #region Refill

    /// <summary>
    /// Event which gets signaled when number of tasks in the queue becomes lower
    /// than 'refill threshold'.
    /// </summary>
    private EventWaitHandle _eventRefillNeeded;

    /// <summary>
    /// Number of items in the queue which is considered 'low number of items'.
    /// </summary>
    /// <remarks>If number of items in the queue goes below this value, Refill event
    /// is signaled, and Refill thread awakes to add some items to the queue.</remarks>
    public int RefillThreshold
    {
      get { return _refillThreshold; }
      set { _refillThreshold = Math.Max(value, 10); }
    }
    private int _refillThreshold = 200;

    /// <summary>
    /// Checks if queue refill is needed.
    /// </summary>
    private void CheckRefillNeeded()
    {
      if(_queue.Count < _refillThreshold)
      {
        _eventRefillNeeded.Set();
      }
      else
      {
        _eventRefillNeeded.Reset();
      }
    }

    #endregion

    #region Queue and sync objects

    /// <summary>
    /// Event which gets signaled when the number of threads must be decremented
    /// by 1 or even decremented to zero.
    /// This is manual reset event, and only one thread wakes up.
    /// </summary>
    private EventWaitHandle _eventDecrementNumWorkerThreads;

    /// <summary>
    /// Semaphore holding number of tasks to process.
    /// </summary>
    private Semaphore _semaphoreNumTasks = new Semaphore(0, int.MaxValue);

    /// <summary>
    /// Task queue.
    /// </summary>
    private Queue<TQueueItem> _queue = new Queue<TQueueItem>();

    #endregion

    #region Queueing
   
    /// <summary>
    /// Adds a task to queue.
    /// </summary>
    /// <param name="item">Item to add.</param>
    public void Enqueue(TQueueItem item)
    {
      lock(_locker)
      {
        _queue.Enqueue(item);

        //
        // increment the semaphore count
        //
        _semaphoreNumTasks.Release();

        CheckRefillNeeded();        
      }
    }

    /// <summary>
    /// Adds a list of tasks to queue.
    /// </summary>
    /// <param name="packets">Packet collection.</param>
    /// <rereturns>Number of items enqueued.</rereturns>
    public int Enqueue(IEnumerable<TQueueItem> items)
    {
      if(items == null)
      {
        return 0;
      }
      lock(_locker)
      {
        int count = 0;
        foreach(TQueueItem item in items)
        {
          _queue.Enqueue(item);
          count++;
        }

        //
        // increment the semaphore count by the number of tasks added
        //
        if(count > 0)
        {
          _semaphoreNumTasks.Release(count);
        }

        CheckRefillNeeded();

        return count;
      }
    }

    #endregion

    #region Dequeueing
    
    /// <summary>
    /// Gets and removes the task from queue.
    /// </summary>
    /// <returns>Task dequeued.</returns>
    /// <exception cref="System.InvalidOperationException">The number of worker
    /// threads must be decremented, or the application is terminating.</exception>
    /// <remarks>
    /// <para>The method blocks the calling thread until data is arrived in queue
    /// or terminate event gets signaled.</para>
    /// <para>When the terminate event is signaled, the method throws the
    /// <c>InvalidOperationException</c>. In such case calling thread must stop execution.</para>
    /// </remarks>
    public TQueueItem Dequeue()
    {
      switch(WaitHandle.WaitAny(
        new WaitHandle[] { _eventDecrementNumWorkerThreads, _semaphoreNumTasks }))
      {
        case 0:
          //
          // decrement num worker threads event signaled.
          // dequeue from an empty queue to throw 'empty queue' exception
          //
          return new Queue<TQueueItem>().Dequeue();

        case 1:
          //
          // there is at least one task in the queue. get it
          //
          lock(_locker)
          {
            try
            {
              TQueueItem result = _queue.Dequeue();
              CheckRefillNeeded();
              return result;
            }
            catch(Exception unexpected)
            {
              _log.Error("Task queue emptied unexpectedly. Exception: {0}", unexpected);
              throw; // let the worker thread die
            }
          }

        default:
          Debug.Assert(false);
          return default(TQueueItem);
      }
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="eventDecrementNumWorkerThreads">
    /// Decrement number of worker threads event handle.
    /// </param>
    /// <param name="eventRefillNeeded">Refill needed event handle.</param>
    public TaskQueue(EventWaitHandle eventDecrementNumWorkerThreads,
      EventWaitHandle eventRefillNeeded)
    {
      ArgValidator.CheckNull(eventDecrementNumWorkerThreads, "eventDecrementNumWorkerThreads");
      _eventDecrementNumWorkerThreads = eventDecrementNumWorkerThreads;

      ArgValidator.CheckNull(eventRefillNeeded, "eventRefillNeeded");
      _eventRefillNeeded = eventRefillNeeded;
    }
  }
}
