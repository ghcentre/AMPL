using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ampl.Core
{
    /// <summary>
    /// Provides a set of <see langword="static"/> methods to extend the <see cref="Task"/>
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Runs the <see cref="Task"/> synchronously.
        /// </summary>
        /// <typeparam name="T">The type of the return value of the task.</typeparam>
        /// <param name="task">The task object.</param>
        /// <returns>The method runs the task and waits for it to complete.</returns>
        /// <remarks>The method calls the <see cref="Task.ConfigureAwait(bool)"/> with the parameter
        /// set to <see langword="false"/>.</remarks>
        /// <seealso cref="Sync{T}(Task{T}, bool?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sync<T>(this Task<T> task)
        {
            Check.NotNull(task, nameof(task));
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Runs the <see cref="Task"/> synchronously.
        /// </summary>
        /// <typeparam name="T">The type of the return value of the task.</typeparam>
        /// <param name="task">The task object.</param>
        /// <param name="configureAwait">The nullable <see cref="Boolean"/> value passed to a call to
        /// <see cref="Task.ConfigureAwait(bool)"/> method. If this parameter is <see langword="null"/>,
        /// the <see cref="Task.ConfigureAwait(bool)"/> is not called.</param>
        /// <returns>The method runs the task and waits for it to complete.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sync<T>(this Task<T> task, bool? configureAwait)
        {
            Check.NotNull(task, nameof(task));
            if(configureAwait.HasValue)
            {
                return task.ConfigureAwait(configureAwait.Value).GetAwaiter().GetResult();
            }
            return task.GetAwaiter().GetResult();
        }
    }
}
