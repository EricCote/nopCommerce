# Build the plugin
Write-Host "Building plugin..." -ForegroundColor Green
dotnet build -c Release

# Create output directory
$outputDir = ".\bin\Package"
if (Test-Path $outputDir) {
    Remove-Item $outputDir -Recurse -Force
}
New-Item -ItemType Directory -Path $outputDir | Out-Null

# Copy DLL
Write-Host "Copying files..." -ForegroundColor Green
$dllPath = ".\bin\Release\net9.0\Ecomzen.Plugin.Misc.Gifts.dll"
Copy-Item $dllPath $outputDir

# Copy Views
if (Test-Path ".\Views") {
    Copy-Item ".\Views" "$outputDir\Views" -Recurse
}

# Copy Content
if (Test-Path ".\Content") {
    Copy-Item ".\Content" "$outputDir\Content" -Recurse
}

# Copy logo
if (Test-Path ".\logo.jpg") {
    Copy-Item ".\logo.jpg" $outputDir
}

# Copy json
if (Test-Path ".\plugin.json") {
    Copy-Item ".\plugin.json" $outputDir
}


# Create ZIP
Write-Host "Creating package..." -ForegroundColor Green
$zipPath = ".\bin\Ecomzen.Plugin.Misc.Gifts.zip"
if (Test-Path $zipPath) {
    Remove-Item $zipPath
}

Compress-Archive -Path "$outputDir\*" -DestinationPath $zipPath

Write-Host "Package created: $zipPath" -ForegroundColor Green