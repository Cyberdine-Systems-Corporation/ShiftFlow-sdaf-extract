# AGENTS.md — Router de agentes ShiftFlow

| Campo | Valor |
|--------|--------|
| Versión | 0.1.0 |
| Estado | Draft |
| Fecha | 2026-08-06 |
| Norma | `handbook/13-ai-agent-framework.md`, `handbook/14-prompt-engineering-standard.md`, `handbook/15-agent-traceability.md` |

---

## Propósito

Índice operativo para invocar agentes de **ingeniería** (no la IA de producto).  
Antes de cualquier feature: Gate 0 (`handbook/09-development-workflow.md`).

## Modelo MVP

| Estado | Agentes |
|--------|---------|
| **Activo** | Specification, Architecture, Domain+Application, Frontend, Testing+Review |
| **Stub** | Product, Domain, Application, Infrastructure, AI, DevOps, Review |

Domain+Application y Testing+Review son **fusiones MVP**. Los stubs `domain-agent`, `application-agent`, `testing-agent` y `review-agent` existen para desacoplar post-MVP.

## Handoff canónico

```text
Specification → Architecture → Domain+Application → Frontend
                                      ↘ Testing+Review ↗
```

El saliente cierra worklog con “siguiente agente”. El entrante lee worklog + specs; no depende del chat efímero.

## Inventario

| Agente | Contrato | Prompt | Estado |
|--------|----------|--------|--------|
| Specification | [agents/specification-agent.md](agents/specification-agent.md) | [prompts/agents/specification-agent.md](prompts/agents/specification-agent.md) | active |
| Architecture | [agents/architecture-agent.md](agents/architecture-agent.md) | [prompts/agents/architecture-agent.md](prompts/agents/architecture-agent.md) | active |
| Domain+Application | [agents/domain-application-agent.md](agents/domain-application-agent.md) | [prompts/agents/domain-application-agent.md](prompts/agents/domain-application-agent.md) | active |
| Frontend | [agents/frontend-agent.md](agents/frontend-agent.md) | [prompts/agents/frontend-agent.md](prompts/agents/frontend-agent.md) | active |
| Testing+Review | [agents/testing-review-agent.md](agents/testing-review-agent.md) | [prompts/agents/testing-review-agent.md](prompts/agents/testing-review-agent.md) | active |
| Product | [agents/product-agent.md](agents/product-agent.md) | [prompts/agents/product-agent.md](prompts/agents/product-agent.md) | stub |
| Domain | [agents/domain-agent.md](agents/domain-agent.md) | [prompts/agents/domain-agent.md](prompts/agents/domain-agent.md) | stub |
| Application | [agents/application-agent.md](agents/application-agent.md) | [prompts/agents/application-agent.md](prompts/agents/application-agent.md) | stub |
| Infrastructure | [agents/infrastructure-agent.md](agents/infrastructure-agent.md) | [prompts/agents/infrastructure-agent.md](prompts/agents/infrastructure-agent.md) | stub |
| AI | [agents/ai-agent.md](agents/ai-agent.md) | [prompts/agents/ai-agent.md](prompts/agents/ai-agent.md) | stub |
| DevOps | [agents/devops-agent.md](agents/devops-agent.md) | [prompts/agents/devops-agent.md](prompts/agents/devops-agent.md) | stub |
| Review | [agents/review-agent.md](agents/review-agent.md) | [prompts/agents/review-agent.md](prompts/agents/review-agent.md) | stub |
| Testing | [agents/testing-agent.md](agents/testing-agent.md) | [prompts/agents/testing-agent.md](prompts/agents/testing-agent.md) | stub |

## Gobernanza

- Prompt de sistema: [prompts/system/master-architect.md](prompts/system/master-architect.md)
- Biblioteca: [prompts/README.md](prompts/README.md)
- Worklogs: `worklogs/` (ATF)
- Idioma: castellano (`.cursor/rules/idioma-castellano.mdc`)
- Coding standards C#: ADR-006 + `.cursor/rules/coding-standards-csharp.mdc` (`CS1591` error en `src/`)

## Restricciones globales

Ningún agente: aprueba handbook/specs por sí solo; salta Gate 0; implementa alcance Out del MVP; introduce secretos; reescribe historia git sin orden humana; entrega diff de `src/` sin cumplir ADR-006.
