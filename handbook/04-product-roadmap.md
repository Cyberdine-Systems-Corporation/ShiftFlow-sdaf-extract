# 04 — Product Roadmap

| Campo | Valor |
|--------|--------|
| **Versión** | 0.2.0 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | I — Constitución de producto |
| **Norma superior** | [03-mvp-definition.md](03-mvp-definition.md) |
| **Deriva hacia** | `backlog/`, planificación de sprints, Parte VI del handbook |

---

## 1. Propósito de este capítulo

Ordenar el trabajo del periodo **1–22 de agosto de 2026** hacia el MVP definido en el capítulo 03, con capacidad humana de referencia (~96 h) y doble entregable (producto + SDAF).

No sustituye el backlog detallado: lo enmarca.

---

## 2. Capacidad

| Tipo de día | Horas |
|-------------|------:|
| Lunes–viernes | 5 |
| Sábado–domingo | 3 |
| **Total 1–22 ago** | **~96** |

Cualquier ampliación de alcance debe compensarse recortando otra partida In del capítulo 03.

---

## 3. Sprints

### Sprint 0 — Fundación SDAF (1–3 ago, ~13 h)

**Objetivo:** repo gobernado; sin features de producto.

- Estructura de carpetas SDAF.
- Co-creación handbook (H1–H5 prioritarias).
- ADRs 001–003 (stack, Web-only, motores).
- Specs de producto MVP + backlog priorizado.
- Skeleton solución .NET (Aspire mínimo + API + Domain + Application + Infra + Web + Tests).
- Prompts system + agentes activos.

**DoD:** estructura en git; handbook usable Parte I–II; ≥3 ADRs; backlog MVP; solución compila; gate “no code without spec” documentado.

### Sprint 1 — Núcleo (4–9 ago, ~28 h)

**Objetivo:** maestros + auth + shell Web.

- Specs e implementación: Organization, Department, Employee, ShiftType.
- EF Core + PostgreSQL (Aspire/Testcontainers).
- Auth y roles básicos.
- Blazor: navegación y CRUD de maestros.

**DoD:** datos maestros persistidos; Blazor navega CRUD; tests de aggregates críticos; worklogs al día.

### Sprint 2 — Calendario y asignación (10–15 ago, ~31 h)

**Objetivo:** flujo demo de planificación manual.

- Schedule/Shift, asignación manual, domain events.
- Rule Engine v1 (≤3 hard rules).
- Leaves básicos integrados en reglas.
- Blazor calendario mensual + feedback de violaciones.
- Acceptance tests del flujo crítico.

**DoD:** demo org → empleados → tipos → calendario → asignar/rechazar → ausencia.

### Sprint 3 — Pulido y cierre demo (16–22 ago, ~24 h)

**Objetivo:** demostración reproducible y SDAF cerrado para continuar.

- Stub IA: explicación de regla (sin mutación).
- UX demo, Serilog, **runbook de arranque local** (Aspire/Docker; sin cloud).
- Completar prompts stubs; handbook Partes III–V mínimas.
- Quality gates; specs de dominio avanzado solo documentadas.
- **Deck de slides** + **vídeo de presentación** (producto, evolución, arquitectura) — cierre mandatory (§4.5 del cap. 03).
- Freeze demo; etiqueta `mvp-0.1`; retrospectiva SDAF.

**DoD:** demo <15 min; prompt library usable; trazabilidad del camino crítico; cero features In sin spec; slides + vídeo publicados/referenciados en el repo.

---

## 4. Hitos

| Fecha | Hito |
|-------|------|
| 3 ago | Fin Sprint 0 — fundación SDAF |
| 9 ago | Fin Sprint 1 — núcleo maestros |
| 15 ago | Fin Sprint 2 — asignación validada |
| 22 ago | Release demo `mvp-0.1` + slides + vídeo de presentación |

---

## 5. Post-MVP (dirección, sin fechas firmes)

Orden sugerido tras el 22 ago:

1. Ampliar Rule Engine (más hard/soft desde specs de dominio).
2. MAUI Hybrid compartiendo UI Razor, si el Web está estable.
3. Colaboración en vivo (SignalR) si hay escenario real.
4. Generación asistida de cuadrantes con confirmación humana.
5. Optimization Engine.
6. Multitenancy e integraciones.

---

## 6. Gestión de desvíos

- Si se pierde capacidad: recortar en este orden — pulido UX → stub IA → leaves no críticos → un hard rule opcional del descanso mínimo; **nunca** el gate SDAF, el flujo demo mínimo (maestros + asignar/rechazar solape), el runtime local, ni el cierre slides+vídeo.
- Si se gana capacidad: documentar reglas avanzadas en specs; no abrir Out del capítulo 03 sin enmienda.

---

## 7. Criterios de aceptación de este capítulo (H3)

- [ ] Los cuatro sprints cuadran con ~96 h y el capítulo 03.
- [ ] Queda claro qué es hito de MVP vs post-MVP.
- [ ] Existe política de recorte ante desvíos.

---

## 8. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.2.0 | 2026-08-05 | Enmienda Approved: slides + vídeo como cierre del Sprint 3 / hito 22 ago |
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H3) |
