# Runbook local — ShiftFlow MVP

| Campo | Valor |
|--------|--------|
| Versión | 0.3.0 |
| Fecha | 2026-08-09 |
| Relacionado | PBI-001…003, ADR-001, ADR-004, ADR-005, C-LOC, C-AUTH, C-ORG |

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

### Usuario demo (PBI-002 / ADR-005)

| Campo | Valor |
|-------|--------|
| Usuario | `demo.admin` |
| Rol | `Administrator` |
| Contraseña (desarrollo) | `ChangeMe!123` si no hay override |

Override recomendado (no commitear secretos):

```powershell
dotnet user-secrets set "Authentication:DemoUser:Password" "<tu-password>" --project src/ShiftFlow.Api
```

O variable de entorno: `Authentication__DemoUser__Password`.

Login Web: `/login`. Endpoints Api: `POST /api/auth/login`, `POST /api/auth/logout`, `GET /api/auth/me`.

### Maestros (PBI-003 / PBI-004)

API (rol `Administrator`):

- `POST/GET /api/organizations`, `GET /api/organizations/{id}`, `PUT .../name`, `PUT .../active`
- `POST/GET /api/organizations/{id}/departments`, `PUT /api/departments/{id}/name|active`
- `POST/GET /api/organizations/{id}/employees`, `GET /api/departments/{id}/employees`, `PUT /api/employees/{id}`, `PUT .../active`
- `POST/GET /api/organizations/{id}/shift-types`, `PUT /api/shift-types/{id}`, `PUT .../active`

Colección Postman: `postman/ShiftFlow-PBI-003-auth-masters.postman_collection.json` (ver `postman/README.md`).

El esquema se crea con `EnsureCreated`. Si ya tenías un volumen Postgres sin tablas nuevas, **resetea el volumen** (§6) y vuelve a arrancar.

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

Ver §3 (usuario `demo.admin`, rol `Administrator`, contraseña vía user-secrets/env o default de desarrollo).
