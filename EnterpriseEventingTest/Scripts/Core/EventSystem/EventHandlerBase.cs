using System;
using System.Collections.Generic;
using System.Reflection;
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
    /// Key: Event type, Value: List of handlers registered for that event type.
    /// </summary>
    private readonly Dictionary<Type, List<Delegate>> _registrations = new();

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
    public void SubscribeToEvents() {
        RegisterEvents();
        GD.Print($"{GetType().Name}: Subscribed to all events");
    }

    /// <summary>
    /// Unsubscribes this handler from all previously registered events.
    /// Uses reflection to properly remove event handlers from their respective buses.
    /// </summary>
    public void UnsubscribeFromEvents() {
        foreach (var (eventType, handlers) in _registrations) {
            // Get the event bus for this event type
            object? bus = GetEventBusForType(eventType);
            if (bus == null) continue;

            // Find the PublishedAsync event on the bus (once per event type)
            EventInfo? eventInfo = bus.GetType().GetEvent("PublishedAsync");
            if (eventInfo == null) continue;

            // Remove all handlers for this event type
            foreach (var handler in handlers) {
                eventInfo.RemoveEventHandler(bus, handler);
            }
        }

        // Clear registration tracking dictionary
        _registrations.Clear();
        GD.Print($"{GetType().Name}: Unsubscribed from all events");
    }

    /// <summary>
    /// Retrieves the appropriate event bus for a given event type using reflection.
    /// </summary>
    /// <param name="eventType">The type of event to get the bus for</param>
    /// <returns>The event bus instance or null if not found</returns>
    private object? GetEventBusForType(Type eventType) {
        MethodInfo? method = typeof(EventRegistry).GetMethod("GetAsyncEventBus",
                                                             BindingFlags.Public | BindingFlags.Instance);
        if (method == null) return null;

        MethodInfo genericMethod = method.MakeGenericMethod(eventType);
        object? bus = genericMethod.Invoke(EventRegistry, []);

        if (bus == null) {
            GD.Print($"Warning: Could not find event bus for event type {eventType.Name}");
        }

        return bus;
    }

    /// <summary>
    /// Registers an async event handler for the specified event type.
    /// This method should be called from within the RegisterEvents implementation.
    /// Also tracks registrations to enable proper cleanup during unsubscription.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to handle.</typeparam>
    /// <param name="handler">The async function that will handle the event.</param>
    protected void RegisterHandler<TEvent>(Func<TEvent, Task> handler) {
        Type eventType = typeof(TEvent);
        IAsyncEventBus<TEvent> bus = EventRegistry.GetAsyncEventBus<TEvent>();
        bus.PublishedAsync += handler;

        // Track the registration for later cleanup
        if (!_registrations.ContainsKey(eventType))
            _registrations[eventType] = new List<Delegate>();

        _registrations[eventType].Add(handler);
    }

}
