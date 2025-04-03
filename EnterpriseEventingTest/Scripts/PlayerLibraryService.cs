using System;
using System.Threading.Tasks;

namespace EnterpriseEventingTest;

internal sealed class PlayerLibraryService {

    private readonly PlayerEventBroker playerEventBroker;

    public PlayerLibraryService(PlayerEventBroker playerEventBroker) =>
        this.playerEventBroker = playerEventBroker;

    public void SubscribeToPlayerAddedEvent() =>
        this.playerEventBroker.Subscribe(CreatePlayerGuidAsync);

    public async ValueTask<Player> CreatePlayerGuidAsync(Player player) {
        string guid = Guid.NewGuid().ToString();
        Console.WriteLine($"Player {player.Name} has been assigned a GUID: {guid}.");

        return player;
    }

}