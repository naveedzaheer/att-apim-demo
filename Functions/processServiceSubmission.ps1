using namespace System.Net

# Input bindings are passed in via param block.
param($Request, $TriggerMetadata)

# Write to the Azure Functions log stream.
Write-Host "PowerShell HTTP trigger function processed a request."

$apimRgName = "att-apim-demo-rg"
$apimServiceName = "nzattdemo"
$apimVnetName = "nz-apimdemo-vnet"
$apimSubmissionStore = "nzattdemomgmt"
$apimSubmissionDataContainer = "service-submission-data"
$vnetPeering = $null

# Storage Actions
$storageContext = New-AzStorageContext -StorageAccountName $apimSubmissionStore -StorageAccountKey $env:ServiceSubmissionStoreKey
Write-Output $storageContext

# Get the service metadata
if ($Error.Count -eq 0)
{
    $serviceMetadatFile = New-TemporaryFile 
    Get-AzStorageBlobContent -Container $apimSubmissionDataContainer -Blob $Request.Body.swaggerFileName -Context $storageContext -Destination $serviceMetadatFile -Force
    # Get-AzStorageBlob -Container $apimSubmissionDataContainer -Blob $submissionObject.swaggerFileName -Context $storageContext
    Write-Output $serviceMetadatFile
}

# Network Actions
if ($Error.Count -eq 0)
{
    $apiVnet = Get-AzVirtualNetwork -ResourceGroupName $apimRgName -Name $apimVnetName
}

if ($Error.Count -eq 0)
{
    $serviceVnetId = '/subscriptions/' + $Request.Body.azureSubscriptionId + '/resourceGroups/' + $Request.Body.resourceGroupName + '/providers/Microsoft.Network/virtualNetworks/' + $Request.Body.vnetName
    $vnetPeeringName = $apimVnetName + '-to-' + $Request.Body.vnetName
    $vnetPeering = Get-AzVirtualNetworkPeering -ResourceGroupName $apimRgName -VirtualNetworkName $apimVnetName -Name $vnetPeeringName
    if ($Error.Count -gt 0)
    {
        # just clear the error for now
        $Error.Clear()
    }
}


if ($Error.Count -eq 0)
{
    if ($null -eq $vnetPeering)
    {
        $vnetPeering = Add-AzVirtualNetworkPeering -Name $vnetPeeringName -VirtualNetwork $apiVnet -RemoteVirtualNetworkId $serviceVnetId
        Write-Output "Peering created"
    }   
    else {
        Write-Output "Peering " $vnetPeeringName " already exists"
    }
}

# API Management Actions

if ($Error.Count -eq 0)
{
    $ApiMgmtContext = New-AzApiManagementContext -ResourceGroupName $apimRgName -ServiceName $apimServiceName
    $Api = Import-AzApiManagementApi -Context $ApiMgmtContext -SpecificationFormat "Swagger" -SpecificationPath $serviceMetadatFile  -Path $Request.Body.apiUrlSuffix
    $ApiId = $Api.ApiId
    Add-AzApiManagementApiToProduct -Context $ApiMgmtContext -ProductId "starter" -ApiId $ApiId
    Write-Output "Successfully Imported API - $Api"
}

$status = [HttpStatusCode]::OK
$body = ""

if ($Error.Count -eq 0)
{
    $peeringInfo = ConvertTo-Json -InputObject $vnetPeering
    # Interact with query parameters or the body of the request.
    $status = [HttpStatusCode]::OK
    $body = "Peering Info: $peeringInfo "
}
else
{
    $errorInfo = ConvertTo-Json -InputObject $Error
    # Interact with query parameters or the body of the request.
    $status = [HttpStatusCode]::InternalServerError 
    $body = "Error Info: $errorInfo "

}

# Associate values to output bindings by calling 'Push-OutputBinding'.
Push-OutputBinding -Name Response -Value ([HttpResponseContext]@{
    StatusCode = $status
    Body = $body
})
