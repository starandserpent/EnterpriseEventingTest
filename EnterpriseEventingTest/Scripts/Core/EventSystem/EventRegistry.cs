using System;
using System.Collections.Generic;
using EnterpriseEventingTest.Core.EventSystem.Interfaces;

namespace EnterpriseEventingTest.Core.EventSystem;

/// <summary>
/// Central registry for all event buses in the application.
/// Provides a single point to register and retrieve event buses.
/// </summary>
internal class EventRegistry {

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
        return GetOrCreateEventBus<IAsyncEventBus<T>, AsyncEventBus<T>>();
    }

    /// <summary>
    /// Gets or creates a synchronous event bus for the specified event type. What this means is
    /// the provided event bus can be used to publish events for the specified type such as
    /// PlayerAddedEvent or PlayerUpdatedEvent, and all subscribers to that event bus will be
    /// notified when an event is published synchronously. Order of execution is guaranteed.
    /// </summary>
    public IEventBus<T> GetEventBus<T>() {
        return GetOrCreateEventBus<IEventBus<T>, EventBus<T>>();
    }

    /// <summary>
    /// Gets an existing or creates a new event bus of the specified interface and implementation type.
    /// </summary>
    private TInterface GetOrCreateEventBus<TInterface, TImplementation>()
        where TImplementation : class, TInterface, new() {

        Type eventType = typeof(TInterface);

        // Check if an event bus of the specified type already exists in the dictionary
        if (_eventBuses.TryGetValue(eventType, out var bus)) {
            return (TInterface)bus;
        }

        // If it doesn't exist, create a new event bus of the specified type
        var newBus = new TImplementation();
        _eventBuses[eventType] = newBus;
        return newBus;
    }
}
