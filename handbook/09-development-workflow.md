# 09 — Development Workflow

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | II — SDAF |
| **Norma superior** | [05-sdaf-framework.md](05-sdaf-framework.md), [08-specification-standard.md](08-specification-standard.md), [06-engineering-principles.md](06-engineering-principles.md) |
| **Deriva hacia** | Partes III–V, `worklogs/`, `backlog/`, agentes |

---

## 1. Propósito

Definir el flujo de trabajo diario de ShiftFlow bajo SDAF: de backlog a release, con **gates** obligatorios para humanos y agentes.

Un solo camino de trabajo. No hay atajo de agente.

---

## 2. Flujo de extremo a extremo

```text
1. Knowledge disponible (si dominio)
2. Specs Draft → revisión → Approved
3. ADR si hay decisión arquitectónica / de stack / de alcance técnico
4. PBI en backlog enlazado a specs + acceptance
5. Worklog de iteración abierto
6. Tests de aceptación (esqueleto o completos) derivados de specs
7. Implementación (vertical slice)
8. Tests verdes + review
9. Worklog cerrado (resultado, archivos, estado)
10. Integración / demo local según MVP
```

Para documentación constitucional (handbook): flujo de co-creación Hn (Draft → revisión humana → Approved), no este pipeline de features.

---

## 3. Gate 0 — Pre-implementación (STOP)

Antes de escribir código de producto en `src/`, **deben** cumplirse:

| # | Requisito | Evidencia |
|---|-----------|-----------|
| G0.1 | Spec(s) **Approved** aplicables | Rutas en `specs/` |
| G0.2 | Acceptance criteria definidos | `specs/acceptance/` o sección en spec |
| G0.3 | ADR si el cambio toca límites, stack o motores | `architecture/decisions/` o N/A justificado en worklog |
| G0.4 | PBI/backlog enlazado | `backlog/` |
| G0.5 | Worklog de iteración iniciado | `worklogs/...` |

Si falta cualquiera → **STOP**. Crear el artefacto faltante; no “codear un poco para ver”.

**Excepción:** spike técnico acotado, con ADR de excepción, duración máxima y sin merge a la demo sin convertir a spec+tests.

---

## 4. Gate 1 — Durante la implementación

| # | Regla |
|---|--------|
| G1.1 | Seguir el prompt/agente versionado; registrar versión en worklog |
| G1.2 | No ampliar alcance Out del MVP sin enmienda al handbook/spec |
| G1.3 | Preferir cambios en una vertical slice (app + tests) coherente |
| G1.4 | Actualizar worklog con archivos leídos/modificados al cerrar la iteración |
| G1.5 | Commits en castellano |

---

## 5. Gate 2 — Listo para revisión / merge a la línea de demo

| # | Requisito |
|---|-----------|
| G2.1 | Acceptance tests del PBI en verde (o justificación ADR temporal) |
| G2.2 | Ninguna contradicción consciente con specs Approved |
| G2.3 | Review (humano o agente Review según Parte IV/V) con checklist |
| G2.4 | Worklog en estado cerrado / listo |
| G2.5 | Runtime local sigue arrancando según runbook (si el cambio lo afecta) |

---

## 6. Gate 3 — Cierre de MVP / release demo

Además de Gates 0–2 en el conjunto In:

| # | Requisito |
|---|-----------|
| G3.1 | DoD del capítulo 03 (producto + SDAF + local + presentación) |
| G3.2 | Flujo demo <15 min en entorno local |
| G3.3 | Slides + vídeo (§4.5 cap. 03) |
| G3.4 | Etiqueta/versión `mvp-0.1` acordada |

---

## 7. Roles en el flujo (MVP)

| Rol | Responsabilidad principal en el flujo |
|-----|----------------------------------------|
| Humano (Director técnico / PO) | Aprueba specs/handbook; decide excepciones; valida demo |
| Specification Agent | Knowledge → specs |
| Architecture Agent | ADRs, boundaries |
| Domain+Application Agent | Modelo + slices CQRS |
| Frontend Agent | Blazor Web |
| Testing+Review Agent | Tests, gates, review |
| Stubs (Product, Infra, AI, DevOps, …) | Bajo demanda |

Handoffs: el agente saliente deja worklog + artefactos; el entrante no asume contexto de chat no registrado.

---

## 8. Trabajo diario orientativo (capacidad ~5 h / 3 h)

1. Elegir PBI con Gate 0 cumplido (o cerrar Gate 0 primero).  
2. Implementar/probar en slice.  
3. Actualizar worklog y specs solo si hay hallazgo que las invalide.  
4. No abrir un segundo PBI en paralelo si Gate 0 del primero está incompleto.

Detalle de sprints: capítulo 04 y Parte VI.

---

## 9. Violaciones

Toda implementación de producto fusionada o presentada en demo **sin** Gate 0 es violación SDAF.  
Debe registrarse, revertirse o regularizarse (spec retroactiva **prohibida** como hábito; solo con ADR de excepción y plan de corrección).

---

## 10. Criterios de aceptación de este capítulo (H5)

- [ ] Gate 0 es claro y bloqueante.
- [ ] Gates 1–3 cubren implementación, merge/demo y cierre MVP.
- [ ] Hay un solo flujo para humanos y agentes.
- [ ] Las excepciones (spike) están acotadas.

---

## 11. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H5) |
