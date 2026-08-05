# 13 — AI Agent Framework

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | IV — Ingeniería IA |
| **Norma superior** | [05-sdaf-framework.md](05-sdaf-framework.md), [09-development-workflow.md](09-development-workflow.md) |
| **Deriva hacia** | `agents/`, `prompts/agents/`, [14-prompt-engineering-standard.md](14-prompt-engineering-standard.md), [15-agent-traceability.md](15-agent-traceability.md) |

---

## 1. Propósito

Definir el equipo de agentes de **desarrollo** de ShiftFlow (no confundir con la IA de producto dentro de Infrastructure).

Los agentes ejecutan el pipeline SDAF. No son un nivel normativo. No aprueban capítulos Approved ni saltan Gate 0.

---

## 2. Distinción crítica

| Tipo | Dónde | Rol |
|------|-------|-----|
| Agentes de ingeniería (este capítulo) | Repo: `agents/`, `prompts/` | Producir specs, ADRs, código, tests, docs |
| IA de producto | Infrastructure (stub en MVP) | Explicar reglas, asistir al planificador; **sin** mutar cuadrantes |

---

## 3. Modelo operativo MVP

**Problema:** once agentes activos con un solo supervisor humano generan thrash de contexto.

**Decisión:** **5 agentes activos** + **stubs** (contrato + prompt listos, activación bajo demanda).

### 3.1 Activos

| Agente | Objetivo | Salidas típicas |
|--------|----------|-----------------|
| Specification | Transformar knowledge → specs/acceptance | `specs/**` |
| Architecture | Boundaries, ADRs, coherencia con Parte III | `architecture/decisions/**` |
| Domain+Application | Aggregates, reglas, slices CQRS | `src/*Domain*`, `src/*Application*` |
| Frontend | Blazor Web acorde al MVP | `src/*Web*` |
| Testing+Review | Tests derivados, gates, checklist de review | `tests/**`, dictámenes de review |

### 3.2 Stubs (MVP)

Product, Infrastructure (separado), Application (separado del Domain), AI (ingeniería de prompts de producto), DevOps, Review (puro, si se desacopla de Testing).

Un stub **debe** tener:

- Contrato en `agents/<nombre>.md`
- Prompt base en `prompts/agents/<nombre>.md`
- Estado `stub` visible en el contrato

---

## 4. Contrato de agente (obligatorio)

Cada agente en `agents/` documenta:

| Sección | Contenido |
|---------|-----------|
| Objetivo | Una frase |
| Responsabilidades | Lista cerrada |
| Entradas | Specs, ADRs, prompts, artefactos previos |
| Salidas | Rutas/tipos de artefacto |
| Restricciones | Qué no puede hacer (p. ej. aprobar handbook) |
| Checklist | Antes de declarar iteración hecha |
| KPIs | Pocas métricas útiles (no vanity) |
| Definition of Done | Por iteración tipica |
| Prompt base | Ruta versionada en `prompts/` |

`AGENTS.md` en la raíz actúa como **router** (índice + cuándo invocar a quién).

---

## 5. Orquestación y handoffs

```text
Specification → Architecture → Domain+Application → Frontend
                                      ↘ Testing+Review ↗
```

Reglas:

1. El agente saliente cierra worklog con “siguiente agente” recomendado.
2. El entrante lee worklog + specs citadas; **no** depende de chat no registrado.
3. El humano puede reordenar o fusionar pasos; no puede omitir Gate 0.
4. Paralelismo solo si no hay conflicto de archivos y Gate 0 está cerrado.

---

## 6. Restricciones globales a todos los agentes

- Castellano en artefactos de ingeniería.
- Respetar handbook Approved y specs Approved.
- No inventar alcance Out del MVP.
- No marcar Approved en handbook/specs.
- No force-push ni destruir history sin orden humana explícita.
- No introducir secretos en el repo.
- Token budget: cargar solo contexto necesario (cap. 14).

---

## 7. Criterios de aceptación de este capítulo (H7)

- [ ] Queda clara la diferencia agentes de ingeniería vs IA de producto.
- [ ] 5 activos + stubs es el modelo MVP.
- [ ] El contrato mínimo de agente es auditable.
- [ ] Los handoffs exigen worklog, no solo chat.

---

## 8. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H7) |
