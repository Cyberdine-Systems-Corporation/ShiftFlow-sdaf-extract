# 06 — Engineering Principles

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | II — SDAF |
| **Norma superior** | [05-sdaf-framework.md](05-sdaf-framework.md), [01-product-charter.md](01-product-charter.md) |
| **Deriva hacia** | Workflow, arquitectura, testing, agentes |

---

## 1. Propósito

Fijar los principios de ingeniería **obligatorios** para humanos y agentes.  
Complementan los principios de producto del charter; no los sustituyen.

Orden de prioridad cuando choquen entre sí (salvo enmienda):

1. Simplicidad  
2. Escalabilidad (del diseño, no microservicios prematuros)  
3. Mantenibilidad  
4. Productividad con IA  
5. Calidad arquitectónica  
6. Trazabilidad  
7. Reutilización (SDAF)

---

## 2. Principios

### 2.1 Specification First

No se implementa funcionalidad de producto sin especificación con criterios de aceptación.

### 2.2 Architecture First (sin sobre-diseño)

Se respetan los límites arquitectónicos acordados.  
No se introduce complejidad (nuevos bounded contexts, buses, motores) sin ADR y necesidad demostrada.

### 2.3 Simplicity over Cleverness

Ante dos diseños correctos, se elige el más simple de explicar, probar y evolucionar con agentes.

### 2.4 Domain Centric

El dominio (expresado en specs derivadas de `knowledge/`) manda sobre comodidades de framework o de UI.

### 2.5 AI Assisted, Human Supervised

La IA acelera; el humano gobierna.  
La IA del producto **no** muta cuadrantes sin confirmación (charter).  
Los agentes de desarrollo **no** aprueban normas ni saltan gates.

### 2.6 Documentation as Product

Handbook, specs, ADRs y worklogs son entregables, no “después del código”.

### 2.7 Traceability by Default

Toda iteración relevante deja rastro (worklog): prompt/versión, specs, archivos, resultado, tests, estado.

### 2.8 Test from Specs

Los tests de aceptación se derivan de las specs.  
Preferir Test First cuando sea viable; como mínimo, acceptance verdes antes de cerrar el PBI.

### 2.9 Evolutionary Architecture

El sistema debe poder crecer (reglas, superficies, motores) sin reescritura.  
Crecer ≠ implementar todo el futuro en el MVP.

### 2.10 Automation First (local)

Automatizar arranque local, tests y calidad.  
Para el MVP, la automatización de **runtime local** prevalece sobre pipelines cloud elaborados.

### 2.11 One Way of Working

Un solo pipeline SDAF para humanos y agentes.  
No existe un “atajo de agente” distinto del flujo oficial.

### 2.12 Castilian for Engineering Artefacts

Commits, PRs, handbook, specs, ADRs, prompts y worklogs en castellano (regla del repo).  
Identificadores de código pueden seguir convenciones técnicas en inglés si el ADR de coding standards lo fija.

---

## 3. Anti-patrones prohibidos

| Anti-patrón | Por qué |
|-------------|---------|
| Código sin spec | Rompe Spec-Driven |
| Spec inventada solo para justificar código ya escrito | Invierte la fuente de verdad |
| Vertical hospitalario encubierto | Rompe plataforma configurable |
| Cinco motores / once agentes activos desde el día 1 | Sobre-diseño vs capacidad |
| “Ya está en cloud, pruébalo ahí” como único camino | Viola MVP local |
| Prompt libre no versionado como norma de trabajo | Pierde trazabilidad y reutilización |
| Merge/demo sin acceptance del flujo crítico | Deuda opaca |

---

## 4. Criterios de aceptación de este capítulo (H4)

- [ ] Los principios son auditables (se puede decir si se cumplieron o no).
- [ ] La prioridad 1–7 del §1 es aceptable.
- [ ] Los anti-patrones cubren los riesgos ya vistos en el proyecto.

---

## 5. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H4) |
