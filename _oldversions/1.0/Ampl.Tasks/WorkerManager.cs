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
  /// Manages task queue, and worker thread pool.
  /// </summary>
  public class WorkerManager<TQueueItem>
  {
    private static Syslog _log = new Syslog("AMPL.Tasks");

    /// <summary>
    /// Terminate event handle.
    /// </summary>
    public EventWaitHandle TerminateEvent
    {
      get { return _eventTerminate; }
    }
    private EventWaitHandle _eventTerminate;

    /// <summary>
    /// Task queue.
    /// </summary>
    private TaskQueue<TQueueItem> _queue;

    /// <summary>
    /// Manager Thread object.
    /// </summary>
    private Thread _managerThread;

    #region Worker Threads

    /// Event which gets signaled when number of worker threads must be decremented.
    /// </summary>
    /// <summary>
    private AutoResetEvent _eventDecrementNumWorkerThreads = new AutoResetEvent(false);

    /// <summary>
    /// Event which gets signaled when at least one thread has finished execution.
    /// Used when decrementing number of threads.
    /// </summary>
    private ManualResetEvent _eventWorkerThreadFinished = new ManualResetEvent(false);

    /// <summary>
    /// List of worker threads.
    /// </summary>
    private List<Thread> _listWorkerThreads = new List<Thread>();

    /// <summary>
    /// Worker thread method.
    /// </summary>
    private void WorkerThreadMethod()
    {
      _log.Debug("Entering Worker thread.");

      while(true)
      {
        try
        {
          //
          // wait for task to become available and dequeue it.
          // an exception is thrown when it's time to terminate
          //
          TQueueItem item;
          try
          {
            item = _queue.Dequeue();
          }
          catch(InvalidOperationException)
          {
            _log.Debug("Nothing to do. Exiting Worker thread.");
            _eventWorkerThreadFinished.Set();
            return;
          }

          _log.Debug("Processing '{0}'.", item);

          //
          // change task status for the task item to 'in Progress'
          //
          _changeTaskStatusDeledate(item, TaskStatus.Progress);

          //
          // process the item
          //
          try
          {
            TaskStatus processedStatus = _processItemDelegate(item);

            //
            // change task status for the task to a new status
            //
            _changeTaskStatusDeledate(item, processedStatus);
          }
          catch(ThreadAbortException)
          {
            throw;
          }
          catch(Exception exception)
          {
            _log.Warn("Exception processing item '{0}': {1}.", item, exception);

            //
            // change task status for the task to 'Error'
            //
            _changeTaskStatusDeledate(item, TaskStatus.Error);
          }
        }
        catch(ThreadAbortException)
        {
          _log.Debug("Worker thread aborted.");
          Thread.ResetAbort();
          return;
        }
        catch(Exception exception)
        {
          _log.Warn("Non-fatal exception in WorkerManager.WorkerThreadMethod(): {0}", exception);
          Thread.Sleep(1000);
        }
      }
    }

    #endregion

    #region Change number of threads

    /// <summary>
    /// Removes dead worker threads from thread list.
    /// </summary>
    private void CleanDeadThreads()
    {
      var listNew = new List<Thread>();
      foreach(var workerThread in _listWorkerThreads)
      {
        if(workerThread.IsAlive)
        {
          listNew.Add(workerThread);
        }
      }
      if(_listWorkerThreads.Count != listNew.Count)
      {
        _log.Debug("Cleaned dead Worker thread(s) from {0} to {1}.",
          _listWorkerThreads.Count, listNew.Count);
      }
      _listWorkerThreads.Clear();
      _listWorkerThreads = null;
      _listWorkerThreads = listNew;
    }

    /// <summary>
    /// Changes number of worker threads.
    /// </summary>
    /// <param name="neededNum">Needed number of threads.</param>
    private void ChangeNumberOfThreads(int neededNum)
    {
      CleanDeadThreads();
      if(_listWorkerThreads.Count == neededNum)
      {
        return;
      }

      _log.Info("Changing number of worker threads from {0} to {1}.",
        _listWorkerThreads.Count, neededNum);

      if(_listWorkerThreads.Count < neededNum)
      {
        //
        // add necessary number of threads
        //
        while(_listWorkerThreads.Count < neededNum)
        {
          var workerThread = new Thread(WorkerThreadMethod)
          {
            IsBackground = false,
            Priority = ThreadPriority.Lowest,
          };
          workerThread.Start();
          _listWorkerThreads.Add(workerThread);
        }
      }
      else
      {
        while(_listWorkerThreads.Count > neededNum)
        {
          //
          // reset 'thread done' event and set 'decrement worker threads' event.
          // when one of the worker threads wakes up from decrement event
          // (one thread only because decrement event is AutoResetEvent)
          // it terminates and sets 'thread done' event.
          //
          _eventWorkerThreadFinished.Reset();
          _eventDecrementNumWorkerThreads.Set();
          int waitResult = WaitHandle.WaitAny(new WaitHandle[]
            {
              _eventTerminate,
              _eventWorkerThreadFinished
            },
            10000);
          if(waitResult == 0)
          {
            return;
          }

          //
          // find dead thread
          //
          CleanDeadThreads();
        }
      }
    }

    #endregion

    #region Number of Threads public interface

    /// <summary>
    /// Gets the actual number of worker threads.
    /// </summary>
    /// <value>Actual number of worker threads.</value>
    public int WorkerThreadCount
    {
      get { return _listWorkerThreads.Count; }
    }

    /// <summary>
    /// Event which gets signaled when number of worker threads needs to change.
    /// </summary>
    private AutoResetEvent _eventNumThreadsNeededChanged = new AutoResetEvent(false);

    /// <summary>
    /// Number of worker threads needed.
    /// </summary>
    private int _numWorkerThreadsNeeded = 0;

    /// <summary>
    /// Sets number of worker threads.
    /// </summary>
    /// <param name="numThreads">Desired number of threads. Mininum number is zero.</param>
    /// <remarks>The method returns immediately. Actual number of worker threads
    /// changes asynchrously. To obtain the actual number of worker threads, see
    /// <see cref="WorkerThreadCount"/> property.</remarks>
    public void SetNumWorkerThreads(int numThreads)
    {
      numThreads = Math.Max(0, numThreads);
      if(numThreads != _numWorkerThreadsNeeded)
      {
        _numWorkerThreadsNeeded = numThreads;
        _eventNumThreadsNeededChanged.Set();
      }
    }

    #endregion

    #region Worker Delegates

    /// <summary>
    /// Delegate which is called when task status must be changed.
    /// </summary>
    public Action<TQueueItem, TaskStatus> ChangeTaskStatusDelegate
    {
      get { return _changeTaskStatusDeledate; }
      set { _changeTaskStatusDeledate = value; }
    }
    private Action<TQueueItem, TaskStatus> _changeTaskStatusDeledate = ((i, s) => { return; });

    /// <summary>
    /// Delegate which is called to process an item.
    /// </summary>
    public Func<TQueueItem, TaskStatus> ProcessItemDelegate
    {
      get { return _processItemDelegate; }
      set { _processItemDelegate = value; }
    }
    private Func<TQueueItem, TaskStatus> _processItemDelegate = ((i) => TaskStatus.Error);

    #endregion

    #region Refill

    /// <summary>
    /// Event which gets signaled when number of tasks in the queue becomes lower than
    /// 'refill threshold'.
    /// </summary>
    private ManualResetEvent _eventQueueRefillNeeded = new ManualResetEvent(true);

    /// <summary>
    /// Gets or sets Queue refill threshold.
    /// </summary>
    /// <value>Queue refill threshold.</value>
    public int QueueRefillThreshold
    {
      get { return _queue.RefillThreshold; }
      set { _queue.RefillThreshold = value; }
    }

    /// <summary>
    /// Gets or set time interval (in seconds) refill thread will wait after
    /// unsuccessful (zero-item) queue refill.
    /// </summary>
    public int UnsuccessfulRefillTimeout
    {
      get { return _emptyQueueRefillTimeout; }
      set { _emptyQueueRefillTimeout = value; }
    }
    private int _emptyQueueRefillTimeout = 10;

    /// <summary>
    /// Delegate used to refill the queue.
    /// </summary>
    /// <remarks>Default delegate returns an empty array.</remarks>
    public Func<IEnumerable<TQueueItem>> RefillDelegate
    {
      get { return _refillDelegate; }
      set { _refillDelegate = value; }
    }
    private Func<IEnumerable<TQueueItem>> _refillDelegate = (() => new TQueueItem[] { });

    /// <summary>
    /// Delegate used to initialize the queue.
    /// </summary>
    public Action InitQueueDelegate
    {
      get { return _initQueueDelegate; }
      set { _initQueueDelegate = value; }
    }
    private Action _initQueueDelegate = (() => { return; });

    /// <summary>
    /// Refill thread.
    /// </summary>
    private Thread _refillThread;

    /// <summary>
    /// Refill thread method.
    /// </summary>
    private void RefillThreadMethod()
    {
      _log.Debug("Entering Refill thread.");

      try
      {
        _log.Debug("Initializing queue.");
        _initQueueDelegate();
      }
      catch(Exception exception)
      {
        _log.Warn("Exception initializing queue: {0}", exception);
      }

      while(true)
      {
        try
        {
          switch(WaitHandle.WaitAny(
            new[] { _eventTerminate, _eventQueueRefillNeeded }))
          {
            case 0:
              _log.Debug("Terminate event signaled. Exiting Refill thread.");
              return;

            case 1:
              _log.Debug("Refill event signaled. Refilling the queue.");

              List<TQueueItem> items = _refillDelegate().ToList();
              items.ForEach(i => _changeTaskStatusDeledate(i, TaskStatus.Queued));
              int numQueued = _queue.Enqueue(items);
              _log.Debug("Refilled {0} item(s).", numQueued);

              if(numQueued == 0)
              {
                _log.Debug("Waiting for items to refill.");
                _eventTerminate.WaitOne(_emptyQueueRefillTimeout * 1000);
              }
              break;
          }
        }
        catch(Exception exception)
        {
          _log.Warn("Non-fatal error refilling: {0}", exception);
          Thread.Sleep(1000);
        }
      }
    }

    #endregion

    #region Start and wait for threads public interface

    /// <summary>
    /// Starts all threads.
    /// </summary>
    public void StartThreads()
    {
      if(!_refillThread.IsAlive)
      {
        _refillThread.Start();
      }

      if(!_configReaderThread.IsAlive)
      {
        _configReaderThread.Start();
      }

      if(!_managerThread.IsAlive)
      {
        _managerThread.Start();
      }
    }

    /// <summary>
    /// Waits for all threads to finish.
    /// </summary>
    public void WaitForThreadsToFinish()
    {
      _managerThread.Join();
    }

    #endregion

    #region Config Reader (Delegates and Thread Method)

    /// <summary>
    /// Delegate which is called periodically to re-read config values.
    /// </summary>
    public Action<WorkerManager<TQueueItem>> ReadConfigDelegate
    {
      get { return _readConfigDelegate; }
      set { _readConfigDelegate = value; }
    }
    private Action<WorkerManager<TQueueItem>> _readConfigDelegate = ((m) => { return; });

    /// <summary>
    /// Timeout, in seconds, between two config reads.
    /// </summary>
    public int ReadConfigTimeout
    {
      get { return _readConfigTimeout; }
      set { _readConfigTimeout = Math.Max(1, value); }
    }
    private int _readConfigTimeout = 1;

    private Thread _configReaderThread;

    private void ConfigReaderThreadMethod()
    {
      _log.Debug("Entering Config Reader thread.");

      while(true)
      {
        if(_eventTerminate.WaitOne(_readConfigTimeout * 1000))
        {
          _log.Debug("Terminate event signaled. Exiting Config Reader thread.");
          return;
        }
          
        try
        {
          _readConfigDelegate(this);
        }
        catch(Exception exception)
        {
          _log.Warn("Exception reading config: {0}", exception);
        }
      }
    }

    #endregion

    #region ManagerThreadMethod Enter and Exit Delegates

    /// <summary>
    /// Delegate which is called when Worker Manager Thread is started.
    /// </summary>
    public Action<WorkerManager<TQueueItem>> ManagerThreadStartedDelegate
    {
      get { return _managerThreadStartedDelegate; }
      set { _managerThreadStartedDelegate = value; }
    }
    private Action<WorkerManager<TQueueItem>> _managerThreadStartedDelegate = ((m) => { return; });

    /// <summary>
    /// Delegate which is called when Worker Manager Thread is about to finish.
    /// </summary>
    public Action<WorkerManager<TQueueItem>> ManagerThreadExitingDelegate
    {
      get { return _managerThreadExitingDelegate; }
      set { _managerThreadExitingDelegate = value; }
    }
    private Action<WorkerManager<TQueueItem>> _managerThreadExitingDelegate = ((m) => { return; });

    #endregion

    /// <summary>
    /// Manager Thread method.
    /// </summary>
    private void ManagerThreadMethod()
    {
      _log.Debug("Entering worker manager thread.");
      _managerThreadStartedDelegate(this);

      while(true)
      {
        try
        {
          switch(WaitHandle.WaitAny(
            new[] { _eventTerminate, _eventNumThreadsNeededChanged }))
          {
            case 0:
              _log.Debug("Terminate event signaled.");

              _log.Debug("Aborting all Worker threads.");
              foreach(var workerThread in _listWorkerThreads)
              {
                workerThread.Abort();
              }
              _log.Debug("Waiting for all Worker threads to finish.");
              foreach(var workerThread in _listWorkerThreads)
              {
                workerThread.Join();
              }

              _log.Debug("Waiting for Refill thread to finish.");
              _refillThread.Join();

              _log.Debug("Waiting for Config Reader thread to finish.");
              _configReaderThread.Join();

              _managerThreadExitingDelegate(this);
              _log.Debug("Exiting Worker manager thread.");
              return;

            case 1:
              _log.Debug("Change number of threads event signaled.");
              ChangeNumberOfThreads(_numWorkerThreadsNeeded);
              break;
          }
        }
        catch(Exception exception)
        {
          _log.Warn("Non-fatal exception in WorkerManagerThread.ThreadMethod(): {0}", exception);
          Thread.Sleep(1000);
        }
      }
    }

    #region Constructor
    
    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="terminateEvent">Terminate event handle. If this parameter is <c>null</c>,
    /// the internal <c>ManualResetEvent</c> is used.</param>
    public WorkerManager(EventWaitHandle terminateEvent = null)
    {
      _eventTerminate = terminateEvent;
      if(_eventTerminate == null)
      {
        _eventTerminate = new ManualResetEvent(false);
      }

      //
      // create task queue
      //
      _queue = new TaskQueue<TQueueItem>(_eventDecrementNumWorkerThreads, _eventQueueRefillNeeded);

      //
      // create refill thread
      //
      _refillThread = new Thread(RefillThreadMethod);
      _refillThread.IsBackground = false;

      //
      // create config reader thread
      //
      _configReaderThread = new Thread(ConfigReaderThreadMethod);
      _configReaderThread.IsBackground = false;

      //
      // create worker manager thread
      //
      _managerThread = new Thread(ManagerThreadMethod);
      _managerThread.IsBackground = false;
      //_thread.Start();
    }

    #endregion
  }
}
