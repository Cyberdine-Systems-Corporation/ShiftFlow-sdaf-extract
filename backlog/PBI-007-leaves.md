# PBI-007 — Leaves / ausencias

| Campo | Valor |
|--------|--------|
| Sprint | 2 |
| Prioridad | 7 |
| Specs | [SPEC-DOM-007](../specs/domain/SPEC-DOM-007-leave.md), [SPEC-APP-004](../specs/application/SPEC-APP-004-leave-use-cases.md), [SPEC-ACC-003](../specs/acceptance/SPEC-ACC-003-leave-and-hr02.md) **Approved**; SPEC-PRD-001/002 (C-LEA, AC-04), SPEC-DOM-006 (HR-02) |
| DoD | Registrar/cancelar leave; ListLeaves; HR-02 bloquea AssignShift; rechazo observable |
| Estado | Ready (Gate 0 OK) |

## Descripción

Gestión básica de vacaciones/ausencias que bloquean turnos.  
Sin workflow de aprobación (Approve/Reject del knowledge = Out MVP).

## Notas

- Gate 0 cumplido (2026-08-12): specs Approved.
- Siguiente: Domain+Application (Leave + activar HR-02); UI/Frontend tras API; HR-03 puede ir en paralelo (PBI-006).
- Registrar leave no autocancela turnos ya Assigned.
