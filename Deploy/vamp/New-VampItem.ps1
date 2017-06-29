param(
    [string]
    $baseUri = "http://localhost:8080/service/vamp/",
    
    [Parameter(Mandatory=$true)]
    [string]
    $deploymentFile
)

# Deployment file should be
# ---
# kind: breed
# name: name-here
# ...
#
# ---
# kind: <next kind>
# ...

# see vamp runner recipes for examples

$deploymentDefinition = Get-Content $deploymentFile -Raw

Invoke-WebRequest `
    -Method PUT `
    -Uri "$baseUri/api/v1/" `
    -ContentType "application/x-yaml" `
    -Body $deploymentDefinition `
    -Headers @{"Accept" = "application/x-yaml"} `
    | Select-Object -ExpandProperty RawContent