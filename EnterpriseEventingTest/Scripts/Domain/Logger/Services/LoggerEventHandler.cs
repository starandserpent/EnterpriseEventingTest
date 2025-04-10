using System;
using System.Threading.Tasks;
using EnterpriseEventingTest.Core.EventMiniBusSystem;
using EnterpriseEventingTest.Core.EventSystem;
using EnterpriseEventingTest.Domain.Player.Events;
using Godot;

namespace EnterpriseEventingTest.Domain.Logger.Services;

internal class LoggerEventHandler : EventHandlerBase {

    public LoggerEventHandler(EventRegistry eventRegistry) : base(eventRegistry) {}

    protected override void RegisterEvents() {
        RegisterHandler<PlayerAddedEvent>(OnPlayerAddedAsync);

        // Testing EventMiniBus events.
        EventMiniBus.Instance.PlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied(PlayerDiedEventArgs @event) {
        Console.ForegroundColor = ConsoleColor.White;
        GD.Print($"[Event] LoggerEventHandler: Player {@event.Player.Name} (ID: {@event.Player.Id}) died at position {@event.Position}.");
        Console.ResetColor();
    }

    // Log the player addition event.
    private async Task OnPlayerAddedAsync(PlayerAddedEvent eventData) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        var player = eventData.Player;
        GD.Print($"[Event] LoggerEventHandler: Player {player.Name} (ID: {player.Id}) added.");
        Console.ResetColor();

        // Simulate some async processing
        await Task.Delay(100);
    }
}
