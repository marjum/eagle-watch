**Candidate:** Mario Mifsud

# Transmax - Back-End Developer Exercise

## In Scope
TODO

## Out of Scope/de-prioritized

1. EagleBots "signing off" from sending traffic data.
1. EagleBot registrar implementation with a suitable data store.

## Assumptions made

1. To start with up to 3 EagleBots will report road traffic throughput/statistics to EagleRock. Solution needs to scale for up to \[X\] EagleBots reporting road traffic data.

1. EagleBots will interact with an EagleRock REST API to send road traffic data.

1. EagleBots will operate for \[X\] hours daily, and expected to make a traffic data call to EagleRock every [Y] seconds. We expect [Z] calls to the API per second.

1. Another EagleRock API will allow a web application to query current EagleBot status via a Redis cache. We expect \[X\] calls to this API per second.

1. The Redis cache in use will limit peristence to "current status" data per EagleBot, and will therefore need to scale based on the number of EagleBots the solution supports.

1. EagleBot statuses will be one of a defined list:
    * Active
    * OffDuty (for example, via a dedicated API, the EagleBot "signs off")
    * Unknown (an API call has not been recieved by the EagleBot since the last [X] minutes)

1. Road traffic data can only be received by registered EagleBot devices. Data received with non-registered EagleBot IDs are to be rejected.

1. Traffic data payload is to comprise:
  * Payload identifier
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
  * Rate of traffic flow (defined as average number of vehicles entering the road segment per 1 minute)
  * Average vehicle speed (kilometers per hour)

1. Traffic data payload will be serialized/de-serialized in JSON form. (TODO: other viable formats to consider?)

1. Received EagleBot data should also be written to a RabbitMQ topic (or similar pub-sub based queue, e.g. AWS SNS) for external applications to consume from. (example use case: write granular road traffic data entries to a data store for querying or analytics)

1. The company implementing/delivering this solution is fictitious, and named "MM". 

1. The Team aligned on standardising with a .NET version with LTS: .NET 6.0 (https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)

## Scoped Stories

1. Design end-to-end EagleRock-based solution, scoping:
    1. EagleBots submitting road traffic to EagleRock API,
    1. Web application querying EagleRock API for EagleBot status,
    1. Publication of road traffic messages to pub-sub topic, comprising planned message consumers to ensure that message content and message delivery SLA will suit for those same consumers' requirements.
    1. Estimation of container scaling requirements, based on:
        1. max number of EagleBot devices to be supported, and estimated traffic update rate per device
        1. web application's status API request rate (e.g. estimated requests per second)
1. Traffic data payload model
1. EagleBot device summary model
1. EagleRock `ReportRoadTrafficUpdate` POST API, with stubs for EagleBot registrar, Redis cache publisher and topic publisher
1. EagleRock `GetDevicesSummary` GET API, with stubs for EagleBot registrar and Redis cache fetcher
1. Redis cache EagleBot road traffic update publication
1. Redis cache EagleBot device summary retrieval
1. Topic road traffic update publication
1. Docker container configuration file  

## Proposed solution structure
Proposed projects naming/structure:
* MM.EagleRock.API
* MM.EagleRock.Contract
* MM.EagleRock.Business
* MM.EagleRock.DAL

## API Verification Testing (other than included unit tests)

### Report road traffic update (happy path)
Request
```json
curl -X 'POST' \
  'https://localhost:7181/api/RoadTrafficUpdates' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "payloadId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "deviceId": "c4567668-5073-41d6-a633-13e5049e7775",
  "geoLocation": {
    "latitude": -27.471407748292812,
    "longitude": 153.02465432944825
  },
  "timestamp": 1688623356,
  "address": {
    "segment": "{\"type\": \"LineString\", \"coordinates\": [[153.02425414910306, -27.471674040322867],[153.0259882639341, -27.470460926973487]]}",
    "streetName": "Elizabeth Street",
    "city": "Brisbane",
    "state": "Queensland",
    "country": "Australia",
    "postalCode": "QLD4000"
  },
  "trafficDirection": "NorthBound",
  "averageTrafficFlowRate": 40.56,
  "averageVehicleSpeed": 33
}'
```
Reponse (HTTP 201)
```
"3fa85f64-5717-4562-b3fc-2c963f66afa6"
```

### Report road traffic update (non-registered device)
Request
```json
curl -X 'POST' \
  'https://localhost:7181/api/RoadTrafficUpdates' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "payloadId": "47a47031-9dfa-4fd4-8a68-03d790f1bc5d",
  "deviceId": "6f9bc2cf-af29-4158-b05c-35ec10d7a49e",
  "geoLocation": {
    "latitude": -27.471407748292812,
    "longitude": 153.02465432944825
  },
  "timestamp": 1688623356,
  "address": {
    "segment": "{\"type\": \"LineString\", \"coordinates\": [[153.02425414910306, -27.471674040322867],[153.0259882639341, -27.470460926973487]]}",
    "streetName": "Elizabeth Street",
    "city": "Brisbane",
    "state": "Queensland",
    "country": "Australia",
    "postalCode": "QLD4000"
  },
  "trafficDirection": "NorthBound",
  "averageTrafficFlowRate": 40.56,
  "averageVehicleSpeed": 33
}'
```
Response (HTTP 400)
```
Device [6f9bc2cf-af29-4158-b05c-35ec10d7a49e] for traffic update payload with Id [47a47031-9dfa-4fd4-8a68-03d790f1bc5d] is not registered
```

### Get device summaries
Request
```json
curl -X 'GET' \
  'https://localhost:7181/api/RoadTrafficUpdates/deviceStatuses' \
  -H 'accept: text/plain'
```
Response
```json
[
  {
    "deviceId": "cda4687e-b8bc-4a50-a9d1-00ef4007e8ca",
    "status": "Unknown",
    "latestRoadTrafficUpdate": null
  },
  {
    "deviceId": "c4567668-5073-41d6-a633-13e5049e7775",
    "status": "Active",
    "latestRoadTrafficUpdate": {
      "payloadId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "deviceId": "c4567668-5073-41d6-a633-13e5049e7775",
      "geoLocation": {
        "latitude": -27.471407748292812,
        "longitude": 153.02465432944825
      },
      "timestamp": 1688623356,
      "address": {
        "segment": "{\"type\": \"LineString\", \"coordinates\": [[153.02425414910306, -27.471674040322867],[153.0259882639341, -27.470460926973487]]}",
        "streetName": "Elizabeth Street",
        "city": "Brisbane",
        "state": "Queensland",
        "country": "Australia",
        "postalCode": "QLD4000"
      },
      "trafficDirection": "NorthBound",
      "averageTrafficFlowRate": 40.56,
      "averageVehicleSpeed": 33
    }
  },
  {
    "deviceId": "71b3978d-2a35-4314-9908-ee5fa704caac",
    "status": "Unknown",
    "latestRoadTrafficUpdate": null
  }
]
```
