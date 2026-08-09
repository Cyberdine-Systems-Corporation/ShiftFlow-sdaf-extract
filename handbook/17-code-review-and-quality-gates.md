# 17 — Code Review and Quality Gates

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.2 |
| **Estado** | Approved |
| **Fecha** | 2026-08-09 |
| **Parte** | V — Calidad y entrega |
| **Norma superior** | [09-development-workflow.md](09-development-workflow.md), [06-engineering-principles.md](06-engineering-principles.md), [16-testing-framework.md](16-testing-framework.md), [ADR-006](../architecture/decisions/ADR-006-coding-standards.md) |
| **Deriva hacia** | Agente Testing+Review, PRs, CI local |

---

## 1. Propósito

Unificar **code review** y **quality gates** para humanos y agentes: qué se revisa y qué bloquea integrar o declarar demo lista.

Los Gates G0–G3 del capítulo 09 siguen vigentes; este capítulo detalla el checklist de review y gates de calidad técnica.

---

## 2. Quién revisa

| Revisor | Alcance |
|---------|---------|
| Humano | Arquitectura sensible, alcance MVP, Approved |
| Testing+Review Agent | Checklist, tests, regresiones obvias, estilo (ADR-006) |
| Architecture Agent | Solo si el diff toca boundaries/ADRs |

Ningún agente aprueba enmiendas constitucionales.

---

## 3. Checklist de code review (MVP)

### 3.1 Gobierno

- [ ] Gate 0 cumplido (specs/ADR/acceptance/worklog).
- [ ] Sin alcance Out del MVP.
- [ ] Worklog actualizado y prompt versionado citado.

### 3.2 Dominio y arquitectura

- [ ] Reglas de negocio en Domain, no solo en UI/API.
- [ ] Dependencias Clean respetadas (Domain sin infra).
- [ ] Slice CQRS coherente (command no consulta mutando).

### 3.3 Calidad

- [ ] Tests nuevos/actualizados alineados a acceptance.
- [ ] Nombres en lenguaje ubicuo.
- [ ] Sin secretos ni connection strings en claro en el repo.
- [ ] Logging útil sin ruido excesivo.
- [ ] **Legibilidad (ADR-006):** regiones conceptuales en tipos no triviales tocados.
- [ ] **Comentarios (ADR-006):** solo en lógica de alto impacto / no obvia; castellano.
- [ ] **XML docs (ADR-006):** `<summary>` (y params/returns/exception si aportan) en API pública/`internal` del diff; build sin `CS1591`.

### 3.4 Producto

- [ ] Auth/roles no rotos si el diff los toca.
- [ ] Runbook local sigue siendo válido si cambia composición.

---

## 4. Quality gates técnicos

| Gate | Condición | Bloquea |
|------|-----------|---------|
| QG-Build | Solución compila (incluye `CS1591` como error en `src/`, ADR-006) | Merge / demo |
| QG-Unit | Tests unitarios Domain (+ app relevantes) verdes | Merge / demo |
| QG-Accept | Acceptance del PBI/flujo tocado verdes | Merge a línea de demo |
| QG-Arch | Sin violaciones obvias de dependencia (manual o test de arquitectura) | Merge si hay infracción nueva |
| QG-Docs | Diff de `src/` cumple XML docs + regiones/comentarios según ADR-006 | Merge |
| QG-Review | Checklist §3 completado | Merge |

CI cloud elaborado es **opcional** en MVP; los gates deben poder ejecutarse **en local**.

---

## 5. Severidad de hallazgos

| Severidad | Acción |
|-----------|--------|
| Bloqueante | No merge |
| Mayor | Corregir o ADR de excepción fechado |
| Menor | Puede ir a deuda registrada en worklog/backlog |

Hallazgos de ADR-006 (falta XML, regiones omitidas en tipo no trivial, comentarios narrativos inútiles en exceso) son **bloqueantes** en el diff del PR.

---

## 6. Criterios de aceptación de este capítulo (H8)

- [ ] Checklist usable por agente y humano.
- [ ] QGs ejecutables en local.
- [ ] Relación con Gates G0–G3 queda clara.

---

## 7. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.2 | 2026-08-09 | ADR-006: regiones, comentarios, XML docs y QG-Docs |
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H8) |
