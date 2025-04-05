using System;
using EnterpriseEventingTest.Core.EventSystem.Interfaces;

namespace EnterpriseEventingTest.Core.EventSystem;

/// <summary>
/// Generic implementation of an event bus that supports synchronous event publishing.
/// </summary>
/// <typeparam name="T">The type of event to be published through this bus</typeparam>
internal sealed class EventBus<T> : IEventBus<T> {
    /// <summary>
    /// Event that is raised when an event is published.
    /// </summary>
    public event Action<T>? Published;

    /// <summary>
    /// Publishes an event to all subscribers.
    /// </summary>
    /// <param name="eventData">The event data to publish</param>
    public void Publish(T eventData) {

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Synchronous EventBus: Publishing (broadcasting) {typeof(T).Name} event");
        Console.ResetColor();

        Published?.Invoke(eventData);
    }

}
