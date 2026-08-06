# ADR-001 — Stack tecnológico del MVP

| Campo | Valor |
|--------|--------|
| Estado | Aceptado |
| Fecha | 2026-08-05 |
| Decisores | Director técnico / Architecture Agent |
| Relacionado | `handbook/03-mvp-definition.md`, `handbook/10-solution-architecture.md`, `handbook/18-devops.md` |

---

## Contexto

ShiftFlow necesita un stack que permita:

- Un MVP demostrable en ~96 h de capacidad humana (1–22 ago 2026).
- Arquitectura Clean + CQRS/slices + DDD en un modular monolith.
- **Runtime local autocontenido** (sin cloud como camino de demo).
- Evolución hacia plataforma SaaS sin reescritura.

El handbook Approved ya fija dirección de producto; este ADR formaliza la elección técnica ejecutable.

---

## Decisión

Adoptar el siguiente stack para el MVP (`mvp-0.1`):

### Plataforma y host

| Tecnología | Uso |
|------------|-----|
| **.NET 10** | Runtime y lenguaje (C#) |
| **ASP.NET Core** | API HTTP y composition root |
| **Blazor Web App** | Única UI de cliente en el MVP |
| **.NET Aspire** (AppHost mínimo) | Orquestación local de app + dependencias |
| **Docker** | Contenedor(es) de infraestructura local (como mínimo PostgreSQL); Compose como alternativa/complemento documentado |

### Datos y aplicación

| Tecnología | Uso |
|------------|-----|
| **PostgreSQL** | Base de datos |
| **EF Core** | Persistencia (Infrastructure) |
| **MediatR** | Pipeline Commands/Queries (Application) |

### Calidad y observabilidad

| Tecnología | Uso |
|------------|-----|
| **xUnit** | Framework de tests |
| **FluentAssertions** | Aserciones |
| **Testcontainers** | PostgreSQL en tests de integración |
| **Serilog** | Logging |

### Auth (dirección)

Autenticación/autorización básica sobre ASP.NET Core (p. ej. Identity u opción equivalente). Detalle de proveedor en ADR de auth dedicado si hace falta.

### Contingencia de runtime

Si .NET 10 bloquea el avance de tooling de forma material, se emitirá enmienda a este ADR adoptando **.NET 9 LTS** sin cambiar el resto del stack.

---

## Alternativas consideradas

| Alternativa | Motivo de rechazo (MVP) |
|-------------|-------------------------|
| SQL Server en lugar de PostgreSQL | Postgres encaja mejor con contenedor local ligero y coste cero de licencia |
| Sin Aspire (solo Compose) | Aspire acelera DX .NET; Compose se mantiene como contingencia documentada, no como única vía |
| Minimal API-only sin Blazor | El MVP exige UI demostrable Web |
| MongoDB / almacenamiento documental | El dominio de planificación encaja en modelo relacional + invariantes |
| Stack no-.NET | Fuera de la competencia y restricciones del proyecto |

---

## Consecuencias

### Positivas

- Alineado al handbook Approved (MVP, runtime local, Clean Architecture).
- Un solo ecosistema (.NET) para API, UI Web y tests.
- Evaluadores pueden levantar el sistema con SDK + Docker, sin cuenta cloud.

### Negativas / costes

- Aspire y .NET 10 pueden tener fricción de tooling (mitigado con contingencia .NET 9 y Compose).
- Blazor Web como única UI deja MAUI fuera (ver ADR-002).
- Hay que disciplinar el composition root para no filtrar EF/Blazor al Domain.

### Diferido explícitamente (no forma parte de esta decisión como obligatorio)

- MAUI Blazor Hybrid → ADR-002  
- Redis, SignalR, OpenTelemetry completo  
- CI/CD cloud elaborado  
- Hosting cloud como camino de demo  

---

## Cumplimiento

- El skeleton de solución y el runbook local deben reflejar este stack.
- Cualquier desviación material requiere nuevo ADR o enmienda de este.
