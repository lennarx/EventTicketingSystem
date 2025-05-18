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
- **MassTransit** is used where high resilience and orchestration is needed
- **Raw RabbitMQ** used in Auth and initially in Users for simplicity and control

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
- GitHub Actions (CI/CD)

---

## 🚀 CI/CD with GitHub Actions

- A single GitHub Actions workflow (`docker-deploy-all.yml`) builds and pushes Docker images for all microservices to Docker Hub on every `main` branch push.
- Requires repository secrets:
  - `DOCKERHUB_USERNAME`
  - `DOCKERHUB_PASSWORD`

Each image is published under the format:
```
<username>/eventsystem-<service>:latest
```

> Example: `fredoni/eventsystem-auth:latest`

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
├── .github/workflows/  # CI pipeline for Docker build/push
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

- Services can mix raw RabbitMQ and MassTransit as long as contracts are consistent
- Shared message contracts live in the `Shared/` project
- GraphQL is selectively used (e.g., in Tickets)
- Each service can be deployed and scaled independently
- CI/CD flow ensures consistent image publishing to Docker Hub

---

## 🧩 Next Steps

- Add Outbox Pattern to ensure event delivery consistency
- Add Distributed Tracing (OpenTelemetry)
- Health checks + readiness probes for orchestration
- Saga tracking dashboard (e.g., Automatonymous + MongoDB)
