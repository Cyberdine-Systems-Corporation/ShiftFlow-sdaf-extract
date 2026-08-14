# PBI-011 — Stub IA explicación de reglas

| Campo | Valor |
|--------|--------|
| Sprint | 3 |
| Prioridad | 11 |
| Specs | [SPEC-APP-005](../specs/application/SPEC-APP-005-rule-explain-stub.md) **Approved**; [SPEC-ACC-005](../specs/acceptance/SPEC-ACC-005-rule-explain-stub.md) **Approved**; SPEC-DOM-006 Approved (HR-*); ADR-003; SPEC-PRD-001 (C-RUL) |
| DoD | Explicación de violación HR-01/02/03 en castellano; stub en Infrastructure; **sin** mutar cuadrante; visible en API y calendario |
| Estado | En curso (API Domain+Application) |

## Descripción

Adaptador de infraestructura (stub determinista) que explica al planificador por qué el Rule Engine rechazó una asignación. No es un LLM obligatorio; no escribe turnos ni bypassea `Evaluate`.

El mensaje corto de `RuleViolation` (SPEC-APP-003) se mantiene; la explicación es una capa aparte (query `ExplainRule` y/o adjunto al error de `AssignShift`).

## Criterios de aceptación

- Dado un rechazo HR-01, HR-02 o HR-03, cuando el cliente pide explicación, entonces recibe título y cuerpo en castellano que identifican esa regla, y no se persiste la asignación rechazada.
- Dado un código distinto de HR-01/02/03, cuando se llama `ExplainRule`, entonces hay fallback de «no soportado» y cero escrituras.
- Dado un cliente anónimo, cuando llama `ExplainRule`, entonces se rechaza sin explicación de negocio.
- Dado un conflicto vigente, cuando se explica y se reintenta el mismo `AssignShift`, entonces el Rule Engine sigue bloqueando.
- Dado un rechazo HR-* en el calendario Web, cuando la Api responde, entonces la UI muestra la explicación y no pinta el turno.

## Fuera de alcance

- LLM real, RAG, propuestas de cuadrante, Optimization Engine.
- Playwright E2E (H16).
- Explicar invariantes estructurales `INV-*`.

## Gate 0

| # | Evidencia |
|---|-----------|
| G0.1 | SPEC-APP-005 y SPEC-ACC-005 **Approved** (2026-08-14). SPEC-DOM-006 / ADR-003 / SPEC-PRD-001 Approved. |
| G0.2 | Criterios de esta PBI + SPEC-ACC-005 ACC-S3-X01…X07. |
| G0.3 | ADR-003 Aceptado (stub de explicación; no motor de escritura). Sin ADR nuevo. |
| G0.4 | Este archivo. |
| G0.5 | `worklogs/PBI-011-ai-explain-stub/` |

Gate 0 cumplido. Siguiente: Domain+Application (puerto + stub) → Frontend (calendario) → Testing+Review.
