param(
    [string]
    $baseUri = "http://localhost:8080/service/vamp/"
)

Invoke-RestMethod -Uri "$baseUri/api/v1/deployments" -Headers @{"Accept" = "application/x-yaml"}