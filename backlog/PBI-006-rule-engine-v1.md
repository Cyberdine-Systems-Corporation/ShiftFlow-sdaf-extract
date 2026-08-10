# PBI-006 — Rule Engine v1

| Campo | Valor |
|--------|--------|
| Sprint | 2 |
| Prioridad | 6 |
| Specs | [SPEC-DOM-006](../specs/domain/SPEC-DOM-006-rule-engine-v1.md) Draft; SPEC-PRD-001/002 (C-RUL), ADR-003 |
| DoD | Tres hard rules con tests; rechazo observable en API/UI |
| Estado | Spec Draft (HR-01 compartido con PBI-005; HR-02/03 + Leave) |

## Descripción

Evaluar solape, leave y descanso mínimo antes de persistir asignaciones.  
HR-01 (solape) se ejercita ya en PBI-005; este PBI completa el motor (HR-02/HR-03) y su batería de tests.

## Notas

- Depende de modelo Leave (PBI-007) para HR-02 completo.
- No implementar código hasta SPEC-DOM-006 **Approved**.
