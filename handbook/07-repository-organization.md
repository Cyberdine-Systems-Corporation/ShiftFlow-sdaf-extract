# 07 — Repository Organization

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | II — SDAF |
| **Norma superior** | [05-sdaf-framework.md](05-sdaf-framework.md) |
| **Deriva hacia** | Specs, ADRs, agentes, prompts, worklogs, `src/`, `tests/` |

---

## 1. Propósito

Definir la organización del repositorio como **almacén de conocimiento, decisiones, especificaciones, trazabilidad y código**.

El repo no es solo `src/`.

---

## 2. Árbol normativo

```text
/
├── README.md                 # entrada humana del repo (cuando exista)
├── AGENTS.md                 # router de agentes (cuando exista)
├── handbook/                 # constitución (este handbook)
├── knowledge/
│   ├── raw/                  # fuentes originales (append-only)
│   └── curated/              # extracciones markdown (append-only)
├── specs/
│   ├── product/
│   ├── domain/
│   ├── application/
│   └── acceptance/
├── architecture/
│   ├── decisions/            # ADRs
│   ├── contexts/
│   └── c4/
├── backlog/
├── agents/                   # contratos operativos por agente
├── prompts/
│   ├── system/
│   ├── agents/
│   ├── planning/
│   ├── review/
│   ├── documentation/
│   └── quality/
├── worklogs/                 # Agent Traceability Framework
├── templates/
├── docs/                     # HOWTO / presentaciones / runbooks (no sustituye handbook ni specs)
│   └── presentation/         # slides MVP (p. ej. mvp-0.1/)
├── src/                      # solución .NET
├── tests/
├── .cursor/rules/            # reglas finas del IDE; apuntan al handbook, no lo duplican
└── .github/
```

---

## 3. Responsabilidad por carpeta

| Carpeta | Contiene | No contiene |
|---------|----------|-------------|
| `knowledge/` | Evidencia de expertos; inmutable | Specs “mejoradas”, código |
| `handbook/` | Norma constitucional | Detalle táctico de un PBI |
| `specs/` | Verdad operativa para implementar | Ensayos de diseño sin aceptación |
| `architecture/decisions/` | ADRs | Tutoriales largos |
| `backlog/` | PBIs / historias trazables a specs | Implementación |
| `agents/` | Contratos de agente | Prompts completos (viven en `prompts/`) |
| `prompts/` | Prompts versionados | Instrucciones ad hoc no registradas |
| `worklogs/` | Iteraciones ATF | Sustituto de commits o specs |
| `docs/` | Runbooks, presentación MVP, HOWTO | Constitución ni specs canónicas |
| `src/`, `tests/` | Código y pruebas | Knowledge crudo |

---

## 4. Reglas de `knowledge/`

1. `raw/` conserva originales (p. ej. DOCX) sin reescritura silenciosa.
2. `curated/` solo añade extracciones; no “corrige” al experto — las interpretaciones van a `specs/`.
3. Nunca se borra knowledge para encajar el código.

---

## 5. Reglas de separación Knowledge / Specs / Código

```text
knowledge  →  qué dijo el experto
specs      →  qué acordamos construir (interpretado, testeable)
src/tests  →  cómo quedó construido
```

Si el código descubre un error de spec: se enmienda la spec (y el test), no se deja el hallazgo solo en un comentario.

---

## 6. Idioma y nombres

- Artefactos de ingeniería en **castellano** (contenido).
- Nombres de carpetas del árbol SDAF en inglés corto estable (`specs`, `handbook`, …) para tooling.
- Código: convención que fije el ADR de coding standards (puede ser inglés idiomático .NET).

---

## 7. Qué queda fuera de este capítulo

- Formato exacto de una spec → cap. 08  
- Flujo día a día y gates → cap. 09  
- Layout de proyectos .NET en `src/` → Parte III / ADR de solución  

---

## 8. Criterios de aceptación de este capítulo (H4)

- [ ] El árbol coincide con el repo real (o declara deltas pendientes explícitos).
- [ ] `docs/` no compite con handbook/specs como fuente de verdad.
- [ ] `knowledge/` queda claramente append-only / inmutable en intención.

---

## 9. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H4); incluye `docs/presentation` del MVP |
