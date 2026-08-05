# 18 — DevOps

| Campo | Valor |
|--------|--------|
| **Versión** | 0.1.1 |
| **Estado** | Approved |
| **Fecha** | 2026-08-05 |
| **Parte** | V — Calidad y entrega |
| **Norma superior** | [03-mvp-definition.md](03-mvp-definition.md), [10-solution-architecture.md](10-solution-architecture.md), [07-repository-organization.md](07-repository-organization.md) |
| **Deriva hacia** | AppHost, Docker, runbooks en `docs/`, `.github/` |

---

## 1. Propósito

Definir DevOps del **MVP**: prioridad absoluta al **runtime local autocontenido**. Cloud no es camino de demo.

---

## 2. Objetivos MVP

1. Un evaluador levanta API + Web + PostgreSQL en local con runbook.
2. Tests automatizados ejecutables en local (incl. Testcontainers donde aplique).
3. Observabilidad mínima: Serilog a consola/archivo.
4. Etiquetado `mvp-0.1` al freeze.

---

## 3. Componentes

| Componente | Uso MVP |
|------------|---------|
| .NET Aspire AppHost | Orquestación preferente de dependencias |
| Docker Compose | Alternativa o complemento documentado |
| PostgreSQL container | Datos |
| `.github/` | Opcional: checks básicos; no sustituye local |
| Secretos | User-secrets / env locales; nunca commit |

---

## 4. Runbook (obligatorio)

Debe vivir en el repo (`docs/` o README) y cubrir:

1. Prerrequisitos (SDK .NET, Docker).
2. Clonado y restore.
3. Comando de arranque (Aspire y/o compose).
4. URLs, usuario demo, cómo parar y resetear datos.
5. Troubleshooting corto (puerto ocupado, contenedor no arranca).

---

## 5. Entornos

| Entorno | MVP |
|---------|-----|
| Local desarrollador / evaluador | **Sí — canónico** |
| CI cloud | Opcional |
| Staging/prod cloud | **Out** del cierre MVP |

---

## 6. Criterios de aceptación de este capítulo (H8)

- [ ] Local-first queda inequívoco.
- [ ] Runbook es artefacto mandatory.
- [ ] Aspire/Compose están contemplados sin exigir cloud.

---

## 7. Historial

| Versión | Fecha | Cambio |
|---------|--------|--------|
| 0.1.1 | 2026-08-05 | Approved tras revisión humana |
| 0.1.0 | 2026-08-05 | Borrador inicial (sesión H8) |
