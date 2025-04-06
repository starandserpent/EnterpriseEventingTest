using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseEventingTest.Core.EventSystem;
using EnterpriseEventingTest.Domain.Player.Events;
using Godot;

namespace EnterpriseEventingTest.Domain.Player.Services;

/// <summary>
/// Service responsible for player management operations like adding new players.
/// This service uses the event registry to publish events when players are added or updated.
/// </summary>
internal sealed class PlayerManager {
    /// <summary>
    /// In-memory collection of players
    /// </summary>
    private readonly Dictionary<Guid, Model.Player> _players = new();

    /// <summary>
    /// Reference to the central event registry
    /// </summary>
    private readonly EventRegistry _eventRegistry;

    /// <summary>
    /// Constructor that takes the event registry dependency
    /// </summary>
    /// <param name="eventRegistry">The central event registry</param>
    public PlayerManager(EventRegistry eventRegistry) {
        _eventRegistry = eventRegistry;
    }

    /// <summary>
    /// Adds a new player and notifies all interested services through the event bus.
    /// </summary>
    /// <param name="player">The player to add</param>
    /// <returns>The player after being processed by all event subscribers</returns>
    public async ValueTask<Model.Player> AddPlayerAsync(Model.Player player) {
        // Store the player in our local collection
        _players[player.Id] = player;

        // Publish the event to notify subscribers
        await _eventRegistry.GetAsyncEventBus<PlayerAddedEvent>().PublishAsync(new PlayerAddedEvent(player));

        return player;
    }

    /// <summary>
    /// Updates an existing player's information
    /// </summary>
    public async ValueTask<Model.Player> UpdatePlayerAsync(Model.Player player) {
        if (!_players.ContainsKey(player.Id)) {
            throw new ArgumentException("PlayerManager: Cannot update a player that doesn't exist");
        }

        // Update the player in our local collection
        _players[player.Id] = player;

        // Publish the event to notify subscribers
        await _eventRegistry.GetAsyncEventBus<PlayerUpdatedEvent>().PublishAsync(new PlayerUpdatedEvent(player));

        return player;
    }

    /// <summary>
    /// Removes a player from the system
    /// </summary>
    /// <param name="playerId">The ID of the player to remove</param>
    public async Task RemovePlayerAsync(Guid playerId) {
        if (!_players.ContainsKey(playerId)) {
            throw new ArgumentException($"PlayerManager: Cannot find player with ID {playerId}");
        }

        // Remove the player from our local collection
        _players.Remove(playerId);

        // Publish the event to notify subscribers
        await _eventRegistry.GetAsyncEventBus<PlayerRemovedEvent>().PublishAsync(new PlayerRemovedEvent(playerId));
    }

    /// <summary>
    /// Gets a player by their ID
    /// </summary>
    public Model.Player? GetPlayer(Guid id) => _players.GetValueOrDefault(id);

    /// <summary>
    /// Gets all players
    /// </summary>
    public IEnumerable<Model.Player> GetAllPlayers() => _players.Values;

    /// <summary>
    /// Creates a new player with the specified name
    /// </summary>
    /// <param name="name">The name of the player to create</param>
    /// <returns>The created player after being processed by all event subscribers</returns>
    public async ValueTask<Model.Player> CreatePlayerAsync(string name) {
        Model.Player player = new (name);
        return await AddPlayerAsync(player);
    }

    /// <summary>
    /// Adds experience to a player and updates them
    /// </summary>
    /// <param name="playerId">ID of the player to modify</param>
    /// <param name="experienceAmount">Amount of experience to add</param>
    /// <returns>The updated player</returns>
    public async ValueTask<Model.Player> AddExperienceAsync(Guid playerId, int experienceAmount) {
        Model.Player? player = GetPlayer(playerId);
        if (player == null) {
            throw new ArgumentException($"PlayerManager: Cannot find player with ID {playerId}");
        }

        player.AddExperience(experienceAmount);
        return await UpdatePlayerAsync(player);
    }


    /// <summary>
    /// Demonstrates the end-to-end player workflow by creating players,
    /// updating their experience, and displaying the results.
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task DemonstratePlayerWorkflowAsync() {
        // 1. Create multiple players
        List<Model.Player> players = await CreateMultiplePlayersAsync(
            "John Doe",
            "Jane Smith",
            "Alex Johnson"
        );

        // 2. Update players' experience
        Dictionary<Guid, int> experienceMap = new() {
            { players[0].Id, 75 },
            { players[1].Id, 150 },
            { players[2].Id, 200 },
        };
        await AddExperienceToMultiplePlayersAsync(experienceMap);

        // 3. Display all players
        DisplayAllPlayers();
    }


    /// <summary>
    /// Creates multiple players with the specified names
    /// </summary>
    /// <param name="names">The names of the players to create</param>
    /// <returns>A list of the created players after being processed by all event subscribers</returns>
    private async Task<List<Model.Player>> CreateMultiplePlayersAsync(params string[] names) {
        return await ExecuteTasksInParallelAsync(names, 
            name => CreatePlayerAsync(name).AsTask());
    }

    /// <summary>
    /// Adds experience to multiple players
    /// </summary>
    /// <param name="playerExperienceMap">Dictionary mapping player IDs to experience amounts</param>
    /// <returns>A list of the updated players</returns>
    private async Task<List<Model.Player>> AddExperienceToMultiplePlayersAsync(Dictionary<Guid, int> playerExperienceMap) {
        return await ExecuteTasksInParallelAsync(playerExperienceMap, 
            pair => AddExperienceAsync(pair.Key, pair.Value).AsTask());
    }

    /// <summary>
    /// Generic helper method to execute multiple tasks in parallel
    /// </summary>
    /// <typeparam name="TSource">The source collection item type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="items">Collection of items to process</param>
    /// <param name="taskSelector">Function to create a task from each item</param>
    /// <returns>List of results from all completed tasks</returns>
    private async Task<List<TResult>> ExecuteTasksInParallelAsync<TSource, TResult>(
        IEnumerable<TSource> items, 
        Func<TSource, Task<TResult>> taskSelector) {
        
        // Convert to array of tasks
        var tasks = items.Select(taskSelector).ToArray();
        
        // Await all tasks to complete
        await Task.WhenAll(tasks);
        
        // Return results
        return tasks.Select(t => t.Result).ToList();
    }

    /// <summary>
    /// Display information about all registered players
    /// </summary>
    private void DisplayAllPlayers() {
        GD.Print("--- All Players ---");
        foreach (var player in GetAllPlayers()) {
            GD.Print(player.ToString());
        }
        GD.Print("--- End ---");
    }

}
