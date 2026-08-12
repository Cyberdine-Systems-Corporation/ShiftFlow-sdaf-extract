# Colecciones Postman — ShiftFlow

| Colección | Contenido |
|-----------|-----------|
| [ShiftFlow-PBI-003-auth-masters.postman_collection.json](ShiftFlow-PBI-003-auth-masters.postman_collection.json) | Auth cookie + maestros Org/Dept/Employee/ShiftType (PBI-002…004) + calendario/asignación (PBI-005) |

## Uso

1. Postman → **Import** → seleccionar el `.json` (o **Replace** si ya la tenías importada).
2. Variable de colección `baseUrl` = URL HTTPS de la Api (dashboard Aspire).
3. Settings → desactivar verificación SSL (cert de desarrollo) o confiar el cert.
4. Orden sugerido:
   1. **Login**
   2. Create organization → department → employee → shift type
   3. **Calendar & Assignments**: Get month calendar → Assign shift → overlap (HR-01) → adjacent → Cancel

La cookie `ShiftFlow.Auth` la gestiona Postman tras el login. Los scripts de test guardan `organizationId`, `departmentId`, `employeeId`, `shiftTypeId` y `assignmentId`.

Variables útiles de calendario: `calendarYear` / `calendarMonth` (por defecto `2026` / `8`, alineadas a los cuerpos de ejemplo de AssignShift).

## Endpoints PBI-005 cubiertos

| Request | Ruta |
|---------|------|
| GET month calendar | `GET /api/organizations/{id}/calendar?year=&month=` |
| Assign shift | `POST /api/organizations/{id}/assignments` |
| Cancel shift | `POST /api/assignments/{id}/cancel` |
