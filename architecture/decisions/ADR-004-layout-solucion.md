# ADR-004 — Layout de la solución .NET (skeleton)

| Campo | Valor |
|--------|--------|
| Estado | Aceptado |
| Fecha | 2026-08-07 |
| Decisores | Architecture Agent / Director técnico |
| Relacionado | ADR-001, ADR-002, `handbook/10-solution-architecture.md`, PBI-001 |

---

## Contexto

PBI-001 exige una solución modular monolith compilable, orquestada en local (Aspire/Compose + PostgreSQL), sin features de negocio. El capítulo 10 deja orientativos los nombres de proyecto y permite unificar Api/Web si un ADR lo justifica.

Hay que fijar nombres, separación de hosts y dependencias antes de los slices del Sprint 1.

---

## Decisión

### Solución y proyectos

| Artefacto | Rol |
|-----------|-----|
| `ShiftFlow.sln` | Solución en la raíz del repo |
| `src/ShiftFlow.Domain` | Modelo y puertos de dominio (sin refs a infra/UI) |
| `src/ShiftFlow.Application` | Vertical slices CQRS + MediatR |
| `src/ShiftFlow.Infrastructure` | EF Core, PostgreSQL, adaptadores |
| `src/ShiftFlow.Api` | Host HTTP Minimal APIs + composition root |
| `src/ShiftFlow.Web` | Blazor Web App (única UI; ADR-002) |
| `src/ShiftFlow.ServiceDefaults` | Defaults Aspire (OpenTelemetry/health/Serilog base) |
| `src/ShiftFlow.AppHost` | Orquesta Postgres + Api + Web |
| `tests/ShiftFlow.UnitTests` | Pruebas unitarias (smoke en skeleton) |
| `tests/ShiftFlow.IntegrationTests` | Integración (Testcontainers en iteraciones posteriores) |

### Separación Api / Web

- **Api y Web son proyectos distintos** en el MVP.
- Api es el composition root de Application + Infrastructure y expone C-API.
- Web consume la API (HTTP); no referencia Infrastructure ni Domain directamente.
- No se unifica Api+Web en un solo host en el skeleton (revisable post-MVP si el DX lo exige).

### Dependencias permitidas

```text
Web → ServiceDefaults (+ HttpClient hacia Api)
Api → Application, Infrastructure, ServiceDefaults
Infrastructure → Application, Domain
Application → Domain
Domain → (ninguna de ShiftFlow)
AppHost → Api, Web (proyectos ejecutables)
```

### Runtime local

- Camino canónico: `dotnet run --project src/ShiftFlow.AppHost`.
- Contingencia: `docker-compose.yml` en la raíz para PostgreSQL si Aspire/Docker tooling falla.
- TFM: `net10.0` (ADR-001).

---

## Alternativas consideradas

| Alternativa | Motivo de rechazo (MVP) |
|-------------|-------------------------|
| Un solo host Blazor + Minimal APIs | Mezcla composition root y UI; más fricción para C-API clara y tests de API |
| Api-only + Blazor WASM puro | Más piezas y CORS; Blazor Web App interactivo es más rápido para demo |
| Sin AppHost (solo Compose) | Pierde DX Aspire acordado en ADR-001; Compose queda como contingencia |

---

## Consecuencias

### Positivas

- Boundaries Clean Architecture explícitos desde el día 1.
- C-API y C-WEB trazables a hosts distintos.
- Listo para vertical slices en Application sin reordenar proyectos.

### Negativas / costes

- Dos hosts que mantener en AppHost y runbook.
- Web debe configurar base address de Api (inyectada por Aspire en local).

### Diferido

- Auth/Identity en Api (PBI-002 / ADR de auth).
- Migraciones EF con datos de dominio (Sprint 1+).
- Testcontainers en IntegrationTests (cuando haya persistencia real).

---

## Cumplimiento

- El skeleton PBI-001 debe materializar esta estructura.
- Cambiar la separación Api/Web o el grafo de dependencias requiere enmendar este ADR.
