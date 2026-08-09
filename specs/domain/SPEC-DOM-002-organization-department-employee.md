# SPEC-DOM-002 — Organization, Department, Employee

| Campo | Valor |
|--------|--------|
| ID | SPEC-DOM-002 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-09 |
| Fuentes | SPEC-PRD-001 (C-ORG, C-DEP, C-EMP), SPEC-DOM-001, `handbook/11-ddd-and-bounded-contexts.md`, `knowledge/raw/Domain-Specs-V1.docx` |
| ADRs relacionados | ADR-001, ADR-004 |
| Backlog | PBI-003 |
| Derivados | SPEC-APP-001, SPEC-ACC-001, tests de aggregates |

---

## 1. Contexto

Modelo de dominio de maestros organizativos necesarios para el journey de demo (pasos 1–2 de SPEC-PRD-002).

---

## 2. Aggregates / entidades

### 2.1 Organization (aggregate root)

| Campo conceptual | Regla |
|------------------|--------|
| Id | Identidad estable |
| Name | Obligatorio; no vacío tras trim; longitud máxima 200 |
| IsActive | Por defecto true; inactiva no admite nuevos Department/Employee/ShiftType |

### 2.2 Department (entity bajo Organization **o** aggregate root referenciando OrganizationId)

Para el MVP se permite:

- **Opción A (preferida):** Department como aggregate root con `OrganizationId` (consistencia: no mover dept entre orgs sin caso de uso).
- **Opción B:** Department entity dentro de Organization si el volumen es trivial.

Invariantes aplicables en ambos casos:

| Invariante | Tipo |
|------------|------|
| Todo Department referencia una Organization existente y activa al crearse | **Hard** |
| Name obligatorio; único dentro de la misma Organization (case-insensitive) | **Hard** |
| IsActive por defecto true | — |

### 2.3 Employee (aggregate root)

| Campo conceptual | Regla |
|------------------|--------|
| Id | Identidad estable |
| OrganizationId | Derivado o explícito; coherente con el Department |
| DepartmentId | Obligatorio; Department debe existir y pertenecer a la misma Organization |
| DisplayName | Obligatorio; no vacío |
| Email | Opcional en MVP; si se informa, formato email válido y único por Organization |
| IsActive | Por defecto true; inactivo **no** es asignable a turnos (Sprint 2) |

---

## 3. Invariantes y reglas

| ID | Regla | Hard/Soft | Sprint |
|----|-------|-----------|--------|
| INV-ORG-01 | Name de Organization obligatorio y no vacío | Hard | 1 |
| INV-DEP-01 | Department exige Organization activa | Hard | 1 |
| INV-DEP-02 | Name único por Organization | Hard | 1 |
| INV-EMP-01 | Employee exige Department de la misma Organization | Hard | 1 |
| INV-EMP-02 | DisplayName obligatorio | Hard | 1 |
| INV-EMP-03 | Employee inactivo no puede recibir nuevas asignaciones | Hard | 2 (declarada; enforce con Schedule) |

**Implementación: diferida (MVP documentado / Out de código Sprint 1):** jerarquías multi-nivel de departamentos, centros de coste, contratos laborales avanzados, skills/certificaciones del DOCX.

---

## 4. Operaciones de dominio (conceptuales)

- Crear / renombrar / activar-desactivar Organization.
- Crear / renombrar / activar-desactivar Department bajo Organization.
- Crear / actualizar / activar-desactivar Employee en Department.
- Consultar listados filtrados por Organization (y Department).

Eliminación física: **no requerida** en MVP; preferir `IsActive = false`.

---

## 5. Criterios de aceptación (dominio)

1. No se puede persistir Department sin Organization válida.  
2. No se puede persistir Employee con Department de otra Organization.  
3. Dos Departments con el mismo Name (ignorando mayúsculas) en la misma Organization son rechazados.  
4. Los aggregates exponen invariantes sin depender de UI/EF.

---

## 6. Fuera de alcance

- Asignación de turnos, leaves, rule engine.
- Importación masiva, sincronización HR externa.

---

## 7. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-09 | Approved tras revisión humana |
| 0.1.0 | 2026-08-09 | Draft Sprint 1 (Specification Agent) |
