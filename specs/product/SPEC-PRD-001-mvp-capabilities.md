# SPEC-PRD-001 — Capacidades del MVP

| Campo | Valor |
|--------|--------|
| ID | SPEC-PRD-001 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-05 |
| Fuentes | `handbook/03-mvp-definition.md` (Approved), `handbook/02-product-vision.md` |
| ADRs relacionados | ADR-001, ADR-002, ADR-003 |
| Backlog | PBI-001 … PBI-012 (ver `backlog/README.md`) |
| Derivados | Specs de dominio/aplicación por capacidad; acceptance del journey (SPEC-PRD-002) |

---

## 1. Contexto

Define el conjunto de **capacidades de producto** que deben existir en `mvp-0.1` (22 ago 2026), sin detalle de modelo de dominio (eso vive en `specs/domain/`).

---

## 2. Alcance

### 2.1 Incluido (In)

| ID capacidad | Capacidad | Notas |
|--------------|-----------|--------|
| C-ORG | Gestionar organizaciones | CRUD mínimo demostrable |
| C-DEP | Gestionar departamentos | Bajo una organización |
| C-EMP | Gestionar empleados | Asignables a turnos |
| C-STT | Gestionar tipos de turno | Catálogo |
| C-CAL | Calendario mensual | Vista de planificación |
| C-ASN | Asignación manual de turnos | Scheduling Engine |
| C-RUL | Validación hard rules | Rule Engine v1 — ver §3 |
| C-LEA | Leaves (vacaciones/ausencias) | Bloquean asignación |
| C-AUTH | Usuarios y roles básicos | Acceso a la app |
| C-API | API REST | Expone comandos/consultas |
| C-WEB | Cliente Blazor Web | Única UI (ADR-002) |
| C-LOC | Runtime local autocontenido | Aspire/Docker + Postgres + runbook |
| C-OBS | Logging mínimo | Serilog |
| C-PRE | Presentación cierre MVP | Slides + vídeo (producto, evolución, arquitectura) |

### 2.2 Hard rules (C-RUL)

1. No solapes de turno para la misma persona.  
2. Leave activo bloquea asignación en el rango afectado.  
3. Descanso mínimo configurable entre turnos.

### 2.3 Excluido (Out)

Según handbook §5 / ADRs: MAUI, Redis, SignalR, optimización automática, IA que escribe cuadrantes, cloud como único camino de demo, reglas avanzadas del DOCX (pares/impares, bolsa mensual, etc.) salvo documentación en domain specs.

---

## 3. Criterios de aceptación (producto)

1. Todas las capacidades C-ORG … C-WEB están ejercitables en el journey SPEC-PRD-002.  
2. C-LOC permite a un evaluador arrancar sin cuenta cloud.  
3. C-PRE existe y cubre los tres bloques obligatorios del handbook §4.5.  
4. Ninguna capacidad Out aparece como dependencia bloqueante del DoD.

---

## 4. Fuera de alcance de esta spec

- Modelo de aggregates y lenguaje ubicuo → `specs/domain/`.  
- Contratos comando/query detallados → `specs/application/`.  
- Escenarios Given/When/Then finos del flujo → SPEC-PRD-002 y `specs/acceptance/`.

---

## 5. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial Sprint 0 |
