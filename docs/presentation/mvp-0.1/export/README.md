# Exports Marp (`mvp-0.1`)

Binarios derivados de las fuentes `.md` del directorio padre. No editar a mano: regenerar desde Marp.

| Fuente | PDF | PPTX |
|--------|-----|------|
| `../product-slides.md` | [product-slides.pdf](product-slides.pdf) | [product-slides.pptx](product-slides.pptx) |
| `../video-slides.md` | [video-slides.pdf](video-slides.pdf) | [video-slides.pptx](video-slides.pptx) |

La extensión Marp de VS Code / Cursor exporta **junto al `.md`**. Tras exportar, mover el fichero aquí (o «Guardar como» en esta carpeta).

CLI (crea `export/` si no existe):

```powershell
New-Item -ItemType Directory -Force -Path docs/presentation/mvp-0.1/export | Out-Null
npx --yes @marp-team/marp-cli docs/presentation/mvp-0.1/product-slides.md --pdf -o docs/presentation/mvp-0.1/export/product-slides.pdf
npx --yes @marp-team/marp-cli docs/presentation/mvp-0.1/video-slides.md --pdf -o docs/presentation/mvp-0.1/export/video-slides.pdf
```
