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
    private EventRegistry? _eventRegistry;
    private PlayerManager? _playerService;
    private PlayerEventHandler? _playerEventSubscriber;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Sets up the event system and demonstrates creating a player.
    /// </summary>
    public override async void _Ready() {
        try {
            // 1. Create a single event registry
            _eventRegistry = new EventRegistry();

            // 2. Create services with the registry
            _playerService = new PlayerManager(_eventRegistry);
            _playerEventSubscriber = new PlayerEventHandler(_eventRegistry);

            // 3. Subscribe to events using the base class method
            _playerEventSubscriber.SubscribeToEvents();

            // 4. Run the player workflow demonstration
            await _playerService.DemonstratePlayerWorkflowAsync();
        } catch (Exception ex) {
            Console.WriteLine($"Main: Error in Main._Ready: {ex.Message}");
        }
    }

    /// <summary>
    /// Called when the node is about to leave the scene tree.
    /// Unsubscribes from events to prevent memory leaks.
    /// </summary>
    public override void _ExitTree() {
        // Unsubscribe from events when quitting using the base class method
        _playerEventSubscriber?.UnsubscribeFromEvents();

        Console.WriteLine("Main: Unsubscribed from events before quitting.");

        base._ExitTree();
    }
}
