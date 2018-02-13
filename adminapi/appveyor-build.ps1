Param(
	[parameter(Mandatory=$true)][string] $branch,
	[parameter(Mandatory=$true)][string] $build,
	[parameter(Mandatory=$true)][string] $image
) 

. ".\appveyor-util.ps1"

Invoke-Exe -cmd "dotnet" -args "restore src/Chiron.Admin.sln"
Invoke-Exe -cmd "dotnet" -args "build src/WebApi --version-suffix $build"
Invoke-Exe -cmd "dotnet" -args "publish src/WebApi --output ../../publish/www --version-suffix $build"

Invoke-Exe -cmd "docker" -args "build -t ${image}:${build} ."
Invoke-Exe -cmd "docker" -args "tag ${image}:${build} ${image}:${branch}"

if ($ENV:DOCKER_PUSH -eq $true) {
        Invoke-Exe -cmd "docker" -args "push ${image}:${build}"
        Invoke-Exe -cmd "docker" -args "push ${image}:${branch}"
} else {
        "Skipping docker push"
}

docker images | select-string $env:IMAGENAME
