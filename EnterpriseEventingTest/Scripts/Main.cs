using Godot;
using System;
using EnterpriseEventingTest.Core.EventSystem;
using EnterpriseEventingTest.Domain.Player.Services;

namespace EnterpriseEventingTest;

/// <summary>
/// The entry point of the application that sets up the event system and services.
/// This demonstrates how to wire up different services using an event registry.
/// </summary>
internal partial class Main : Node {
    /// <summary>
    /// The event registry that manages the events in the system.
    /// </summary>
    private EventRegistry? _eventRegistry;
    /// <summary>
    /// The player service that manages player-related operations.
    /// </summary>
    private PlayerManager? _playerService;
    /// <summary>
    /// The event handler that subscribes to player events.
    /// Event handlers automatically subscribe to events during construction.
    /// </summary>
    private PlayerEventHandler? _playerEventSubscriber;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Sets up the event system and demonstrates creating a player.
    /// </summary>
    public override async void _Ready() {
        try {
            // 1. Create a single event registry for all events.
            _eventRegistry = new EventRegistry();

            // 2. Initialize all services
            InitializeServices(_eventRegistry);
            // Event subscription happens automatically in EventHandlerBase constructor

            // 3. Run the player workflow demonstration
            if (_playerService != null) {
                await _playerService.DemonstratePlayerWorkflowAsync();
            } else {
                GD.Print("Main: PlayerService is null. Cannot demonstrate player workflow. Something must have gone wrong.");
            }

        } catch (Exception ex) {
            GD.Print($"Main: Error in Main._Ready: {ex.Message}");
        }
    }

    /// <summary>
    /// Initializes all services with the provided event registry.
    /// Event handlers will automatically register their events during construction.
    /// </summary>
    /// <returns>True if all services were initialized successfully, false otherwise</returns>
    /// <param name="eventRegistry">The event registry to use for service initialization</param>
    private void InitializeServices(EventRegistry eventRegistry) {
        // Create all the services and pass in the registry dependency.
        try {
            _playerService = new PlayerManager(eventRegistry);
            _playerEventSubscriber = new PlayerEventHandler(eventRegistry);
        } catch (Exception e) {
            GD.Print($"Main: Error initializing services. {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Called when the node is about to leave the scene tree.
    /// Unsubscribes from events to prevent memory leaks.
    /// </summary>
    public override void _ExitTree() {

        // Unsubscribe from events when quitting - this must be done explicitly
        _playerEventSubscriber?.UnsubscribeFromEvents();

        GD.Print("Main: Unsubscribed from events before quitting.");

        base._ExitTree();
    }

}
