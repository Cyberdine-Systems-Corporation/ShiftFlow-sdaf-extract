# Apéndice A — Glossary

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | Apéndices |
| **Norma superior** | Handbook; el glossary de dominio canónico vive en `specs/domain/` |

---

## 1. Propósito

Glosario **de ingeniería y SDAF** (no sustituye el ubiquitous language de negocio en `specs/domain/`).

---

## 2. Términos

| Término | Definición |
|---------|------------|
| **SDAF** | Spec-Driven AI Development Framework: marco de ingeniería de este repo |
| **Handbook** | Constitución del proyecto |
| **Knowledge** | Fuente primaria inmutable de expertos (`knowledge/`) |
| **Spec** | Especificación testeable en `specs/` |
| **ADR** | Architecture Decision Record |
| **PBI** | Product Backlog Item |
| **Gate 0** | Condiciones pre-implementación (spec, acceptance, ADR si aplica, worklog) |
| **ATF** | Agent Traceability Framework (`worklogs/`) |
| **Worklog** | Registro de una iteración de agente/humano |
| **Agente activo** | Agente de ingeniería en uso regular en el MVP |
| **Stub (agente)** | Contrato+prompt listos, activación bajo demanda |
| **Hard rule** | Restricción que bloquea una asignación |
| **Soft preference** | Preferencia que ordena/penaliza sin bloqueo duro |
| **Rule Engine** | Mecanismo de evaluación de reglas en el BC WorkforceScheduling |
| **Scheduling Engine** | Mecanismo de asignación/ciclo de vida de turnos (manual en MVP) |
| **Runtime local autocontenido** | App + infra (p. ej. Postgres) levantables sin cloud |
| **Vertical slice** | Unidad de feature en Application (command/query + handler) |
| **WorkforceScheduling** | Bounded context único del MVP |
| **IA de producto** | Adaptadores en Infrastructure que asisten al usuario final |
| **Agente de ingeniería** | Agente que produce artefactos del repo |

---

## 3. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H8) |
