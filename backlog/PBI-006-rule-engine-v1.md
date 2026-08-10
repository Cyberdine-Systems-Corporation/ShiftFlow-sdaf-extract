# PBI-006 — Rule Engine v1

| Campo | Valor |
|--------|--------|
| Sprint | 2 |
| Prioridad | 6 |
| Specs | [SPEC-DOM-006](../specs/domain/SPEC-DOM-006-rule-engine-v1.md) **Approved**; SPEC-PRD-001/002 (C-RUL), ADR-003 |
| DoD | Tres hard rules con tests; rechazo observable en API/UI |
| Estado | Ready parcial (SPEC-DOM-006 Approved; HR-02 requiere Leave / PBI-007) |

## Descripción

Evaluar solape, leave y descanso mínimo antes de persistir asignaciones.  
HR-01 (solape) se ejercita ya en PBI-005; este PBI completa el motor (HR-02/HR-03) y su batería de tests.

## Notas

- Depende de modelo Leave (PBI-007) para HR-02 completo.
- Dominio de reglas Approved; implementación de HR-02/03 tras spec/modelo Leave.
