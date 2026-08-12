# PBI-006 — Rule Engine v1

| Campo | Valor |
|--------|--------|
| Sprint | 2 |
| Prioridad | 6 |
| Specs | [SPEC-DOM-006](../specs/domain/SPEC-DOM-006-rule-engine-v1.md) **Approved**; Leave [SPEC-DOM-007](../specs/domain/SPEC-DOM-007-leave.md) **Approved**; SPEC-PRD-001/002 (C-RUL), ADR-003 |
| DoD | Tres hard rules con tests; rechazo observable en API/UI |
| Estado | Ready (HR-01 hecho; HR-02/03 implementables — Leave Gate 0 OK) |

## Descripción

Evaluar solape, leave y descanso mínimo antes de persistir asignaciones.  
HR-01 (solape) se ejercita ya en PBI-005; este PBI completa el motor (HR-02/HR-03) y su batería de tests.

## Notas

- Modelo Leave Approved (PBI-007): HR-02 puede implementarse junto con RegisterLeave.
- HR-03: umbral configurable por Organization; puede ir en el mismo slice de Domain o en PR aparte.
