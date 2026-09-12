$adb = "$env:LOCALAPPDATA\Android\Sdk\platform-tools\adb.exe"

Write-Host "Restoring API tunnels for all connected Android devices..."
$devices = & $adb devices | Select-String -Pattern "\tdevice$" | ForEach-Object { ($_.ToString() -split "\t")[0] }

if (-not $devices) {
    Write-Host "No devices found."
    exit
}

foreach ($device in $devices) {
    Write-Host "Tunneling port 5032 for device: $device"
    & $adb -s $device reverse tcp:5032 tcp:5032
}

Write-Host "Done!"
Pause
