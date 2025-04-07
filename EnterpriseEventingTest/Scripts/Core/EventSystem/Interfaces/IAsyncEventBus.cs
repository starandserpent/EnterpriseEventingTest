using System;
using System.Threading.Tasks;

namespace EnterpriseEventingTest.Core.EventSystem.Interfaces;

/// <summary>
/// Generic event bus interface for events requiring asynchronous handlers.
/// </summary>
/// <typeparam name="T">The type of event to be published through this bus</typeparam>
internal interface IAsyncEventBus<T> {
    /// <summary>
    /// Event that is raised when an event is published, allowing async handlers.
    /// </summary>
    event Func<T, Task>? PublishedAsync;

    /// <summary>
    /// Publishes an event to all subscribers and waits for all handlers to complete.
    /// </summary>
    /// <param name="eventData">The event data to publish</param>
    Task PublishAsync(T eventData);

    /// <summary>
    /// Checks if the event has any subscribers.
    /// </summary>
    /// <returns></returns>
    bool HasSubscribers();
}