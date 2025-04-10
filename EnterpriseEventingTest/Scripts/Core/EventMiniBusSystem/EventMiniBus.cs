using System;
using EnterpriseEventingTest.Domain.Enemy.Model;
using EnterpriseEventingTest.Domain.Player.Model;
using Godot;

namespace EnterpriseEventingTest.Core.EventMiniBusSystem;

/// <summary>
/// EventBus is a simpleevent bus that allows for event subscription and invocation.
/// It is a basic implementation of the event system. Everything is defined in three steps:
/// 1. Define the events - The events can be subscribed to by other classes to listen for the event.
/// 2. Define the mappings for the events - The mappings are used to map the events to the event handlers.
/// 3. Define the event arguments.
/// </summary>
internal sealed class EventMiniBus {

    // Singleton.
    // The singleton instance of the EventMiniBus. This is used to access the event bus from anywhere in the code.
    private static EventMiniBus? _instance;
    public static EventMiniBus Instance => _instance ??= new EventMiniBus();
    // The constructor is private to prevent instantiation from outside the class.
    private EventMiniBus() {}

    // 1. Event definitions.

    // Here starts the event definitions. They are used to define the events that can be triggered in the system.
    // The events are defined as public events that can be subscribed to by other classes.
    // You can do this by using the += operator to add a method to the event. Example:
    // eventBus.PlayerDied += OnPlayerDied;
    // You would generally do this in the constructor of the class that wants to listen to the event.

    public event Action<PlayerDiedEventArgs>? PlayerDied;
    public event Action<PlayerLevelUpEventArgs>? PlayerLevelUp;

    // 2. Event mappings.

    // Here starts the mappings for the events. They are used to map the events to the event handlers.
    // The event handlers are the methods that will be called when the event is triggered.
    // You should call them in the methods that trigger the events. Example:
    // eventBus.InvokePlayerDied(new PlayerDiedEventArgs(player, position, lastHitBy));

    public void InvokePlayerDied(PlayerDiedEventArgs args) => PlayerDied?.Invoke(args);
    public void InvokePlayerLevelUp(PlayerLevelUpEventArgs args) => PlayerLevelUp?.Invoke(args);
 }

// 3. Event arguments.

// Here starts the event argument definitions. They are used to define the arguments that will be passed
// to the event handlers. They are defined as record structs to provide a lightweight way to pass data around.
// What they do is that they define the data that will be passed to the event handlers when the event is triggered.
// They are used to pass the data that is relevant to the event. For example, when a player dies, we want to
// pass the player that died, the position where they died, and the enemy that killed them. This is done by
// defining the PlayerDiedEventArgs struct with the relevant data. The same goes for the PlayerLevelUpEventArgs
// struct, which defines the data that will be passed when a player levels up.

internal readonly record struct PlayerDiedEventArgs(Player Player, Vector2 Position, Enemy LastHitBy);
internal readonly record struct PlayerLevelUpEventArgs(Player Player, int NewLevel, int OldLevel);

