**Candidate:** Mario Mifsud

# Transmax - Back-End Developer Exercise


## Assumptions Made

1. To start with up to 3 EagleBots will report road traffic throughput/statistics to EagleRock. Solution needs to scale for up to \[X\] EagleBots reporting road traffic data.

1. EagleBots will interact with an EagleRock REST API to send road traffic data.

1. EagleBots will operate for \[X\] hours daily, and expected to make a traffic data call to EagleRock every [Y] seconds. We expect [Z] calls to the API per second.

1. Another EagleRock API will allow a web application to query current EagleBot status via a Redis cache. We expect \[X\] calls to this API per second.

1. The Redis cache in use will limit peristence to "current status" data per EagleBot, and will therefore need to scale based on the number of EagleBots the solution supports.

1. EagleBot statuses will be one of a defined list:
    * Active
    * Suspended (for example, via a dedicated API, the EagleBot "signs off")
    * Unknown (an API call has not been recieved by the EagleBot since the last [X] minutes)

1. Road traffic data can only be received by registered EagleBot devices. Data received with non-registered EagleBot IDs are to be rejected.

1. Traffic data payload is to comprise:
  * EagleBot unique identifier (recommend GUID for better data portability, especially with say, 1000s of EagleBots in operation)
  * Current geo-location of EagleBot, express in latitude/longitude terms
  * Timestamp for the data exchange (epoch timestamp may be preferable over ISO-8601 date/time format due to its smaller payload size)
  * Road name under inspection, should allow for each of:
    * Road segment
    * Street name
    * City/Suburb
    * State
    * Country
    * Postal code
  * Direction of traffic flow (limited to: North, South, East, West)
  * Rate of traffic flow (defined as number of vehicles entering the road segment per 1 minute)
  * Average vehicle speed (kilometers per hour)

1. Traffic data payload will be serialized/de-serialized in JSON form. (TODO: other viable formats to consider?)

1. Received EagleBot data should also be written to a RabbitMQ topic (or similar pub-sub based queue, e.g. AWS SNS) for external applications to consume from. (example use case: write granular road traffic data entries to a data store for querying or analytics)

1. The company implementing/delivering this solution is fictitious, and named "MM". 

## Proposed Solution Structure

* MM.EagleRock.API
* MM.EagleRock.Contract
* MM.EagleRock.Model
* MM.EagleRock.Business
* MM.EagleRock.DAL

## Scoped Stories

1. Design end-to-end solution, scoping:
    1. EagleBots submitting road traffic to EagleRock,
    1. Web application querying EagleRock for EagleBot status,
    1. Publication of road traffic messages to pub-sub topic, comprising planned message consumers to ensure that message content and message delivery SLA will suit for those same consumers' requirements.
1. Traffic data payload model
1. EagleBot status model
1. EagleRock `SendRoadTrafficStat` POST API, with stubs for EagleBot registrar, Redis cache publisher and topic publisher
1. EagleRock `GetEagleBotStatus` GET API, with stubs for EagleBot registrar and Redis cache fetcher
1. Redis cache EagleBot status publication
1. Redis cache EagleBot status retrieval
1. Topic road traffic payload publication
1. Docker container configuration file  

## Out of Scope

1. EagleBots "signing off" from sending traffic data.
1. EagleBot registrar implementation with a suitable data store.


