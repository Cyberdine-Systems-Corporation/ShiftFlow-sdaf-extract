# SPEC-ACC-004 — Aceptación HR-03 (descanso mínimo)

| Campo | Valor |
|--------|--------|
| ID | SPEC-ACC-004 |
| Versión | 0.1.0 |
| Estado | Approved |
| Fecha | 2026-08-13 |
| Fuentes | SPEC-DOM-006 §2.3, SPEC-PRD-002 paso 8, PBI-006 |
| ADRs relacionados | ADR-003 |
| Backlog | PBI-006 |
| Derivados | Tests unitarios Domain + integración API |

---

## 1. Contexto

Cubre **HR-03** (descanso mínimo entre turnos `Assigned` del mismo empleado), con umbral configurable por Organization.

**No** sustituye ACC-002 (calendario/HR-01) ni ACC-003 (Leave/HR-02).

Gate 0 de implementación: criterios de dominio en SPEC-DOM-006 Approved §2.3; este ACC formaliza escenarios testeables.

---

## 2. Escenarios

### ACC-S2-R01 — Rechazo por descanso insuficiente

**Dado** una Organization con `MinimumRestMinutes` = 660 (11 h)  
**Y** un empleado con turno `Assigned` 08:00–16:00  
**Cuando** se intenta asignar otro turno el mismo día 16:00–20:00 (gap 0 &lt; 660)  
**Entonces** la API responde error con código `HR-03`  
**Y** no se persiste la segunda asignación

### ACC-S2-R02 — Permite si el gap ≥ umbral

**Dado** Organization con `MinimumRestMinutes` = 660  
**Y** turno `Assigned` 08:00–16:00  
**Cuando** se asigna turno al día siguiente 03:00–07:00 (gap 11 h = 660)  
**Entonces** la asignación se acepta (`Assigned`)

### ACC-S2-R03 — Umbral 0 no aplica HR-03 (adyacentes OK)

**Dado** Organization con `MinimumRestMinutes` = 0 (default)  
**Cuando** se asignan dos turnos adyacentes (fin = inicio)  
**Entonces** ambos se aceptan (comportamiento ACC-S2-04)

---

## 3. Default de producto

- `Organization.MinimumRestMinutes` default = **0** (HR-03 inactivo hasta configurar).
- Valores &lt; 0 rechazados (invariante de dominio).
- Comparación: bloquea si `gap < TimeSpan.FromMinutes(umbral)` (gap igual al umbral **sí** permitido).

---

## 4. Fuera de alcance

- Soft preferences; cumplimiento legal sectorial; UI avanzada de políticas.

---

## 5. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.0 | 2026-08-13 | Approved tras revisión humana (ACC-S2-R01…R03) |
