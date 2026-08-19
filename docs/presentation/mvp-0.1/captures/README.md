# Capturas del deck de producto

PNG de las láminas 7–11 de [product-slides.md](../product-slides.md). Verificadas 2026-08-19.

## Cómo retomarlas

1. Arrancar con el [runbook](../../../runbook-local.md) (`--launch-profile https`).
2. Login `demo.admin`. Elegir `Demo — Operación` en la barra (salvo la 6b).
3. Ventana ~1280×800 o mayor; tema claro; sin DevTools ni barra de Favoritos si se puede.
4. Recortar al contenido de la app (incluir la barra de navegación de ShiftFlow, no la del navegador).
5. Sobrescribir aquí con el nombre exacto de la tabla y reexportar PDF/PPTX a `../export/`.

## Inventario

| Archivo | Ruta | Qué se ve | Lámina |
|---------|------|-----------|--------|
| `01-login.png` | `/login` | Marca **ShiftFlow**, formulario, CTA Entrar | 7 |
| `02-home.png` | `/` autenticado | Planificación, org activa, API lista, atajos | 7 |
| `03-organizations.png` | `/organizations` | Listado como bloque principal | 8 |
| `04-org-personal.png` | `/organizations/{id}` | Pestaña **Personal** activa | 8 |
| `05-calendar-ok.png` | `/calendar` | Grilla + panel; Ana/Bruno el 14 ago (día del seed) | 9 |
| `06-calendar-reject.png` | `/calendar` | HR-01 en Bruno 12:00–16:00 UTC el 14 ago | 10 |
| `07-leaves.png` | `/leaves` | Leave activo de Carla, 14–16 ago | 11 |

Opcional, no embebida: `06b-calendar-hr03.png` en `Demo — Descanso` (Diego 20:00–22:00 UTC) si en defensa oral se muestra HR-03.

En Marp las imágenes se referencian así: `![Login](captures/01-login.png)`.

No commitear capturas con datos que no sean el catálogo de demo. `03-organizations.png` incluye una org extra (`Policía Nacional`) ajena al seed; sustituir si se quiere una vitrina limpia.
