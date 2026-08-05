# 14 — Prompt Engineering Standard

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | IV — Ingeniería IA |
| **Norma superior** | [13-ai-agent-framework.md](13-ai-agent-framework.md), [07-repository-organization.md](07-repository-organization.md) |
| **Deriva hacia** | `prompts/`, worklogs, agentes |

---

## 1. Propósito

Los prompts son **artefactos versionados**, no texto libre improvisado como norma de trabajo.

Este capítulo define estructura, ubicación, versionado y reglas de consumo de tokens.

---

## 2. Principios

1. **Una responsabilidad** por prompt.  
2. **Contexto mínimo necesario** — enlazar rutas del repo; no pegar el handbook entero.  
3. **Reutilizar** artefactos Approved (specs, ADRs) por referencia.  
4. **Sin duplicar** la constitución: el prompt remite al handbook.  
5. **Castellano** en instrucciones y criterios.  
6. Toda ejecución relevante **cita la versión** del prompt en el worklog.

---

## 3. Árbol de la biblioteca

```text
prompts/
  system/           # p. ej. master-architect.md
  agents/           # un prompt base por agente
  planning/         # sprint, backlog, roadmap
  review/           # architecture, code, specification
  documentation/    # handbook-author, spec-author, adr-author
  quality/          # testing-strategy, quality-gates
```

El prompt maestro de gobierno vive en `prompts/system/master-architect.md` (materialización del rol Director técnico).

---

## 4. Estructura obligatoria de un prompt

| Sección | Contenido |
|---------|-----------|
| Metadatos | ID, versión, estado (`Draft`/`Approved`), agente/rol, fecha |
| Objetivo | Qué debe lograr una ejecución |
| Contexto | Enlaces a handbook/specs/ADRs (rutas), no copias masivas |
| Entradas | Artefactos que el operador debe proporcionar |
| Restricciones | Prohibiciones (gates, alcance MVP, idioma) |
| Artefactos utilizados | Lista de paths esperados |
| Resultado esperado | Qué archivos/cambios producir |
| Formato de salida | Plantilla (p. ej. ADR, spec, resumen + diff) |
| Criterios de aceptación | Cómo saber que la ejecución es válida |
| Versionado | Historial breve o puntero a CHANGELOG del prompt |

---

## 5. Versionado

- `MAJOR.MINOR` en cabecera.
- Cambio incompatible de comportamiento → MAJOR.
- El worklog registra `prompt_id@versión`.
- No editar en silencio un prompt Approved usado en iteraciones abiertas: versionar.

---

## 6. Prompts ad hoc

Permitidos solo como:

- Experimentos locales, o  
- Contenido **incrustado en el worklog** de esa iteración, con nota de que no es biblioteca.

Si un ad hoc se reutiliza → promover a `prompts/` versionado.

---

## 7. Economía de tokens

| Práctica | Norma |
|----------|--------|
| Adjuntar handbook completo | Prohibido por defecto |
| Citar capítulo/sección | Obligatorio cuando basen la decisión |
| Pegar specs enteras irrelevantes | Evitar; pasar IDs + extracto mínimo |
| Multi-agente en un solo mega-prompt | Prohibido; usar handoff |

---

## 8. Relación con Cursor / otros IDEs

- `.cursor/rules/` contiene reglas **finas** (p. ej. idioma) que apuntan al handbook.
- No duplicar Parte II–IV enteras en rules.
- El trabajo de agente debe poder reproducirse desde `prompts/` + repo, no solo desde el historial del chat.

---

## 9. Criterios de aceptación de este capítulo (H7)

- [ ] Estructura de prompt y árbol de carpetas son claros.
- [ ] Versionado y citación en worklog son obligatorios.
- [ ] Ad hoc queda acotado; biblioteca es la norma.
- [ ] Economía de tokens está normada.

---

## 10. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H7) |
