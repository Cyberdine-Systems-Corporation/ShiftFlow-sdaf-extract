# SPEC-DOM-001 — Glossary WorkforceScheduling (Sprint 1)

| Campo | Valor |
|--------|--------|
| ID | SPEC-DOM-001 |
| Versión | 0.1.2 |
| Estado | Approved |
| Fecha | 2026-08-10 |
| Fuentes | `handbook/11-ddd-and-bounded-contexts.md`, `handbook/03-mvp-definition.md`, SPEC-PRD-001, `knowledge/raw/Domain-Specs-V1.docx` (semántica sectorial filtrada) |
| ADRs relacionados | ADR-001, ADR-002, ADR-004 |
| Backlog | PBI-002, PBI-003, PBI-004 |
| Derivados | SPEC-DOM-002…004, SPEC-APP-001/002, SPEC-ACC-001 |

---

## 1. Contexto

Lenguaje ubicuo del bounded context **WorkforceScheduling** para el núcleo del Sprint 1 (maestros + auth).  
Los términos de sector del DOCX (p. ej. “guardia hospitalaria”) son **ejemplos de configuración**, no tipos del núcleo.

---

## 2. Términos

| Término | Definición | Sinónimos prohibidos en Domain |
|---------|------------|--------------------------------|
| **Organization** | Unidad raíz de configuración y datos de planificación. | Company, Tenant (hasta ADR de multitenancy) |
| **Department** | Subdivisión estructural bajo una Organization. | Area, Unit (salvo UI de presentación) |
| **Employee** | Persona asignable a turnos dentro de un Department. | Worker, User (User = identidad de acceso) |
| **ShiftType** | Tipo de turno del catálogo (plantilla/etiqueta asignable). | Shift template, Duty type |
| **User** | Identidad de autenticación que accede a la aplicación. | Employee (no son el mismo aggregate) |
| **Role** | Conjunto nombrado de permisos de aplicación. | Group, Profile |
| **Administrator** | Rol MVP que permite administrar maestros y planificar. | Superuser (evitar) |

---

## 3. Relaciones conceptuales (Sprint 1)

```text
Organization 1──* Department 1──* Employee
Organization 1──* ShiftType
User *──* Role   (asignación de roles a usuarios)
```

- Un Employee **no** es automáticamente un User.
- ShiftType pertenece a una Organization (catálogo por organización).

---

## 4. Fuera de alcance de este glossary

- **ShiftAssignment**, **CalendarMonth**, **RuleViolation** → [SPEC-DOM-005](SPEC-DOM-005-shift-assignment-calendar.md) (Sprint 2).
- Leave y detalle Rule Engine → [SPEC-DOM-006](SPEC-DOM-006-rule-engine-v1.md) / PBI-007.
- Roles granulares por departamento, SSO, multitenancy SaaS.

---

## 5. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.2 | 2026-08-10 | Cross-ref glossary Sprint 2 → SPEC-DOM-005/006 (editorial) |
| 0.1.1 | 2026-08-09 | Approved tras revisión humana |
| 0.1.0 | 2026-08-09 | Draft Sprint 1 (Specification Agent) |
