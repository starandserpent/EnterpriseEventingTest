using EnterpriseEventingTest.Domain.Player.Services;

namespace EnterpriseEventingTest.Domain.World.Services;

internal sealed class WorldManager {
    private WorldManager() {}
    public WorldManager(PlayerManager playerManager) {

        // Create enemy instance for testing.
        Enemy.Model.Enemy enemy = new (playerManager);
    }

}