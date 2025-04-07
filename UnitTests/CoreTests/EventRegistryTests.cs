using EnterpriseEventingTest.Core.EventSystem;
using EnterpriseEventingTest.Core.EventSystem.Interfaces;

namespace UnitTests.CoreTests;

public class EventRegistryTests {
    [Fact]
    public void GetAsyncEventBus_ReturnsSameInstance_ForSameType() {
        // Arrange
        var registry = new EventRegistry();

        // Act
        var bus1 = registry.GetAsyncEventBus<string>();
        var bus2 = registry.GetAsyncEventBus<string>();

        // Assert
        Assert.Same(bus1, bus2);
    }

    [Fact]
    public void GetEventBus_ReturnsSameInstance_ForSameType() {
        // Arrange
        var registry = new EventRegistry();

        // Act
        var bus1 = registry.GetEventBus<string>();
        var bus2 = registry.GetEventBus<string>();

        // Assert
        Assert.Same(bus1, bus2);
    }

    [Fact]
    public void GetAsyncEventBus_ReturnsDifferentInstances_ForDifferentTypes() {
        // Arrange
        var registry = new EventRegistry();

        // Act
        var stringBus = registry.GetAsyncEventBus<string>();
        var intBus = registry.GetAsyncEventBus<int>();

        // Assert
        Assert.NotSame(stringBus, intBus);
        Assert.IsType<AsyncEventBus<string>>(stringBus);
        Assert.IsType<AsyncEventBus<int>>(intBus);
    }

    [Fact]
    public void GetEventBus_ReturnsDifferentInstances_ForDifferentTypes() {
        // Arrange
        var registry = new EventRegistry();

        // Act
        var stringBus = registry.GetEventBus<string>();
        var intBus = registry.GetEventBus<int>();

        // Assert
        Assert.NotSame(stringBus, intBus);
        Assert.IsType<EventBus<string>>(stringBus);
        Assert.IsType<EventBus<int>>(intBus);
    }

    [Fact]
    public void GetAsyncAndSyncBuses_ReturnDifferentInstances_ForSameType() {
        // Arrange
        var registry = new EventRegistry();

        // Act
        var asyncBus = registry.GetAsyncEventBus<string>();
        var syncBus = registry.GetEventBus<string>();

        // Assert
        Assert.NotSame(asyncBus, syncBus);
        Assert.IsType<AsyncEventBus<string>>(asyncBus);
        Assert.IsType<EventBus<string>>(syncBus);
    }

    [Fact]
    public void GetAsyncEventBus_ReturnsCorrectType() {
        // Arrange
        var registry = new EventRegistry();

        // Act
        var bus = registry.GetAsyncEventBus<CustomEventType>();

        // Assert
        Assert.IsType<AsyncEventBus<CustomEventType>>(bus);
        Assert.IsAssignableFrom<IAsyncEventBus<CustomEventType>>(bus);
    }

    [Fact]
    public void GetEventBus_ReturnsCorrectType() {
        // Arrange
        var registry = new EventRegistry();

        // Act
        var bus = registry.GetEventBus<CustomEventType>();

        // Assert
        Assert.IsType<EventBus<CustomEventType>>(bus);
        Assert.IsAssignableFrom<IEventBus<CustomEventType>>(bus);
    }

    // Test class to use as a custom event type
    private class CustomEventType { }
}
