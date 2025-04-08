using EnterpriseEventingTest.Core.EventSystem;

namespace EnterpriseEventingTest.Domain.World.Services;

public class WorldManager {

    public WorldManager() {

        // Create player instance for testing.
        Player.Model.Player player = new ();

        // Create enemy instance for testing.
        Enemy.Model.Enemy enemy = new ();
    }

}