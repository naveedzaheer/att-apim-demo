param($eventGridEvent, $TriggerMetadata)

# Make sure to pass hashtables to Out-String so they're logged correctly
$eventGridEvent | Out-String | Write-Host

$triggerData = $eventGridEvent.data 
$triggerData | Out-String | Write-Host
$fileUrl = $triggerData.url
$fileUrl | Out-String | Write-Host
$body = "{'apiMetadataFileName': '$fileUrl'}"
$body | Out-String | Write-Host
$uri = "https://prod-02-att-ise-demo-mervase2fa6im.southcentralus.environments.microsoftazurelogicapps.net:443/workflows/1efb210d7abb49e99ebca5c35c67f7c6/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=7tifH-cMNQaaTprE-203PrzmxQr5-cneo7zYK3AiDb4"
Invoke-WebRequest -Uri $uri -Body $body -Method "POST" -ContentType "application/json"