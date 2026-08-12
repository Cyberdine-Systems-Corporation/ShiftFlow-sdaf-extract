# ADR-006 — Tipado explícito sin var — Iteration 001

| Campo | Valor |
|--------|--------|
| Fecha | 2026-08-12 |
| Agente | Architecture / chore |
| Modelo | Cursor agent |
| Contexto | Norma de tipado explícito (contrato o implementación; sin `var`) |
| Archivos modificados | ADR-006; handbook 17; `.editorconfig`; regla Cursor; `.gitignore`; `src/**` y `tests/**` (sustitución de `var`); este worklog |
| Resultado | Norma documentada y aplicada en solución; 0 `var` en src/tests; suite verde |
| Pruebas ejecutadas | `dotnet test ShiftFlow.sln` → Unit 21 OK, Integration 24 OK |
| Estado | hecho |
| Siguiente agente | humano (commit/PR) |

## Notas

- Criterio: tipar con interfaz/puerto cuando se consume por abstracción; con tipo concreto cuando es implementación estable.
- Enforcement: `.editorconfig` (`csharp_style_var_* = false`) + checklist QG-Docs.
- Herramienta local `tools/VarRewriter` (gitignored) usada para reescritura masiva; Razor y deconstrucción tratados a mano.
- Ajuste post-rewriter: `Login.razor` — `out HeaderStringValues` en `NonValidated.TryGetValues` (el rewriter había inferido mal `IEnumerable<string>`).
