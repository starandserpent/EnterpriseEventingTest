using System;

namespace EnterpriseEventingTest.Domain.Player.Model;

/// <summary>
/// Represents a player entity in the game.
/// This class encapsulates all player-specific behavior and state management.
/// </summary>
internal sealed class Player {
    /// <summary>
    /// The unique identifier for the player.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// The player's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The player's experience points.
    /// </summary>
    public int Experience { get; set; }

    /// <summary>
    /// The player's current level, calculated from experience.
    /// </summary>
    public int Level => Experience / 100 + 1;

    /// <summary>
    /// Default constructor that generates a new ID automatically
    /// </summary>
    public Player() {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Constructor with name that generates a new ID automatically
    /// </summary>
    public Player(string name) : this() {
        Name = name;
    }

    /// <summary>
    /// Constructor that allows setting a specific ID
    /// </summary>
    public Player(Guid id, string name) {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Adds experience to the player and handles any level-up logic
    /// </summary>
    /// <param name="amount">Amount of experience to add</param>
    /// <returns>True if the player leveled up</returns>
    public bool AddExperience(int amount) {
        int oldLevel = Level;
        Experience += amount;
        return Level > oldLevel;
    }

    /// <summary>
    /// Returns a formatted string representation of the player
    /// </summary>
    public override string ToString() => $"Player: {Name}, ID: {Id}, Level: {Level} (XP: {Experience})";
}
