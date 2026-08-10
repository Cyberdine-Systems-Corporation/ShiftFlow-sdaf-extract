# SPEC-PRD-003 — NFR de UI demo (Web)

| Campo | Valor |
|--------|--------|
| ID | SPEC-PRD-003 |
| Versión | 0.1.1 |
| Estado | Approved |
| Fecha | 2026-08-10 |
| Fuentes | `handbook/03-mvp-definition.md`, `handbook/04-product-roadmap.md` (UX demo), SPEC-PRD-002 |
| ADRs relacionados | ADR-002 (Web-only); sin librería UI externa en este alcance |
| Backlog | PBI-013 |
| Derivados | Implementación en `src/ShiftFlow.Web`; worklog ATF |

---

## 1. Contexto

El journey de demo (SPEC-PRD-002) y el shell CRUD (PBI-008) son funcionales, pero la UI es un formulario mínimo sin sistema visual. El roadmap cita **UX demo** como ítem de cierre. Esta spec fija NFR **observables** de presentación e interacción sin cambiar reglas de dominio ni el flujo de negocio.

Fuente canónica de NFR visual/UX demo: **este documento**. No se duplican tokens ni wireframes en handbook ni ADRs salvo decisión de stack.

---

## 2. Alcance

**In**

- Superficies Web actuales: login, home, nav, organizaciones, detalle de org (dept/empleado/shift type), placeholder de calendario.
- Design system CSS propio (tokens, tipografía, estados vacío/error/loading).
- Jerarquía de marca en login/home; shell estable en páginas autenticadas.

**Out**

- Calendario funcional (PBI-005), librerías UI Blazor (Mud/Fluent), dark mode, MAUI, rebranding sectorial (hospital/policía embebido).

---

## 3. Criterios de aceptación

### AC-UX-01 — Login como composición de marca

**Dado** un visitante en `/login`  
**Cuando** carga el primer viewport  
**Entonces** el nombre de producto **ShiftFlow** es la señal dominante; hay un titular breve, el formulario de credenciales y un CTA primario; no hay dashboard ni bloques secundarios competidores.

### AC-UX-02 — Shell autenticado

**Dado** un Administrator autenticado  
**Cuando** navega entre Home, Organizaciones y Calendario  
**Entonces** ve una barra de navegación estable con marca, enlaces con indicación de ruta activa, identidad de usuario y acción Salir; el contenido vive en un contenedor de lectura con ancho máximo.

### AC-UX-03 — Estados de maestros

**Dado** las pantallas de organizaciones / detalle  
**Cuando** hay carga, lista vacía o error de Api  
**Entonces** se muestra un estado explícito (texto de carga, empty state con siguiente acción, o alerta de error legible — no solo un código HTTP crudo sin contexto).

### AC-UX-04 — Responsive básico

**Dado** viewport ~375px y ~1280px  
**Cuando** se recorre login y listado de organizaciones  
**Entonces** el contenido no desborda horizontalmente de forma inutilizable; formularios y nav permanecen usables.

### AC-UX-05 — Sin dependencia UI externa

**Dado** el proyecto `ShiftFlow.Web`  
**Cuando** se inspeccionan referencias de paquetes  
**Entonces** el rediseño no introduce MudBlazor, Fluent UI Blazor u otro kit como dependencia del MVP (CSS + fuentes web sí permitidos).

---

## 4. Dirección visual (no normativa de producto)

Orientación de implementación (puede evolucionar sin enmendar el journey):

- Tema claro “tablero operativo”: tinta oscura, acento teal, tipografía display + UI distintas de stacks por defecto (Inter/Roboto/Arial/system).
- Fondo con atmósfera (gradiente/patrón), no plano único.
- Motion sobrio (entrada de página, nav, botones); respetar `prefers-reduced-motion`.

---

## 5. Fuera de alcance / no criterios

- Pixel-perfect Figma; auditoría WCAG completa AA formal (sí: foco visible y contraste razonable).
- Cambiar pasos de SPEC-PRD-002 ni mensajes INV-* del dominio.

---

## 6. Gate 0

| Ítem | Estado |
|------|--------|
| Spec | Approved |
| PBI | PBI-013 |
| ADR | N/A (CSS propio; sin cambio de stack) |
| Worklog | `worklogs/PBI-013-ux-blazor-redesign/` |

---

## Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.0 | 2026-08-10 | Draft inicial NFR UI demo |
| 0.1.1 | 2026-08-10 | Approved tras revisión humana |
