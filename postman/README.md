# Colecciones Postman — ShiftFlow

| Colección | Contenido |
|-----------|-----------|
| [ShiftFlow-PBI-003-auth-masters.postman_collection.json](ShiftFlow-PBI-003-auth-masters.postman_collection.json) | Auth cookie + maestros Org/Dept/Employee (PBI-002/003) |

## Uso

1. Postman → **Import** → seleccionar el `.json`.
2. Variable de colección `baseUrl` = URL HTTPS de la Api (dashboard Aspire).
3. Settings → desactivar verificación SSL (cert de desarrollo) o confiar el cert.
4. Orden sugerido: **Login** → create organization → create department → create employee.

La cookie `ShiftFlow.Auth` la gestiona Postman tras el login. Los scripts de test guardan `organizationId`, `departmentId` y `employeeId`.
