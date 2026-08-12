---
name: adr-propose
description: Propone o enmienda ADRs en Draft con plantilla y consecuencias. Usar cuando el cambio toca límites, stack, motores o estándares.
---

# adr-propose

| Campo | Valor |
|--------|--------|
| ID | adr-propose |
| Versión | 0.1.0 |
| Estado | Approved |
| Prioridad | media |
| Fecha | 2026-08-12 |
| Norma | [handbook/10](../../handbook/10-solution-architecture.md), `architecture/decisions/` |

## Disparadores

- Nuevo ADR; enmendar ADR existente; G0.3 requiere decisión arquitectónica.

## Pasos

1. Revisar ADRs vigentes en [architecture/decisions/](../../architecture/decisions/) y README.
2. Usar [templates/adr.md](../../templates/adr.md): contexto, decisión, alternativas, consecuencias.
3. Estado **Propuesto/Draft**; no “Aceptado/Approved” sin humano.
4. Actualizar índice `architecture/decisions/README.md` si aplica.
5. Si impacta handbook (p. ej. coding standards), proponer enmienda Draft del capítulo (no auto-aprobar).
6. Worklog Architecture + `adr-propose@0.1.0`.

## Definition of Done

- [ ] ADR Draft con alternativas y consecuencias.
- [ ] Enlaces a PBI/specs si existen.
- [ ] Índice actualizado.

## Restricciones

- No código de producto salvo spike con excepción documentada.
- No contradecir handbook Approved sin enmienda explícita.

## Referencias

- [prompts/agents/architecture-agent.md](../../prompts/agents/architecture-agent.md)
- [templates/adr.md](../../templates/adr.md)
