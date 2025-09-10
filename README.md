🔑 Tech Stack

.NET 9 / C# 13 → modern language features (primary constructors, interceptors, required members, advanced pattern matching).

ASP.NET Core Minimal APIs → lightweight, expressive HTTP endpoints.

Entity Framework Core 9 → persistence with LINQ, migrations, optimized queries.

MediatR → CQRS with request/response + pipeline behaviors.

ErrorOr → functional-style error handling and standardized API responses.

Aspire → service orchestration for local dev and cloud deployment.

Kafka → asynchronous communication between microservices.

Serilog + OpenTelemetry → structured logging, metrics, distributed tracing.

🏗 Architecture

Microservices:

Posts Service → handles CRUD and publishing of posts.

Notifications Service → consumes Kafka events and delivers real-time notifications.

Vertical Slice Architecture → each feature encapsulates commands, queries, handlers, and validators.

Infrastructure: EF Core for persistence, Kafka for messaging, Redis (optional) for caching, OpenAPI for documentation.

API Gateway: YARP for unified entry point and routing.

🚀 Features

RESTful endpoints for post management and notifications delivery.

Validation and global error handling via MediatR pipeline behaviors + ErrorOr.

Distributed communication through Kafka topics (decoupled services).

Full observability: structured logs, tracing, and metrics.

Containerized stack with Aspire + Docker for local dev.

📦 DevOps & Deployment

Docker & Aspire → one-command local environment (services + Kafka + Postgres).

CI/CD with GitHub Actions → build, test, lint, deploy.

Infrastructure as Code (Terraform) → provision cloud infra (DB, Kafka cluster, monitoring).