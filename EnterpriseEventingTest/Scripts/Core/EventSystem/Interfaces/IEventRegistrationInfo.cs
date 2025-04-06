using System;

namespace EnterpriseEventingTest.Core.EventSystem.Interfaces;

public interface IEventRegistrationInfo {
    Type EventType { get; }
    string EventTypeName { get; }
    string HandlerMethodName { get; }
}