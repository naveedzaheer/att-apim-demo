param($eventGridEvent, $TriggerMetadata)

# Make sure to pass hashtables to Out-String so they're logged correctly
$eventGridEvent | Out-String | Write-Host
$triggerData = $eventGridEvent.data 
$triggerData | Out-String | Write-Host
$fileUrl = $triggerData.url
$fileUrl | Out-String | Write-Host
$body = "{'apiMetadataFileName': '$fileUrl'}"
$body | Out-String | Write-Host
$uri = "https://prod-02-att-ise-demo-mervase2fa6im.southcentralus.environments.microsoftazurelogicapps.net:443/workflows/59a9a354b6524466aa85c7b201a2ddbd/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=4KVABYz_hrRQwjs60uzaM6RyjfTwGhGQBI-xRH5Qo-4"
Invoke-WebRequest -Uri $uri -Body $body -Method "POST" -ContentType "application/json"
