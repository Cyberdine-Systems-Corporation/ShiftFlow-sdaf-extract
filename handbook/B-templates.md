# Apéndice B — Templates

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | Apéndices |
| **Norma superior** | Caps. 08, 09, 14, 15; copias operativas en `templates/` |

---

## 1. Propósito

Índice de plantillas. Las copias editables deben vivir en `templates/` del repo; aquí se define el contenido mínimo.

---

## 2. Plantilla — Spec (cabecera)

```markdown
# SPEC-XXX-000 — Título

| Campo | Valor |
|--------|--------|
| ID | SPEC-XXX-000 |
| Versión | 0.1.0 |
| Estado | Draft |
| Fecha | YYYY-MM-DD |
| Fuentes | knowledge/... |
| ADRs | ADR-... |
| Backlog | PBI-... |

## Contexto
## Alcance
## Criterios de aceptación
### Dado / Cuando / Entonces
## Fuera de alcance
## Historial
```

---

## 3. Plantilla — ADR

```markdown
# ADR-XXX — Título

| Campo | Valor |
|--------|--------|
| Estado | Propuesto / Aceptado / Deprecado |
| Fecha | YYYY-MM-DD |

## Contexto
## Decisión
## Alternativas consideradas
## Consecuencias
```

---

## 4. Plantilla — Worklog (ATF)

```markdown
# PBI-XXX / Iteration-NNN

| Campo | Valor |
|--------|--------|
| Fecha | |
| Agente | |
| Modelo | |
| Versión prompt | |
| Contexto | |
| Especificaciones utilizadas | |
| Archivos leídos | |
| Archivos modificados | |
| Resultado | |
| Tiempo | |
| Coste | N/D |
| Observaciones | |
| Pruebas ejecutadas | |
| Estado | en_curso / hecho / bloqueado / abortado |
| Siguiente agente | |
```

---

## 5. Plantilla — Contrato de agente

Ver secciones obligatorias del capítulo 13; archivo `agents/<nombre>.md`.

---

## 6. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H8) |
