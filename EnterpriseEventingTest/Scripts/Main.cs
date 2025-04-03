using Godot;

namespace EnterpriseEventingTest;

internal partial class Main : Node {
    public override async void _Ready() {
        var playerEventBroker = new PlayerEventBroker();
        var playerService = new PlayerService(playerEventBroker);
        var playerLibraryService = new PlayerLibraryService(playerEventBroker);

        playerLibraryService.SubscribeToPlayerAddedEvent();

        await playerService.AddPlayerAsync(new Player { Name = "John Doe", });
    }

}