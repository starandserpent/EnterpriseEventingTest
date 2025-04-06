using System;
using System.Threading.Tasks;
using EnterpriseEventingTest.Core.EventSystem;
using EnterpriseEventingTest.Domain.Player.Events;
using Godot;

namespace EnterpriseEventingTest.Domain.Logger.Services;

internal class LoggerEventHandler : EventHandlerBase {

    public LoggerEventHandler(EventRegistry eventRegistry) : base(eventRegistry) {}

    protected override void RegisterEvents() {
        base.RegisterHandler<PlayerAddedEvent>(OnPlayerAddedAsync);
    }

    // Log the player addition event.
    private async Task OnPlayerAddedAsync(PlayerAddedEvent eventData) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Player.Model.Player player = eventData.Player;
        GD.Print($"[Event] LoggerEventHandler: Player {player.Name} (ID: {player.Id}) added.");
        Console.ResetColor();

        // Simulate some async processing
        await Task.Delay(100);
    }


}