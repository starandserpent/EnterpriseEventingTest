using System;
using System.Threading.Tasks;
using EnterpriseEventingTest.Core.EventSystem;
using EnterpriseEventingTest.Domain.Player.Events;
using Godot;

namespace EnterpriseEventingTest.Domain.Player.Services;

/// <summary>
/// Service that handles incoming events for the player domain. These are generally events which are relevant
/// to the player domain and are published by other services to which Player is interested in subscribing.
/// </summary>
internal sealed class PlayerEventHandler : EventHandlerBase {

    /// <summary>
    /// Constructor that takes an event registry dependency and passes it on to the base class.
    /// which is responsible for managing event subscriptions and unsubscriptions. Do not remove.
    /// </summary>
    public PlayerEventHandler(EventRegistry eventRegistry) : base(eventRegistry) {}

    /// <summary>
    /// Registers the event handlers for player events. This method is called when the service is initialized.
    /// It sets up the event handlers for player events. You can add new events by simply adding a new line
    /// in this method which calls:
    /// <code>
    /// RegisterHandler&lt; NewEventType &gt;(OnNewEventAsync);
    /// </code>
    /// Where NewEventType is the type of the new event and OnNewEventAsync is the method that handles the event.
    /// </summary>
    protected override void RegisterEvents() {
        base.RegisterHandler<PlayerAddedEvent>(OnPlayerAddedAsync);
        base.RegisterHandler<PlayerUpdatedEvent>(OnPlayerUpdatedAsync);
        base.RegisterHandler<PlayerRemovedEvent>(OnPlayerRemovedAsync);

        // New events just need one line here
    }

    /// <summary>
    /// Event handler that gets called when a new player is added.
    /// This method initializes the player with starting data.
    /// </summary>
    /// <param name="eventData">The event data containing the player that was added</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task OnPlayerAddedAsync(PlayerAddedEvent eventData) {
        // Get the player from the event
        Model.Player player = eventData.Player;

        // Assign some initial experience to the new player
        player.AddExperience(50);

        Console.ForegroundColor = ConsoleColor.Cyan;
        GD.Print($"[Event] PlayerEventHandler: Player {player.Name} (ID: {player.Id}) initialized with {player.Experience} XP at level {player.Level}.");
        Console.ResetColor();

        // Simulate some async processing
        await Task.Delay(100);
    }

    /// <summary>
    /// Event handler that gets called when a player is updated.
    /// </summary>
    /// <param name="eventData">The event data containing the player that was updated</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task OnPlayerUpdatedAsync(PlayerUpdatedEvent eventData) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        GD.Print($"[Event] PlayerEventHandler: Player {eventData.Player.Name} was updated with {eventData.Player.Experience} XP at level {eventData.Player.Level}.");
        Console.ResetColor();

        await Task.Delay(50);
    }

    /// <summary>
    /// Event handler that gets called when a player is removed.
    /// </summary>
    /// <param name="eventData">The event data containing the ID of the removed player</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task OnPlayerRemovedAsync(PlayerRemovedEvent eventData) {
        Console.ForegroundColor = ConsoleColor.Red;
        GD.Print($"[Event] PlayerEventHandler: Player with ID {eventData.PlayerId} was removed from the system.");
        Console.ResetColor();

        await Task.Delay(50);
    }
}
