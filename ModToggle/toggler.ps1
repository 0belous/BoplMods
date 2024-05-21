Get-Process -Name "BoplBattle" -ErrorAction SilentlyContinue | Stop-Process -Force

$modNames = $args

Write-Host "Toggled mods:"
$modNames | ForEach-Object { Write-Host $_ }

$modFiles = Get-ChildItem -Path .\ -Filter "*.dll*"

foreach ($modName in $modNames) {
    $modFile = $modFiles | Where-Object { $_.Name -like "$modName.dll*" }

    if ($modFile) {
        if ($modFile.Name -like "*.dll.disabled") {
            Rename-Item -Path $modFile.FullName -NewName ($modFile.Name -replace ".disabled", "") -ErrorAction Stop
        }
        elseif ($modFile.Name -like "*.dll") {
            Rename-Item -Path $modFile.FullName -NewName ($modFile.Name + ".disabled") -ErrorAction Stop
        }
    }
}


# we have to wait for the game to fully close
Start-Sleep -Seconds 10

$scriptDir = $PSScriptRoot

Start-Process -FilePath "$scriptDir\..\..\BoplBattle.exe"

    