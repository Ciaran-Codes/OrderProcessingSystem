# OrderProcessingSystem

A modular, event-driven microservices architecture built in .NET 8 using RabbitMQ, MassTransit, and PostgreSQL.

This project demonstrates a production-style architecture for distributed systems using domain-driven design, asynchronous messaging, and clear service boundaries — without overengineering.

---

## Architecture Overview

```plaintext
   [Client / Frontend]
           │
           ▼
     ┌─────────────┐
     │ OrderService│
     └─────┬───────┘
           │
           │ publishes
           ▼
  [OrderPlacedEvent 📨 via RabbitMQ]
           │
 ┌────────────┬──────────────┐
 ▼            ▼              ▼
PaymentSvc  InventorySvc  NotificationSvc
   │            │              ▲
   │            │              │
   │            └───┐     listens for
   │ emits          ▼     PaymentSucceeded / Failed
   ▼        [InventoryAdjustedEvent]
[PaymentSucceededEvent]
     or
[PaymentFailedEvent]
```

---

## Tech Stack

| Area             | Tech                          |
|------------------|-------------------------------|
| Language         | .NET 8 (C#)                   |
| Messaging        | RabbitMQ + MassTransit        |
| Internal Logic   | MediatR (CQRS pattern)        |
| Persistence      | PostgreSQL + EF Core          |
| API Documentation| Swagger/OpenAPI               |
| Containerization | Docker                        |

---

## Features Implemented

- 🔹 **OrderService**: Accepts new orders, persists to DB, publishes events
- 🔹 **PaymentService**: Consumes orders, simulates payment, publishes success/fail
- 🔹 **InventoryService**: Decrements stock on order placement
- 🔹 **NotificationService**: Sends fake notifications based on payment status
- 🔹 **Shared.Contracts**: Central message/event schema for inter-service communication
- 🔹 **PostgreSQL**: Used by OrderService for order persistence
- 🔹 **RabbitMQ**: Central message broker
- 🔹 **EF Core Migrations**: Code-first database creation

---

## Running Locally

> Prerequisites: [.NET 8 SDK](https://dotnet.microsoft.com/download), [Docker](https://www.docker.com/)

1. Start RabbitMQ and PostgreSQL containers:
```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
docker run -d --name orderdb -e POSTGRES_PASSWORD=postgres -p 5432:5432 postgres
```

2. Apply DB Migrations:
```bash
cd OrderService
dotnet ef database update
```

3. Run each service in separate terminals or set them as multiple startup projects in Visual Studio:
```bash
dotnet run --project OrderService
dotnet run --project PaymentService
dotnet run --project InventoryService
dotnet run --project NotificationService
```

4. Test via Swagger at:
- `http://localhost:5000/swagger` (OrderService)

---

## Possible Future Improvements

- Distributed tracing (OpenTelemetry)
- Docker Compose orchestration
- Centralized logging (Serilog + Seq)
- JWT authentication
- Unit + Integration tests

---

## What This Project Demonstrates

- Clean microservice separation with shared contracts
- Event-driven async communication (pub/sub model)
- Realistic .NET 8 usage with MassTransit and RabbitMQ
- Persistence and migrations via EF Core
- A clear, extensible architecture for real-world systems

---

## License

MIT - Go nuts.
