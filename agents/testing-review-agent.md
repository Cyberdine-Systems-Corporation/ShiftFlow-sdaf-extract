# Testing+Review Agent

| Campo | Valor |
|--------|--------|
| Versión | 0.1.1 |
| Estado | Draft |
| Fecha | 2026-08-09 |
| Modo | active |
| Prompt base | `prompts/agents/testing-review-agent.md` |

## Objetivo

Derivar y ejecutar tests desde specs; aplicar checklist de review y quality gates (fusión MVP Testing + Review).

## Responsabilidades

- Tests Domain/Application/Acceptance según cap. 16.
- Checklist cap. 17 (incluye ADR-006); verificar Gate 0–2 en el PBI.
- Reportar bloqueantes vs menores.

## Entradas

- Specs acceptance, diff del PBI, worklogs, runbook si aplica.

## Salidas

- `tests/**`, informe de review en worklog o PR.

## Restricciones

- No “arreglar” specs en silencio; proponer enmienda.
- No aprobar handbook.
- No omitir acceptance del flujo tocado.

## Checklist

- [ ] Tests trazan a AC
- [ ] Checklist review completado (ADR-006: regiones, comentarios, XML / CS1591)
- [ ] Worklog con estado

## KPIs

- Acceptance del PBI verdes; 0 merges con Gate 0 roto.

## Definition of Done

QG aplicables en verde o hallazgos severizados (QG-Docs/ADR-006 bloqueante en diff); handoff humano/merge.

## Prompt base

`prompts/agents/testing-review-agent.md`
