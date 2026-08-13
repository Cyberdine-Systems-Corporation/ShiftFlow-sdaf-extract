# PBI-009 — Acceptance journey — Iteration 001

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-13 |
| Agente | Testing+Review |
| Prompt | PROMPT-AGT-TESTREV-001@0.1.1 |
| Skills | `sdaf-gate0@0.1.0`, `postman-contract-sync@0.1.0` |
| Specs | SPEC-PRD-002 Approved (AC-01…AC-05); ACC-001…004 Approved |
| ADR | N/A (sin cambio de límites; ADR-001/003 vigentes) |
| Contexto | Suite acceptance del journey demo (H16: API > E2E UI) |
| Archivos modificados | `DemoJourneyApiTests`; ACC-S2-R01 aserta calendario; Postman HR-03; backlog; este worklog |
| Resultado | Journey AC-01…AC-04 + HR-03 + AC-05 status; Integration 28 / Unit 26 verdes |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` |
| Siguiente agente | Testing+Review / humano (PR) |

## Gate 0

| Ítem | Evidencia |
|------|-----------|
| G0.1 | SPEC-PRD-002 Approved |
| G0.2 | AC-01…AC-05 en SPEC-PRD-002 §4; detalle ACC-001…004 |
| G0.3 | N/A — tests/contrato HTTP, sin motor nuevo |
| G0.4 | `backlog/PBI-009-acceptance-tests-demo.md` |
| G0.5 | este worklog |

## Trazabilidad

| AC | Cobertura |
|----|-----------|
| AC-01 | Journey: alta org/dept/emp/tipo + GET listas |
| AC-02 | Journey: assign válido + visible en calendario |
| AC-03 | Journey: overlap → `HR-01` |
| AC-04 | Journey: leave + assign → `HR-02` |
| Paso 8 | Journey: umbral 660 + adyacente → `HR-03`; calendario sigue con 1 assignment |
| AC-05 / paso 9 | `AC05_api_status_sin_despliegue_cloud` (factory local, sin cloud). Arranque Aspire/Docker sigue siendo runbook (PBI-010). UI E2E Playwright: Out (H16). |
