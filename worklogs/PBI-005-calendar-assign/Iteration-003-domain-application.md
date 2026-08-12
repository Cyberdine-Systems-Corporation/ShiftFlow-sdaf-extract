# PBI-005 — Calendario / AssignShift — Iteration 001 (Domain+Application)

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-10 |
| Agente | Domain+Application |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-DOMAPP-001@0.1.1 |
| Contexto | Gate 0 Approved (#17); implementar C-CAL/C-ASN + HR-01 |
| Especificaciones utilizadas | SPEC-DOM-005/006, SPEC-APP-003, SPEC-ACC-002 Approved; ADR-003/006 |
| Archivos leídos | CreateShiftType, Employee, MasterDataEndpoints, DbContext, specs PBI-005 |
| Archivos modificados | Domain ShiftAssignments/Rules; Application ShiftAssignments; Infra repo/config/DI; Api SchedulingEndpoints; tests unit+integration; backlog; este worklog |
| Resultado | AssignShift / CancelShift / GetMonthCalendar + RuleEngine HR-01; API bajo `/api/...`; ACC_S2_01…07 verdes |
| Tiempo | ~1.5 h |
| Coste | N/D |
| Observaciones | UI Blazor calendario queda para Frontend. Volumen Postgres existente puede requerir reset (`EnsureCreated` no altera esquema viejo). Filtros DateTimeOffset en memoria por compat. SQLite tests. |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` (unit + integration) |
| Estado | hecho |
| Siguiente agente | Frontend (Calendar.razor + cliente API) → Testing+Review |

## Entregado

- Domain: `ShiftAssignment`, `RuleEngine` (HR-01), puerto `IShiftAssignmentRepository`
- Application: `AssignShift`, `CancelShift`, `GetMonthCalendar`
- API: `GET .../calendar`, `POST .../assignments`, `POST /api/assignments/{id}/cancel`
- Tests: unit INV-ASN/HR-01; integration ACC-S2-01…07
