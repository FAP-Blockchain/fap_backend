#!/usr/bin/env pwsh
# =============================================================================
# Docker Test Script - FAP Backend
# =============================================================================

Write-Host "`n===============================================" -ForegroundColor Cyan
Write-Host " FAP Backend - Docker Health Check" -ForegroundColor Cyan
Write-Host "===============================================`n" -ForegroundColor Cyan

# Step 1: Check Docker Installation
Write-Host "[1/8] Checking Docker installation..." -ForegroundColor Yellow
try {
    $dockerVersion = (docker --version) -replace 'Docker version ', ''
    Write-Host "  ? Docker: $dockerVersion" -ForegroundColor Green
    
    $composeVersion = (docker compose version) -replace 'Docker Compose version ', ''
    Write-Host "  ? Docker Compose: $composeVersion" -ForegroundColor Green
} catch {
    Write-Host "  ? Docker not installed or not running!" -ForegroundColor Red
exit 1
}

# Step 2: Check Docker daemon
Write-Host "`n[2/8] Checking Docker daemon..." -ForegroundColor Yellow
try {
    docker ps | Out-Null
    Write-Host "  ? Docker daemon is running" -ForegroundColor Green
} catch {
    Write-Host "  ? Docker daemon not running! Start Docker Desktop." -ForegroundColor Red
  exit 1
}

# Step 3: Check Docker images
Write-Host "`n[3/8] Checking Docker images..." -ForegroundColor Yellow
$image = docker images fap-backend:latest --format "{{.Repository}}:{{.Tag}}" 2>$null
if ($image) {
    $imageSize = docker images fap-backend:latest --format "{{.Size}}"
    $imageCreated = docker images fap-backend:latest --format "{{.CreatedSince}}"
    Write-Host "  ? Image found: $image" -ForegroundColor Green
    Write-Host "    Size: $imageSize, Created: $imageCreated" -ForegroundColor Gray
} else {
    Write-Host "  ? Image 'fap-backend:latest' not found!" -ForegroundColor Red
    Write-Host "    Build it: docker build -t fap-backend:latest ." -ForegroundColor Yellow
    exit 1
}

# Step 4: Check containers
Write-Host "`n[4/8] Checking containers..." -ForegroundColor Yellow
$allContainers = docker ps -a --filter "name=fap-backend" --format "{{.Names}}" 2>$null
if ($allContainers) {
    $status = docker ps -a --filter "name=fap-backend" --format "{{.Status}}"
    $running = docker ps --filter "name=fap-backend" --format "{{.Names}}" 2>$null
    
    if ($running) {
        Write-Host "  ? Container running: $allContainers" -ForegroundColor Green
    Write-Host "    Status: $status" -ForegroundColor Gray
    } else {
        Write-Host "  ? Container exists but not running: $allContainers" -ForegroundColor Yellow
      Write-Host "    Status: $status" -ForegroundColor Gray
        Write-Host "`n  Starting container..." -ForegroundColor Yellow
   docker start $allContainers | Out-Null
        Start-Sleep -Seconds 3
    $newStatus = docker ps --filter "name=fap-backend" --format "{{.Status}}"
        Write-Host "  ? Container started: $newStatus" -ForegroundColor Green
    }
} else {
    Write-Host "  ? No container found" -ForegroundColor Yellow
    Write-Host "    Create with: docker run -d -p 8080:80 --name fap-backend fap-backend:latest" -ForegroundColor Yellow
    exit 1
}

# Step 5: Wait for app to be ready
Write-Host "`n[5/8] Waiting for application to start..." -ForegroundColor Yellow
$maxRetries = 10
$retryCount = 0
$appReady = $false

while ($retryCount -lt $maxRetries -and -not $appReady) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:8080/health" -UseBasicParsing -TimeoutSec 2 -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            $appReady = $true
      Write-Host "  ? Application is ready!" -ForegroundColor Green
        }
    } catch {
        $retryCount++
        Write-Host "  ? Waiting... ($retryCount/$maxRetries)" -ForegroundColor Gray
        Start-Sleep -Seconds 2
    }
}

if (-not $appReady) {
    Write-Host "  ? Application did not start in time" -ForegroundColor Red
    Write-Host "`n  Recent logs:" -ForegroundColor Yellow
 docker logs fap-backend --tail 10
    exit 1
}

# Step 6: Test Health endpoint
Write-Host "`n[6/8] Testing Health endpoint..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "http://localhost:8080/health" -Method Get
    Write-Host "  ? GET /health - Status: 200 OK" -ForegroundColor Green
    Write-Host "    Service: $($response.service)" -ForegroundColor Gray
    Write-Host "    Status: $($response.status)" -ForegroundColor Gray
    Write-Host "    Version: $($response.version)" -ForegroundColor Gray
    Write-Host "    Environment: $($response.environment)" -ForegroundColor Gray
} catch {
    Write-Host "  ? Health endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Step 7: Test Detailed Health endpoint
Write-Host "`n[7/8] Testing Detailed Health endpoint..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "http://localhost:8080/health/detailed" -Method Get
    Write-Host "  ? GET /health/detailed - Status: 200 OK" -ForegroundColor Green
    Write-Host "    Status: $($response.status)" -ForegroundColor Gray
    Write-Host "    Database Connected: $($response.database.connected)" -ForegroundColor Gray
  Write-Host "    Database Provider: $($response.database.provider)" -ForegroundColor Gray
} catch {
    Write-Host "  ? Detailed health failed (DB connection issue)" -ForegroundColor Yellow
    Write-Host "    Error: $($_.Exception.Message)" -ForegroundColor Gray
}

# Step 8: Test Swagger UI
Write-Host "`n[8/8] Testing Swagger UI..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:8080/swagger/index.html" -UseBasicParsing -TimeoutSec 5
    if ($response.StatusCode -eq 200) {
        Write-Host "  ? Swagger UI is accessible" -ForegroundColor Green
    }
} catch {
    Write-Host "  ? Swagger UI failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Summary
Write-Host "`n===============================================" -ForegroundColor Cyan
Write-Host " Docker Status Summary" -ForegroundColor Cyan
Write-Host "===============================================" -ForegroundColor Cyan

# Container info
$containerInfo = docker inspect fap-backend --format '{{.State.Status}}|{{.State.StartedAt}}|{{.NetworkSettings.IPAddress}}' 2>$null
if ($containerInfo) {
    $parts = $containerInfo -split '\|'
    Write-Host "`nContainer Details:" -ForegroundColor Cyan
    Write-Host "  Name: fap-backend" -ForegroundColor White
    Write-Host "  State: $($parts[0])" -ForegroundColor White
    Write-Host "  Started: $($parts[1])" -ForegroundColor White
    Write-Host "  IP: $($parts[2])" -ForegroundColor White
}

# Port mappings
Write-Host "`nPort Mappings:" -ForegroundColor Cyan
Write-Host "  HTTP:  http://localhost:8080" -ForegroundColor White
Write-Host "  HTTPS: https://localhost:8443 (if configured)" -ForegroundColor White

# Endpoints
Write-Host "`nAvailable Endpoints:" -ForegroundColor Cyan
Write-Host "  Health:         http://localhost:8080/health" -ForegroundColor White
Write-Host "  Detailed Health: http://localhost:8080/health/detailed" -ForegroundColor White
Write-Host "  Swagger UI:     http://localhost:8080/swagger" -ForegroundColor White
Write-Host "  API Docs:       http://localhost:8080/swagger/v1/swagger.json" -ForegroundColor White

# Useful commands
Write-Host "`nUseful Commands:" -ForegroundColor Cyan
Write-Host "  docker logs fap-backend -f   # View logs" -ForegroundColor Gray
Write-Host "  docker logs fap-backend --tail 50    # Last 50 lines" -ForegroundColor Gray
Write-Host "  docker restart fap-backend           # Restart container" -ForegroundColor Gray
Write-Host "  docker stop fap-backend  # Stop container" -ForegroundColor Gray
Write-Host "  docker exec -it fap-backend bash     # Enter container" -ForegroundColor Gray
Write-Host "  docker compose up -d      # Start with compose" -ForegroundColor Gray
Write-Host "  docker compose logs -f api       # View compose logs" -ForegroundColor Gray

# Check if SQL Server is accessible
Write-Host "`nDatabase Check:" -ForegroundColor Cyan
try {
    $sqlTest = Test-NetConnection -ComputerName localhost -Port 1433 -WarningAction SilentlyContinue -InformationLevel Quiet
    if ($sqlTest) {
  Write-Host "  ? SQL Server accessible on localhost:1433" -ForegroundColor Green
    } else {
        Write-Host "  ? SQL Server not accessible on localhost:1433" -ForegroundColor Yellow
    }
} catch {
    Write-Host "  ? Could not test SQL Server connection" -ForegroundColor Yellow
}

Write-Host "`n===============================================" -ForegroundColor Cyan
Write-Host " ? Docker Health Check Complete!" -ForegroundColor Green
Write-Host "===============================================`n" -ForegroundColor Cyan
