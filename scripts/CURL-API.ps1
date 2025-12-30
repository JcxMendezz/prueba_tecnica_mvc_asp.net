Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   PRUEBA COMPLETA DE API REST" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 1. GET ALL
Write-Host "`n[1] GET - Todas las tareas" -ForegroundColor Green
Invoke-RestMethod -Uri "http://localhost:5236/api/tasksapi" -Method Get | ConvertTo-Json -Depth 3

# 2. GET BY ID
Write-Host "`n[2] GET - Tarea ID=2" -ForegroundColor Green
Invoke-RestMethod -Uri "http://localhost:5236/api/tasksapi/2" -Method Get | ConvertTo-Json -Depth 3

# 3. POST - Crear
Write-Host "`n[3] POST - Crear nueva tarea" -ForegroundColor Yellow
$newTask = @{
    title = "Nueva tarea de prueba"
    description = "Creada desde script de prueba"
    status = 0
    priority = 1
    dueDate = "2025-02-20"
} | ConvertTo-Json

$created = Invoke-RestMethod -Uri "http://localhost:5236/api/tasksapi" -Method Post -Body $newTask -ContentType "application/json"
$created | ConvertTo-Json -Depth 3
$newId = $created.id
Write-Host "Tarea creada con ID: $newId" -ForegroundColor Cyan

# 4. PUT - Actualizar
Write-Host "`n[4] PUT - Actualizar tarea ID=$newId" -ForegroundColor Yellow
$updateTask = @{
    id = $newId
    title = "Tarea actualizada por script"
    description = "Modificada para probar PUT"
    status = 1
    priority = 2
    dueDate = "2025-03-15"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5236/api/tasksapi/$newId" -Method Put -Body $updateTask -ContentType "application/json" | ConvertTo-Json -Depth 3

# 5. DELETE
Write-Host "`n[5] DELETE - Eliminar tarea ID=$newId" -ForegroundColor Red
Invoke-RestMethod -Uri "http://localhost:5236/api/tasksapi/$newId" -Method Delete | ConvertTo-Json

# 6. Verificar eliminación
Write-Host "`n[6] GET - Verificar eliminación (debe dar error 404)" -ForegroundColor Magenta
try {
    Invoke-RestMethod -Uri "http://localhost:5236/api/tasksapi/$newId" -Method Get
} catch {
    Write-Host "Tarea eliminada correctamente (404 Not Found)" -ForegroundColor Green
}

# 7. Health Check
Write-Host "`n[7] HEALTH CHECK" -ForegroundColor Cyan
Invoke-RestMethod -Uri "http://localhost:5236/health" -Method Get

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "   PRUEBA COMPLETADA" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
