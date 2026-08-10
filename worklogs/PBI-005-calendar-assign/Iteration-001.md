# PBI-005 — Specs calendario / asignación — Iteration 001

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-10 |
| Agente | Specification |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-SPEC-001@0.1.0 |
| Contexto | Tras merge PBI-013; Gate 0 Sprint 2 para C-CAL/C-ASN (+ HR-01) |
| Especificaciones utilizadas | SPEC-PRD-001/002 Approved; ADR-003; SPEC-DOM-001…004 Approved; handbook 03/08/11/12 |
| Archivos leídos | agents/specification-agent.md, prompts/agents/specification-agent.md, handbook/08, SPEC-PRD-001/002, ADR-003, backlog PBI-005/006/013 |
| Archivos modificados | `specs/domain/SPEC-DOM-005*`, `SPEC-DOM-006*`, `SPEC-DOM-001` (cross-ref), `specs/application/SPEC-APP-003*`, `specs/acceptance/SPEC-ACC-002*`, índices specs, backlog PBI-005/006/013 + README, este worklog |
| Resultado | Draft listos para revisión humana. PBI-013 marcado Hecho (mergeado). |
| Tiempo | ~1 h |
| Coste | N/D |
| Observaciones | HR-01 acoplado a PBI-005 AssignShift; HR-02/03 y Leave fuera de SPEC-ACC-002. Ninguna spec nueva marcada Approved. |
| Pruebas ejecutadas | N/A (solo specs) |
| Estado | hecho |
| Siguiente agente | **humano** (revisar/aprobar SPEC-DOM-005/006, SPEC-APP-003, SPEC-ACC-002) → Architecture (si refinamiento) → Domain+Application (PBI-005) |

## ACs redactados (resumen)

- Dominio: ShiftAssignment + proyección CalendarMonth; invariantes INV-ASN-01…06.
- Reglas: HR-01 solape (borde no solapa); HR-02/03 diferidas a PBI-006/007 en acceptance.
- App: AssignShift, CancelShift, GetMonthCalendar; Evaluate obligatorio antes de persistir.
- Acceptance: ACC-S2-01…07 (calendario, assign OK, solape, adyacentes, tipo inactivo, cancel, anónimo).
