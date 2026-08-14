# PBI-010 — Runbook, seed opcional, freeze demo

| Campo | Valor |
|--------|--------|
| Sprint | 3 |
| Prioridad | 10 |
| Specs | SPEC-PRD-001 (C-LOC), SPEC-PRD-002 (journey), ADR-007 |
| DoD | Runbook en `docs/`; catálogo de demo opcional; etiqueta `mvp-0.1`; arranque en frío verificado |
| Estado | Catálogo hecho (mergeado #31; Gate 2 OK); freeze pendiente |

## Descripción

Documentar y congelar el camino local de evaluación.

El **seed de catálogo** (vitrina de casuísticas) está mergeado en #31. No sustituye el journey SPEC-PRD-002 (el evaluador puede seguir creando maestros). Queda pendiente el freeze: etiqueta `mvp-0.1` y verificación de arranque en frío.

## Catálogo de demo (hecho, #31)

Tras migraciones e Identity, si `Demo:SeedCatalog` es true y el provider no es SQLite, se siembran dos organizaciones ancla con datos del mes en curso.

### Criterios de aceptación

- Dado Postgres vacío y `Demo:SeedCatalog=true`, cuando arranca la Api, entonces existen `Demo — Operación` (umbral 0) y `Demo — Descanso` (660 min).
- Dado que esas orgs ya existen, cuando arranca de nuevo, entonces no se duplican filas.
- Dado el host de tests (SQLite), cuando arranca, entonces **no** se siembra el catálogo (H16).
- El seed usa factories de dominio; no `HasData` en migraciones.
- Fechas relativas al reloj local para que el calendario del mes actual las muestre.

### Pendiente (freeze)

- Etiqueta git `mvp-0.1`.
- Verificación formal de arranque en frío.

Worker Aspire de migraciones sigue fuera de alcance (ADR-007).

## Gate 0 (slice catálogo)

| # | Evidencia |
|---|-----------|
| G0.1 | SPEC-PRD-001, SPEC-PRD-002 Approved |
| G0.2 | Criterios de esta PBI |
| G0.3 | N/A (datos; ADR-007 ya separa seed del esquema) |
| G0.4 | Este archivo |
| G0.5 | `worklogs/PBI-010-demo-catalog/` |
