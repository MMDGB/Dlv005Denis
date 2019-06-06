using System;

namespace Dlv005.UI
{
    internal interface IObservable
    {
        /// <summary>
        /// Sets the observer for event.
        /// </summary>
        /// <returns></returns>
        EventHandler SetObserverForEvent();
    }
}