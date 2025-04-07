using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseEventingTest.Core.EventSystem;
using Xunit;

namespace UnitTests.CoreTests;

public class AsyncEventBusTests {
    [Fact]
    public async Task PublishAsync_NoSubscribers_DoesNotThrow() {
        // Arrange
        var eventBus = new AsyncEventBus<string>();

        // Act & Assert
        // Should not throw
        await eventBus.PublishAsync("test event");
    }

    [Fact]
    public async Task PublishAsync_WithSubscribers_CallsAllSubscribers() {
        // Arrange
        var eventBus = new AsyncEventBus<string>();
        var subscriber1Called = false;
        var subscriber2Called = false;
        string receivedEvent1 = null;
        string receivedEvent2 = null;

        eventBus.PublishedAsync += async (eventData) => {
            subscriber1Called = true;
            receivedEvent1 = eventData;
            await Task.CompletedTask;
        };

        eventBus.PublishedAsync += async (eventData) => {
            subscriber2Called = true;
            receivedEvent2 = eventData;
            await Task.CompletedTask;
        };

        // Act
        string testEvent = "test event data";
        await eventBus.PublishAsync(testEvent);

        // Assert
        Assert.True(subscriber1Called, "First subscriber should be called");
        Assert.True(subscriber2Called, "Second subscriber should be called");
        Assert.Equal(testEvent, receivedEvent1);
        Assert.Equal(testEvent, receivedEvent2);
    }

    [Fact]
    public async Task PublishAsync_PropagatesExceptionsFromSubscribers() {
        // Arrange
        var eventBus = new AsyncEventBus<string>();

        eventBus.PublishedAsync += async (eventData) => {
            await Task.CompletedTask;
            throw new InvalidOperationException("Test exception");
        };

        // Act & Assert
        // Verify that exceptions are propagated
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await eventBus.PublishAsync("test event"));
    }

    [Fact]
    public async Task PublishAsync_HandlesMultipleEventsInSequence() {
        // Arrange
        var eventBus = new AsyncEventBus<int>();
        var sum = 0;

        eventBus.PublishedAsync += async (eventData) => {
            await Task.Delay(10); // Small delay to simulate async work
            sum += eventData;
        };

        // Act
        await eventBus.PublishAsync(5);
        await eventBus.PublishAsync(7);
        await eventBus.PublishAsync(3);

        // Assert
        Assert.Equal(15, sum);
    }

    [Fact]
    public async Task PublishAsync_SubscriberCanUnsubscribe() {
        // Arrange
        var eventBus = new AsyncEventBus<string>();
        var callCount = 0;

        Func<string, Task> handler = async (eventData) => {
            await Task.Delay(10);
            callCount++;
        };

        eventBus.PublishedAsync += handler;

        // Act & Assert
        await eventBus.PublishAsync("first message");
        Assert.Equal(1, callCount);

        // Unsubscribe
        eventBus.PublishedAsync -= handler;

        await eventBus.PublishAsync("second message");
        Assert.True(1 == callCount, "Handler should not be called after unsubscribing");
    }

    [Fact]
    public async Task PublishAsync_HandlersRunInParallel() {
        // Arrange
        var eventBus = new AsyncEventBus<string>();
        var executionTimes = new List<DateTime>();
        var completionTimes = new List<DateTime>();
        
        // Add handlers that have different delays
        eventBus.PublishedAsync += async (eventData) => {
            executionTimes.Add(DateTime.Now);
            await Task.Delay(100); // Longer delay
            completionTimes.Add(DateTime.Now);
        };

        eventBus.PublishedAsync += async (eventData) => {
            executionTimes.Add(DateTime.Now);
            await Task.Delay(50); // Medium delay
            completionTimes.Add(DateTime.Now);
        };

        eventBus.PublishedAsync += async (eventData) => {
            executionTimes.Add(DateTime.Now);
            await Task.Delay(10); // Short delay
            completionTimes.Add(DateTime.Now);
        };

        // Act
        var startTime = DateTime.Now;
        await eventBus.PublishAsync("test event");
        var endTime = DateTime.Now;

        // Assert
        // All handlers should start at roughly the same time
        Assert.Equal(3, executionTimes.Count);
        Assert.Equal(3, completionTimes.Count);
        
        // Total time should be approximately the time of the longest handler
        // If they ran sequentially, it would be over 160ms
        var totalTime = (endTime - startTime).TotalMilliseconds;
        Assert.True(totalTime < 150, $"Handlers should run in parallel. Total time: {totalTime}ms");
    }
    
    [Fact]
    public async Task PublishAsync_AllHandlersCompleteBeforeReturning() {
        // Arrange
        var eventBus = new AsyncEventBus<string>();
        var handlerCompletions = new List<bool> { false, false, false };

        eventBus.PublishedAsync += async (eventData) => {
            await Task.Delay(30);
            handlerCompletions[0] = true;
        };

        eventBus.PublishedAsync += async (eventData) => {
            await Task.Delay(50);
            handlerCompletions[1] = true;
        };

        eventBus.PublishedAsync += async (eventData) => {
            await Task.Delay(20);
            handlerCompletions[2] = true;
        };

        // Act
        await eventBus.PublishAsync("test event");

        // Assert
        // All handlers should have completed
        Assert.True(handlerCompletions[0], "First handler should complete");
        Assert.True(handlerCompletions[1], "Second handler should complete");
        Assert.True(handlerCompletions[2], "Third handler should complete");
    }
}
