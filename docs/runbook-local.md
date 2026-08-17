# Runbook local — ShiftFlow MVP

| Campo | Valor |
|--------|--------|
| Versión | 0.6.0 |
| Fecha | 2026-08-17 |
| Relacionado | PBI-001…015, ADR-001, ADR-002, ADR-004, ADR-005, ADR-007, C-LOC, C-AUTH, C-ORG, SPEC-PRD-002, SPEC-PRD-003 0.2.0 |

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
dotnet tool restore
```

---

## 3. Arranque canónico (Aspire AppHost)

Desde la raíz del repo:

```powershell
dotnet run --project src/ShiftFlow.AppHost --launch-profile https
```

`--launch-profile https` fija **Development** (`ASPNETCORE_ENVIRONMENT` / `DOTNET_ENVIRONMENT`). `dotnet run` sin `-c` compila en **Debug**. Eso es el camino de evaluador: el catálogo (`Demo:SeedCatalog`) solo está activo en Development. No hace falta `-c Release` para la demo.

**Depurar en Visual Studio / Cursor:** proyecto de arranque = `ShiftFlow.AppHost`, perfil de lanzamiento = **`https`** (no `http`).

Si usas el perfil `http`, Aspire exige `ASPIRE_ALLOW_UNSECURED_TRANSPORT=true` (ya está en ese perfil del AppHost). Sin eso verás:

`The 'applicationUrl' setting must be an https address unless the 'ASPIRE_ALLOW_UNSECURED_TRANSPORT' environment variable is set to true`.

No depurar solo `ShiftFlow.Web` o `ShiftFlow.Api` como arranque principal: el Web resuelve la Api vía service discovery de Aspire (`https+http://api`).

Aspire levantará:

1. Contenedor **PostgreSQL** (`postgres` → DB `shiftflow`, host port **5433** fijo)
2. **ShiftFlow.Api**
3. **ShiftFlow.Web**

Abre el dashboard de Aspire (URL en la consola) para ver endpoints HTTP de Api y Web.

Comprobación rápida:

- Api: `GET /api/status` → JSON con `"status":"ok"`
- Web: Home de planificación (tras login) y selector de organización en la barra
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

Login Web: `/login` (prefill demo). Tras login:

- Home de planificación (briefing; selector de org en la barra)
- `/organizations` — listado (inventario antes que alta)
- `/organizations/{id}` — detalle por pestañas Personal / Tipos / Ajustes
- `/calendar` — mes + aside de asignación; la org activa sale del shell
- `/leaves` — ausencias de la org del shell

Endpoints Api de auth: `POST /api/auth/login`, `POST /api/auth/logout`, `GET /api/auth/me`.

### Maestros (PBI-003 / PBI-004)

API (rol `Administrator`):

- `POST/GET /api/organizations`, `GET /api/organizations/{id}`, `PUT .../name`, `PUT .../active`
- `PUT /api/organizations/{id}/minimum-rest`
- `POST/GET /api/organizations/{id}/departments`, `PUT /api/departments/{id}/name|active`
- `POST/GET /api/organizations/{id}/employees`, `GET /api/departments/{id}/employees`, `PUT /api/employees/{id}`, `PUT .../active`
- `POST/GET /api/organizations/{id}/shift-types`, `PUT /api/shift-types/{id}`, `PUT .../active`

### Calendario / asignación (PBI-005)

- `GET /api/organizations/{id}/calendar?year=&month=`
- `POST /api/organizations/{id}/assignments`
- `POST /api/assignments/{id}/cancel`

### Ausencias (PBI-007)

- `GET/POST /api/organizations/{id}/leaves`
- `POST /api/leaves/{id}/cancel`

### Explicación de reglas (PBI-011)

- `GET /api/rules/explain?code=HR-01` (también HR-02 / HR-03; otro código → no soportado)
- El `400` de `AssignShift` incluye `title` / `body`; el calendario los muestra. La UI no reimplementa las hard rules.

Colección Postman: `postman/ShiftFlow-PBI-003-auth-masters.postman_collection.json` (auth + maestros + calendario + leaves + explain; ver `postman/README.md`).

### 3.1. Migraciones EF Core (ADR-007 / PBI-014)

Tras `dotnet tool restore` en la raíz:

```powershell
dotnet ef migrations add <Nombre> --project src/ShiftFlow.Infrastructure --startup-project src/ShiftFlow.Api --output-dir Persistence/Migrations --context ShiftFlowDbContext
```

Commitear los archivos generados en `src/ShiftFlow.Infrastructure/Persistence/Migrations/`. Al arrancar la Api se aplica `MigrateAsync` (PostgreSQL). Los tests de integración siguen usando SQLite + `EnsureCreated`.

Si el volumen se creó con `EnsureCreated` (antes de PBI-014), **resetea el volumen una vez** (§6) y vuelve a arrancar. Mezclar `EnsureCreated` y `Migrate` en la misma base no es compatible. Los cambios de modelo posteriores no exigen wipe si la migración es aditiva.

### 3.2. Catálogo de demo (PBI-010)

Con `Demo:SeedCatalog=true` (default en Development) y PostgreSQL, el arranque siembra dos organizaciones de vitrina **si no existen**. No corre en SQLite (tests). Desactivar: `"Demo": { "SeedCatalog": false }` o `Demo__SeedCatalog=false`.

| Organización | Umbral HR-03 | Para qué |
|--------------|--------------|----------|
| `Demo — Operación` | 0 min | Calendario del **mes UTC** en curso: Ana (turno válido 08:00–14:00 UTC), Bruno (10–14 y 14–18 UTC; intenta 12–16 → HR-01), Carla (leave activo → HR-02), Elena (inactiva), Fran (asignación cancelada), tipo Noche inactivo |
| `Demo — Descanso` | 660 min | Diego 08:00–20:00 UTC hoy; intentar 20:00–22:00 UTC → HR-03 |

Los instantes de turno se guardan con offset 0 (UTC): Npgsql no acepta `DateTimeOffset` local en `timestamptz`. La UI de calendario ya usa el mismo convenio.

El journey SPEC-PRD-002 (crear maestros a mano) sigue válido; el catálogo **complementa** para ver casuísticas sin partir de cero. Reset de datos: §6.

Dos caminos de demo (menos de 15 min):

1. **Catálogo:** login → elegir `Demo — Operación` o `Demo — Descanso` en la barra → calendario / ausencias → provocar HR-01/02/03.
2. **Journey a mano (SPEC-PRD-002):** crear org, depto, empleado, tipo → asignar OK → solape → leave que bloquea.

### 3.3. Verificación de arranque en frío (freeze)

Checklist de evaluador (PBI-010). Humano verificado 2026-08-17 (post-merge #36).

1. Parar AppHost (`Ctrl+C`).
2. Borrar el volumen Docker de Postgres creado por Aspire (§6).
3. Arrancar con el comando canónico de §3 (`--launch-profile https`, Debug/Development).
4. `GET /api/status` → `"status":"ok"` y base reachable.
5. Login `demo.admin` → aparecen `Demo — Operación` y `Demo — Descanso`.
6. Recorrer el journey (menos de 15 min; catálogo o SPEC-PRD-002): asignación válida, rechazo con explicación, ausencia que bloquea.
7. Smoke UX (PBI-015): cambiar org en la barra desde Calendario y desde el detalle.

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
Host=localhost;Port=5433;Database=shiftflow;Username=shiftflow;Password=shiftflow
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
| Puerto 5433 ocupado / AppHost cancela al arrancar | Liberar 5433 o cambiar `WithHostPort`; **no** uses 5432 si tienes `postgresql-x64-*` de Windows |
| pgAdmin estable | Host=`localhost`, Port=`5433`, DB/user/pass=`shiftflow` (Aspire: `WithHostPort(5433)`) |
| TaskCanceledException al crear Postgres | Suele ser puerto host ya ocupado (p. ej. intento de mapear 5432 con el servicio Windows activo) |
| `docker` no reconocido | Instalar Docker Desktop y reiniciar la terminal |
| Api `database: unreachable` | Postgres aún no listo; esperar healthcheck o `docker compose ps` |
| Web no ve la Api | Arrancar vía AppHost (inyecta service discovery) o configurar base address manualmente |
| SDK incorrecto | Este skeleton usa **net10.0**. Instala .NET 10 SDK (`dotnet --list-sdks`) |
| Dashboard Aspire: `UntrustedRoot` / gRPC SSL | `dotnet dev-certs https --trust` (aceptar el diálogo de Windows). Cerrar navegadores y reiniciar el AppHost. |
| Api falla al arrancar: tablas ya existen / historial de migraciones vacío | Volumen creado con `EnsureCreated` (pre PBI-014). Resetear volumen (§6) una vez. |
| Catálogo vacío en Production / `-c Release` sin Development | `Demo:SeedCatalog` es false fuera de Development. Usa el perfil `https` del AppHost. |

---

## 8. Usuario demo

Ver §3 (usuario `demo.admin`, rol `Administrator`, contraseña vía user-secrets/env o default de desarrollo).
