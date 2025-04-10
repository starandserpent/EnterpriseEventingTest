using System.Collections.Generic;
using EnterpriseEventingTest.Domain.Player.Services;
using Godot;

namespace EnterpriseEventingTest.Domain.Enemy.Model;

// (This is just here so that the code example doesn't throw an error and compiles.)
internal sealed class Enemy {

    private PlayerManager _playerManager;
    private const int SearchRadius = 10;
    private Vector2 Position = new (0, 0);

    public Enemy(PlayerManager playerManager) {
        _playerManager = playerManager;

        Player.Model.Player? player = FindTargetInRadius(SearchRadius);

        if (player is not null) {
            GD.Print("Enemy spotted a player! Attacking!");
        } else {
            GD.Print("Enemy is patrolling.");
        }

        player?.TakeDamage(10, this); // Damage the player by 10 points.
    }

    private Player.Model.Player? FindTargetInRadius(int searchRadius) {
        // Search players that are in proximity from PlayerManager.
        IEnumerable<Player.Model.Player> allPlayers = _playerManager.GetAllPlayers();
        foreach (Player.Model.Player player in allPlayers) {
            if (player.Position.DistanceSquaredTo(this.Position) <= searchRadius * searchRadius) {
                return player;
            }
        }
        return null;
    }
}