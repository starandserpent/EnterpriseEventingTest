using EnterpriseEventingTest.Domain.Player.Model;

namespace UnitTests.DomainTests;

public class PlayerTests {
    [Fact]
    public void Constructor_DefaultConstructor_GeneratesId() {
        // Act
        var player = new Player();

        // Assert
        Assert.NotEqual(Guid.Empty, player.Id);
        Assert.Equal(string.Empty, player.Name);
        Assert.Equal(0, player.Experience);
        Assert.Equal(1, player.Level);
    }

    [Fact]
    public void Constructor_WithName_GeneratesIdAndSetsName() {
        // Arrange
        string expectedName = "Test Player";

        // Act
        var player = new Player(expectedName);

        // Assert
        Assert.NotEqual(Guid.Empty, player.Id);
        Assert.Equal(expectedName, player.Name);
    }

    [Fact]
    public void Constructor_WithIdAndName_SetsIdAndName() {
        // Arrange
        var expectedId = Guid.NewGuid();
        string expectedName = "Test Player";

        // Act
        var player = new Player(expectedId, expectedName);

        // Assert
        Assert.Equal(expectedId, player.Id);
        Assert.Equal(expectedName, player.Name);
    }

    [Fact]
    public void Level_CalculatedCorrectly_BasedOnExperience() {
        // Arrange
        var player = new Player("Level Test Player");

        // Act & Assert
        Assert.Equal(1, player.Level); // Starting level with 0 XP

        player.Experience = 50;
        Assert.Equal(1, player.Level); // Level 1: 0-99 XP

        player.Experience = 100;
        Assert.Equal(2, player.Level); // Level 2: 100-199 XP

        player.Experience = 250;
        Assert.Equal(3, player.Level); // Level 3: 200-299 XP

        player.Experience = 999;
        Assert.Equal(10, player.Level); // Level 10: 900-999 XP
    }

    [Fact]
    public void AddExperience_IncreasesExperience() {
        // Arrange
        var player = new Player("XP Test Player");

        // Act
        player.AddExperience(50);

        // Assert
        Assert.Equal(50, player.Experience);
    }

    [Fact]
    public void AddExperience_ReturnsTrue_WhenLevelIncreases() {
        // Arrange
        var player = new Player("Leveling Player");

        // Act
        bool leveledUp = player.AddExperience(100); // 0 -> 100 XP (Level 1 -> 2)

        // Assert
        Assert.True(leveledUp);
        Assert.Equal(2, player.Level);
    }

    [Fact]
    public void AddExperience_ReturnsFalse_WhenLevelDoesNotIncrease() {
        // Arrange
        var player = new Player("Non-leveling Player");

        // Act
        bool leveledUp = player.AddExperience(50); // 0 -> 50 XP (still Level 1)

        // Assert
        Assert.False(leveledUp);
        Assert.Equal(1, player.Level);
    }

    [Fact]
    public void ToString_ReturnsFormattedString() {
        // Arrange
        var id = Guid.NewGuid();
        var player = new Player(id, "Format Test Player");
        player.Experience = 150;

        // Act
        string result = player.ToString();

        // Assert
        Assert.Contains("Format Test Player", result);
        Assert.Contains(id.ToString(), result);
        Assert.Contains("Level: 2", result);
        Assert.Contains("XP: 150", result);
    }

    [Fact]
    public void AddExperience_AddsCorrectAmount_ForMultipleCalls() {
        // Arrange
        var player = new Player("Multiple XP Test");

        // Act
        player.AddExperience(30);
        player.AddExperience(40);
        player.AddExperience(50);

        // Assert
        Assert.Equal(120, player.Experience);
        Assert.Equal(2, player.Level);
    }
}