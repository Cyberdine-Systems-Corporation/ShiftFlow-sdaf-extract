# PBI-005 — Calendario y asignación manual

| Campo | Valor |
|--------|--------|
| Sprint | 2 |
| Prioridad | 5 |
| Specs | [SPEC-DOM-005](../specs/domain/SPEC-DOM-005-shift-assignment-calendar.md), [SPEC-DOM-006](../specs/domain/SPEC-DOM-006-rule-engine-v1.md) (HR-01), [SPEC-APP-003](../specs/application/SPEC-APP-003-calendar-assign-use-cases.md), [SPEC-ACC-002](../specs/acceptance/SPEC-ACC-002-calendar-assign.md) — **Approved**; SPEC-PRD-001/002 (C-CAL, C-ASN), ADR-003 |
| DoD | Calendario mensual + AssignShift vía Scheduling Engine (+ rechazo solape HR-01) |
| Estado | Hecho (mergeado #18 API + #19 UI; Gate 2 documentado) |

## Descripción

Vista mensual y asignación manual de turnos (sin optimización).  
`AssignShift` invoca Rule Engine antes de persistir; en este PBI el mínimo de reglas es **HR-01** (no solape). HR-02/HR-03 → PBI-006 (+ Leave en PBI-007).

## Notas

- Gate 0 cumplido. Domain+Application mergeado (#18); **Gate 2 API regularizado** en `worklogs/PBI-005-calendar-assign/Iteration-006-testing-review-retro-pr18.md`.
- Frontend UI mergeado (#19); Testing+Review UI en `worklogs/PBI-005-calendar-assign/Iteration-005-testing-review.md`.
- Si el volumen Postgres local es anterior a esta tabla, resetear volumen Aspire/Compose y volver a arrancar.
