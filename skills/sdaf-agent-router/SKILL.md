---
name: sdaf-agent-router
description: Resuelve qué agente SDAF debe actuar leyendo AGENTS.md y el contrato. Usar ante ambigüedad de rol o al iniciar una tarea multi-agente.
---

# sdaf-agent-router

| Campo | Valor |
|--------|--------|
| ID | sdaf-agent-router |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | alta |
| Fecha | 2026-08-12 |
| Norma | [AGENTS.md](../../AGENTS.md), [handbook/13](../../handbook/13-ai-agent-framework.md) |

## Disparadores

- “¿Quién hace esto?”, mezcla de specs + código + UI en un solo pedido.
- Antes de producir artefactos fuera del rol actual.

## Pasos

1. Leer [AGENTS.md](../../AGENTS.md): activos vs stubs; handoff canónico.
2. Elegir agente por tipo de salida:
   - Specs/acceptance → Specification
   - ADR/boundaries → Architecture
   - Domain/Application/API → Domain+Application
   - Blazor Web → Frontend
   - Tests/dictamen PR → Testing+Review
3. Abrir contrato `agents/<nombre>.md` + prompt `prompts/agents/<nombre>.md`.
4. Recordar fusiones MVP: Domain+Application; Testing+Review.
5. Si el trabajo cruza agentes → secuenciar handoffs; no fusionar salidas en un mega-diff sin worklog.
6. Invocar `sdaf-gate0` si hay implementación de producto.

## Definition of Done

- [ ] Agente (o secuencia) elegido y justificado.
- [ ] Contrato/prompt citados en worklog.
- [ ] Sin salidas Outside del contrato sin handoff.

## Restricciones

- Stubs: no activar alcance Out; solo contrato/prompt listos.
- No aprobar norma Approved.

## Referencias

- [AGENTS.md](../../AGENTS.md)
- [skills/README.md](../README.md)
