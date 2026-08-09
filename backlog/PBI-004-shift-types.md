# PBI-004 — Shift types

| Campo | Valor |
|--------|--------|
| Sprint | 1 |
| Prioridad | 4 |
| Specs | SPEC-PRD-001 (C-STT), SPEC-DOM-003, SPEC-APP-001, SPEC-ACC-001 (**Approved**) |
| DoD | Catálogo de tipos de turno usable en asignación |
| Estado | Implementado en rama `feat/pbi-004-shift-types` (pendiente merge) |

## Descripción

Gestionar tipos de turno del catálogo.

## Notas de implementación

- Aggregate `ShiftType` por Organization; Name/Code únicos; End > Start sin overnight (INV-STT-04).
- API: `POST/GET /api/organizations/{id}/shift-types`, `PUT /api/shift-types/{id}`, `PUT .../active`.
- UI Blazor diferida a PBI-008.
