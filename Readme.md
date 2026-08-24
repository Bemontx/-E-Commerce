# ECommerce.OrderService

Microservicio de gestión de órdenes construido sobre **.NET** con **Clean Architecture**, **Domain-Driven Design (DDD)** y el patrón **CQRS**, parte de la plataforma distribuida `ECommerce.DistributedPlatform`.

---

## 📋 Tabla de Contenidos

- [Visión General](#visión-general)
- [Stack Tecnológico](#stack-tecnológico)
- [Arquitectura](#arquitectura)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Capas y Responsabilidades](#capas-y-responsabilidades)
- [Flujo de una Solicitud](#flujo-de-una-solicitud)
- [Modelo de Dominio](#modelo-de-dominio)
- [API Reference](#api-reference)
- [Base de Datos](#base-de-datos)
- [Testing](#testing)
- [Levantar el Proyecto](#levantar-el-proyecto)
- [Roadmap](#roadmap)

---

## Visión General

`ECommerce.OrderService` es el microservicio responsable del ciclo de vida de las órdenes dentro de la plataforma de e-commerce. Su núcleo es un **Rich Domain Model** que protege las invariantes de negocio, completamente aislado de los detalles de infraestructura.

### Capacidades actuales

| Operación | Endpoint | Estado |
|---|---|---|
| Crear orden | `POST /api/orders` | ✅ Implementado |
| Agregar ítem | `POST /api/orders/{id}/items` | ✅ Implementado |
| Consultar orden | `GET /api/orders/{id}` | 🔜 Próximo |

---

## Stack Tecnológico

| Componente | Tecnología |
|---|---|
| Framework | ASP.NET Core |
| ORM | Entity Framework Core |
| Base de datos | PostgreSQL |
| Mediador | MediatR |
| Contenedores | Docker / Docker Compose |
| Testing | xUnit + Fakes in-memory |

---

## Arquitectura

El proyecto implementa **Clean Architecture** en 4 capas con **Inversión de Dependencias (DIP)** entre Application e Infrastructure.

```
┌───────────────────────────────────────────────────────┐
│                        API                            │
│  Controllers · Swagger · Configuración HTTP           │
└─────────────────────────┬─────────────────────────────┘
                          │
                          ▼
┌───────────────────────────────────────────────────────┐
│                   APPLICATION                         │
│  Commands · Handlers · DTOs · Interfaces              │
│  "¿Qué caso de uso quiero ejecutar?"                  │
└─────────────────────────┬─────────────────────────────┘
                          │
                          ▼
┌───────────────────────────────────────────────────────┐
│                      DOMAIN                           │
│  Aggregates · Value Objects · Rules · Domain Events   │
│  "¿Qué está permitido hacer según el negocio?"        │
└─────────────────────────┬─────────────────────────────┘
                          ▲
                          │ (Inversión de Dependencias)
┌─────────────────────────┴─────────────────────────────┐
│                   INFRASTRUCTURE                      │
│  EF Core · PostgreSQL · Repositories · Migrations     │
│  "¿Cómo y dónde se persisten los datos?"              │
└───────────────────────────────────────────────────────┘
```

### Patrones implementados

- **DDD** — Aggregates, Aggregate Root, Entities, Value Objects, Domain Events, Rich Domain Model
- **Clean Architecture** — Separación de capas, aislamiento del dominio, DIP
- **CQRS** — Separación explícita del lado de escritura (Commands / Handlers)
- **Repository Pattern** — Abstracción de la persistencia mediante interfaz en Application
- **Mediator Pattern** — Desacoplamiento entre API y casos de uso vía MediatR

---

## Estructura del Proyecto

```
📦 Backend
 ┣ 📂 infrastructure
 ┃ ┗ 📂 postgres
 ┣ 📂 src
 ┃ ┗ 📂 Services
 ┃ ┃ ┗ 📂 OrderService
 ┃ ┃ ┃ ┣ 📂 ECommerce.OrderService.API
 ┃ ┃ ┃ ┃ ┣ 📂 Controllers
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 OrdersController.cs
 ┃ ┃ ┃ ┃ ┣ 📂 Properties
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 launchSettings.json
 ┃ ┃ ┃ ┃ ┣ 📜 Program.cs
 ┃ ┃ ┃ ┃ ┣ 📜 appsettings.json
 ┃ ┃ ┃ ┃ ┗ 📜 appsettings.Development.json
 ┃ ┃ ┃ ┣ 📂 ECommerce.OrderService.Application
 ┃ ┃ ┃ ┃ ┣ 📂 Commands
 ┃ ┃ ┃ ┃ ┃ ┣ 📂 AddOrderItem
 ┃ ┃ ┃ ┃ ┃ ┃ ┣ 📜 AddOrderItemCommand.cs
 ┃ ┃ ┃ ┃ ┃ ┃ ┗ 📜 AddOrderItemCommandHandler.cs
 ┃ ┃ ┃ ┃ ┃ ┗ 📂 CreateOrder
 ┃ ┃ ┃ ┃ ┃ ┃ ┣ 📜 CreateOrderCommand.cs
 ┃ ┃ ┃ ┃ ┃ ┃ ┗ 📜 CreateOrderCommandHandler.cs
 ┃ ┃ ┃ ┃ ┣ 📂 DTOs
 ┃ ┃ ┃ ┃ ┃ ┣ 📜 OrderItemResponse.cs
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 OrderResponse.cs
 ┃ ┃ ┃ ┃ ┣ 📂 Interfaces
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 IOrderRepository.cs
 ┃ ┃ ┃ ┃ ┗ 📜 ECommerce.OrderService.Application.csproj
 ┃ ┃ ┃ ┣ 📂 ECommerce.OrderService.Domain
 ┃ ┃ ┃ ┃ ┣ 📂 Aggregates
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 Order.cs
 ┃ ┃ ┃ ┃ ┣ 📂 DomainEvents
 ┃ ┃ ┃ ┃ ┃ ┣ 📜 IDomainEvent.cs
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 OrderPlacedDomainEvent.cs
 ┃ ┃ ┃ ┃ ┣ 📂 Entities
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 OrderItem.cs
 ┃ ┃ ┃ ┃ ┣ 📂 Enums
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 OrderStatus.cs
 ┃ ┃ ┃ ┃ ┣ 📂 Exceptions
 ┃ ┃ ┃ ┃ ┣ 📂 ValueObjects
 ┃ ┃ ┃ ┃ ┃ ┣ 📜 CustomerId.cs
 ┃ ┃ ┃ ┃ ┃ ┣ 📜 Money.cs
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 OrderId.cs
 ┃ ┃ ┃ ┃ ┗ 📜 ECommerce.OrderService.Domain.csproj
 ┃ ┃ ┃ ┗ 📂 ECommerce.OrderService.Infrastructure
 ┃ ┃ ┃ ┃ ┣ 📂 Persistence
 ┃ ┃ ┃ ┃ ┃ ┣ 📂 Configurations
 ┃ ┃ ┃ ┃ ┃ ┃ ┣ 📜 OrderConfiguration.cs
 ┃ ┃ ┃ ┃ ┃ ┃ ┗ 📜 OrderItemConfiguration.cs
 ┃ ┃ ┃ ┃ ┃ ┣ 📂 Migrations
 ┃ ┃ ┃ ┃ ┃ ┃ ┣ 📜 20260824004056_InitialCreate.Designer.cs
 ┃ ┃ ┃ ┃ ┃ ┃ ┣ 📜 20260824004056_InitialCreate.cs
 ┃ ┃ ┃ ┃ ┃ ┃ ┗ 📜 OrderDbContextModelSnapshot.cs
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 OrderDbContext.cs
 ┃ ┃ ┃ ┃ ┣ 📂 Repositories
 ┃ ┃ ┃ ┃ ┃ ┗ 📜 OrderRepository.cs
 ┃ ┃ ┃ ┃ ┣ 📜 DependencyInjection.cs
 ┃ ┃ ┃ ┃ ┗ 📜 ECommerce.OrderService.Infrastructure.csproj
 ┣ 📂 tests
 ┃ ┗ 📂 ECommerce.OrderService.Tests
 ┃ ┃ ┣ 📂 Application
 ┃ ┃ ┃ ┣ 📂 AddOrderItem
 ┃ ┃ ┃ ┃ ┣ 📜 AddOrderItemCommandHandlerTests.cs
 ┃ ┃ ┃ ┃ ┗ 📜 FakeOrderRepository.cs
 ┃ ┃ ┃ ┗ 📂 CreateOrder
 ┃ ┃ ┃ ┃ ┗ 📜 CreateOrderCommandHandlerTests.cs
 ┃ ┃ ┣ 📂 Domain
 ┃ ┃ ┃ ┗ 📜 OrderTests.cs
 ┃ ┃ ┗ 📜 ECommerce.OrderService.Tests.csproj
 ┣ 📜 ECommerce.DistributedPlatform.sln
 ┗ 📜 docker-compose.yml
```

---

## Capas y Responsabilidades

### 🌐 API — `ECommerce.OrderService.API`

Punto de entrada HTTP. Traduce requests a Commands y delega a MediatR. No contiene lógica de negocio.

```
HTTP Request  ──►  OrdersController  ──►  CreateOrderCommand  ──►  MediatR
```

> El controlador **nunca** instancia agregados directamente (`var order = new Order(...)` está prohibido aquí).

---

### ⚙️ Application — `ECommerce.OrderService.Application`

Coordina los casos de uso. Conoce el dominio y los contratos de infraestructura, pero no sus implementaciones.

**Responsabilidades:**
- Recibir Commands e invocar el dominio
- Persistir el estado a través de `IOrderRepository`
- Mapear entidades de dominio a DTOs de respuesta

**No decide** reglas de negocio — eso es responsabilidad exclusiva del Domain.

```
Commands/
├── CreateOrder/
│   ├── CreateOrderCommand.cs          # Datos de entrada del caso de uso
│   └── CreateOrderCommandHandler.cs   # Orquestación del flujo
└── AddOrderItem/
    ├── AddOrderItemCommand.cs
    └── AddOrderItemCommandHandler.cs

DTOs/
├── OrderResponse.cs                   # Contrato de salida para la API
└── OrderItemResponse.cs

Interfaces/
└── IOrderRepository.cs                # Contrato de persistencia (sin implementación)
```

---

### 🧠 Domain — `ECommerce.OrderService.Domain`

Núcleo de la aplicación. Contiene la lógica de negocio pura, sin dependencias externas.

#### Aggregate Root: `Order`

La entidad central que actúa como puerta de entrada al agregado y protege todas sus invariantes.

| Aspecto | Detalle |
|---|---|
| Identidad | `OrderId` (Value Object sobre `Guid`) |
| Estado inicial | `OrderStatus.Pending`, `Total = Money(0, "COP")`, `Items = []` |
| Comportamiento | `AddItem()`, `Place()`, `Cancel()` |
| Invariantes | Protegidas dentro del agregado, no en la capa de aplicación |

#### Value Objects

En lugar de primitivos sueltos, el dominio usa tipos con semántica de negocio:

| Primitivo | Value Object | Beneficio |
|---|---|---|
| `Guid` | `OrderId` / `CustomerId` | Strongly Typed IDs, imposible confundir IDs entre sí |
| `decimal + string` | `Money` | Encapsula cantidad y moneda como unidad cohesiva |

#### Domain Events

```
DomainEvents/
├── IDomainEvent.cs              # Contrato base
└── OrderPlacedDomainEvent.cs   # Evento: orden confirmada
```

> La **publicación** a un broker (RabbitMQ) y el patrón Outbox son parte del roadmap futuro.

---

### 🗄️ Infrastructure — `ECommerce.OrderService.Infrastructure`

Implementa los contratos definidos en Application. Conoce EF Core y PostgreSQL; el Dominio no sabe que existen.

```
Application ──► IOrderRepository ◄── (implementa) ── OrderRepository (Infrastructure)
```

**Mapeo ORM con Fluent API:**

```
Persistence/
├── OrderDbContext.cs
└── Configurations/
    ├── OrderConfiguration.cs       # Mapeo: Order  ──►  tabla "orders"
    └── OrderItemConfiguration.cs   # Mapeo: OrderItem ──►  tabla "order_items"
```

El registro de dependencias está encapsulado en `DependencyInjection.cs`, manteniendo `Program.cs` limpio:

```csharp
// Program.cs
builder.Services.AddInfrastructure(builder.Configuration);
```

---

## Flujo de una Solicitud

Ejemplo completo: `POST /api/orders`

```
                        CLIENTE
                           │ HTTP POST /api/orders
                           ▼
                ┌────────────────────┐
                │  OrdersController  │  Traduce request → Command
                └─────────┬──────────┘
                          │ CreateOrderCommand
                          ▼
                ┌────────────────────┐
                │      MediatR       │  Desacopla API del Handler
                └─────────┬──────────┘
                          │
                          ▼
                ┌────────────────────┐
                │ CreateOrderCommand │  Orquesta el caso de uso
                │      Handler       │
                └─────────┬──────────┘
                          │ Order.Create(customerId)
                          ▼
                ┌────────────────────┐
                │   ORDER DOMAIN     │  Aplica reglas, genera estado inicial
                │  (Aggregate Root)  │
                └─────────┬──────────┘
                          │
                          ▼
                ┌────────────────────┐
                │  IOrderRepository  │  Contrato (capa Application)
                └─────────┬──────────┘
                          │ (implementado por)
                          ▼
                ┌────────────────────┐
                │  OrderRepository   │  EF Core + DbContext
                │  (Infrastructure)  │
                └─────────┬──────────┘
                          │
                          ▼
                ┌────────────────────┐
                │  PostgreSQL (DB)   │  Persistencia final
                └────────────────────┘
                          │
                          ▼ Order ──► OrderResponse (DTO)
                        JSON al cliente
```

**Request:**
```http
POST /api/orders
Content-Type: application/json

{
  "customerId": "8d6f7c8e-1e2d-4a4c-9d8e-3e6b7b5c1a22"
}
```

**Response:**
```json
{
  "id": "210b8bca-f3d2-42f0-9694-ba38891d873f",
  "customerId": "8d6f7c8e-1e2d-4a4c-9d8e-3e6b7b5c1a22",
  "status": "Pending",
  "total": 0,
  "currency": "COP",
  "items": []
}
```

> El estado `Pending`, el total `0` y la moneda `COP` son determinados por el **Dominio**, no por el controlador.

---

## Modelo de Dominio

```
┌─────────────────────────────────────────────────┐
│                    Order                        │
│            (Aggregate Root)                     │
│                                                 │
│  + OrderId       : OrderId (Value Object)       │
│  + CustomerId    : CustomerId (Value Object)    │
│  + Status        : OrderStatus (Enum)           │
│  + Total         : Money (Value Object)         │
│  + Items         : IReadOnlyList<OrderItem>     │
│  + DomainEvents  : List<IDomainEvent>           │
│                                                 │
│  + Create(customerId) : Order                   │
│  + AddItem(...)       : void                    │
│  + Place()            : void                    │
│  + Cancel()           : void                    │
└──────────────────────┬──────────────────────────┘
                       │ contiene
                       ▼
         ┌─────────────────────────┐
         │        OrderItem        │
         │        (Entity)         │
         │                         │
         │  + ProductId : Guid     │
         │  + Quantity  : int      │
         │  + UnitPrice : Money    │
         └─────────────────────────┘
```

---

## API Reference

### `POST /api/orders`

Crea una nueva orden en estado `Pending`.

**Body:**
```json
{
  "customerId": "string (GUID)"
}
```

**Response `201 Created`:**
```json
{
  "id": "string (GUID)",
  "customerId": "string (GUID)",
  "status": "Pending",
  "total": 0,
  "currency": "COP",
  "items": []
}
```

---

### `POST /api/orders/{id}/items`

Agrega un ítem a una orden existente.

**Body:**
```json
{
  "productId": "string (GUID)",
  "quantity": "int",
  "unitPrice": "decimal"
}
```

---

## Base de Datos

### Esquema PostgreSQL

```
orders_db
├── __EFMigrationsHistory
├── orders
└── order_items
```

#### Tabla `orders`

| Columna | Tipo | Descripción |
|---|---|---|
| `Id` | `uuid` | PK — OrderId |
| `CustomerId` | `uuid` | Identificador del cliente |
| `Status` | `varchar` | Estado de la orden (`Pending`, ...) |
| `TotalAmount` | `decimal` | Monto total |
| `TotalCurrency` | `varchar` | Moneda (`COP`) |

#### Tabla `order_items`

| Columna | Tipo | Descripción |
|---|---|---|
| `Id` | `uuid` | PK |
| `OrderId` | `uuid` | FK → `orders.Id` |
| `ProductId` | `uuid` | Identificador del producto |
| `Quantity` | `int` | Cantidad |
| `UnitPriceAmount` | `decimal` | Precio unitario |
| `UnitPriceCurrency` | `varchar` | Moneda del precio |

### Migraciones

```bash
# Aplicar migraciones
dotnet ef database update --project ECommerce.OrderService.Infrastructure \
                          --startup-project ECommerce.OrderService.API
```

---

## Testing

Las pruebas están completamente aisladas de infraestructura (sin EF Core, sin HTTP, sin MediatR real).

```
tests/ECommerce.OrderService.Tests
├── Domain/
│   └── OrderTests.cs                          # Reglas e invariantes del dominio
└── Application/
    ├── CreateOrder/
    │   └── CreateOrderCommandHandlerTests.cs  # Coordinación del caso de uso
    └── AddOrderItem/
        ├── AddOrderItemCommandHandlerTests.cs
        └── FakeOrderRepository.cs             # Repositorio en memoria
```

### Estrategia

| Capa | Qué se prueba | Dependencias externas |
|---|---|---|
| Domain | Invariantes, reglas, estado del agregado | Ninguna |
| Application | Coordinación del caso de uso, mapeo a DTO | `FakeOrderRepository` (in-memory) |

```bash
# Ejecutar todas las pruebas
dotnet test
```

---

## Levantar el Proyecto

### Prerrequisitos

- .NET SDK
- Docker & Docker Compose

### Con Docker Compose

```bash
# Levantar PostgreSQL y la API
docker-compose up -d
```

### Solo la API (con PostgreSQL ya corriendo)

```bash
cd src/Services/OrderService/ECommerce.OrderService.API
dotnet run
```

La API queda disponible en `https://localhost:{puerto}/swagger`.

---

## Roadmap

- [x] `POST /api/orders` — Crear orden
- [x] `POST /api/orders/{id}/items` — Agregar ítem
- [ ] `GET /api/orders/{id}` — Consultar orden (CQRS Read side)
- [ ] Publicación de Domain Events a RabbitMQ
- [ ] Patrón Outbox para garantía de entrega
- [ ] `PUT /api/orders/{id}/place` — Confirmar orden
- [ ] `DELETE /api/orders/{id}` — Cancelar orden

### Próximo paso: CQRS Read Side

```
GET /api/orders/{id}
       │
       ▼
GetOrderQuery ──► GetOrderQueryHandler ──► IOrderRepository ──► PostgreSQL ──► OrderResponse
```