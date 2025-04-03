using System;
using System.Threading.Tasks;

namespace EnterpriseEventingTest;

internal sealed class PlayerEventBroker {

    private event OnPlayerAdded onPlayerAdded;
    private event OnPlayerAdded OnPlayerAddedEvent {
        add {
            Console.WriteLine($"{value.Method.Name} has been subscribed to the event.");
            onPlayerAdded += value;
        }
        remove {
            onPlayerAdded -= value;
        }
    }

    public delegate ValueTask<Player> OnPlayerAdded(Player player);

    public void Subscribe(OnPlayerAdded onPlayerAdded) =>
        this.onPlayerAdded += onPlayerAdded;

    public async ValueTask<Player> PublishAsync(Player player) =>
    await this.onPlayerAdded.Invoke(player);

}