# Infrastructure Agent

| Campo | Valor |
|--------|--------|
| Versión | 0.1.0 |
| Estado | Approved |
| Fecha | 2026-08-06 |
| Modo | stub |
| Prompt base | `prompts/agents/infrastructure-agent.md` |

## Objetivo

EF, Postgres, adapters (IA stub, email, etc.) bajo demanda.

## Responsabilidades

- Activar solo bajo demanda humana explícita.
- No sustituir al agente activo fusionado del MVP salvo desacoplamiento acordado.

## Entradas

Worklog + specs/ADRs del encargo puntual.

## Salidas

Artefactos de su especialidad (ver prompt).

## Restricciones

Mismas globales que `AGENTS.md`; modo stub = no invocar por defecto en el handoff canónico.

## Checklist

- [ ] Encargo explícito
- [ ] Worklog
- [ ] No solapar con activo sin coordinación

## KPIs

Uso justificado; 0 thrash por activación espontánea.

## Definition of Done

Entrega del encargo puntual + handoff documentado.

## Prompt base

`prompts/agents/infrastructure-agent.md`
