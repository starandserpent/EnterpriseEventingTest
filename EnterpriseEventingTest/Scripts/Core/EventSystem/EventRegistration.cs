using System;
using System.Threading.Tasks;
using EnterpriseEventingTest.Core.EventSystem.Interfaces;

namespace EnterpriseEventingTest.Core.EventSystem;

/// <summary>
/// Class to track event registrations and unsubscriptions for a specific event type.
/// </summary>
/// <typeparam name="TEvent">
/// The type of event that this registration is for. E.g. PlayerAddedEvent, PlayerUpdatedEvent, etc.
/// </typeparam>
internal sealed class EventRegistration<TEvent> : IDisposable, IEventRegistrationInfo {
    private readonly IAsyncEventBus<TEvent> _bus;
    private readonly Func<TEvent, Task> _handler;

    // Expose these properties
    public Type EventType => typeof(TEvent);
    public string EventTypeName => typeof(TEvent).Name;
    public string HandlerMethodName => _handler.Method.Name;

    public EventRegistration(IAsyncEventBus<TEvent> bus, Func<TEvent, Task> handler) {
        _bus = bus;
        _handler = handler;
    }

    public void Dispose() {
        _bus.PublishedAsync -= _handler;
    }

}