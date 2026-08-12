# Contribuir a ShiftFlow

Gracias por contribuir. Este repo combina **producto** (MVP demostrable) y **método** (SDAF). La norma vive en el handbook; este archivo resume el flujo operativo.

## Principios

1. El **handbook Approved** manda; las **specs Approved** mandan sobre el código.
2. **Gate 0** antes de implementar producto ([handbook/09](handbook/09-development-workflow.md)): specs + acceptance + ADR si aplica + PBI + worklog.
3. Artefactos de ingeniería en **castellano** (commits, PRs, issues, worklogs, specs).
4. Código C# en `src/` / `tests/`: [ADR-006](architecture/decisions/ADR-006-coding-standards.md) (tipado explícito sin `var`, XML docs/`CS1591`, regiones cuando apliquen).

## Flujo recomendado

1. Elegir un PBI en [`backlog/`](backlog/) con Gate 0 cerrado (o cerrarlo primero).
2. Resolver rol con [AGENTS.md](AGENTS.md); usar playbooks en [`skills/`](skills/README.md) (`sdaf-gate0`, `sdaf-worklog-handoff`, `csharp-adr006-slice`, `testing-review-pr`, …).
3. Registrar la iteración en [`worklogs/`](worklogs/) (ATF). Citar `prompt_id@version` y, si aplica, `skill-id@version`.
4. Implementar / documentar según el agente; tests alineados a acceptance.
5. Verificar: `dotnet test ShiftFlow.sln`.
6. Abrir PR en castellano. Gate 2 / checklist: [handbook/17](handbook/17-code-review-and-quality-gates.md).

Handoff canónico:

```text
Specification → Architecture → Domain+Application → Frontend
                                      ↘ Testing+Review ↗
```

## Commits y pull requests

- Mensajes claros en castellano (1–2 frases; prefijos `docs:` / `feat:` / `fix:` / `chore:` opcionales).
- Un tema coherente por PR cuando sea posible.
- No incluir secretos (`.env`, passwords, user-secrets).
- No force-push a `main` ni reescribir historia sin acuerdo explícito.

## Qué no hacer

- Saltar Gate 0 o fusionar producto sin specs/acceptance.
- Ampliar alcance **Out** del MVP ([handbook/03](handbook/03-mvp-definition.md)) sin enmienda.
- Marcar handbook/specs/ADR como Approved sin decisión humana.
- Sustituir el worklog por el chat efímero.

## Entorno local

Runbook: [docs/runbook-local.md](docs/runbook-local.md).  
Colección HTTP: [postman/README.md](postman/README.md).

## Seguridad

Vulnerabilidades: [SECURITY.md](SECURITY.md). No publicar PoC explotables en issues abiertos.

## Licencia

Al contribuir, aceptas que tus aportaciones se licencian bajo la [MIT](LICENSE) (Copyright © 2026 mortiz-iadev), salvo acuerdo distinto y escrito.
