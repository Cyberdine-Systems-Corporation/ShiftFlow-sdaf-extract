# Skills — playbooks SDAF (tool-agnostic)

| Campo | Valor |
|--------|--------|
| Versión | 0.1.0 |
| Estado | Approved |
| Fecha | 2026-08-13 |
| Norma | `handbook/07`, `handbook/13` §6, `handbook/14`, `AGENTS.md` |

## Propósito

Playbooks operativos reutilizables. Viven en **`skills/`** del repo. **No** dependen de Cursor ni de `.cursor/skills/`.

Cualquier humano, agente o IDE las consume **por ruta**. En worklogs citar `skill-id@version`.

## Capas

| Capa | Rol |
|------|-----|
| Handbook / specs / ADR | Norma |
| `agents/` + `prompts/` | Quién / rol |
| `skills/` | Cómo (flujo) |
| `.cursor/rules/` | Restricciones IDE finas |

## Consumo

1. Identificar disparador → abrir `skills/<id>/SKILL.md`.
2. Seguir pasos; enlazar normas (no pegar handbook).
3. Cerrar con DoD; registrar skill en worklog.
4. Gate 0 manda antes de implementar producto ([handbook/09](../handbook/09-development-workflow.md)).

## Handoff y skills

```text
Specification → Architecture → Domain+Application → Frontend
                                      ↘ Testing+Review ↗
```

| Fase | Skills habituales |
|------|-------------------|
| Pre-implementación | `sdaf-gate0`, `sdaf-agent-router`, `spec-draft-pbi`, `adr-propose` |
| Implementación | `csharp-adr006-slice`, `blazor-bff-slice`, `rule-engine-hr`, `postman-contract-sync` |
| Runtime local | `aspire-local-run` |
| Cierre / review | `sdaf-worklog-handoff`, `testing-review-pr` |
| Stubs / futuro | `devops-ci-gate`, `product-ia-prompt` |

## Catálogo

### Prioridad alta

| ID | Disparador | Agente típico | Ruta |
|----|------------|---------------|------|
| `sdaf-gate0` | Empezar PBI / implementar | Todos | [sdaf-gate0/SKILL.md](sdaf-gate0/SKILL.md) |
| `sdaf-worklog-handoff` | Cerrar iteración / handoff | Todos | [sdaf-worklog-handoff/SKILL.md](sdaf-worklog-handoff/SKILL.md) |
| `sdaf-agent-router` | Ambigüedad de rol | Todos | [sdaf-agent-router/SKILL.md](sdaf-agent-router/SKILL.md) |
| `csharp-adr006-slice` | Diff `src/`/`tests/` | Domain+Application, Frontend, Infra | [csharp-adr006-slice/SKILL.md](csharp-adr006-slice/SKILL.md) |
| `testing-review-pr` | Gate 2 / review PR | Testing+Review | [testing-review-pr/SKILL.md](testing-review-pr/SKILL.md) |

### Prioridad media

| ID | Disparador | Agente típico | Ruta |
|----|------------|---------------|------|
| `spec-draft-pbi` | Draft de specs | Specification | [spec-draft-pbi/SKILL.md](spec-draft-pbi/SKILL.md) |
| `adr-propose` | Nuevo/enmienda ADR | Architecture | [adr-propose/SKILL.md](adr-propose/SKILL.md) |
| `blazor-bff-slice` | UI Blazor + BFF | Frontend | [blazor-bff-slice/SKILL.md](blazor-bff-slice/SKILL.md) |
| `aspire-local-run` | Arrancar demo local | Architecture / DevOps | [aspire-local-run/SKILL.md](aspire-local-run/SKILL.md) |
| `postman-contract-sync` | Cambio de API | Domain+Application / Testing | [postman-contract-sync/SKILL.md](postman-contract-sync/SKILL.md) |

### Prioridad baja

| ID | Disparador | Agente típico | Ruta |
|----|------------|---------------|------|
| `rule-engine-hr` | HR / Leave / calendario | Domain+Application | [rule-engine-hr/SKILL.md](rule-engine-hr/SKILL.md) |
| `devops-ci-gate` | CI / quality gate auto | DevOps (stub) | [devops-ci-gate/SKILL.md](devops-ci-gate/SKILL.md) |
| `product-ia-prompt` | IA de producto | AI (stub) | [product-ia-prompt/SKILL.md](product-ia-prompt/SKILL.md) |

## Restricciones globales

- No aprobar handbook/specs/ADR.
- No saltar Gate 0.
- No secretos en el repo.
- Castellano en artefactos de ingeniería.
- Plantilla: [`templates/skill.md`](../templates/skill.md).
