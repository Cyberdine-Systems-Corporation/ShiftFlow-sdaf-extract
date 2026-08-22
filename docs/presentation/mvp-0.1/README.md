# Presentación MVP `mvp-0.1`

| Campo | Valor |
|--------|--------|
| Spec | SPEC-PRD-001 (C-PRE), handbook 03 §4.5 |
| PBI | [PBI-012](../../../backlog/PBI-012-presentacion-slides-video.md) |
| Tag / release | [`mvp-0.1`](https://github.com/mortiz-iadev/ShiftFlow/releases/tag/mvp-0.1) (código 2026-08-17; assets de presentación 2026-08-21/22) |

## Publicación (C-PRE)

GitHub Releases **no reproduce** el MP4 en el navegador: hay que **descargarlo**.

| Campo | Valor |
|--------|--------|
| Release | <https://github.com/mortiz-iadev/ShiftFlow/releases/tag/mvp-0.1> |
| Vídeo | [`ShiftFlow-mvp-0.1-arquitectura-gobernanza.mp4`](https://github.com/mortiz-iadev/ShiftFlow/releases/download/mvp-0.1/ShiftFlow-mvp-0.1-arquitectura-gobernanza.mp4) |
| Duración | 10:01 (1080p, H.264 + AAC; láminas + locución, sin cámara) |
| SHA-256 (MP4) | `7c54313e8b340713190123f5a1f15b7ec7090abd020d9e9ab656653110da30b6` |
| Deck de producto | [`ShiftFlow-mvp-0.1-producto.pptx`](https://github.com/mortiz-iadev/ShiftFlow/releases/download/mvp-0.1/ShiftFlow-mvp-0.1-producto.pptx) |
| Fecha de assets | vídeo 2026-08-21 · PPTX 2026-08-22 |

El vídeo cubre arquitectura y método (SDAF). El PPTX cubre el journey de la aplicación. Juntos cumplen handbook 03 §4.5.

## Artefactos en el repo

| Artefacto | Estado | Ruta |
|-----------|--------|------|
| **Vídeo** (arquitectura + método IA, 10:01) | Publicado en el release `mvp-0.1` | [descarga](https://github.com/mortiz-iadev/ShiftFlow/releases/download/mvp-0.1/ShiftFlow-mvp-0.1-arquitectura-gobernanza.mp4) |
| Guion del vídeo | Listo | [guion-video-arquitectura.md](guion-video-arquitectura.md) |
| Láminas del vídeo (17) | Fuente + PDF/PPTX | [video-slides.md](video-slides.md) · [export/](export/) |
| **Deck de producto** (14 láminas, puente en la 5) | Fuente + capturas 01–07 | [product-slides.md](product-slides.md) |
| Exports (PDF / PPTX) | En `export/` y el PPTX de producto también en el release | [export/](export/) |
| Inventario de capturas | 01–07 (2026-08-19) | [captures/README.md](captures/README.md) |

Los dos artefactos son **complementarios**: el vídeo no sustituye el journey de la app; el deck no detalla SDAF. Juntos cubren Producto + Evolución + Arquitectura de handbook §4.5 (el puente es la lámina 5 del deck + el cierre del vídeo).

Audiencia del vídeo: **evaluador de máster en desarrollo asistido por IA**. Registro de decisiones y método, no repaso de tecnologías.

Audiencia del deck: la misma, en defensa oral **sin** el vídeo. Recorre el producto (problema, alcance, journey, arranque) y deja la arquitectura en un mapa de cinco piezas.

## Cómo usar el deck de producto

1. Abrir `product-slides.md` con **Marp**, o exportar:

   ```powershell
   npx --yes @marp-team/marp-cli docs/presentation/mvp-0.1/product-slides.md --pdf -o docs/presentation/mvp-0.1/export/product-slides.pdf
   ```

2. Las láminas 7–11 embeben las PNG de [captures/](captures/). Tras cambiar capturas, **reexportar** PDF/PPTX a [export/](export/).
3. No improvisar SDAF ni puertas: si preguntan por método, apuntar al vídeo.

**Convención de exports:** las fuentes `.md` viven en este directorio; PDF y PPTX van a [export/](export/). La extensión Marp de VS Code / Cursor escribe junto al `.md`: tras exportar, mover el fichero a `export/` (o «Guardar como» ahí). Ya están los cuatro: `product-slides` y `video-slides` en PDF y PPTX.

## Cómo usar las láminas del vídeo (Fase 0)

1. Abrir `video-slides.md` con la extensión **Marp** en VS Code / Cursor, o exportar:

   ```powershell
   npx --yes @marp-team/marp-cli docs/presentation/mvp-0.1/video-slides.md --pdf -o docs/presentation/mvp-0.1/export/video-slides.pdf
   ```

2. Presentar a pantalla completa. El guion abre con la tabla **bloque → láminas** (17 láminas sobre 8 bloques).
3. Leer [guion-video-arquitectura.md](guion-video-arquitectura.md) en voz alta; no improvisar listas de ADRs.

Las láminas del vídeo llevan **más detalle que la locución** a propósito: identificadores de ADR y spec, semántica exacta de HR-01…03, grafo de dependencias y cadena de trazabilidad. Quien escucha sigue el argumento; quien pausa puede verificarlo. Las láminas 2, 6, 9 y 14 **no se narran**: se dejan en pantalla 2–3 s extra.

Las cifras de la lámina 2 (20 specs, 7 ADRs, 61 worklogs, 13 contratos de agente…) son las del repositorio en la etiqueta `mvp-0.1`. Si se regraba más tarde, reverificarlas antes de exportar.

## Setup de grabación (histórico)

La locución se montó contra las 17 láminas (8 takes). El MP4 publicado es `ShiftFlow-mvp-0.1-arquitectura-gobernanza.mp4`. No grabar clics de la app.
