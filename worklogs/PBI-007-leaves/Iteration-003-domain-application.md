# PBI-007 — Leave / HR-02 — Iteration 003 (Domain+Application)

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-12 |
| Agente | Domain+Application |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-DOMAPP-001@0.1.1 |
| Contexto | Gate 0 Approved (#21); implementar C-LEA + HR-02 |
| Especificaciones utilizadas | SPEC-DOM-007, SPEC-APP-004, SPEC-ACC-003, SPEC-DOM-006 Approved; ADR-003/006 |
| Archivos leídos | AssignShift, RuleEngine, ShiftAssignment, SchedulingEndpoints, CalendarAssignApiTests |
| Archivos modificados | Domain Leaves + RuleEngine HR-02; Application Leaves + AssignShift; Infra Leave config/repo/DbSet/DI; Api LeaveEndpoints; tests unit+integration; Postman; backlog; este worklog |
| Resultado | RegisterLeave / CancelLeave / ListLeaves + HR-02 en AssignShift; ACC-S2-L01…07 verdes |
| Tiempo | ~1.5 h |
| Coste | N/D |
| Observaciones | UI Blazor de leaves queda para Frontend. HR-03 no incluido. Volumen Postgres previo puede requerir reset (EnsureCreated). Leaves aún no se proyectan en GetMonthCalendar (SPEC lo permite como opcional). |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` → Unit 21 OK, Integration 24 OK |
| Estado | hecho |
| Siguiente agente | Frontend (UI registro/listado leave + mensaje HR-02) → Testing+Review |

## Entregado

- Domain: `Leave`, `LeaveStatus`, `ILeaveRepository`; `RuleEngine.Evaluate` con `activeLeaves` (HR-02)
- Application: `RegisterLeave`, `CancelLeave`, `ListLeaves`; `AssignShift` carga leaves activos
- API: `POST/GET .../leaves`, `POST /api/leaves/{id}/cancel`
- Tests: unit INV-LEA/HR-02; integration ACC-S2-L01…07
