# Inventario Core / Stack / Product — extracción SDAF

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-22 |
| Estado | Draft revisado (acompaña ADR-008 Propuesto v0.1.1) |
| Repo | `ShiftFlow-sdaf-extract` (laboratorio) |
| Norma | [ADR-008](../../architecture/decisions/ADR-008-extraccion-sdaf-core.md) |

## Principios confirmados en revisión

1. **Specs (norma vs contenido):** el core **debe** fijar que la verdad operativa del proyecto en desarrollo está en `specs/` del consumidor (H05/H08, `templates/spec.md`, Gate 0). El **contenido** de `specs/` de ShiftFlow es Product y no viaja al core.
2. **Stack (gobernanza vs tecnología):** el core **debe** exigir ADRs/gates ante decisiones de stack. El **stack concreto** (.NET, Blazor, PostgreSQL, ADR-001…007, skills C#/Blazor) es Stack/Product y va a pack o al producto.

## Leyenda

| Etiqueta | Destino |
|----------|---------|
| **Core** | Repo futuro `sdaf-core` (tras scrub si aplica) |
| **Stack** | Pack futuro `sdaf-stack-dotnet` (o equivalente) |
| **Product** | Solo ShiftFlow / no viaja al core |
| **Scrub** | Entra a Core solo después de quitar referencias a ShiftFlow, Blazor, ADR-006, etc. |

---

## Handbook

| Ruta | Capa | Notas |
|------|------|-------|
| `handbook/README.md` | Product (+ índice) | Sustituir por índice compositor en consumidores; en core: índice solo de método |
| `handbook/00-preface.md` | Scrub → Core | Quitar narrativa ShiftFlow; preface del método |
| `handbook/01-product-charter.md` | Product | |
| `handbook/02-product-vision.md` | Product | |
| `handbook/03-mvp-definition.md` | Product | |
| `handbook/04-product-roadmap.md` | Product | |
| `handbook/05-sdaf-framework.md` | Scrub → Core | §8 ya anticipa reutilización |
| `handbook/06-engineering-principles.md` | Scrub → Core | |
| `handbook/07-repository-organization.md` | Scrub → Core | Árbol genérico; sin asumir `src/` .NET concreto |
| `handbook/08-specification-standard.md` | Scrub → Core | Norma: `specs/` = verdad operativa del consumidor |
| `handbook/09-development-workflow.md` | Scrub → Core | Gates; genérico |
| `handbook/10-solution-architecture.md` | Product | |
| `handbook/11-ddd-and-bounded-contexts.md` | Product / Stack-opcional | DDD genérico podría ir a pack; hoy acoplado a ShiftFlow |
| `handbook/12-cqrs-vertical-slices.md` | Stack | CQRS/.NET slices |
| `handbook/13-ai-agent-framework.md` | Scrub → Core | Quitar “Frontend = Blazor”; agentes por config |
| `handbook/14-prompt-engineering-standard.md` | Core | |
| `handbook/15-agent-traceability.md` | Core | |
| `handbook/16-testing-framework.md` | Scrub → Core | Quitar acoplamientos a stack concreto |
| `handbook/17-code-review-and-quality-gates.md` | Scrub / Stack | Checklist ADR-006 → pack stack |
| `handbook/18-devops.md` | Scrub → Core | Principios; Aspire/Compose → stack |
| `handbook/19-sprint-planning-and-metrics.md` | Product / Scrub | Métricas de este MVP; generalizar solo si aporta |
| `handbook/20-security-standards.md` | Scrub → Core | Base MVP genérica; detalles Identity → stack/product |
| `handbook/A-glossary.md` | Product | Lenguaje de dominio turnos |
| `handbook/B-templates.md` | Core | |
| `handbook/CHANGELOG.md` | Product | Historial de este handbook; core tendrá el suyo |

---

## Router, agentes y prompts

| Ruta | Capa | Notas |
|------|------|-------|
| `AGENTS.md` | Scrub → Core (como `.template`) | Parametrizar agentes activos y fusiones MVP |
| `agents/README.md` | Core | |
| `agents/specification-agent.md` | Scrub → Core | |
| `agents/architecture-agent.md` | Scrub → Core | |
| `agents/testing-review-agent.md` | Scrub → Core | |
| `agents/testing-agent.md` | Core (stub) | |
| `agents/review-agent.md` | Core (stub) | |
| `agents/product-agent.md` | Core (stub) | Rol genérico |
| `agents/domain-agent.md` | Core (stub) | |
| `agents/application-agent.md` | Core (stub) | |
| `agents/domain-application-agent.md` | Scrub / Stack | Fusión MVP; salidas `*Domain*`/`*Application*` hoy .NET |
| `agents/frontend-agent.md` | Stack | Hoy Blazor |
| `agents/infrastructure-agent.md` | Stack (stub) | |
| `agents/devops-agent.md` | Core (stub) | |
| `agents/ai-agent.md` | Product / Core stub | IA de producto vs ingeniería: separar en scrub |
| `prompts/README.md` | Core | |
| `prompts/system/master-architect.md` | Scrub → Core | Hoy nombra ShiftFlow |
| `prompts/agents/*.md` | Según agente | Misma etiqueta que el contrato |
| `prompts/documentation/*.md` | Core | |
| `prompts/planning/*.md` | Scrub → Core | |
| `prompts/review/*.md` | Scrub → Core | |
| `prompts/quality/*.md` | Scrub → Core | |

---

## Skills

| Ruta | Capa | Notas |
|------|------|-------|
| `skills/README.md` | Scrub → Core | Catálogo solo de skills core; stack en pack |
| `skills/sdaf-gate0/` | Core | |
| `skills/sdaf-agent-router/` | Core | |
| `skills/sdaf-worklog-handoff/` | Core | |
| `skills/spec-draft-pbi/` | Core | |
| `skills/adr-propose/` | Core | |
| `skills/testing-review-pr/` | Scrub → Core | Quitar ADR-006 duro o mover checklist a stack |
| `skills/security-review-mvp/` | Scrub → Core | |
| `skills/devops-ci-gate/` | Core (stub) | |
| `skills/product-ia-prompt/` | Product / stub | |
| `skills/csharp-adr006-slice/` | Stack | |
| `skills/blazor-bff-slice/` | Stack | |
| `skills/aspire-local-run/` | Stack | |
| `skills/postman-contract-sync/` | Stack / genérico-API | Útil fuera de .NET si se generaliza |
| `skills/rule-engine-hr/` | Product | |

---

## Templates e IDE

| Ruta | Capa | Notas |
|------|------|-------|
| `templates/spec.md` | Core | |
| `templates/adr.md` | Core | |
| `templates/agent-contract.md` | Core | |
| `templates/prompt.md` | Core | |
| `templates/skill.md` | Core | |
| `.cursor/rules/idioma-castellano.mdc` | Core | |
| `.cursor/rules/coding-standards-csharp.mdc` | Stack | |

---

## Architecture / ADRs (ShiftFlow)

| Ruta | Capa | Notas |
|------|------|-------|
| `architecture/decisions/ADR-001` … `ADR-007` | Product / Stack | No van a `sdaf-core` |
| `architecture/decisions/ADR-008-extraccion-sdaf-core.md` | Product (lab) | Gobierna la extracción en este fork; el core documentará su propia constitución |

---

## Artefactos de producto (excluidos del core)

| Ruta | Capa |
|------|------|
| `src/`, `tests/`, `ShiftFlow.sln` | Product |
| `knowledge/` | Product |
| `specs/` (contenido ShiftFlow) | Product | La **norma** de carpeta `specs/` sí es Core (H08) |
| `backlog/` | Product |
| `postman/` | Product |
| `docs/runbook-local.md`, `docs/presentation/` | Product |
| `worklogs/` (históricos) | Product |
| `docker-compose.yml`, `.config/dotnet-tools.json` | Product / Stack |
| `README.md`, `CONTRIBUTING.md` actuales | Product | Core tendrá README propio del framework |

---

## Mínimo viable `sdaf-core@0.1.0`

Incluir en la primera publicación:

1. Handbook scrub: `00` (genérico), `05`–`09`, `13`–`15`, `B-templates`, README de método.
2. `templates/` completo.
3. Skills: `sdaf-gate0`, `sdaf-agent-router`, `sdaf-worklog-handoff`, `spec-draft-pbi`, `adr-propose`.
4. Agentes/prompts: Specification, Architecture, Testing+Review (+ stubs genéricos).
5. `AGENTS.md.template` + `sdaf.config.schema.yaml` (o equivalente).
6. `.cursor/rules/idioma-castellano.mdc`.
7. LICENSE / README del framework.

Aplazar a v0.2+: Partes V (16–18, 20) tras scrub, pack stack, template de proyecto, CLI `sdaf init`.

---

## Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-22 | Revisión: principios norma specs + gobernanza stack |
| 0.1.0 | 2026-08-22 | Inventario inicial en laboratorio |
