param(
    [ValidateSet('Debug', 'Release')][string]$Configuration = 'Release',
    [switch]$SkipBuild,
    [switch]$NoZip
)

. "$PSScriptRoot\_common.ps1"

$manifestPath = Join-Path $RepoRoot 'Sts2RollingBoulderToPotatoMod\Sts2RollingBoulderToPotato.json'
$exampleConfig = Join-Path $RepoRoot 'config.example.json'
$readmeTemplate = Join-Path $PSScriptRoot 'dist-README.md'
$distRoot = Join-Path $RepoRoot 'dist'
$releaseDir = Join-Path $distRoot $ModId

if (-not (Test-Path $manifestPath)) {
    Write-Error "Missing manifest: $manifestPath"
}

if (-not (Test-Path $exampleConfig)) {
    Write-Error "Missing config template: $exampleConfig"
}

if (-not (Test-Path $readmeTemplate)) {
    Write-Error "Missing release readme template: $readmeTemplate"
}

if (-not $SkipBuild) {
    & (Join-Path $PSScriptRoot 'build-mod.ps1') -Configuration $Configuration
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

$dll = Get-ModDllPath -Configuration $Configuration
if (-not (Test-Path $dll)) {
    Write-Error "Mod DLL missing. Run build-mod.ps1 first. Expected: $dll"
}

$manifest = Get-Content $manifestPath -Raw | ConvertFrom-Json
$version = if ($manifest.version) { [string]$manifest.version } else { '0.0.0' }

if (Test-Path $distRoot) {
    Remove-Item -Recurse -Force $distRoot
}
New-Item -ItemType Directory -Force -Path $releaseDir | Out-Null

Copy-Item -Force $dll (Join-Path $releaseDir 'Sts2RollingBoulderToPotato.dll')
Copy-Item -Force $manifestPath (Join-Path $releaseDir 'Sts2RollingBoulderToPotato.json')
Copy-Item -Force $exampleConfig (Join-Path $releaseDir 'config.example.json')
Copy-Item -Force $exampleConfig (Join-Path $releaseDir 'config.json')
Copy-Item -Force $readmeTemplate (Join-Path $releaseDir 'README.md')

$thumbnail = Join-Path $RepoRoot 'mod-thumbnail.jpg'
if (Test-Path $thumbnail) {
    Copy-Item -Force $thumbnail (Join-Path $releaseDir 'mod_image.jpg')
}

Write-Host ""
Write-Host "Release package ready:" -ForegroundColor Green
Write-Host "  $releaseDir" -ForegroundColor DarkGray
Get-ChildItem $releaseDir | ForEach-Object { Write-Host "    $($_.Name)" -ForegroundColor DarkGray }

$zipPath = Join-Path $RepoRoot "Sts2RollingBoulderToPotato-$version.zip"
if (-not $NoZip) {
    if (Test-Path $zipPath) {
        Remove-Item -Force $zipPath
    }

    $staging = Join-Path $env:TEMP "Sts2RollingBoulderToPotato-release-$([Guid]::NewGuid().ToString('N'))"
    $modFolder = Join-Path $staging $ModId
    New-Item -ItemType Directory -Force -Path $modFolder | Out-Null
    Copy-Item -Force (Join-Path $releaseDir '*') $modFolder
    Compress-Archive -Path $modFolder -DestinationPath $zipPath -Force
    Remove-Item -Recurse -Force $staging

    Write-Host ""
    Write-Host "Zip for upload:" -ForegroundColor Green
    Write-Host "  $zipPath" -ForegroundColor DarkGray
    Write-Host "  (extract $ModId folder into <STS2>\mods\)" -ForegroundColor DarkGray
}

Write-Host ""
Write-Host "Share the dist folder or zip for public download." -ForegroundColor Yellow
