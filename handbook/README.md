# ShiftFlow Engineering Handbook

| Campo | Valor |
|--------|--------|
| **Versión** | 1.0.0 |
| **Estado** | Approved |
| **Idioma** | Español |
| **Clasificación** | Constitución del proyecto |
| **Última actualización** | 2026-08-05 |

---

## Propósito

Este handbook es la **constitución** de ShiftFlow y del Spec-Driven AI Development Framework (SDAF).

- Define qué se puede decidir, cómo se decide y qué es obligatorio.
- Toda especificación, ADR, backlog, prompt, worklog e implementación debe poder justificarse remontándose a este handbook.
- Si un artefacto lo contradice, prevalece el handbook hasta que se apruebe un cambio formal aquí.

No es un tutorial ni un dump de requisitos. Es norma.

---

## Mapa normativo

```text
Handbook (esta constitución)
    → Product (charter, vision, MVP, roadmap)
    → Domain Specs (glossary → model → rules → use cases → acceptance)
    → Architecture + ADRs
    → Backlog
    → Implementation ∥ Spec-derived Tests
    → Review / Quality Gates
    → Release (demo)
```

Los **agentes IA** no son un nivel normativo: ejecutan el pipeline bajo estas reglas.  
Los **prompts** y **worklogs** son infraestructura de ingeniería, no sustituyen al handbook.

---

## Índice

### Front matter

| Cap. | Archivo | Título | Estado |
|------|---------|--------|--------|
| 00 | [00-preface.md](00-preface.md) | Preface | Approved |

### Parte I — Constitución de producto

| Cap. | Archivo | Título | Estado |
|------|---------|--------|--------|
| 01 | [01-product-charter.md](01-product-charter.md) | Product Charter | Approved |
| 02 | [02-product-vision.md](02-product-vision.md) | Product Vision | Approved |
| 03 | [03-mvp-definition.md](03-mvp-definition.md) | MVP Definition | Approved |
| 04 | [04-product-roadmap.md](04-product-roadmap.md) | Product Roadmap | Approved |

### Parte II — SDAF

| Cap. | Archivo | Título | Estado |
|------|---------|--------|--------|
| 05 | [05-sdaf-framework.md](05-sdaf-framework.md) | SDAF Framework | Approved |
| 06 | [06-engineering-principles.md](06-engineering-principles.md) | Engineering Principles | Approved |
| 07 | [07-repository-organization.md](07-repository-organization.md) | Repository Organization | Approved |
| 08 | [08-specification-standard.md](08-specification-standard.md) | Specification Standard | Approved |
| 09 | [09-development-workflow.md](09-development-workflow.md) | Development Workflow | Approved |

### Parte III — Arquitectura

| Cap. | Archivo | Título | Estado |
|------|---------|--------|--------|
| 10 | [10-solution-architecture.md](10-solution-architecture.md) | Solution Architecture | Approved |
| 11 | [11-ddd-and-bounded-contexts.md](11-ddd-and-bounded-contexts.md) | DDD and Bounded Contexts | Approved |
| 12 | [12-cqrs-vertical-slices.md](12-cqrs-vertical-slices.md) | CQRS and Vertical Slices | Approved |

### Parte IV — Ingeniería IA

| Cap. | Archivo | Título | Estado |
|------|---------|--------|--------|
| 13 | [13-ai-agent-framework.md](13-ai-agent-framework.md) | AI Agent Framework | Approved |
| 14 | [14-prompt-engineering-standard.md](14-prompt-engineering-standard.md) | Prompt Engineering Standard | Approved |
| 15 | [15-agent-traceability.md](15-agent-traceability.md) | Agent Traceability Framework | Approved |

### Parte V — Calidad y entrega

| Cap. | Archivo | Título | Estado |
|------|---------|--------|--------|
| 16 | [16-testing-framework.md](16-testing-framework.md) | Testing Framework | Approved |
| 17 | [17-code-review-and-quality-gates.md](17-code-review-and-quality-gates.md) | Code Review and Quality Gates | Approved |
| 18 | [18-devops.md](18-devops.md) | DevOps | Approved |

### Parte VI — Operación

| Cap. | Archivo | Título | Estado |
|------|---------|--------|--------|
| 19 | [19-sprint-planning-and-metrics.md](19-sprint-planning-and-metrics.md) | Sprint Planning and Metrics | Approved |

### Apéndices

| Cap. | Archivo | Título | Estado |
|------|---------|--------|--------|
| A | [A-glossary.md](A-glossary.md) | Glossary | Approved |
| B | [B-templates.md](B-templates.md) | Templates | Approved |
| — | [CHANGELOG.md](CHANGELOG.md) | Historial de versiones | Draft |

---

## Estados de capítulo

| Estado | Significado |
|--------|-------------|
| **Pendiente** | Archivo aún no creado |
| **Draft** | Borrador en revisión; usable como guía, no cerrado |
| **Approved** | Norma vigente; cambios requieren revisión formal y entrada en CHANGELOG |

---

## Cómo contribuir (humano y agentes)

1. Un cambio al handbook es un cambio constitucional: debe ser explícito y revisado.
2. Redactar o editar el capítulo correspondiente; actualizar su cabecera (`Versión`, `Estado`, `Fecha`).
3. Actualizar la tabla de índice de este README (estado del capítulo).
4. Registrar el cambio en [CHANGELOG.md](CHANGELOG.md) cuando exista.
5. No “arreglar” el handbook en silencio desde el código: si el código exige otra norma, proponer ADR + enmienda al handbook.

### Prioridad de resolución de conflictos

1. Capítulos **Approved** del handbook  
2. ADRs vigentes en `architecture/decisions/`  
3. Specs en `specs/`  
4. Backlog  
5. Implementación / prompts / worklogs  

---

## Lectura mínima por rol

| Rol | Leer primero |
|-----|----------------|
| Producto / PO | Parte I |
| Arquitecto | Preface, Parte I, II, III |
| Agente Specification | Preface, Parte I–II, cap. 08 |
| Agente Domain / Application | Parte I–III, glosario |
| Agente Frontend | Parte I (MVP), 07, 10 |
| Agente Testing / Review | Parte II (08–09), V |
| Nuevo contribuidor | Preface + Product Charter + SDAF Framework |

---

## Relación con el repositorio

| Carpeta | Relación con el handbook |
|---------|---------------------------|
| `knowledge/` | Fuente primaria inmutable de expertos; el handbook no la modifica |
| `specs/` | Derivan del handbook + knowledge |
| `architecture/` | Decisiones acotadas por Partes II–III |
| `agents/`, `prompts/` | Operan bajo Parte IV |
| `worklogs/` | Trazabilidad exigida por Parte IV |
| `src/`, `tests/` | Solo tras gates de Parte II y V |

---

## Historial de este índice

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 1.0.0 | 2026-08-05 | Handbook completo Approved (caps. 00–19, A–B) |
| 0.1.14 | 2026-08-05 | H8 Draft: testing, quality gates, devops, metrics, apéndices |
| 0.1.13 | 2026-08-05 | H7 Draft: agentes, prompts, ATF |
| 0.1.12 | 2026-08-05 | Parte II Approved (caps. 05–09) |
| 0.1.11 | 2026-08-05 | H6 Draft: solution architecture, DDD, CQRS/slices |
| 0.1.10 | 2026-08-05 | H5 Draft: specification standard + development workflow |
| 0.1.9 | 2026-08-05 | H4 Draft: SDAF, principios, organización del repo |
| 0.1.8 | 2026-08-05 | Enmienda MVP/Roadmap: vídeo + slides de presentación mandatory |
| 0.1.7 | 2026-08-05 | H3 cerrado: MVP Definition y Product Roadmap Approved |
| 0.1.6 | 2026-08-05 | H3: Product Vision Approved |
| 0.1.5 | 2026-08-05 | MVP: runtime local autocontenido mandatory |
| 0.1.4 | 2026-08-05 | H2 Approved: product charter |
| 0.1.3 | 2026-08-05 | H3 Draft: vision, MVP, roadmap |
| 0.1.2 | 2026-08-01 | H2 Draft: product charter |
| 0.1.1 | 2026-08-01 | H1 Approved: preface |
| 0.1.0 | 2026-08-01 | Creación del índice y mapa normativo (sesión H1) |
