# PROMPT-AGT-FE-001 — Frontend Agent

| Campo | Valor |
|--------|--------|
| ID | PROMPT-AGT-FE-001 |
| Versión | 0.1.1 |
| Estado | Approved |
| Agente / rol | Frontend |
| Fecha | 2026-08-09 |

## Objetivo

Implementar Blazor Web para el PBI (journey demo / CRUD / calendario).

## Contexto

- ADR-002; SPEC-PRD-002; `agents/frontend-agent.md`; **ADR-006** (legibilidad / XML docs en `src/`)
- API/contracts existentes

## Entradas

PBI; endpoints o handlers; wireframes implícitos del journey.

## Restricciones

Solo Blazor Web; sin MAUI/SignalR; sin reimplementar Rule Engine en UI.

## Resultado esperado

UI demostrable del alcance del PBI.

## Formato de salida

Archivos Web + notas UX + handoff Testing+Review.

## Criterios de aceptación

Alineado a SPEC-PRD-002 donde aplique; violaciones de regla visibles; cumplimiento ADR-006 en código `src/` tocado.

## Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-09 | ADR-006 |
| 0.1.0 | 2026-08-06 | Borrador inicial |
