# Shared paths for sts2-rolling-boulder-to-potato scripts.

$ErrorActionPreference = 'Stop'

$Script:RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$Script:ModProject = Join-Path $RepoRoot 'Sts2RollingBoulderToPotatoMod\Sts2RollingBoulderToPotatoMod.csproj'
$Script:ModId = 'Sts2RollingBoulderToPotato'

function Get-SteamSts2Root {
    if ($env:STS2_GAME_ROOT -and (Test-Path $env:STS2_GAME_ROOT)) {
        return (Resolve-Path $env:STS2_GAME_ROOT).Path
    }

    $candidates = @(
        (Join-Path ${env:ProgramFiles(x86)} 'Steam\steamapps\common\Slay the Spire 2'),
        (Join-Path $env:ProgramFiles 'Steam\steamapps\common\Slay the Spire 2'),
        (Join-Path $env:LOCALAPPDATA 'Programs\Steam\steamapps\common\Slay the Spire 2')
    )
    foreach ($p in $candidates) {
        if (Test-Path $p) { return $p }
    }
    return $null
}

function Get-ModDeployDir {
    param([string]$Sts2Root = $(Get-SteamSts2Root))
    if (-not $Sts2Root) {
        throw "STS2 install not found. Set `$env:STS2_GAME_ROOT to your Slay the Spire 2 folder."
    }
    Join-Path $Sts2Root "mods\$ModId"
}

function Get-ModDllPath {
    param([ValidateSet('Debug', 'Release')][string]$Configuration = 'Release')
    $paths = @(
        (Join-Path $RepoRoot "Sts2RollingBoulderToPotatoMod\.godot\mono\temp\bin\$Configuration\Sts2RollingBoulderToPotato.dll"),
        (Join-Path $RepoRoot "Sts2RollingBoulderToPotatoMod\bin\$Configuration\net9.0\Sts2RollingBoulderToPotato.dll")
    )
    foreach ($p in $paths) {
        if (Test-Path $p) { return (Resolve-Path $p).Path }
    }
    return $paths[1]
}
