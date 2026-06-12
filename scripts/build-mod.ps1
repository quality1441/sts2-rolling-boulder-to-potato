param(
    [ValidateSet('Debug', 'Release')][string]$Configuration = 'Release'
)

. "$PSScriptRoot\_common.ps1"

Write-Host "Building Rolling Boulder to Potato mod ($Configuration)..." -ForegroundColor Cyan
Push-Location (Join-Path $RepoRoot 'Sts2RollingBoulderToPotatoMod')
try {
    dotnet build $ModProject -c $Configuration
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
finally {
    Pop-Location
}

$dll = Get-ModDllPath -Configuration $Configuration
if (-not (Test-Path $dll)) {
    Write-Error "Build succeeded but DLL not found at: $dll"
}
Write-Host "OK: $dll" -ForegroundColor Green
