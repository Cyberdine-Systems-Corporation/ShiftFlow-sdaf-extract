# ADR-002 — Cliente Web-only en el MVP (diferir MAUI)

| Campo | Valor |
|--------|--------|
| Estado | Aceptado |
| Fecha | 2026-08-05 |
| Decisores | Director técnico / Architecture Agent |
| Relacionado | `handbook/03-mvp-definition.md`, `handbook/04-product-roadmap.md`, ADR-001 |

---

## Contexto

El stack de plataforma contempla Blazor Web App y, a medio plazo, .NET MAUI Blazor Hybrid.  
La capacidad del MVP es ~96 h y el DoD exige demo local reproducible, no dos superficies de cliente.

Hay que decidir si MAUI entra en `mvp-0.1` o se difiere.

---

## Decisión

1. El **único cliente de usuario del MVP** es **Blazor Web App**.
2. **.NET MAUI Blazor Hybrid** queda **fuera del MVP** (Out explícito).
3. La UI se diseñará de forma que, post-MVP, se pueda compartir Razor/componentes con MAUI sin reescribir el Domain/Application.
4. No se abren proyectos MAUI en la solución hasta un ADR post-MVP que reactive esta decisión.

---

## Alternativas consideradas

| Alternativa | Motivo de rechazo (MVP) |
|-------------|-------------------------|
| Web + MAUI desde el día 1 | Duplica integración, empaquetado y superficie de bugs; ~96 h insuficientes |
| Solo MAUI (sin Web) | Peor para demo multiplataforma en navegador y evaluación rápida local |
| App nativa no-Blazor | Fuera de stack y de alcance |

---

## Consecuencias

### Positivas

- Enfoque total en un flujo demo Web + API + Postgres local.
- Menos proyectos en el skeleton Aspire.
- Cumple el corte Approved del handbook (cliente Web-only).

### Negativas / costes

- No hay instalable móvil/desktop nativo en la demo del 22 ago.
- Habrá trabajo post-MVP para extraer UI compartible si MAUI se activa.

### Relación con presentación

Las slides/vídeo del MVP deben presentar Web como superficie actual y MAUI como evolución, no como entregable `mvp-0.1`.
