$currentPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$rootPath = Split-Path -Parent $currentPath

cd $currentPath
$result =  Invoke-WebRequest -Method "POST"  -Uri "http://localhost:6634/api/services/app/interfaceExport/GetReactDownloadUrl"
$info = ConvertFrom-Json -InputObject $result.Content
$url = $info.result
Invoke-WebRequest -Uri "$url" -OutFile "typescript-fetch-client-generated.zip"
Expand-Archive -Path "typescript-fetch-client-generated.zip" $currentPath -Force
cd "typescript-fetch-client"
$contents = Get-Content $rootPath"\src\api\api.ts" -TotalCount 27 -Encoding "utf8"
Set-Content $rootPath"\src\api\api.ts" $contents -Encoding "utf8"

$lines = Get-Content ".\api.ts" -Encoding "utf8"
Add-Content $rootPath"\src\api\api.ts" $lines[24..$lines.Count] -Encoding "utf8"

cd $currentPath 
Remove-Item -Path "typescript-fetch-client" -Force -Recurse 
Remove-Item -Path "typescript-fetch-client-generated.zip" -Force

cd $rootPath

.\node_modules\.bin\tsc.cmd $rootPath"\src\api\api.ts"