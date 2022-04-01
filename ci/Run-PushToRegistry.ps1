
[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $DockerfileDir,
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $Version
)

$PATH = Join-Path $DockerfileDir 'Dockerfile'

$VERSION_TAG = "pratikthanki9/fourthdown-api:${Version}"
$LATEST_TAG = "pratikthanki9/fourthdown-api:latest"


if ($env:DOCKER_USERNAME != "" -and $env:DOCKER_PASSWORD !="") {
    docker login -u="${DOCKER_USERNAME}" -p="${DOCKER_PASSWORD}"
    Write-Host "Docker login successful"
}
else {
    Write-Host "Username and Password not set"
}

Write-Host "Building Api docker image"
docker build -f $PATH -t $VERSION_TAG -t $LATEST_TAG .

Write-Host "Pushing image to registry"
docker push $VERSION_TAG
docker push $LATEST_TAG

Write-Host "Push completed.."
