using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseEventingTest.Core.EventSystem.Interfaces;
using Godot;

namespace EnterpriseEventingTest.Core.EventSystem;

/// <summary>
/// Abstract base class for event handlers that provides common event subscription management functionality.
/// Derived classes must implement RegisterEvents() to define their specific event subscriptions.
/// This class maintains a registry of all event handlers to facilitate clean unsubscription.
/// Event handlers automatically subscribe to events during construction - no need to manually call SubscribeToEvents().
/// </summary>
internal abstract class EventHandlerBase {
    /// <summary>
    /// Central registry that manages event buses and subscriptions.
    /// </summary>
    protected readonly EventRegistry EventRegistry;

    /// <summary>
    /// Tracks all event registrations made by this handler for proper cleanup.
    /// </summary>
    private readonly List<IDisposable> _registrations = new();

    /// <summary>
    /// Initializes a new instance of the event handler with the specified event registry.
    /// Automatically subscribes to events defined in RegisterEvents during construction.
    /// </summary>
    /// <param name="eventRegistry">The event registry used to access event buses.</param>
    protected EventHandlerBase(EventRegistry eventRegistry) {
        EventRegistry = eventRegistry;

        try {
            // Auto-register events during construction
            SubscribeToEvents();
        }
        catch (Exception ex) {
            GD.Print($"{GetType().Name}: Failed to subscribe to events during construction: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Defines which events this handler should subscribe to.
    /// Derived classes must implement this method to register their event handlers
    /// using the RegisterHandler method.
    /// </summary>
    protected abstract void RegisterEvents();

    /// <summary>
    /// Subscribes this handler to all events defined in the RegisterEvents implementation.
    /// This is automatically called during handler construction.
    /// </summary>
    public void SubscribeToEvents() => RegisterEvents();

    /// <summary>
    /// Unsubscribes this handler from all previously registered events.
    /// </summary>
    public void UnsubscribeFromEvents() {
        foreach (var registration in _registrations) {
            // Use properties via interface
            if (registration is IEventRegistrationInfo info) {
                Console.ForegroundColor = ConsoleColor.Red;
                GD.Print($"[{GetType().Name}]: Unsubscribing from {info.EventTypeName} event handled by {info.HandlerMethodName}");
                Console.ResetColor();
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
                GD.Print($"[{GetType().Name}]: Unsubscribing from event handler");
                Console.ResetColor();
            }
            registration.Dispose();
        }
        _registrations.Clear();
    }

    /// <summary>
    /// Registers an async event handler for the specified event type.
    /// This method should be called from within the RegisterEvents implementation.
    /// Also tracks registrations to enable proper cleanup during unsubscription.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to handle.</typeparam>
    /// <param name="handler">The async function that will handle the event.</param>
    protected void RegisterHandler<TEvent>(Func<TEvent, Task> handler) {
        IAsyncEventBus<TEvent> bus = EventRegistry.GetAsyncEventBus<TEvent>();
        bus.PublishedAsync += handler;

        // Create and store the registration object
        var registration = new EventRegistration<TEvent>(bus, handler);
        _registrations.Add(registration);

        Console.ForegroundColor = ConsoleColor.Green;
        GD.Print($"[{GetType().Name}]: Registered handler for {typeof(TEvent).Name} event -> {handler.Method.Name}().");
        Console.ResetColor();
    }

}
