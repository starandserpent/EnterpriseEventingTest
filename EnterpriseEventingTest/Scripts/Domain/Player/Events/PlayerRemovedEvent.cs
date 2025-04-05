using System;

namespace EnterpriseEventingTest.Domain.Player.Events;

/// <summary>
/// Event raised when a player is removed
/// </summary>
internal sealed class PlayerRemovedEvent(Guid playerId) {
    /// <summary>
    /// The ID of the player that was removed
    /// </summary>
    public Guid PlayerId { get; } = playerId;
}

