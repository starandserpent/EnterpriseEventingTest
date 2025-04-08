using Godot;

namespace EnterpriseEventingTest.Domain.Enemy.Model;

// (This is just here so that the code example doesn't throw an error and compiles.)
internal sealed class Enemy {

    private const int SearchRadius = 10;

    public Enemy() {
        Player.Model.Player? player = FindTargetInRadius(SearchRadius);

        if (player is not null) {
            GD.Print("Enemy spotted a player! Attacking!");
        } else {
            GD.Print("Enemy is patrolling.");
        }

        player?.TakeDamage(10, this); // Damage the player by 10 points.
    }

    private Player.Model.Player? FindTargetInRadius(int searchRadius) => new ();
}