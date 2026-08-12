---
name: testing-review-pr
description: Ejecuta Gate 2 / review de PR con checklist H17, acceptance del PBI, tests y dictamen de merge. Usar en Testing+Review o al pedir merge recomendado.
---

# testing-review-pr

| Campo | Valor |
|--------|--------|
| ID | testing-review-pr |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | alta |
| Fecha | 2026-08-12 |
| Norma | [handbook/17](../../handbook/17-code-review-and-quality-gates.md), [handbook/09](../../handbook/09-development-workflow.md) §Gate 2 |

## Disparadores

- “Gate 2”, “review del PR”, “¿merge?”, cierre Testing+Review.

## Pasos

1. Confirmar Gate 0 del PBI (o regularización documentada).
2. Checklist H17: gobierno, dominio/arquitectura, calidad, ADR-006, tests vs acceptance.
3. Ejecutar `dotnet test ShiftFlow.sln` (o proyectos afectados); anotar conteos.
4. Contrastar acceptance del PBI (ACC-*) con tests presentes.
5. Dictamen: **merge sí / no / condicionado** con hallazgos priorizados.
6. Worklog de review (skills + prompt Testing+Review citados).
7. No auto-merge; el humano decide.

## Definition of Done

- [ ] Checklist H17 recorrido.
- [ ] Resultado de tests registrado.
- [ ] Dictamen de merge explícito.
- [ ] Worklog cerrado con siguiente agente = humano (salvo follow-up).

## Restricciones

- No aprobar handbook/specs.
- No ampliar alcance Out del MVP en el review “de paso”.

## Referencias

- [handbook/17-code-review-and-quality-gates.md](../../handbook/17-code-review-and-quality-gates.md)
- [prompts/agents/testing-review-agent.md](../../prompts/agents/testing-review-agent.md)
