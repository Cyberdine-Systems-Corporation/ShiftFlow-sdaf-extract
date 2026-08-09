# PBI-003 — Organization, Department, Employee

| Campo | Valor |
|--------|--------|
| Sprint | 1 |
| Prioridad | 3 |
| Specs | SPEC-PRD-001 (C-ORG, C-DEP, C-EMP), SPEC-DOM-001/002, SPEC-APP-001, SPEC-ACC-001 (**Approved**) |
| DoD | Persistencia + API + tests de aggregates críticos |
| Estado | Implementado en rama `feat/pbi-003-maestros` (pendiente merge) |

## Descripción

Maestros de estructura organizativa y empleados asignables.

## Notas de implementación

- Aggregates `Organization`, `Department` (AR con `OrganizationId`), `Employee`.
- CQRS vía MediatR; endpoints bajo `/api/organizations|departments|employees`.
- UI Blazor CRUD diferida a PBI-008; ShiftType a PBI-004 (ACC-S1-03 completo con STT ahí).
