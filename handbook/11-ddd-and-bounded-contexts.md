# 11 — DDD and Bounded Contexts

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | III — Arquitectura |
| **Norma superior** | [10-solution-architecture.md](10-solution-architecture.md), [08-specification-standard.md](08-specification-standard.md) |
| **Deriva hacia** | `specs/domain/`, `architecture/contexts/`, Domain project |

---

## 1. Propósito

Fijar cómo se aplica Domain-Driven Design en ShiftFlow para el MVP: un bounded context, lenguaje ubicuo, aggregates iniciales y reglas de crecimiento.

---

## 2. Bounded context inicial

| Campo | Valor |
|--------|--------|
| Nombre | **WorkforceScheduling** |
| Intención | Planificación y asignación de turnos configurable por organización |
| MVP | Único contexto desplegado |

No se crean contextos adicionales (Identity como BC separado, Billing, Notifications, …) en el MVP.  
Auth puede vivir como **generic subdomain / supporting** dentro del monolito sin BC propio hasta que el lenguaje lo exija (ADR).

---

## 3. Lenguaje ubicuo

- Se define en `specs/domain/` (glossary) a partir de `knowledge/`.
- Términos de un sector (p. ej. “guardia hospitalaria”) son **ejemplos de configuración**, no tipos del núcleo, salvo que la spec de plataforma los generalice.
- El código del Domain debe usar los mismos nombres que el glossary Approved.

---

## 4. Building blocks (obligatorios donde aporten)

| Building block | Uso en MVP |
|----------------|------------|
| Aggregates | Consistencia transaccional de asignaciones y maestros |
| Entities / Value Objects | Identidades y medidas (DateRange, ShiftTime, …) |
| Domain Events | Hechos relevantes in-process (p. ej. ShiftAssigned); sin bus externo |
| Domain Services | Reglas que no caben limpio en un solo aggregate (p. ej. evaluación cruzada) |
| Repositories | Puertos en Domain; implementaciones en Infrastructure |
| Shared Kernel | Mínimo (IDs, Result/Error, DateRange); evitar “utils” gordos |

Factories/specifications de DDD: solo si reducen complejidad real.

---

## 5. Aggregates candidatos (MVP)

Lista conceptual — nombres y límites exactos salen de specs de dominio:

| Aggregate / cluster | Responsabilidad aproximada |
|---------------------|----------------------------|
| Organization | Organización y su configuración raíz |
| Department | Estructura bajo organización |
| Employee | Persona asignable |
| ShiftType | Catálogo de tipos de turno |
| Schedule / Calendar period | Ventana de planificación (p. ej. mes) y asignaciones |
| Shift (o entity dentro de Schedule) | Asignación concreta |
| Leave | Ausencia/vacación que bloquea |

El ADR de modelo y las specs deciden si `Shift` es aggregate raíz o entidad de `Schedule`. Preferir **pocos aggregates** con invariantes claras frente a un aggregate “dios”.

---

## 6. Invariantes y Rule Engine

- Las hard rules del MVP son invariantes de dominio (o políticas de dominio invocadas antes de persistir una asignación).
- El Rule Engine v1 **no** es un BC aparte: es un mecanismo dentro de WorkforceScheduling.
- Soft preferences (post-MVP) no deben disfrazarse de invariantes hard.

---

## 7. Domain events (MVP)

- Publicación in-process tras commits de aplicación (patrón a fijar en ADR).
- Ejemplos: `ShiftAssigned`, `ShiftAssignmentRejected`, `LeaveRegistered`.
- No hay integración event-driven entre servicios.

---

## 8. Crecimiento de contextos (post-MVP)

Se abre un nuevo BC solo si:

1. El lenguaje ubicuo diverge de forma sostenida, y  
2. Hay un ADR que justifique el coste de integración, y  
3. No basta un módulo dentro del monolito.

Hasta entonces: **modularidad interna**, no partición prematura.

---

## 9. Anti-patrones

| Anti-patrón | Mitigación |
|-------------|------------|
| Anemic domain (solo DTOs + servicios de infra) | Lógica de invariantes en Domain |
| Un aggregate por tabla EF | Diseñar por invariantes, no por tablas |
| Shared Kernel como cajón de sastre | Revisar en review; máximo mínimo viable |
| BC por carpeta técnica (Api, Web) | BC = lenguaje, no proyecto UI |

---

## 10. Criterios de aceptación de este capítulo (H6)

- [ ] Un solo BC WorkforceScheduling para el MVP es norma.
- [ ] Aggregates candidatos son orientativos y ligados a specs.
- [ ] Rule Engine no es un BC separado.
- [ ] Criterio de apertura de nuevos BC es restrictivo.

---

## 11. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H6) |
