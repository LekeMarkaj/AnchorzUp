# AnchorzUp Setup Script for Windows
Write-Host "🚀 Setting up AnchorzUp URL Shortener..." -ForegroundColor Green

# Check if .NET 8 is installed
Write-Host "Checking .NET 8 installation..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    if ($dotnetVersion -match "^8\.") {
        Write-Host "✅ .NET 8 found: $dotnetVersion" -ForegroundColor Green
    } else {
        Write-Host "❌ .NET 8 not found. Please install .NET 8 SDK from https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "❌ .NET not found. Please install .NET 8 SDK from https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Red
    exit 1
}

# Check if Node.js is installed
Write-Host "Checking Node.js installation..." -ForegroundColor Yellow
try {
    $nodeVersion = node --version
    Write-Host "✅ Node.js found: $nodeVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Node.js not found. Please install Node.js from https://nodejs.org/" -ForegroundColor Red
    exit 1
}

# Note about database
Write-Host "Database: SQLite will be used (no additional setup required)" -ForegroundColor Green

# Restore .NET dependencies
Write-Host "Restoring .NET dependencies..." -ForegroundColor Yellow
Set-Location "server\AnchorzUp.API"
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Failed to restore .NET dependencies" -ForegroundColor Red
    exit 1
}
Write-Host "✅ .NET dependencies restored" -ForegroundColor Green

# Install frontend dependencies
Write-Host "Installing frontend dependencies..." -ForegroundColor Yellow
Set-Location "..\..\frontend"
npm install
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Failed to install frontend dependencies" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Frontend dependencies installed" -ForegroundColor Green

# Create environment file for frontend
Write-Host "Creating frontend environment file..." -ForegroundColor Yellow
$envContent = "REACT_APP_API_URL=https://localhost:52053/api"
$envContent | Out-File -FilePath ".env" -Encoding UTF8
Write-Host "✅ Environment file created" -ForegroundColor Green

Set-Location ".."

Write-Host "`n🎉 Setup completed successfully!" -ForegroundColor Green
Write-Host "`n📋 Next steps:" -ForegroundColor Cyan
Write-Host "1. Start the backend: cd server\AnchorzUp.API && dotnet run" -ForegroundColor White
Write-Host "   (Or open AnchorzUp.sln in Visual Studio and press F5)" -ForegroundColor Gray
Write-Host "2. Start the frontend: cd frontend && npm start" -ForegroundColor White
Write-Host "`n🌐 The application will be available at:" -ForegroundColor Cyan
Write-Host "   Frontend: http://localhost:3000" -ForegroundColor White
Write-Host "   Backend API (HTTPS): https://localhost:52053" -ForegroundColor White
Write-Host "   Backend API (HTTP): http://localhost:52054" -ForegroundColor White
