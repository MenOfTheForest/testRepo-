# SpeedFest API Test Script
# Run this script in a separate PowerShell window while the application is running

Write-Host "Testing SpeedFest API..." -ForegroundColor Green
Write-Host "Make sure the application is running on http://localhost:5000" -ForegroundColor Yellow
Write-Host ""

# Test 1: Get all racing teams
Write-Host "1. Testing GET /api/racingteams" -ForegroundColor Cyan
try {
    $teams = Invoke-RestMethod -Uri "http://localhost:5000/api/racingteams" -Method GET
    Write-Host "✅ Found $($teams.Count) racing teams:" -ForegroundColor Green
    $teams | ForEach-Object { Write-Host "  - $($_.name) (Principal: $($_.teamPrincipal))" }
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 2: Get all racing drivers  
Write-Host "2. Testing GET /api/racingdrivers" -ForegroundColor Cyan
try {
    $drivers = Invoke-RestMethod -Uri "http://localhost:5000/api/racingdrivers" -Method GET
    Write-Host "✅ Found $($drivers.Count) racing drivers:" -ForegroundColor Green
    $drivers | ForEach-Object { Write-Host "  - #$($_.driverNumber) $($_.name) ($($_.teamName))" }
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 3: Get fastest lap times
Write-Host "3. Testing GET /api/laptimes/fastest" -ForegroundColor Cyan
try {
    $fastestLaps = Invoke-RestMethod -Uri "http://localhost:5000/api/laptimes/fastest?count=5" -Method GET
    Write-Host "✅ Found $($fastestLaps.Count) fastest lap times:" -ForegroundColor Green
    $fastestLaps | ForEach-Object { 
        $totalTime = $_.totalLapTime
        if ($totalTime) {
            Write-Host "  - $($_.driverName): $($totalTime.ToString('F3'))s"
        }
    }
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 4: Create a new team (POST)
Write-Host "4. Testing POST /api/racingteams (Create new team)" -ForegroundColor Cyan
try {
    $newTeam = @{
        name = "Test Team"
        teamPrincipal = "Test Principal"
    }
    $headers = @{ "Content-Type" = "application/json" }
    $body = $newTeam | ConvertTo-Json
    
    $createdTeam = Invoke-RestMethod -Uri "http://localhost:5000/api/racingteams" -Method POST -Body $body -Headers $headers
    Write-Host "✅ Created new team: $($createdTeam.name) (ID: $($createdTeam.racingTeamId))" -ForegroundColor Green
    
    # Clean up - delete the test team
    Invoke-RestMethod -Uri "http://localhost:5000/api/racingteams/$($createdTeam.racingTeamId)" -Method DELETE
    Write-Host "✅ Cleaned up test team" -ForegroundColor Green
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "Testing completed!" -ForegroundColor Green
Write-Host "Visit http://localhost:5000/swagger for interactive API testing" -ForegroundColor Yellow