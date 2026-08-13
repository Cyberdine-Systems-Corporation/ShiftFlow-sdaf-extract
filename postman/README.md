# Colecciones Postman — ShiftFlow

| Colección | Contenido |
|-----------|-----------|
| [ShiftFlow-PBI-003-auth-masters.postman_collection.json](ShiftFlow-PBI-003-auth-masters.postman_collection.json) | Auth cookie + maestros (PBI-002…004) + calendario/asignación (PBI-005) + Leaves/HR-02 (PBI-007) + descanso mínimo HR-03 (PBI-006/009) |

## Uso

1. Postman → **Import** → seleccionar el `.json` (o **Replace** si ya la tenías importada).
2. Variable de colección `baseUrl` = URL HTTPS de la Api (dashboard Aspire).
3. Settings → desactivar verificación SSL (cert de desarrollo) o confiar el cert.
4. Orden sugerido:
   1. **Login**
   2. Create organization → department → employee → shift type
   3. **Calendar & Assignments**: Get month calendar → Assign shift → overlap (HR-01) → adjacent → Cancel
   4. **HR-03 (opcional):** PUT minimum-rest 660 → assign too soon → `HR-03`
   5. **Leaves**: Register leave → List → Assign under leave (HR-02) → Cancel leave

La cookie `ShiftFlow.Auth` la gestiona Postman tras el login. Los scripts de test guardan `organizationId`, `departmentId`, `employeeId`, `shiftTypeId`, `assignmentId` y `leaveId`.

Variables útiles de calendario: `calendarYear` / `calendarMonth` (por defecto `2026` / `8`).

## Endpoints PBI-005 / PBI-007 cubiertos

| Request | Ruta |
|---------|------|
| GET month calendar | `GET /api/organizations/{id}/calendar?year=&month=` → `{ assignments, leaves }` |
| Assign shift | `POST /api/organizations/{id}/assignments` |
| Cancel shift | `POST /api/assignments/{id}/cancel` |
| Register leave | `POST /api/organizations/{id}/leaves` |
| List leaves | `GET /api/organizations/{id}/leaves` |
| PUT minimum rest | `PUT /api/organizations/{id}/minimum-rest` |
| Assign too soon (HR-03) | `POST /api/organizations/{id}/assignments` → `HR-03` |
