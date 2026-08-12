# PBI-007 — Specs Leave / HR-02 — Iteration 001

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-12 |
| Agente | Specification |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-SPEC-001@0.1.0 |
| Contexto | Tras cierre PBI-005 + Gate 2 retroactivo; Gate 0 para C-LEA / AC-04 / HR-02 |
| Especificaciones utilizadas | SPEC-PRD-001/002 Approved; SPEC-DOM-005/006 Approved; handbook 03/08/11/12; knowledge plan (Approve/Reject filtrado Out) |
| Archivos leídos | agents/specification-agent.md, prompts/agents/specification-agent.md, handbook/08, SPEC-PRD-001/002, SPEC-DOM-005/006, SPEC-APP-003, backlog PBI-006/007 |
| Archivos modificados | `specs/domain/SPEC-DOM-007*`, `specs/application/SPEC-APP-004*`, `specs/acceptance/SPEC-ACC-003*`, índices specs, cross-ref SPEC-DOM-001/006 y SPEC-APP-003, backlog PBI-006/007 + README, este worklog |
| Resultado | Draft listos para revisión humana. Ninguna spec nueva marcada Approved. |
| Tiempo | ~0.75 h |
| Coste | N/D |
| Observaciones | Leave básico Active/Cancelled; sin Approve/Reject. Cobertura por fechas civiles inclusive. No autocancela turnos ya Assigned. HR-03 fuera de SPEC-ACC-003. |
| Pruebas ejecutadas | N/A (solo specs) |
| Estado | hecho |
| Siguiente agente | **humano** (revisar/aprobar SPEC-DOM-007, SPEC-APP-004, SPEC-ACC-003) → Domain+Application (PBI-007 + HR-02; HR-03 en PBI-006) |

## ACs redactados (resumen)

- Dominio: aggregate Leave; INV-LEA-01…05; LeaveCoverage para HR-02; proyección opcional en CalendarMonth.
- App: RegisterLeave, CancelLeave, ListLeaves; AssignShift debe emitir HR-02.
- Acceptance: ACC-S2-L01…L07 (registro, bloqueo, fuera de rango, cancel, EndOn inválido, anónimo, HR-01≠HR-02).
