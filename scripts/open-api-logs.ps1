$dateSuffix = (Get-Date).ToString("yyyyMMdd")
$filePath = "c:/temp/logs/reference-architecture-api-$dateSuffix.log"

if (Test-Path $filePath)
{
    Start-Process $filePath
}
else
{
    Write-Host "File not found: $filePath"
}