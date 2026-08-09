# SPEC-DOM-004 — Auth y roles básicos

| Campo | Valor |
|--------|--------|
| ID | SPEC-DOM-004 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-09 |
| Fuentes | SPEC-PRD-001 (C-AUTH), SPEC-PRD-002 (precondiciones), `handbook/03-mvp-definition.md`, `handbook/11-ddd-and-bounded-contexts.md` §2 |
| ADRs relacionados | ADR-001 (dirección Identity); ADR de auth dedicado si Architecture lo exige |
| Backlog | PBI-002 |
| Derivados | SPEC-APP-002, SPEC-ACC-001 |

---

## 1. Contexto

Auth es **supporting / generic subdomain** dentro del monolito (no BC Identity separado en MVP).  
Objetivo: usuario demo autenticado con un rol que permita administrar maestros y (más adelante) planificar.

---

## 2. Conceptos

| Concepto | Definición MVP |
|----------|----------------|
| **User** | Credencial + identidad de acceso (no es Employee) |
| **Role** | Nombre estable de permiso de aplicación |
| **Administrator** | Único rol obligatorio del MVP con permisos de administración y planificación |

Permisos lógicos del rol Administrator (Sprint 1+):

| Permiso | Sprint 1 | Sprint 2 |
|---------|----------|----------|
| Gestionar Organization / Department / Employee / ShiftType | Sí | Sí |
| Consultar maestros | Sí | Sí |
| Asignar turnos / leaves / calendario | — | Sí |

Otros roles (Viewer, Planner-only, etc.): **Out** del MVP salvo necesidad demostrada + enmienda.

---

## 3. Reglas

| ID | Regla | Hard/Soft |
|----|-------|-----------|
| INV-AUTH-01 | Operaciones de escritura de maestros requieren usuario autenticado con rol Administrator | Hard |
| INV-AUTH-02 | Consultas de maestros en API/UI de administración requieren autenticación (mismo rol en MVP) | Hard |
| INV-AUTH-03 | Existe al menos un usuario demo usable tras seed o setup documentado en runbook | Hard (producto) |
| INV-AUTH-04 | Credenciales no se registran en knowledge ni se commitean fuera de secretos locales / user-secrets | Hard |

Política de contraseñas avanzada, MFA, SSO, OAuth externo, reset por email: **Out** MVP.

---

## 4. Criterios de aceptación (dominio/producto de acceso)

1. Un sujeto anónimo no puede crear/modificar maestros.  
2. Un Administrator autenticado sí puede.  
3. El journey SPEC-PRD-002 asume precondición C-AUTH satisfecha con este rol.

---

## 5. Fuera de alcance

- BC Identity separado; claims finos por Department; auditoría forense.
- Autorización a nivel de fila por organización múltiple (multitenancy).

---

## 6. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-09 | Approved tras revisión humana |
| 0.1.0 | 2026-08-09 | Draft Sprint 1 (Specification Agent) |
