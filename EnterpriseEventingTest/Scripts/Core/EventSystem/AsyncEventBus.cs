using System;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseEventingTest.Core.EventSystem.Interfaces;

namespace EnterpriseEventingTest.Core.EventSystem;

/// <summary>
/// Generic implementation of an event bus that supports asynchronous event publishing.
/// </summary>
/// <typeparam name="T">The type of event to be published through this bus</typeparam>
internal sealed class AsyncEventBus<T> : IAsyncEventBus<T> {
    /// <summary>
    /// Event that is raised when an event is published, allowing async handlers.
    /// </summary>
    public event Func<T, Task>? PublishedAsync;

    /// <summary>
    /// Publishes an event to all subscribers and waits for all handlers to complete.
    /// </summary>
    /// <param name="eventData">The event data to publish</param>
    public async Task PublishAsync(T eventData) {

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Asyncronous EventBus: Publishing (broadcasting) {typeof(T).Name} event");
        Console.ResetColor();

        if (PublishedAsync == null)
            return;

        // Get all subscribers and invoke them in parallel
        Task[] handlers = PublishedAsync.GetInvocationList()
                                        .Cast<Func<T, Task>>()
                                        .Select(handler => handler(eventData))
                                        .ToArray();

        // Wait for all handlers to complete
        await Task.WhenAll(handlers);
    }
}