# EnterpriseEventingTest

## Root folders and files

/Main.cs - Entry point.

/Core/   - Contains technical boiler plate to make everything work. Shouldn't require changes when adding
           new events etc.

/Domain/ - Contains business domain specific example implementations.

## Suggested structure for domain, examples

/Domain/Player/Events/ - Holds definitions of the events' data structure (what information is carried), e.g.
  PlayerAddedEvent.cs: Invoked by PlayerManager when a new player is added.
  PlayerRemovedEvent.cs: Invoked by PlayerManager when a player is removed.
  PlayerUpdatedEvent.cs: Invoked by PlayerManager when a player is updated.

/Domain/Player/Services/ - Holds services related to Player which get things done:
  PlayerManager.cs: Contains business logic to trigger the events (the publisher) across multiple Players, e.g.
                    It acts as the entry point for operations like "Create a new player," "Change player's name,"
                    "Award points to player."
  PlayerEventHandler.cs: Contains logic that needs to run in reaction to specific domain events happening.
                         It listens for events. When an event it's listening for is published (e.g., by
                         PlayerManager or a service from another domain), the corresponding method in
                         PlayerEventHandler is executed.

/Domain/Player/Model/ - Holds the data model classes for the Player domain, e.g.
  Player.cs: Represents the Player entity, containing both its data attributes (ID, Name, Score, etc.) 
             and related behavior/logic (e.g., IncreaseScore(), ChangeName(), CanPerformAction()).

## Adding new events
To add new events (like PlayerBannedEvent), you now only need to:

1) Create the event class
2) Add a handler method in PlayerEventHandler
3) Register the handler in RegisterEvents()
4) Add a method in PlayerManager to trigger the event
