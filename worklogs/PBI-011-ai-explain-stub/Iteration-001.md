# PBI-011-ai-explain-stub / Iteration-001

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-14 |
| Agente | Specification |
| Modelo | Cursor Grok 4.6 |
| Versión prompt | `PROMPT-AGT-SPEC-001@0.1.0` |
| Skills | `spec-draft-pbi@0.1.0`, `sdaf-gate0@0.1.0`, `sdaf-worklog-handoff@0.1.0`, `sdaf-agent-router@0.1.0` |
| Contexto | Sprint 3: contrato + acceptance del stub de explicación de reglas (PBI-011). Encargo humano: Draft sin auto-aprobar. |
| Especificaciones utilizadas | SPEC-PRD-001/002 Approved; SPEC-DOM-006 Approved; SPEC-APP-003 Approved; ADR-003 Aceptado; handbook 03/08/10/13/16 |
| Archivos leídos | `agents/specification-agent.md`, `prompts/agents/specification-agent.md`, handbook 08, SPEC-APP-003/004, SPEC-ACC-002/004, SPEC-DOM-006, PBI-011, ADR-003, `AssignShift` / `RuleEngine` (solo contexto) |
| Archivos modificados | `specs/application/SPEC-APP-005-rule-explain-stub.md`, `specs/acceptance/SPEC-ACC-005-rule-explain-stub.md`, índices APP/ACC, `backlog/PBI-011-ai-explain-stub.md`, `backlog/README.md`, este worklog |
| Resultado | Draft listos para revisión humana. Ninguna spec nueva marcada Approved. Gate 0 de `src/` en STOP hasta aprobación. |
| Tiempo | ~0.5 h |
| Coste | N/D |
| Observaciones | Sin SPEC-DOM nueva (RuleViolation ya existe). Sin ADR nuevo (ADR-003 cubre el límite). Stub determinista; LLM real Out. Query `ExplainRule` + adjunto opcional al error de AssignShift. |
| Pruebas ejecutadas | N/A (solo specs) |
| Estado | hecho |
| Siguiente agente | **humano** (revisar/aprobar SPEC-APP-005 y SPEC-ACC-005) → Domain+Application (puerto + stub) → Frontend (calendario) → Testing+Review |

## Gate 0 (hacia implementación)

| # | Resultado |
|---|-----------|
| G0.1 | STOP — APP-005 / ACC-005 Draft. DOM-006 / ADR-003 / PRD-001 Approved. |
| G0.2 | ACC-S3-X01…X07 redactados (testeables). |
| G0.3 | OK — ADR-003; N/A ADR nuevo. |
| G0.4 | PBI-011 enlazado. |
| G0.5 | este worklog |

## ACs redactados (resumen)

- App: puerto `IRuleExplanation`; query `ExplainRule`; cero escrituras; AssignShift puede adjuntar explicación; UI muestra Body.
- Acceptance: X01–X03 HR-01/02/03; X04 código desconocido; X05 anónimo; X06 no bypass; X07 UI calendario.
