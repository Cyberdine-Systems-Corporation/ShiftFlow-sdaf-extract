# 15 — Agent Traceability Framework

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | IV — Ingeniería IA |
| **Norma superior** | [13-ai-agent-framework.md](13-ai-agent-framework.md), [14-prompt-engineering-standard.md](14-prompt-engineering-standard.md), [09-development-workflow.md](09-development-workflow.md) |
| **Deriva hacia** | `worklogs/`, métricas (Parte VI), templates |

---

## 1. Propósito

Definir el **Agent Traceability Framework (ATF)**: cómo se registra cada iteración de agente/humano asistido para que el desarrollo sea auditable durante toda la vida del proyecto.

Sin ATF, SDAF no es demostrable.

---

## 2. Principio

> Si no está en el worklog (o en un artefacto enlazado desde él), **no forma parte del contexto oficial** del handoff.

El chat es efímero. El worklog es evidencia.

---

## 3. Organización en el repo

```text
worklogs/
  PBI-001/
    Iteration-001.md
    Iteration-002.md
  PBI-002/
    ...
```

- Un directorio por PBI (o por iniciativa si aún no hay PBI: `INIT-handbook-H7/`).
- Una iteración = un ciclo agente (o humano+agente) con objetivo cerrado o abortado.
- Plantilla en `templates/` cuando exista (Apéndice B).

---

## 4. Campos obligatorios de una iteración

| Campo | Descripción |
|--------|-------------|
| Fecha | Inicio/fin o timestamp |
| Agente | Nombre del contrato (`agents/...`) o `humano` |
| Modelo | Modelo LLM usado (si aplica) |
| Versión prompt | `prompt_id@x.y` |
| Contexto | Resumen + links a handbook/ADR |
| Especificaciones utilizadas | IDs/rutas Approved |
| Archivos leídos | Lista |
| Archivos modificados | Lista |
| Resultado | Qué se logró / qué falló |
| Tiempo | Duración estimada o real |
| Coste | Estimado si se conoce (tokens/€); si no, `N/D` |
| Observaciones | Riesgos, deuda, decisiones |
| Pruebas ejecutadas | Comandos/resultados |
| Estado | `en_curso` / `hecho` / `bloqueado` / `abortado` |
| Siguiente agente | Recomendación de handoff |

Campos adicionales permitidos; no eliminar los obligatorios.

---

## 5. Cuándo crear worklog

| Situación | ¿Worklog? |
|-----------|-----------|
| Feature/PBI con Gate 0 | Sí (G0.5) |
| Cambio de handbook/ADR/spec material | Sí (iteración de documentación) |
| Typo trivial sin decisión | No obligatorio |
| Spike con ADR de excepción | Sí |
| Ejecución que toca `src/` o `tests/` de producto | Sí |

Objetivo de métrica (Parte VI): alto % de commits de producto con worklog referenciado — no un worklog por cada pulsación de tecla.

---

## 6. Cadena de trazabilidad

```text
Backlog (PBI)
  → Specs / ADRs
  → Prompt@version
  → Worklog Iteration
  → Diff / commits
  → Tests
  → Review
```

Un auditor debe poder ir de un commit de feature al PBI y a la spec Approved sin adivinar.

---

## 7. Retención

- Los worklogs **se conservan** durante la vida del proyecto (no purgar por “limpieza” estética).
- Correcciones: añadir nota en la misma iteración o iteración `Iteration-00N-corrección`; no reescribir historia para ocultar fallos.

---

## 8. Relación con presentación MVP

La trazabilidad ATF alimenta el relato de **evolución** (slides/vídeo): no sustituye la demo de producto, pero evidencia el sistema de ingeniería.

---

## 9. Criterios de aceptación de este capítulo (H7)

- [ ] Campos obligatorios cubren el master prompt original (fecha, agente, modelo, prompt, specs, archivos, resultado, tiempo, coste, tests, estado).
- [ ] Convención de rutas `worklogs/PBI-xxx/Iteration-nnn.md` es clara.
- [ ] Chat no sustituye worklog en handoffs.
- [ ] Retención permanente queda normada.

---

## 10. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H7) |
