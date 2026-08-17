# SPEC-PRD-003 — NFR de UI demo (Web)

| Campo | Valor |
|--------|--------|
| ID | SPEC-PRD-003 |
| Versión | 0.2.0 |
| Estado | Approved |
| Fecha | 2026-08-17 |
| Fuentes | `handbook/03-mvp-definition.md`, `handbook/04-product-roadmap.md` (UX demo), SPEC-PRD-002 |
| ADRs relacionados | ADR-002 (Web-only); sin librería UI externa en este alcance |
| Backlog | PBI-013 (tokens/shell 0.1.x); PBI-015 (jerarquía IA freeze) |
| Derivados | Implementación en `src/ShiftFlow.Web`; worklog ATF |

---

## 1. Contexto

El journey de demo (SPEC-PRD-002) es funcional y PBI-013 cubrió design system CSS + shell. La organización visual sigue siendo un CRUD apilado (alta encima del inventario; detalle de org como scroll de formularios; calendario debajo del formulario de asignar). Esta revisión fija NFR **observables de jerarquía de información** para el freeze, sin cambiar reglas de dominio ni el flujo de negocio.

Fuente canónica de NFR visual/UX demo: **este documento**. No se duplican tokens ni wireframes en handbook ni ADRs salvo decisión de stack.

---

## 2. Alcance

**In**

- Superficies Web: login, home, nav, organizaciones, detalle de org, calendario mensual, ausencias.
- Design system CSS propio (tokens, tipografía, estados vacío/error/loading, pestañas, panel de alta).
- Organización activa en el shell, compartida entre Calendario y Ausencias.
- Jerarquía de marca en login; shell operativo en páginas autenticadas.

**Out**

- Librerías UI Blazor (Mud/Fluent), dark mode, MAUI, rebranding sectorial (hospital/policía embebido).
- Drag-and-drop de turnos, vista semanal, endpoints nuevos, cambios de Domain/API.

---

## 3. Criterios de aceptación

### AC-UX-01 — Login como composición de marca

**Dado** un visitante en `/login`  
**Cuando** carga el primer viewport  
**Entonces** el nombre de producto **ShiftFlow** es la señal dominante; hay un titular breve, el formulario de credenciales y un CTA primario; no hay dashboard ni bloques secundarios competidores.

### AC-UX-02 — Shell autenticado

**Dado** un Administrator autenticado  
**Cuando** navega entre Home, Organizaciones, Calendario y Ausencias  
**Entonces** ve una barra de navegación estable con marca, enlaces con indicación de ruta activa, identidad de usuario y acción Salir; el contenido vive en un contenedor de lectura con ancho máximo.

### AC-UX-03 — Estados de maestros

**Dado** las pantallas de organizaciones / detalle  
**Cuando** hay carga, lista vacía o error de Api  
**Entonces** se muestra un estado explícito (texto de carga, empty state con siguiente acción, o alerta de error legible — no solo un código HTTP crudo sin contexto).

### AC-UX-04 — Responsive básico

**Dado** viewport ~375px y ~1280px  
**Cuando** se recorre login, listado de organizaciones y calendario  
**Entonces** el contenido no desborda horizontalmente de forma inutilizable; formularios, nav y panel de asignar permanecen usables (en viewport estrecho el mes puede pasar a listado).

### AC-UX-05 — Sin dependencia UI externa

**Dado** el proyecto `ShiftFlow.Web`  
**Cuando** se inspeccionan referencias de paquetes  
**Entonces** el rediseño no introduce MudBlazor, Fluent UI Blazor u otro kit como dependencia del MVP (CSS + fuentes web sí permitidos).

### AC-UX-06 — Detalle de organización por pestañas

**Dado** un Administrator en el detalle de una organización  
**Cuando** carga el primer viewport  
**Entonces** ve cabecera (nombre y estado) y pestañas **Personal**, **Tipos de turno** y **Ajustes**; la pestaña inicial es Personal (no el formulario de rename/descanso).

### AC-UX-07 — Inventario antes que alta

**Dado** las pantallas de listado de organizaciones y de ausencias activas  
**Cuando** hay al menos un registro  
**Entonces** el inventario es el bloque principal; el formulario de alta no ocupa el primer tercio del viewport (se abre con una acción «Nueva…» / «Registrar…», o queda visible si la lista está vacía).

### AC-UX-08 — Calendario como artefacto dominante

**Dado** un Administrator en `/calendar` con organización y maestros  
**Cuando** carga el mes  
**Entonces** la grilla mensual es el artefacto dominante; asignar turno vive en un panel secundario; un clic en un día del mes rellena la fecha del formulario; un rechazo HR-* se muestra junto a ese panel, no como único bloque encima de toda la página.

### AC-UX-09 — Organización activa en el shell

**Dado** un Administrator con al menos una organización  
**Cuando** cambia la organización en la barra de navegación y abre Calendario o Ausencias  
**Entonces** ambas pantallas usan esa organización sin un selector duplicado en el cuerpo de la página.

### AC-UX-10 — Copy de producto y Home operativo

**Dado** Home autenticado y los ledes de Organizaciones, Calendario y Ausencias  
**Cuando** un evaluador lee el primer viewport  
**Entonces** el texto es de producto (planificar, asignar, ausencias); no aparecen «AC-01», «Sprint» ni «journey demo» en ledes; Home no es un hero de marca a tamaño display; los códigos de regla (HR-*) sí pueden aparecer en la alerta de rechazo.

---

## 4. Dirección visual (no normativa de producto)

Orientación de implementación (puede evolucionar sin enmendar el journey):

- Tema claro de consola operativa: tinta oscura, acento teal, títulos de pantalla en fuente UI; Fraunces reservada a marca (login/nav).
- Fondo calmo (color o degradado suave); atmósfera decorativa mínima.
- Botones de formulario con radio contenido (no pastilla); motion sobrio; respetar `prefers-reduced-motion`.

---

## 5. Fuera de alcance / no criterios

- Pixel-perfect Figma; auditoría WCAG completa AA formal (sí: foco visible y contraste razonable).
- Cambiar pasos de SPEC-PRD-002 ni mensajes INV-* del dominio.

---

## 6. Gate 0

| Ítem | Estado |
|------|--------|
| Spec | Approved 0.2.0 |
| PBI | PBI-015 (jerarquía); PBI-013 (histórico tokens) |
| ADR | N/A (CSS propio; sin cambio de stack) |
| Worklog | `worklogs/PBI-015-ux-ia-freeze/` |

---

## Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.0 | 2026-08-10 | Draft inicial NFR UI demo |
| 0.1.1 | 2026-08-10 | Approved tras revisión humana |
| 0.2.0 | 2026-08-14 | Draft: AC-UX-06…10 jerarquía IA |
| 0.2.0 | 2026-08-17 | Approved tras confirmación humana |
