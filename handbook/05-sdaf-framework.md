# 05 — SDAF Framework

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | II — SDAF |
| **Norma superior** | [00-preface.md](00-preface.md), Parte I |
| **Deriva hacia** | [06-engineering-principles.md](06-engineering-principles.md), [07-repository-organization.md](07-repository-organization.md), [08-specification-standard.md](08-specification-standard.md), [09-development-workflow.md](09-development-workflow.md) |

---

## 1. Propósito

Definir el **Spec-Driven AI Development Framework (SDAF)**: el sistema de ingeniería con el que se produce ShiftFlow (y que debe ser reutilizable fuera de este producto).

SDAF no es un conjunto de prompts sueltos. Es una jerarquía normativa, un pipeline de artefactos y unas reglas de gobierno para humanos y agentes.

---

## 2. Definición

**SDAF** es un marco Spec-Driven donde:

1. El **conocimiento** de expertos es la fuente primaria del dominio (`knowledge/`, inmutable).
2. El **handbook** es la constitución.
3. Las **especificaciones** son la única fuente de verdad operativa para implementar.
4. El **código** y los **tests** se derivan de las specs (nunca al revés como norma).
5. Los **agentes IA** ejecutan el pipeline bajo supervisión humana y trazabilidad.

---

## 3. Jerarquía normativa

```text
Knowledge (inmutable)
    → Handbook (constitución)
    → Product (Vision / MVP / Roadmap)
    → Domain Specs (Glossary → Model → Rules → Use Cases → Acceptance)
    → Architecture + ADRs
    → Backlog
    → Implementation ∥ Spec-derived Tests
    → Review / Quality Gates
    → Release (demo + presentación)
```

### 3.1 Qué no es un nivel normativo

| Artefacto | Rol |
|-----------|-----|
| Agentes IA | Actores que operan el pipeline |
| Prompts | Contratos operativos versionados |
| Worklogs | Trazabilidad de iteraciones |
| Código | Resultado derivado |

### 3.2 Prioridad ante conflicto

1. Capítulos **Approved** del handbook  
2. ADRs vigentes  
3. Specs en `specs/`  
4. Backlog  
5. Implementación / prompts / worklogs  

---

## 4. Pipeline de dominio

Transformación obligatoria del knowledge funcional:

```text
Knowledge
    → Glossary + Ubiquitous Language
    → Domain Model
    → Business Rules (Hard Constraints vs Soft Preferences)
    → Calculation Rules
    → Use Cases
    → Acceptance Tests
    → Implementation
```

No se implementa el documento de experto “tal cual”. Se transforma.

---

## 5. Doble entregable

Cada incremento significativo debe proteger:

| Entregable | Significado |
|------------|-------------|
| Producto | Capacidad demostrable acorde al MVP / roadmap |
| Metodología | Artefactos SDAF actualizados (specs, ADRs, worklogs, prompts) |

Acelerar el producto destruyendo la metodología es una violación de SDAF.

---

## 6. Gobierno antes de implementar

Antes de generar implementación de una feature de producto, **debe** existir:

1. Especificación aplicable (o enmienda Approved del alcance).
2. Decisión de arquitectura relevante (ADR) cuando el cambio cruza límites o stack.
3. Criterios de aceptación / tests derivados de la spec.
4. Entrada de trazabilidad (worklog) de la iteración.

Si falta alguno → **STOP**. Proponer su creación; no improvisar código.

Detalle operativo en el capítulo 09.

---

## 7. Agentes en SDAF (resumen)

- Equipo especializado, no un único agente omnisciente.
- En el MVP: **5 agentes activos** + stubs documentados (detalle en Parte IV).
- El humano aprueba handoffs relevantes y todo capítulo Approved.
- Ningún agente puede autodeclarar Approved ni saltarse el gate del §6.

---

## 8. Relación con ShiftFlow

ShiftFlow es el **primer producto** construido con SDAF.  
Las reglas de esta Parte II deben poder aplicarse a otro producto .NET con cambios mínimos de Parte I.

---

## 9. Criterios de aceptación de este capítulo (H4)

- [ ] La jerarquía normativa corrige Architecture-antes-de-Specs y no trata agentes como nivel normativo.
- [ ] El gate “no implementar sin spec/ADR/aceptación/worklog” queda explícito.
- [ ] El doble entregable (producto + metodología) es norma.

---

## 10. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H4) |
