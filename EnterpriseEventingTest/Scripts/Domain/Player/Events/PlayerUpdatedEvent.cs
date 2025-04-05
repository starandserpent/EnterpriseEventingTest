namespace EnterpriseEventingTest.Domain.Player.Events;

/// <summary>
/// Event raised when a player is updated
/// </summary>
internal sealed class PlayerUpdatedEvent(Model.Player player) {
    /// <summary>
    /// The player that was updated
    /// </summary>
    public Model.Player Player { get; } = player;
}