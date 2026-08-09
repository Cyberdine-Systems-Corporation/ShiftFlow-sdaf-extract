# ADR-006 — Estándares de código (legibilidad, regiones, XML docs)

| Campo | Valor |
|--------|--------|
| Estado | Aceptado |
| Fecha | 2026-08-09 |
| Decisores | Product Owner / Engineering |
| Relacionado | ADR-001, ADR-004, `handbook/07-repository-organization.md`, `handbook/17-code-review-and-quality-gates.md` |

## Contexto

El código del MVP debe ser legible para humanos y agentes. Hasta ahora no había norma explícita de regiones conceptuales, comentarios ni documentación XML, ni un gate de build que la exija. El handbook (§7) anticipaba un ADR de coding standards.

## Decisión

1. **Identificadores:** inglés idiomático .NET (tipos, miembros, namespaces).  
2. **Comentarios y XML docs:** castellano.  
3. **Regiones conceptuales** (`#region` / `#endregion`) en tipos no triviales, con nombres de frontera de responsabilidad (p. ej. `Factory`, `Invariants`, `Behavior`, `Commands`, `Queries`, `Endpoints`, `Mapping`). Orden estable dentro de la clase. Prohibidas regiones “misc” o de una sola línea sin frontera real.  
4. **Comentarios de línea/bloque:** solo en lógica de alto impacto o no obvia (invariantes sutiles, trade-offs, porqués Application vs Domain). No narrar lo que el identificador ya dice.  
5. **XML documentation** obligatoria en la API pública e `internal` visible de proyectos bajo `src/` (`<summary>`; `<param>` / `<returns>` / `<exception>` cuando aporten).  
6. **Enforcement:** `GenerateDocumentationFile` + warning `CS1591` tratado como error en `src/` (`Directory.Build.props`). `tests/` queda exento.  
7. **Review / PR:** checklist de `handbook/17` incluye regiones, comentarios y XML docs; sin `CS1591` limpio no hay merge.

Código tocado en un PR debe cumplir la norma en el diff. La deuda residual de archivos no tocados se salda al modificarlos o en chores dedicados.

## Alternativas consideradas

| Alternativa | Por qué no |
|-------------|------------|
| Solo checklist humano sin analizador | Se olvida; no bloquea build |
| StyleCop completo / EditorConfig agresivo | Coste alto para MVP; se puede ampliar post-MVP |
| Regiones prohibidas (estilo “flat”) | El equipo pide fronteras conceptuales explícitas para legibilidad |

## Consecuencias

- Builds de `src/` fallan sin XML docs en miembros públicos/`internal`.  
- Agentes deben aplicar ADR-006 (regla Cursor + prompts).  
- Más verbosidad en tipos públicos; mejor onboarding y review.
