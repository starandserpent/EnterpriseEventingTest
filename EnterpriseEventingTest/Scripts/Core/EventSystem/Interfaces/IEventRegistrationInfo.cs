namespace EnterpriseEventingTest.Core.EventSystem.Interfaces;

/// <summary>
/// Interface providing information about an event registration.
/// </summary>
public interface IEventRegistrationInfo
{
    /// <summary>
    /// Gets the name of the event type this registration is for.
    /// </summary>
    string EventTypeName { get; }
    
    /// <summary>
    /// Gets the name of the handler method.
    /// </summary>
    string HandlerMethodName { get; }
}
