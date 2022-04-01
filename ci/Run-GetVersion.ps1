
[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)] [ValidateNotNullOrEmpty()] [string] $ProjectDir
)

try {
    $PATH = Join-Path $ProjectDir 'Version.xml'
    [xml]$xmlElement = Get-Content -Path $PATH

    $VERSION = $xmlElement.Project.Version

    $UNIX_TIME_SECS = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds()
    $BUILD_VERSION = $VERSION + "." + $UNIX_TIME_SECS

    Write-Host "Preparing to build and test - ${BUILD_VERSION}"
    Write-Host "::set-output name=BUILD_VERSION::${BUILD_VERSION}"
}
catch {
    Write-Error "Error occurred in retrieving Version.."
}
