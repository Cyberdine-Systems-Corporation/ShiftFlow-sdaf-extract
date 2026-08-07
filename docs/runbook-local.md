# Runbook local — ShiftFlow MVP

| Campo | Valor |
|--------|--------|
| Versión | 0.1.0 |
| Fecha | 2026-08-07 |
| Relacionado | PBI-001, ADR-001, ADR-004, C-LOC |

---

## 1. Prerrequisitos

| Herramienta | Notas |
|-------------|--------|
| .NET SDK **10** | `dotnet --version` ≥ 10.0 |
| Docker Desktop (o motor compatible) | Necesario para PostgreSQL vía Aspire AppHost o Compose |
| Git | Clonar el repo |

Opcional: Visual Studio 2022 / VS Code / Cursor con workload ASP.NET.

---

## 2. Clonar y restaurar

```powershell
git clone <url-del-repo> ShiftFlow
cd ShiftFlow
dotnet restore ShiftFlow.sln
```

---

## 3. Arranque canónico (Aspire AppHost)

Desde la raíz del repo:

```powershell
dotnet run --project src/ShiftFlow.AppHost
```

Aspire levantará:

1. Contenedor **PostgreSQL** (`postgres` → DB `shiftflow`)
2. **ShiftFlow.Api**
3. **ShiftFlow.Web**

Abre el dashboard de Aspire (URL en la consola) para ver endpoints HTTP de Api y Web.

Comprobación rápida:

- Api: `GET /api/status` → JSON con `"status":"ok"`
- Web: página de inicio “ShiftFlow” (consulta el status de la Api)
- Health Aspire (Development): `/health`, `/alive`

---

## 4. Contingencia: solo PostgreSQL con Compose

Si AppHost no puede orquestar contenedores:

```powershell
docker compose up -d
dotnet run --project src/ShiftFlow.Api
dotnet run --project src/ShiftFlow.Web
```

Connection string por defecto (también en `src/ShiftFlow.Api/appsettings.json`):

```text
Host=localhost;Port=5432;Database=shiftflow;Username=shiftflow;Password=shiftflow
```

> Credenciales solo para desarrollo local. No usar en ningún entorno compartido.

---

## 5. Compilar y tests

```powershell
dotnet build ShiftFlow.sln
dotnet test ShiftFlow.sln
```

---

## 6. Parar y resetear datos

| Acción | Comando |
|--------|---------|
| Parar AppHost | `Ctrl+C` en la consola del AppHost |
| Parar Compose | `docker compose down` |
| Borrar volumen Postgres (Compose) | `docker compose down -v` |
| Volumen Aspire | Eliminar el volumen Docker creado por el recurso `postgres` (Docker Desktop → Volumes) |

---

## 7. Troubleshooting

| Síntoma | Qué revisar |
|---------|-------------|
| AppHost no arranca Postgres | Docker Desktop en ejecución; WSL2/backend activo |
| Puerto 5432 ocupado | Detener otro Postgres local o cambiar el mapeo en Compose |
| `docker` no reconocido | Instalar Docker Desktop y reiniciar la terminal |
| Api `database: unreachable` | Postgres aún no listo; esperar healthcheck o `docker compose ps` |
| Web no ve la Api | Arrancar vía AppHost (inyecta service discovery) o configurar base address manualmente |
| SDK incorrecto | Este skeleton usa **net10.0**. Instala .NET 10 SDK (`dotnet --list-sdks`) |
| Dashboard Aspire: `UntrustedRoot` / gRPC SSL | `dotnet dev-certs https --trust` (aceptar el diálogo de Windows). Cerrar navegadores y reiniciar el AppHost. |

---

## 8. Usuario demo

Auth y usuarios demo: **fuera de PBI-001** (PBI-002).
