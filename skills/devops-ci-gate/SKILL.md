---
name: devops-ci-gate
description: Playbook stub-aware para quality gates locales y huecos de CI futura. Usar al preparar automatización o checklist pre-merge sin inventar pipeline sin ADR/PBI.
---

# devops-ci-gate

| Campo | Valor |
|--------|--------|
| ID | devops-ci-gate |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | baja |
| Nota | stub-aware (agente DevOps stub) |
| Fecha | 2026-08-12 |
| Norma | [handbook/18](../../handbook/18-devops.md), [handbook/17](../../handbook/17-code-review-and-quality-gates.md) |

## Disparadores

- “Añadir CI”, quality gate automático, checklist pre-merge reproducible.

## Pasos

1. Gate **local** mínimo (hoy): `dotnet test ShiftFlow.sln`; 0 `var` en `src`/`tests`; build Web/Api.
2. Documentar en worklog comandos y resultados.
3. Si se pide GitHub Actions/pipeline: exigir **PBI + ADR/spec Approved** (Gate 0); no inventar YAML “de regalo”.
4. Huecos futuros a registrar (no implementar sin mandato): restore/cache, test en PR, fail on CS1591, artifact Postman opcional.
5. Coordinar con `testing-review-pr` para dictamen humano.

## Definition of Done

- [ ] Checklist local ejecutado o bloqueo justificado.
- [ ] Sin CI inventada fuera de alcance Approved.
- [ ] Worklog + siguiente paso (humano / DevOps cuando deje de ser stub).

## Restricciones

- Agente DevOps es **stub** en MVP: no ampliar plataforma sin decisión humana.
- No secretos en workflows.

## Referencias

- [agents/devops-agent.md](../../agents/devops-agent.md)
- [handbook/18-devops.md](../../handbook/18-devops.md)
