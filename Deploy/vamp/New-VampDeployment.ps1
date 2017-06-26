param(
    [string]
    $baseUri = "http://localhost:8080/service/vamp/",
    
    [Parameter(Mandatory=$true)]
    [string]
    $deploymentName,

    [Parameter(Mandatory=$true)]
    [string]
    $deploymentFile
)
$deploymentDefinition = Get-Content $deploymentFile -Raw

Invoke-WebRequest `
    -Method PUT `
    -Uri "$baseUri/api/v1/deployments/$deploymentName" `
    -ContentType "application/x-yaml" `
    -Body $deploymentDefinition `
    -Headers @{"Accept" = "application/x-yaml"}
