# Application server

## Folder tree description

- **Application** - The application layer contains the main business logic of the system.
    - Controllers - Controllers handle HTTP requests by defining REST APIs routes. They interact with services and return appropriate HTTP responses.
    - DTOs - Data transfer objects are the data structures sent and received via HTTP requests and responses. Their purpose is to ensure that exchanged data is properly formatted.
    - Services - Services are the core logic of the application. They are the main components of the application server. They operate with domain models, with the data infrastructure, and sometimes with other services.

- **Domain** - The domain layer defines the core business objects blueprints of the system.
    - Models - The models describe the real-world entities, with their properties and operations. These classes reflect key business concepts, which may also include validation and rules.
    - Interfaces - Interfaces define the contracts that the system services adhere to. They help decouple dependencies and provide a way to implement abstraction.

- **Infrastructure** - The infrastructure layer contains the code responsible for integrating the system with external resources and services.
    - Data - This component handles the database-related logic, including connections, queries, and data retrieval. It contains query service classes that abstract data access and provide a clean interface to interact with the database.
    - Email - This component implements the logic to interact with the email provider, such as sending email messages.

## Request call trace

1. **Application/Controller** - The API request is firstly intercepted by the controller containing the corresponding route. If data is received, it is deserialized as a DTO. The controller then performs the main operations required to fulfill the request, by calling methods of the corresponding service.
2. **Application/Service** - The main business logic operations to perform are defined here. The service will call methods on the model objects, on the data service, and if needed, on other application services. This calls are made through the domain interfaces. This component also manages the conversion of objects between DTOs, DB entities and models.
3. **Domain/Model** - The model of an object defines the operations that can be applied on it, such as state validation.
4. **Infrastructure/Data** - Low level database queries and updates operations are collected as methods in the queries classes. The database tables are perfectly reflected by the entity classes.
5. **Application/Service** - After all the operations are performed, the final object to respond with, if any, is converted back to a DTO and returned to the controller.
6. **Application/Controller** - The request is responded with the right code, and with the DTO as payload, if any.

