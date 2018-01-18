Param(
	[parameter(Mandatory=$true)][string] $branch,
	[parameter(Mandatory=$true)][string] $build,
	[parameter(Mandatory=$true)][string] $image
) 

. ".\appveyor-util.ps1"

$build = Get-Build $branch $build

Invoke-Exe -cmd "dotnet" -args "restore"
Invoke-Exe -cmd "dotnet" -args "build --version-suffix $build"
Invoke-Exe -cmd "dotnet" -args "publish src/WebApi --output ../../publish/www --version-suffix $build"

Invoke-Exe -cmd "docker" -args "build -t ${image}:${build} ."
Invoke-Exe -cmd "docker" -args "tag ${image}:$build ${image}:$branch"

Invoke-Exe -cmd "docker" -args "images" | select-string ${image}
