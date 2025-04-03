using System.Threading.Tasks;

namespace EnterpriseEventingTest;

internal class PlayerService {

    private readonly PlayerEventBroker playerEventBroker;

    public PlayerService(PlayerEventBroker playerEventBroker) {
        this.playerEventBroker = playerEventBroker;
    }

    public async ValueTask<Player> AddPlayerAsync(Player player) => await this.playerEventBroker.PublishAsync(player);

}