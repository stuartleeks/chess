param(
    [string]
    $baseUri = "http://localhost:8080/service/vamp/",
    
    [Parameter(Mandatory=$true)]
    [string]
    $tag # TODO - support multiple tags, and other properties!
)

Invoke-WebRequest `
    -Method POST `
    -Uri "$baseUri/api/v1/events" `
    -ContentType "application/json" `
    -Body "{`"tags`": [`"$tag`"]}" `
    -Headers @{"Accept" = "application/json"}