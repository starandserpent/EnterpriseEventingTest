using System;

namespace EnterpriseEventingTest.Core.EventSystem.Interfaces;

/// <summary>
/// Generic event bus interface that allows subscribing to and publishing events of a specific type.
/// </summary>
/// <typeparam name="T">The type of event to be published through this bus</typeparam>
internal interface IEventBus<T> {
    /// <summary>
    /// Event that is raised when an event is published.
    /// </summary>
    event Action<T>? Published;

    /// <summary>
    /// Publishes an event to all subscribers.
    /// </summary>
    /// <param name="eventData">The event data to publish</param>
    void Publish(T eventData);
}