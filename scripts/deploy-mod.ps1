param(
    [ValidateSet('Debug', 'Release')][string]$Configuration = 'Release',
    [switch]$SkipBuild,
    [string]$Sts2Root = $(if ($env:STS2_GAME_ROOT) { $env:STS2_GAME_ROOT } else { $null })
)

. "$PSScriptRoot\_common.ps1"

if (-not $SkipBuild) {
    & (Join-Path $PSScriptRoot 'build-mod.ps1') -Configuration $Configuration
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

if ($Sts2Root) {
    $deployDir = Join-Path $Sts2Root "mods\$ModId"
}
else {
    $sts2 = Get-SteamSts2Root
    if (-not $sts2) {
        Write-Error @"
Could not find Slay the Spire 2. Either:
  - Install via Steam to the default path, or
  - Run: `$env:STS2_GAME_ROOT = 'C:\path\to\Slay the Spire 2'; .\scripts\deploy-mod.ps1
"@
    }
    $deployDir = Get-ModDeployDir -Sts2Root $sts2
}

$dll = Get-ModDllPath -Configuration $Configuration
if (-not (Test-Path $dll)) {
    Write-Error "Mod DLL missing. Run build-mod.ps1 first. Expected: $dll"
}

$manifest = Join-Path $RepoRoot "Sts2RollingBoulderToPotatoMod\Sts2RollingBoulderToPotato.json"
$defaultConfig = Join-Path $RepoRoot 'potato.cfg.example'
if (-not (Test-Path $manifest)) {
    Write-Error "Missing manifest: $manifest"
}

New-Item -ItemType Directory -Force -Path $deployDir | Out-Null

Copy-Item -Force $dll (Join-Path $deployDir 'Sts2RollingBoulderToPotato.dll')
Copy-Item -Force $manifest (Join-Path $deployDir 'Sts2RollingBoulderToPotato.json')
if (-not (Test-Path (Join-Path $deployDir 'potato.cfg'))) {
    Copy-Item -Force $defaultConfig (Join-Path $deployDir 'potato.cfg')
}

foreach ($legacyConfig in @('config.json', 'config.example.json')) {
    $legacyPath = Join-Path $deployDir $legacyConfig
    if (Test-Path $legacyPath) {
        Remove-Item -Force $legacyPath
        Write-Host "  Removed legacy $legacyConfig (STS2 treats *.json as mod manifests)" -ForegroundColor DarkYellow
    }
}

$thumbnail = Join-Path $RepoRoot 'mod-thumbnail.jpg'
if (Test-Path $thumbnail) {
    Copy-Item -Force $thumbnail (Join-Path $deployDir 'mod_image.jpg')
}

Write-Host "Deployed to: $deployDir" -ForegroundColor Green
Write-Host "  Sts2RollingBoulderToPotato.dll" -ForegroundColor DarkGray
Write-Host "  Sts2RollingBoulderToPotato.json" -ForegroundColor DarkGray
Write-Host "  potato.cfg (default: random)" -ForegroundColor DarkGray
Write-Host ""
Write-Host "Restart STS2 with mods enabled to see the potato." -ForegroundColor Yellow
