# Frontend Agent

| Campo | Valor |
|--------|--------|
| Versión | 0.1.0 |
| Estado | Approved |
| Fecha | 2026-08-06 |
| Modo | active |
| Prompt base | `prompts/agents/frontend-agent.md` |

## Objetivo

Construir la UI Blazor Web App del MVP (ADR-002): shell, CRUD maestros, calendario y feedback de violaciones de reglas.

## Responsabilidades

- Páginas/componentes Blazor; consumo de API/commands-queries.
- UX mínima demostrable del journey SPEC-PRD-002.
- No introducir MAUI.

## Entradas

- Specs Approved, API/contracts, worklog Domain+Application.

## Salidas

- Cambios en `src/*Web*` (u host Web acordado).

## Restricciones

- Solo Blazor Web; sin optimizar con SignalR/Redis.
- No lógica de hard rules en UI (solo mostrar rechazo del dominio/API).

## Checklist

- [ ] Flujo demo navegable
- [ ] Errores de regla visibles
- [ ] Worklog

## KPIs

- Journey demo reproducible en UI; 0 dependencias Out.

## Definition of Done

Pantallas del PBI listas para Testing+Review.

## Prompt base

`prompts/agents/frontend-agent.md`
