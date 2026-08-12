---
name: product-ia-prompt
description: Distingue IA de producto (explicar reglas, asistir planificador) de agentes de ingeniería; sin mutar cuadrantes. Usar solo cuando el stub AI de producto esté en alcance.
---

# product-ia-prompt

| Campo | Valor |
|--------|--------|
| ID | product-ia-prompt |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | baja |
| Nota | stub-aware (IA de producto ≠ agentes de ingeniería) |
| Fecha | 2026-08-12 |
| Norma | [handbook/13](../../handbook/13-ai-agent-framework.md) §2 |

## Disparadores

- Pedidos de “IA que explique reglas / sugiera turnos” dentro del producto.
- Trabajo del stub AI agent de ingeniería de prompts de producto.

## Pasos

1. Confirmar distinción H13 §2: agentes repo (`agents/`, `prompts/`) vs IA **dentro** del producto (Infrastructure stub).
2. Alcance permitido (cuando exista): explicar reglas, asistir planificador; **sin mutar cuadrantes** ni bypassear RuleEngine.
3. No implementar features Out del MVP sin Gate 0 + specs Approved.
4. Si solo hay stub: documentar prompt Draft en `prompts/` (área a acordar) o worklog; no código de producto inventado.
5. Handoff a Architecture/Domain+Application si hace falta puerto de aplicación real.

## Definition of Done

- [ ] Límites (no mutar) explícitos en el artefacto.
- [ ] Sin confusión con Testing+Review / Domain agents.
- [ ] Worklog; siguiente = humano o Architecture.

## Restricciones

- Prohibido usar esta skill para saltar Gate 0 de features.
- Prohibido que la IA de producto escriba asignaciones saltándose HR rules.

## Referencias

- [agents/ai-agent.md](../../agents/ai-agent.md)
- [handbook/13-ai-agent-framework.md](../../handbook/13-ai-agent-framework.md)
- [handbook/03-mvp-definition.md](../../handbook/03-mvp-definition.md)
