param(
    [string]
    $baseUri = "http://localhost:8080/service/vamp/",
    
    [Parameter(Mandatory=$true)]
    [string]
    $gatewayName,

    [Parameter(Mandatory=$true)]
    [string]
    $gatewayDefinitionFile
)
$gatewayDefinition = Get-Content $gatewayDefinitionFile -Raw

Invoke-WebRequest `
    -Method PUT `
    -Uri "$baseUri/api/v1/gateways/$gatewayName" `
    -ContentType "application/x-yaml" `
    -Body $gatewayDefinition `
    -Headers @{"Accept" = "application/x-yaml"}
