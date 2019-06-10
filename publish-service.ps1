$apimRgName = "att-apim-demo-rg"
$apimServiceName = "nzattdemo"
$apimVnetName = "nz-apimdemo-vnet"
$apimSubmissionStore = "nzattdemomgmt"
$apimSubmissionContainer = "service-submissions"
$apimSubmissionDataContainer = "service-submission-data"
$apimSubmissionBlobName = "submission-blob.json"

# Storage Actions
$storageContext = New-AzStorageContext -StorageAccountName $apimSubmissionStore -UseConnectedAccount
$storageContext = New-AzStorageContext -StorageAccountName $apimSubmissionStore -StorageAccountKey $env:ServiceSubmissionStore
Write-Output $storageContext

if ($Error.Count -gt 0)
{
    # Get the service submission info
    $serviceSubmissionFile = New-TemporaryFile 
    Get-AzStorageBlobContent -Context $storageContext -Container $apimSubmissionContainer -Blob $apimSubmissionBlobName -Destination $serviceSubmissionFile -Force
    $submissionObject = Get-Content $serviceSubmissionFile | ConvertFrom-Json
}
[HttpStatusCode]::
# Get the service metadata
$serviceMetadatFile = New-TemporaryFile 
Get-AzStorageBlobContent -Container $apimSubmissionDataContainer -Blob $submissionObject.swaggerFileName -Context $storageContext -Destination $serviceMetadatFile -Force
Get-AzStorageBlob -Container $apimSubmissionDataContainer -Blob $submissionObject.swaggerFileName -Context $storageContext
# Network Actions
$apiVnet = Get-AzVirtualNetwork -ResourceGroupName $apimRgName -Name $apimVnetName

$serviceVnetId = '/subscriptions/' + $submissionObject.azureSubscriptionId + '/resourceGroups/' + $submissionObject.resourceGroupName + '/providers/Microsoft.Network/virtualNetworks/' + $submissionObject.vnetName

$vnetPeeringName = $apimVnetName + '-to-' + $submissionObject.vnetName
$vnetPeering = Get-AzVirtualNetworkPeering -ResourceGroupName $apimRgName -VirtualNetworkName $apimVnetName
if ($null -eq $vnetPeering)
{
    Add-AzVirtualNetworkPeering -Name $vnetPeeringName -VirtualNetwork $apiVnet -RemoteVirtualNetworkId $serviceVnetId
    Write-Output "Peering created"
}   
else {
    Write-Output "Peering " $vnetPeeringName " already exists"
}

# API Management Actions

$ApiMgmtContext = New-AzApiManagementContext -ResourceGroupName $apimRgName -ServiceName $apimServiceName
Import-AzApiManagementApi -Context $ApiMgmtContext -SpecificationFormat "Swagger" -SpecificationPath $serviceMetadatFile  -Path "confapi"
Write-Output "Successfully Imported API"

$uri = "https://prod-03-att-ise-demo-mervase2fa6im.southcentralus.environments.microsoftazurelogicapps.net:443/workflows/2cc0eeb65d034e2d8c1af5d6f0aafd73/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=ax94UXFZbP3qVREQvcT9o4CYLmD705gstVdjlndzAkw"
Invoke-WebRequest -Uri $uri -Body $body -Method "POST" -ContentType "application/json"


