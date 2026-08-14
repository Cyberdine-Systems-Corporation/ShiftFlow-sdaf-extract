# PBI-010-demo-catalog / Iteration-001

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-14 |
| Agente | Domain+Application (Infrastructure Agent en stub) |
| Modelo | Cursor Grok 4.6 |
| Versión prompt | `PROMPT-AGT-DOMAPP-001@0.1.1` |
| Contexto | Catálogo de demo para ver casuísticas del MVP tras migraciones EF (ADR-007). No es freeze completo ni etiqueta `mvp-0.1`. |
| Especificaciones utilizadas | SPEC-PRD-001 (C-LOC) Approved, SPEC-PRD-002 Approved (journey; el seed complementa, no sustituye altas), ADR-007 Aceptado, handbook 16 (seed ≠ fixtures) |
| Archivos leídos | PBI-010, IdentitySeed, DatabaseInitializer, aggregates, DemoJourneyApiTests, runbook, handbook 09/16 |
| Archivos modificados | `DemoCatalogSeed.cs`, `IdentitySeed.cs`, `appsettings.json` / Development, `DemoCatalogSeedTests.cs`, PBI-010, backlog README, runbook §3.2, este worklog |
| Resultado | Catálogo idempotente en Postgres (Development); omitido en SQLite. 26 unit + 29 integration verdes. |
| Tiempo | ~0.6 h |
| Coste | N/D |
| Observaciones | Skills: `sdaf-gate0@0.1.0`, `csharp-adr006-slice@0.1.0`, `sdaf-worklog-handoff@0.1.0`. G0.3 N/A. Volumen Aspire existente sin orgs ancla recibirá el catálogo al arrancar; si ya hay datos manuales, no pisa (solo crea si falta `Demo — Operación`). Freeze `mvp-0.1` sigue pendiente. |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` — 26 unit + 29 integration OK |
| Estado | hecho |
| Siguiente agente | Testing+Review (Gate 2 / PR) o humano (commit) |

## Gate 0

| # | Evidencia |
|---|-----------|
| G0.1 | `specs/product/SPEC-PRD-001-mvp-capabilities.md`, `specs/product/SPEC-PRD-002-demo-journey.md` Approved |
| G0.2 | Criterios de catálogo en `backlog/PBI-010-runbook-demo-freeze.md` |
| G0.3 | N/A — no toca límites/stack/motores; seed separado del esquema (ADR-007) |
| G0.4 | PBI-010 |
| G0.5 | este worklog |
