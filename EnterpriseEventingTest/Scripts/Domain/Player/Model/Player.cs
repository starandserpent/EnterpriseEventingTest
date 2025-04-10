using System;
using EnterpriseEventingTest.Core.EventMiniBusSystem;
using Godot;

namespace EnterpriseEventingTest.Domain.Player.Model;

/// <summary>
/// Represents a player entity in the game.
/// This class encapsulates all player-specific behavior and state management.
/// </summary>
internal sealed class Player {
    /// <summary>
    /// The unique identifier for the player.
    /// </summary>
    public Guid Id { get; internal set; }

    /// <summary>
    /// The player's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The player's experience points.
    /// </summary>
    public int Experience { get; set; }

    /// <summary>
    /// Health points of the player.
    /// </summary>
    public int Health { get; set; } = 10;

    /// <summary>
    /// The player's current level, calculated from experience.
    /// </summary>
    public int Level => Experience / 100 + 1;

    /// <summary>
    /// Position is the player's position in the game world.
    /// <summary>
    public Vector2 Position { get; set; } = new (0, 0);

    /// Default constructor that generates a new ID automatically and leaves the name empty.
    /// </summary>
    public Player() {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Constructor that takes a name and generates a new ID automatically by calling the default constructor.
    /// </summary>
    public Player(string name) : this() {
        Name = name;
    }

    /// <summary>
    /// Constructor that allows setting a specific ID and name.
    /// </summary>
    public Player(Guid id, string name) {
        Id = id;
        Name = name;
    }

    public Player(string name, Vector2 position) : this(name) {
        Position = position;
    }

    public Player(Guid id, string name, Vector2 position) : this(id, name) {
        Position = position;
    }

    public Player(Guid id, string name, Vector2 position, int health) : this(id, name, position) {
        Health = health;
    }

    public Player(Guid id, string name, Vector2 position, int health, int experience) : this(id, name, position, health) {
        Experience = experience;
    }

    public Player(Guid id, string name, Vector2 position, int health, int experience, int level) : this(id, name, position, health, experience) {
        Experience = level * 100 - 100;
    }


    /// <summary>
    /// Adds experience to the player and handles any level-up logic.
    /// </summary>
    /// <param name="amount">Amount of experience to add.</param>
    /// <returns>True if the player leveled up.</returns>
    public bool AddExperience(int amount) {
        int oldLevel = Level;
        Experience += amount;
        return Level > oldLevel;
    }

    /// <summary>
    /// Returns a formatted string representation of the player.
    /// </summary>
    public override string ToString() => $"Player: {Name}, ID: {Id}, Level: {Level} (XP: {Experience})";

    public void TakeDamage(int damage, Enemy.Model.Enemy enemy) {

        if (damage <= 0) {
            return;
        }

        if (Health - damage <= 0) {
            Health = 0;
            Die(enemy);
        } else {
            Health -= damage;
        }
    }

    private void Die(Enemy.Model.Enemy enemy) {
        EventMiniBus.Instance.InvokePlayerDied(new PlayerDiedEventArgs(this, Position, enemy));
    }

}
