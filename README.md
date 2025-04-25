# Breadcrumbs API – Architecture Overview

Welcome! This short README gives a **big‑picture tour** of how requests travel through the Breadcrumbs back‑end.  If you understand three layers—**Controllers → Application (MediatR) → Persistence (EF Core)**—you can already follow 80 % of the codebase.

---

## 1. Layer Cake at a Glance

```
┌──────────────────────────────┐   HTTP/JSON
│  Presentation Layer          │   ———————————————————————————
│  (ASP.NET Core API)          │   Receives requests, returns
│  • Controllers               │   responses.
└──────────────┬───────────────┘
               │  ↓ calls MediatR
┌──────────────┴───────────────┐   Pure application logic
│  Application Layer           │   (no I/O)
│  (CQRS via MediatR)          │   • Commands / Queries
│  • Command / Query Handlers  │   • Validators
└──────────────┬───────────────┘
               │  ↓ via DI
┌──────────────┴───────────────┐   Infrastructure & data access
│  Persistence Layer           │   • EF Core DbContext
│  (Infrastructure)            │   • Repositories
└──────────────────────────────┘   Reads / writes SQL
```

> **Why bother with layers?**  Separation of concerns keeps the domain logic testable and free from web‑specific or database‑specific details.

---

## 2. Request / Response Flow

1. **HTTP Request** hits an **API controller** (e.g., `CrumbsController`).  Controllers do minimal work: translate JSON → C# objects and dispatch to MediatR.
2. The controller sends either a **Command** ("do something") or **Query** ("get something") through **`IMediator.Send()`**.
3. MediatR locates the corresponding **Handler** in the **Application layer**.  The handler contains business rules but _no_ Entity Framework code.
4. If data is needed, the handler calls an **interface** such as `ICrumbRepository`.  That interface is registered in DI to resolve a concrete class in the **Persistence layer**.
5. The repository uses **`BreadcrumbsDbContext`** (Entity Framework Core) to talk to Azure SQL (or your local SQL Server).  It returns entities or DTOs back up the chain.
6. The handler builds a **Result DTO** and returns it to the controller, which serializes it back to JSON.

```
Browser ↔️ Controller → MediatR → Handler → Repository → DbContext → SQL
                                         ↑  DTOs / Entities  ↓
                                    JSON ←────────── Result
```

---

## 3. The Layers in Detail

### 3.1 Presentation Layer (Controllers)
* **Location:** `Breadcrumbs.Api/Controllers/*`
* **Responsibilities**
  * Accept/validate HTTP input (route, query string, body)
  * Call `IMediator` with a command/query
  * Map result to `ActionResult` (200/400/404/etc.)
* **Never:** contain business rules or direct database code.

### 3.2 Application Layer (CQRS + MediatR)
* **Location:** `BreadcrumbsAPI/Applications/*`
* **Key concepts**
  * **Command** – write action (`CreateCrumbCommand`)
  * **Query** – read action (`GetCrumbsQuery`)
  * **Handler** – the class that implements `IRequestHandler<TRequest, TResult>`
  * **Validators** – FluentValidation classes ensuring each request is sane before hitting the DB
* **Benefits**
  * Pure .NET Standard; easy to unit‑test
  * No framework dependencies except MediatR & validation

### 3.3 Persistence Layer (Repositories & DbContext)
* **Location:** `BreadcrumbsAPI/Data/* & BreadcrumbsAPI/Repositories/*`
* **Components**
  * **`BreadcrumbsDbContext`** – EF Core context mapping entities to tables
  * **Repositories** – small classes that wrap DbContext queries so the Application layer doesn’t reference EF Core directly
* **Transaction flow**
  * Handlers can ask a repository to `Add`, `Update`, `Delete`, or `Find` entities
  * EF Core translates LINQ to SQL and executes against Azure SQL

---

## 4. Dependency Injection (DI)

All interfaces are wired up in **`Program.cs`**:
```csharp
builder.Services.AddScoped<ICrumbRepository, CrumbRepository>();
```
ASP.NET will therefore inject the concrete `CrumbRepository` wherever `ICrumbRepository` is required—e.g., inside a MediatR handler.

---

## 5. Adding a New Feature (Quick Recipe)

1. **Define a Command or Query** in `Application`.
2. **Implement its Handler**, injecting any needed repositories.
3. **Add a Controller endpoint** that forwards the request to MediatR.
4. **Update Repository / DbContext** only if new tables or queries are required.
5. Write tests at the handler level; controllers stay thin.

---

## 6. Key Libraries

| Purpose                | Library / NuGet            |
|------------------------|----------------------------|
| Web framework          | ASP.NET Core               |
| Dependency Injection   | Built‑in (`builder.Services`) |
| CQRS / Mediator        | **MediatR**                |
| ORM                    | **Entity Framework Core**  |
| Auth                   | ASP.NET Identity + JWT     |
| Validation             | **FluentValidation** (optional but common) |

---

## 7. FAQ for Non‑C# Folks

* **Q: Where’s the `main()` method?**  Top‑level statements in `Program.cs` _are_ the main method.
* **Q: How do handlers get instantiated?**  Through DI—ASP.NET builds them and injects dependencies.
* **Q: Why use MediatR instead of calling services directly?**  It enforces a clean CQRS pattern, centralizes cross‑cutting concerns (logging, transactions), and keeps controllers thin.
* **Q: Can a handler call another handler?**  Yes, by injecting `IMediator`, but use sparingly to avoid spaghetti.

---

Happy coding!  Ping Josh if something here still feels like magic. 🙂

---

## 🧪 Swagger Test Guide

This section walks you through making sample requests using Swagger UI.

---

## ✅ Auth Setup in Swagger

**POST** `/Users/Register`  
```json
{
  "firstName": "Test",
  "lastName": "User",
  "email": "test@email.com",
  "password": "Password1!"
}
```

**POST** `/Users/Login`  
```json
{
  "email": "test@email.com",
  "password": "Password1!"
}
```

Make sure you paste your JWT token in the 🔓 "Authorize" button like this:

```
Bearer YOUR_JWT_HERE
```

This will allow protected endpoints to resolve your `UserId` from the token claims.

---

### 🧱 Create a Group

**POST** `/Groups/AddGroup`  
```json
{
  "name": "Breadcrumbs",
  "code": "123456"
}
```

---

### 🎵 Create a Crumb

**POST** `/Crumbs/AddCrumb`  
```json
{
  "title": "First Crumb",
  "groupId": "6f1795ae-4290-4947-b6b4-08dd839a8d51", <-- ENTER AN ACTUAL GROUP ID
  "location": {
    "latitude": 0,
    "longitude": 0
  }
}
```