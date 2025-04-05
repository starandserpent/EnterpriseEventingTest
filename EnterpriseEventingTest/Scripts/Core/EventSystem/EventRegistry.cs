using System;
using System.Collections.Generic;
using EnterpriseEventingTest.Core.EventSystem.Interfaces;

namespace EnterpriseEventingTest.Core.EventSystem;

/// <summary>
/// Central registry for all event buses in the application.
/// Provides a single point to register and retrieve event buses.
/// </summary>
internal sealed class EventRegistry {

    /// <summary>
    /// Dictionary to hold event buses for different event types, e.g. PlayerAddedEvent, PlayerUpdatedEvent, etc.
    /// </summary>
    private readonly Dictionary<Type, object> _eventBuses = new();

    /// <summary>
    /// Gets or creates an async event bus for the specified event type. What this means is
    /// the provided event bus can be used to publish events for the specified type such as
    /// PlayerAddedEvent or PlayerUpdatedEvent, and all subscribers to that event bus will be
    /// notified when an event is published asynchronously. Order of execution is not guaranteed.
    /// </summary>
    public IAsyncEventBus<T> GetAsyncEventBus<T>() {
        // This code is implementing a registry pattern with lazy initialization for event buses.

        // Creates a runtime Type object representing the generic type parameter.
        Type eventType = typeof(T);

        // Check if an event bus of the specified type already exists in the dictionary.
        if (_eventBuses.TryGetValue(eventType, out var bus)) {
            return (IAsyncEventBus<T>) bus;
        }

        // If it doesn't exist, create a new event bus of the specified type.
        bus = new AsyncEventBus<T>();
        _eventBuses[eventType] = bus;
        return (IAsyncEventBus<T>) bus;
    }

    /// <summary>
    /// Gets or creates a synchronous event bus for the specified event type. What this means is
    /// the provided event bus can be used to publish events for the specified type such as
    /// PlayerAddedEvent or PlayerUpdatedEvent, and all subscribers to that event bus will be
    /// notified when an event is published synchronously. Order of execution is guaranteed.
    /// </summary>
    public IEventBus<T> GetEventBus<T>() {
        // This code is implementing a registry pattern with lazy initialization for event buses.

        // Creates a runtime Type object representing the generic type parameter.
        Type eventType = typeof(T);

        // Check if an event bus of the specified type already exists in the dictionary.
        if (_eventBuses.TryGetValue(eventType, out var bus)) {
            return (IEventBus<T>) bus;
        }

        // If it doesn't exist, create a new event bus of the specified type.
        bus = new EventBus<T>();
        _eventBuses[eventType] = bus;
        return (IEventBus<T>) bus;
    }

}