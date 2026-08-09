# SPEC-DOM-003 — ShiftType

| Campo | Valor |
|--------|--------|
| ID | SPEC-DOM-003 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-09 |
| Fuentes | SPEC-PRD-001 (C-STT), SPEC-DOM-001, `handbook/11-ddd-and-bounded-contexts.md`, `knowledge/raw/Domain-Specs-V1.docx` |
| ADRs relacionados | ADR-001, ADR-003, ADR-004 |
| Backlog | PBI-004 |
| Derivados | SPEC-APP-001, SPEC-ACC-001 |

---

## 1. Contexto

Catálogo de tipos de turno por Organization, usable en la asignación manual (Sprint 2). En Sprint 1 basta CRUD + consulta.

---

## 2. Aggregate ShiftType

| Campo conceptual | Regla |
|------------------|--------|
| Id | Identidad estable |
| OrganizationId | Obligatorio; Organization activa al crear |
| Name | Obligatorio; único por Organization (case-insensitive) |
| Code | Opcional; si se informa, único por Organization |
| DefaultStartTime | Opcional (TimeOnly); plantilla orientativa para UI |
| DefaultEndTime | Opcional (TimeOnly); si ambos existen, End puede ser ≤ Start solo si se interpreta cruce de medianoche (**diferido**: en MVP sin cruce, End > Start) |
| IsActive | Por defecto true; inactivo no seleccionable en nuevas asignaciones (Sprint 2) |

---

## 3. Invariantes

| ID | Regla | Hard/Soft | Sprint |
|----|-------|-----------|--------|
| INV-STT-01 | ShiftType pertenece a una Organization existente y activa al crearse | Hard | 1 |
| INV-STT-02 | Name obligatorio y único por Organization | Hard | 1 |
| INV-STT-03 | Code, si existe, único por Organization | Hard | 1 |
| INV-STT-04 | Si DefaultStart y DefaultEnd están definidos, End > Start (sin overnight) | Hard | 1 |
| INV-STT-05 | ShiftType inactivo no usable en nuevas asignaciones | Hard | 2 |

**Implementación: diferida (DOCX / Out MVP código):** turnos pares/impares, bolsas mensuales, tipologías sectoriales fijas, colores obligatorios, plantillas multi-día.

---

## 4. Criterios de aceptación (dominio)

1. No se crea ShiftType huérfano (sin Organization).  
2. Colisión de Name o Code en la misma Organization se rechaza.  
3. Rango horario por defecto inválido (End ≤ Start) se rechaza cuando ambos están informados.

---

## 5. Fuera de alcance

- Motor de reglas y asignación (usa ShiftType; no lo define).
- Generación automática de turnos.

---

## 6. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-09 | Approved tras revisión humana |
| 0.1.0 | 2026-08-09 | Draft Sprint 1 (Specification Agent) |
