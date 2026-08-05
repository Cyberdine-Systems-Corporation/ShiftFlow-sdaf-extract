# 08 — Specification Standard

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | II — SDAF |
| **Norma superior** | [05-sdaf-framework.md](05-sdaf-framework.md), [07-repository-organization.md](07-repository-organization.md) |
| **Deriva hacia** | `specs/`, [09-development-workflow.md](09-development-workflow.md), acceptance tests, backlog |

---

## 1. Propósito

Definir qué es una especificación válida en ShiftFlow: formato mínimo, tipos, estados, trazabilidad y relación con knowledge, ADRs y código.

Sin este estándar, “tener una spec” es ambiguo y el gate SDAF no es auditable.

---

## 2. Definición

Una **especificación** es un artefacto versionado en `specs/` que describe de forma **testeable** qué debe cumplirse, con criterios de aceptación explícitos y referencias a knowledge/handbook/ADRs cuando aplique.

No es:

- Un ensayo de diseño sin criterios verificables.
- Un ticket de backlog sin detalle (el backlog **apunta** a specs).
- Código comentado o un PR description como única fuente.

---

## 3. Tipos y ubicación

| Tipo | Carpeta | Contenido típico |
|------|---------|------------------|
| Producto | `specs/product/` | Epics/capabilities alineadas al MVP, journeys, NFRs de producto |
| Dominio | `specs/domain/` | Glossary, modelo, reglas hard/soft, cálculos, invariantes |
| Aplicación | `specs/application/` | Casos de uso, comandos/consultas, contratos API a nivel app |
| Aceptación | `specs/acceptance/` | Escenarios Given/When/Then mapeables a tests automatizados |

Una capacidad puede tener varios archivos enlazados; no duplicar el mismo criterio en tres sitios: **una fuente canónica** y referencias.

---

## 4. Cabecera obligatoria

Toda spec debe incluir:

| Campo | Descripción |
|--------|-------------|
| Título | Nombre estable |
| ID | Identificador único (p. ej. `SPEC-DOM-001`) |
| Versión | Semver o `MAJOR.MINOR` |
| Estado | `Draft` / `Approved` / `Deprecated` |
| Fecha | Última actualización |
| Fuentes | Rutas en `knowledge/` y capítulos de handbook |
| ADRs relacionados | Si aplica |
| PBIs / backlog | IDs vinculados |
| Derivados | Tests, slices, worklogs esperados |

Solo specs **Approved** autorizan implementación de producto (salvo spike explícito con ADR de excepción y fecha de caducidad).

---

## 5. Contenido mínimo por tipo

### 5.1 Dominio — Glossary / Ubiquitous Language

- Término, definición, sinónimos prohibidos, contexto.
- Separar jerga de sector (ejemplo) de conceptos de plataforma.

### 5.2 Dominio — Model / Rules

- Aggregates, entities, value objects (a nivel conceptual).
- Invariantes.
- Reglas **Hard** (bloquean) vs **Soft** (penalizan/ordenan) — nunca mezclar sin etiquetar.
- Ejemplos y contraejemplos.

### 5.3 Application — Use cases

- Actor, precondiciones, flujo principal, alternativos, postcondiciones.
- Comandos/queries afectados (nombre), no código.

### 5.4 Acceptance

Cada criterio debe ser:

- **Observables** (dado un estado, una acción, un resultado).
- **Independientes** en lo razonable.
- **Trazables** a una regla o use case.

Formato preferido:

```text
Dado [contexto]
Cuando [acción]
Entonces [resultado observable]
```

---

## 6. Pipeline de elaboración

```text
knowledge/raw|curated
    → specs/domain (glossary → model → rules)
    → specs/application (use cases)
    → specs/acceptance
    → tests en tests/ (derivados)
    → implementación en src/
```

Reglas:

1. No saltar de knowledge a código.
2. Si una acceptance contradice una regla de dominio, se corrige antes de codear.
3. Las reglas avanzadas del MVP documentadas pero no implementadas viven en domain specs con marca `Implementación: diferida (MVP Out)`.

---

## 7. Versionado y cambios

- Cambio incompatible de comportamiento → subir versión mayor de la spec y actualizar tests.
- Specs Approved solo cambian con revisión humana explícita (como el handbook).
- Deprecated: dejar archivo con puntero a la sucesor; no borrar historia sin motivo.

---

## 8. Relación con ADRs

| Pregunta | Artefacto |
|----------|-----------|
| ¿Qué debe hacer el negocio/sistema? | Spec |
| ¿Qué opción técnica elegimos y por qué? | ADR |
| ¿Está permitido por la constitución? | Handbook |

Una spec no sustituye un ADR de stack o de límites de contexto.

---

## 9. Criterios de aceptación de este capítulo (H5)

- [ ] Queda claro qué carpeta usa cada tipo de spec.
- [ ] Cabecera y estados Draft/Approved son obligatorios y auditables.
- [ ] Hard vs Soft y Given/When/Then quedan normados.
- [ ] Solo Approved autoriza implementación (con excepción ADR temporal).

---

## 10. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H5) |
