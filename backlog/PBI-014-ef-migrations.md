# PBI-014 — Migraciones EF Core (histórico de esquema)

| Campo | Valor |
|--------|--------|
| Sprint | 2–3 |
| Prioridad | Infra transversal (habilita C-LOC sin reset rutinario) |
| Specs | SPEC-PRD-001 (C-LOC), ADR-001, ADR-004, **ADR-007** (Aceptado) |
| DoD | Baseline `InitialCreate`; `MigrateAsync` en Postgres; SQLite de tests con `EnsureCreated`; runbook actualizado; tests verdes |
| Estado | Hecho (mergeado #31; Gate 2 OK) |

## Descripción

Sustituir `EnsureCreated` como mecanismo de creación de esquema en PostgreSQL por migraciones EF Core con historial (`__EFMigrationsHistory`). El seed de Identity permanece, separado del esquema.

No cambia capacidades de producto; cierra el diferido de ADR-004 «Migraciones EF con datos de dominio».

## Criterios de aceptación

- Dado un Postgres vacío, cuando arranca la Api, entonces se aplican migraciones pendientes y existe `__EFMigrationsHistory`.
- Dado un cambio de modelo posterior, cuando se añade una migración y se arranca, entonces el esquema evoluciona sin exigir wipe (cambios aditivos).
- Dado un volumen creado previamente con `EnsureCreated`, cuando se adopta este PBI, entonces el runbook exige **un** reset de volumen (incompatibilidad EnsureCreated + Migrate).
- Dado el factory de tests (SQLite in-memory), cuando arranca el host de integración, entonces el esquema se crea con `EnsureCreated` y los tests existentes siguen verdes.
- Prohibido `EnsureCreated` contra el provider Npgsql.

## Fuera de alcance

- Worker Aspire de migraciones.
- Testcontainers Postgres.
- Reescritura del historial PBI a PBI (un solo baseline).

## Gate 0

| # | Evidencia |
|---|-----------|
| G0.1 | `specs/product/SPEC-PRD-001-mvp-capabilities.md` Approved (C-LOC). Sin spec de dominio nueva: no hay invariantes de negocio nuevas. |
| G0.2 | Criterios de esta PBI. |
| G0.3 | `architecture/decisions/ADR-007-ef-migrations.md` (Aceptado 2026-08-13). |
| G0.4 | Este archivo. |
| G0.5 | `worklogs/PBI-014-ef-migrations/` |

## Notas

- Mergeado en #31 junto al catálogo de demo (PBI-010). Gate 2: `worklogs/PBI-014-ef-migrations/Iteration-004.md`.
