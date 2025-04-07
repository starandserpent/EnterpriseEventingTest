using EnterpriseEventingTest.Core.EventSystem;

namespace UnitTests.CoreTests;

public class EventBusTests {
    [Fact]
    public void Publish_NoSubscribers_DoesNotThrow() {
        // Arrange
        var eventBus = new EventBus<string>();

        // Act & Assert
        // Should not throw
        eventBus.Publish("test event");
    }

    [Fact]
    public void Publish_WithSubscribers_CallsAllSubscribers() {
        // Arrange
        var eventBus = new EventBus<string>();
        var subscriber1Called = false;
        var subscriber2Called = false;
        string receivedEvent1 = null;
        string receivedEvent2 = null;

        eventBus.Published += (eventData) => {
            subscriber1Called = true;
            receivedEvent1 = eventData;
        };

        eventBus.Published += (eventData) => {
            subscriber2Called = true;
            receivedEvent2 = eventData;
        };

        // Act
        string testEvent = "test event data";
        eventBus.Publish(testEvent);

        // Assert
        Assert.True(subscriber1Called, "First subscriber should be called");
        Assert.True(subscriber2Called, "Second subscriber should be called");
        Assert.Equal(testEvent, receivedEvent1);
        Assert.Equal(testEvent, receivedEvent2);
    }

    [Fact]
    public void Publish_PropagatesExceptionsFromSubscribers() {
        // Arrange
        var eventBus = new EventBus<string>();

        eventBus.Published += (eventData) => {
            throw new InvalidOperationException("Test exception");
        };

        // Act & Assert
        // Verify that exceptions are propagated
        Assert.Throws<InvalidOperationException>(() => eventBus.Publish("test event"));
    }

    [Fact]
    public void Publish_HandlesMultipleEventsInSequence() {
        // Arrange
        var eventBus = new EventBus<int>();
        var sum = 0;

        eventBus.Published += (eventData) => {
            sum += eventData;
        };

        // Act
        eventBus.Publish(5);
        eventBus.Publish(7);
        eventBus.Publish(3);

        // Assert
        Assert.Equal(15, sum);
    }

    [Fact]
    public void Publish_SubscriberCanUnsubscribe() {
        // Arrange
        var eventBus = new EventBus<string>();
        var callCount = 0;

        Action<string> handler = (eventData) => {
            callCount++;
        };

        eventBus.Published += handler;

        // Act & Assert
        eventBus.Publish("first message");
        Assert.Equal(1, callCount);

        // Unsubscribe
        eventBus.Published -= handler;

        eventBus.Publish("second message");
        Assert.True(1 == callCount, "Handler should not be called after unsubscribing");
    }

    [Fact]
    public void Publish_SubscribersExecuteInOrder() {
        // Arrange
        var eventBus = new EventBus<string>();
        var executionOrder = new List<int>();

        eventBus.Published += (eventData) => {
            executionOrder.Add(1);
        };

        eventBus.Published += (eventData) => {
            executionOrder.Add(2);
        };

        eventBus.Published += (eventData) => {
            executionOrder.Add(3);
        };

        // Act
        eventBus.Publish("test event");

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, executionOrder);
    }
}
