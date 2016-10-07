using System;
using System.Collections.Generic;
using System.Threading;

namespace Core.Threads
{
    /// <summary>
    /// A object to main Thread Pool in which object execution can be aborted
    /// </summary>
    public static class AbortableThreadPool
    {        
        #region Fields
        private static readonly LinkedList<WorkItem> CallbacksList = new LinkedList<WorkItem>();
        private static readonly Dictionary<WorkItem, System.Threading.Thread> ThreadList = new Dictionary<WorkItem, System.Threading.Thread>();
        #endregion

        #region Methods
        /// <summary>
        /// Queues a method for execution. The method executes when a thread pool thread becomes available. 
        /// </summary>
        /// <param name="callback">A WaitCallback representing the method to be executed. </param>
        /// <returns>created workItem in Queue</returns>
        public static WorkItem QueueUserWorkItem(WaitCallback callback)
        {
            return QueueUserWorkItem(callback, null);
        }

        /// <summary>
        /// Queues a method for execution, and specifies an object containing data to be
        /// used by the method. The method executes when a thread pool thread becomes available.
        /// </summary>
        /// <param name="callback">A WaitCallback representing the method to be executed.</param>
        /// <param name="state">An object containing data to be used by the method.</param>
        /// <returns>created workItem in Queue</returns>
        public static WorkItem QueueUserWorkItem(WaitCallback callback, object state)
        {
            WorkItem item = new WorkItem(callback, state, ExecutionContext.Capture());

            lock (CallbacksList)
            {
                CallbacksList.AddLast(item);
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(HandleItem));
            return item;
        }

        /// <summary>
        /// Handles the queue workitem in thread pool.
        /// </summary>
        /// <param name="ignored">The ignored.</param>
        private static void HandleItem(object ignored)
        {
            WorkItem item = null;

            try
            {
                lock (CallbacksList)
                {
                    if (CallbacksList.Count > 0)
                    {
                        item = CallbacksList.First.Value;
                        CallbacksList.RemoveFirst();
                    }

                    if (item == null)
                    {
                        return;
                    }
                    ThreadList.Add(item, System.Threading.Thread.CurrentThread);

                }
                ExecutionContext.Run(item.Context, delegate { item.Callback(item.State); }, null);
            }
            finally
            {
                lock (CallbacksList)
                {
                    if (item != null)
                    {
                        ThreadList.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// Cancels the specified Queue workitem.
        /// </summary>
        /// <param name="item">The item to cancel in thread pool.</param>
        /// <param name="allowAbort">if set to <see langword="true"/> [allow abort].</param>
        /// <returns>Status of item queue</returns>
        public static WorkItemStatus Cancel(WorkItem item, bool allowAbort)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            lock (CallbacksList)
            {
                LinkedListNode<WorkItem> node = CallbacksList.Find(item);

                if (node != null)
                {
                    CallbacksList.Remove(node);
                    return WorkItemStatus.Queued;
                }
                else if (ThreadList.ContainsKey(item))
                {
                    if (allowAbort)
                    {
                        ThreadList[item].Abort();
                        ThreadList.Remove(item);
                        return WorkItemStatus.Aborted;
                    }
                    else
                    {
                        return WorkItemStatus.Executing;
                    }
                }
                else
                {
                    return WorkItemStatus.Completed;
                }
            }
        }

        /// <summary>
        /// Get the status the specified Queue workitem.
        /// </summary>
        /// <param name="item">The item to get status in thread pool.</param>
        /// <returns>Status of item queue</returns>
        public static WorkItemStatus GetStatus(WorkItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            lock (CallbacksList)
            {
                LinkedListNode<WorkItem> node = CallbacksList.Find(item);

                if (node != null)
                {
                    return WorkItemStatus.Queued;
                }
                else if (ThreadList.ContainsKey(item))
                {
                    return WorkItemStatus.Executing;
                }
                else
                {
                    return WorkItemStatus.Completed;
                }
            }
        }
        #endregion
    }
}
