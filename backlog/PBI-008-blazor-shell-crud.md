# PBI-008 — Blazor shell + CRUD maestros

| Campo | Valor |
|--------|--------|
| Sprint | 1–2 |
| Prioridad | 8 |
| Specs | SPEC-PRD-002 (AC-01), SPEC-APP-001 |
| DoD | Nav + pantallas para org/dept/employee/shift type/calendario |
| Estado | Hecho (mergeado #13; Gate 2 regularizado 2026-08-12) |

## Descripción

UI Web demostrable sobre las capacidades de maestros y planificación.

## Notas de implementación

- Login + nav Administrator → Organizaciones / Calendario / Ausencias (calendario real: PBI-005; jerarquía: PBI-015).
- CRUD maestros en `/organizations` y `/organizations/{id}` vía `MastersApiClient`.
- Gate 2 Testing+Review documentado a posteriori: `worklogs/PBI-008-blazor-crud/Iteration-002-testing-review-retro.md`.
