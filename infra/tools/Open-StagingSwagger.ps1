[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidatePattern('^i-[a-zA-Z0-9]+$')]
    [string] $InstanceId,

    [Parameter(Mandatory)]
    [ValidateSet('gateway', 'auth', 'cart', 'catalog', 'order')]
    [string] $Service,

    [ValidateRange(1024, 65535)]
    [int] $LocalPort
)

$servicePorts = @{
    gateway = 18080
    auth    = 18235
    cart    = 18087
    catalog = 18989
    order   = 18888
}

$remotePort = $servicePorts[$Service]
if (-not $PSBoundParameters.ContainsKey('LocalPort')) {
    $LocalPort = $remotePort
}

Write-Host "Abrindo $Service em http://localhost:$LocalPort"
Write-Host 'Mantenha esta sessao aberta; Ctrl+C encerra o tunel.'

& aws ssm start-session `
    --target $InstanceId `
    --document-name AWS-StartPortForwardingSession `
    --parameters "portNumber=$remotePort,localPortNumber=$LocalPort"

if ($LASTEXITCODE -ne 0) {
    throw "Nao foi possivel abrir o tunel SSM. Confirme AWS CLI, Session Manager plugin, IAM e status da EC2."
}

