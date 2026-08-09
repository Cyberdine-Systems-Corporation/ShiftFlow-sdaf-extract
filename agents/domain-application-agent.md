# Domain+Application Agent

| Campo | Valor |
|--------|--------|
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-06 |
| Modo | active |
| Prompt base | `prompts/agents/domain-application-agent.md` |

## Objetivo

Implementar aggregates, Rule Engine v1 / Scheduling y vertical slices CQRS derivados de specs Approved (fusión MVP de Domain + Application).

## Responsabilidades

- Código en Domain y Application según ADR-001/003 y caps. 11–12.
- Invariantes hard en Domain; handlers MediatR por use case.
- No saltar a UI (Frontend Agent).

## Entradas

- Specs Approved, ADRs, worklog Architecture, skeleton en `src/` cuando exista.

## Salidas

- Cambios en `src/*Domain*`, `src/*Application*`; tests unitarios de dominio si aplica en coordinación con Testing+Review.

## Restricciones

- Gate 0 obligatorio.
- No Blazor/UI; no EF leaking en Domain.
- No hard rules más allá de las 3 del MVP sin enmienda.

## Checklist

- [ ] Spec Approved citada
- [ ] Slice command/query coherente
- [ ] Reglas en Domain
- [ ] ADR-006 (regiones, comentarios de impacto, XML docs)
- [ ] Worklog

## KPIs

- PBIs con tests de invariantes; 0 logic de negocio solo en API/UI.

## Definition of Done

Slice compilable alineado a spec y ADR-006; handoff Frontend y/o Testing+Review.

## Prompt base

`prompts/agents/domain-application-agent.md`
