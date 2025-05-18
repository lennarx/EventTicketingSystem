# 🎫 Event Ticketing System - Microservices Architecture

This is a distributed system built using microservices for managing event-based operations like ticket reservation, user authentication, and payments. It leverages an orchestrated **SAGA pattern**, RabbitMQ for message communication, and **MassTransit** in all services except Auth and partially Users.

---

## 🧩 Microservices Overview

| Service               | Purpose                                  | Messaging             | API Exposure |
|------------------------|-------------------------------------------|------------------------|--------------|
| `Authentication.Api`  | Handles login and JWT token issuance     | 🔁 Raw RabbitMQ        | ✅ REST       |
| `Users.Api`           | Manages user lifecycle and registration  | 🔁 Raw RabbitMQ + MassTransit | ✅ REST   |
| `Tickets.*`           | Domain-driven ticket service (reservation, confirmation) | ✅ MassTransit | ✅ REST/GraphQL |
| `Payments.Api`        | Simulates and publishes payment outcome  | ✅ MassTransit         | ✅ REST       |
| `Orchestration.Api`   | Coordinates all actions in a SAGA pattern| ✅ MassTransit         | ✅ REST (start saga endpoint) |

All services are independently accessible and can be queried/tested individually.

---

## 🧠 Architecture Summary

- **Event-Driven Design** using RabbitMQ
- **SAGA Orchestration** from `Orchestration.Api` coordinating:
  - Ticket reservation
  - Payment execution
  - Ticket confirmation or cancellation
- **MassTransit** is used in all core services for publishing/consuming commands & events
- `Authentication.Api` and `Users.Api` use direct RabbitMQ consumers for foundational simplicity

---

## 🔐 Authentication

- Based on **JWT tokens**
- Protected APIs require a valid **API Key** in addition to a valid token
- Tokens are issued by `Authentication.Api`

---

## 🚦 SAGA Flow

1. `Orchestration.Api` publishes `TicketReservedEvent`
2. `TicketService` reserves and emits `TicketReservedEvent`
3. Orchestrator publishes `ProcessPaymentCommand`
4. `Payments.Api` processes it, emits:
   - `PaymentSucceededEvent` → confirms ticket
   - `PaymentFailedEvent` → cancels reservation
5. `Tickets.Api` reacts accordingly and updates ticket status
6. `ReservationConfirmedEvent` or `ReservationCanceledEvent` emitted

> Optional: `SagaCompletedEvent` can be used to close the flow

---

## 🛠️ Tech Stack

- .NET 8
- RabbitMQ (Messaging)
- MassTransit (Abstraction layer over RabbitMQ)
- MediatR + Vertical Slice (Handler architecture)
- EF Core (Relational persistence)
- Docker & Docker Compose

---

## 📦 Folder Structure

```
EventTicketingSystem/
│
├── Services/
│   ├── Authentication/Authentication.Api
│   ├── Users/Users.Api
│   ├── Tickets/ (Api, Application, Domain, Infrastructure)
│   ├── Payments/Payments.Api
│   └── Orchestration/Orchestration.Api
│
├── Shared/         # Shared contracts, events, and interfaces
├── docker-compose.yml
└── README.md
```

---

## 🧪 Testing the Flow

- Each service exposes endpoints for testing and inspection
- `Orchestration.Api` provides a POST endpoint (`/saga/start`) to trigger the SAGA manually
- Use tools like Postman or Banana Cake Pop for GraphQL endpoints (Tickets)
- RabbitMQ Admin UI: http://localhost:15672 (guest/guest) to inspect queues, messages, etc.

---

## 🔍 Observations

- You can mix services with raw RabbitMQ and MassTransit as long as message contracts are consistent
- Events follow a shared DTO contract in the `Shared/` project
- GraphQL is used selectively (e.g., in Tickets)
- Each microservice can be scaled and deployed independently

---

## 🚀 Next Steps

- Add Outbox Pattern to ensure event delivery consistency
- Add Distributed Tracing (OpenTelemetry)
- Health checks + readiness probes for container orchestration
- Saga tracking dashboard (e.g., Automatonymous + MongoDB)

---

**Feel free to fork, extend, or contribute.** Built to be modular, testable, and production-ready. 🔥
