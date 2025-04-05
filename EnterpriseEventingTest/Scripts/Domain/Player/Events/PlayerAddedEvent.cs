namespace EnterpriseEventingTest.Domain.Player.Events;

/// <summary>
/// Event raised when a player is added to the system
/// </summary>
internal sealed class PlayerAddedEvent(Model.Player player) {
    /// <summary>
    /// The player that was added
    /// </summary>
    public Model.Player Player { get; } = player;
}