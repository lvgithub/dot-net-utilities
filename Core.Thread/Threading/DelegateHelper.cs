using System;
using System.Threading;

namespace Core.Threads
{
    /// <summary>
    /// Help functions for Delgates.
    /// </summary>
    public static  class DelegateHelper
    {
        private static WaitCallback dynamicInvoker = new WaitCallback(DynamicInvoke);

        /// <summary>
        /// Executes the delegate.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="args">The args.</param>
        public static WorkItem InvokeDelegate(Delegate target, params object[] args)
        {
            return AbortableThreadPool.QueueUserWorkItem(dynamicInvoker, new TargetInfo(target, args));
        }

        /// <summary>
        /// Executes the delegate.
        /// </summary>
        /// <param name="target">The target.</param>
        public static WorkItem InvokeDelegate(Delegate target)
        {
            return AbortableThreadPool.QueueUserWorkItem(dynamicInvoker, new TargetInfo(target, null));
        }

        /// <summary>
        /// Aborts the specified Queue delegate..
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>Status of abort operation on item queue</returns>
        public static WorkItemStatus AbortDelegate(WorkItem target)
        {
            return AbortableThreadPool.Cancel(target, true);
        }

        /// <summary>
        /// Dynamically invoke the delegate.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private static void DynamicInvoke(object obj)
        {
            TargetInfo ti = (TargetInfo)obj;
            ti.Target.DynamicInvoke((object[])ti.Arguments);
        }     
    }
}
