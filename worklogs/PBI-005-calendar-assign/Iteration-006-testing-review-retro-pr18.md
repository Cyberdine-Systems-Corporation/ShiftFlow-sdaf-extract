# PBI-005 — Testing+Review Gate 2 retroactivo (PR #18 API)

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-12 |
| Agente | Testing+Review |
| Modelo | Cursor agent |
| Versión prompt | PROMPT-AGT-TESTREV-001@0.1.1 |
| Contexto | **Retroactivo:** PR #18 (Domain+Application + ACC-S2) mergeó sin informe Gate 2 dedicado; UI se revisó aparte en Iteration-005 / PR #19 |
| Especificaciones utilizadas | SPEC-DOM-005/006, SPEC-APP-003, SPEC-ACC-002; ADR-003/006; handbook 09/17 |
| Archivos leídos | Iteration-003-domain-application; CalendarAssignApiTests; ShiftAssignmentAndRulesTests; SchedulingEndpoints |
| Archivos modificados | este worklog; nota en backlog PBI-005 |
| Resultado | **Gate 2 regularizado para el slice API** — 0 bloqueantes |
| Tiempo | ~0.3 h |
| Coste | N/D |
| Observaciones | Los ACC_S2_01…07 y unit HR-01/INV-ASN se escribieron en el mismo PR de implementación (práctica válida); faltaba el veredicto/checklist documentado. EnsureCreated: reset volumen si DB previa. |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` (2026-08-12) → Unit 14 OK, Integration 17 OK (ACC-S2 incluidos) |
| Estado | hecho |
| Siguiente agente | — (API); UI Gate 2 = Iteration-005 / PR #19 |

## Quality gates (sobre main vigente)

| Gate | Resultado |
|------|-----------|
| QG-Build | Verde |
| QG-Unit | Verde (incl. ShiftAssignmentAndRulesTests) |
| QG-Accept | Verde ACC-S2_01…07 (SPEC-ACC-002) |
| QG-Arch | OK — RuleEngine en Domain; AssignShift Evaluate antes de persistir (ADR-003) |
| QG-Docs / ADR-006 | OK en Domain/Application/Api/Infra del slice |
| QG-Review | Checklist §3 completado a posteriori |

## Checklist review (handbook 17 §3)

### Gobierno
- [x] Gate 0 (specs Approved #17)
- [x] Sin Leave/HR-02/03 como DoD de este PR
- [x] Worklog Domain+Application + Postman en #18

### Dominio / arquitectura
- [x] Hard rules en Domain (`RuleEngine` HR-01); no solo en API
- [x] Domain sin EF; repos en Infrastructure
- [x] Slices AssignShift / CancelShift / GetMonthCalendar

### Calidad
- [x] Tests trazan a ACC-S2 / INV-ASN / HR-01
- [x] Códigos de error observables (`HR-01`, `INV-ASN-*`)
- [x] XML docs / regiones en tipos públicos del slice

### Producto
- [x] Endpoints con rol Administrator
- [x] Runbook / Postman actualizados en el PR

## Hallazgos

| Severidad | Hallazgo | Acción |
|-----------|----------|--------|
| Menor | Filtros `DateTimeOffset` en memoria (compat. SQLite tests) | Aceptable MVP; documentado en Iteration-003 |
| Menor | Gate 2 no documentado al merge #18 | **Cerrado** con este worklog |
| Info | UI Blazor = PR #19 / Iteration-005 | Fuera del alcance de #18 |

## Veredicto

**Gate 2 del PR #18 (API calendario/AssignShift) regularizado.** No requiere cambios de código ni re-merge.
