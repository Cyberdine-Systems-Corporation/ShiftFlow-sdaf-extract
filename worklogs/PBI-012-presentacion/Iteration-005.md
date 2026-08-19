# PBI-012-presentacion / Iteration-005

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-18 |
| Agente | documentación (deck de producto) |
| Modelo | Cursor Grok 4.6 |
| Versión prompt | N/A (docs C-PRE; sin `src/`) |
| Skills | `sdaf-worklog-handoff@0.1.0` |
| Contexto | Siguiente paso de PBI-012 tras el guion y las 17 láminas del vídeo: el deck de producto aún no existía. Complementario al vídeo; cubre Producto + Evolución + puente de Arquitectura (handbook 03 §4.5) sin SDAF ni puertas. |
| Especificaciones utilizadas | SPEC-PRD-001 C-PRE; SPEC-PRD-002 0.1.1 (journey 9 pasos, AC-01…05); SPEC-PRD-003 0.2.0 (AC-UX-06…10, copy de producto); handbook 03 §4.5 y §5 Out; handbook 04 sprints y post-MVP; runbook local 0.6.0 §3 / §3.2 / §3.3 |
| Archivos leídos | `docs/runbook-local.md`; SPEC-PRD-002; SPEC-PRD-003; handbook 03 §4.5; handbook 04; `Home.razor` / `Login.razor`; PBI-012; README de presentación |
| Archivos modificados | `docs/presentation/mvp-0.1/product-slides.md` (nuevo, 14 láminas); `docs/presentation/mvp-0.1/captures/README.md`; `docs/presentation/mvp-0.1/README.md`; `backlog/PBI-012-presentacion-slides-video.md`; `backlog/README.md`; este worklog. Rama `docs/pbi-012-presentacion`. |
| Resultado | Deck de producto en Marp: problema, valor, In/Out, **lámina 5 puente** (stack en 5 filas + «detalle en el vídeo»), dos caminos de demo, login/shell, maestros por pestañas, calendario, rechazo explicado, ausencias, arranque en frío, camino 1–22 ago, cierre. Siete huecos de captura documentados; PNG no incluidos. |
| Tiempo | ~0,6 h |
| Coste | N/D |
| Observaciones | Las láminas 7–11 son usables en defensa oral sin PNG (recuadro + descripción). No se ha grabado el vídeo. No se ha commiteado. Export Marp no ejecutado: `npx` no está en el PATH de esta máquina (Node no instalado). |
| Pruebas ejecutadas | Intento de `npx @marp-team/marp-cli` → CommandNotFoundException. Revisión humana del markdown. |
| Estado | hecho |
| Siguiente agente | humano: (1) tomar las 7 capturas del catálogo de demo; (2) ensayo + grabación del vídeo 8–10 min; (3) URL/checksum del MP4 |
