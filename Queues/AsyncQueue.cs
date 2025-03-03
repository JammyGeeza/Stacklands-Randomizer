using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Stacklands_Randomizer_Mod
{
    public static class AsyncQueue
    {
        private static object _lock = new object();
        private static Task _previousTask = Task.CompletedTask;

        private static int _count = 0;

        /// <summary>
        /// Add an asynchronous task to the queue and perform tasks in order of arrival.
        /// </summary>
        /// <param name="task">The asynchronous task to be added to the queue.</param>
        /// <remarks>
        /// Logic for this method credited to the author of this blog post: https://zerowidthjoiner.net/2020/10/10/how-to-ensure-tasks-are-executed-serially-fifo
        /// </remarks>
        public static async Task Enqueue(Func<Task> task)
        {
            Task previousTask;
            TaskCompletionSource<bool> complete = new TaskCompletionSource<bool>();

            // Lock to ensure order
            lock (_lock)
            {
                // Set the task that precedes this task, if any
                previousTask = _previousTask;
                _previousTask = complete.Task;

                // Increment queue count
                Interlocked.Increment(ref _count);
            }

            // Wait for previous job to finish
            await previousTask;

            try
            {
                // Perform task
                await task();
            }
            finally
            {
                // Set result and decrement from current queue count
                complete.SetResult(true);
                Interlocked.Decrement(ref _count);
            }
        }
    }
}
