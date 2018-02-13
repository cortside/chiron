Param(
	[parameter(Mandatory=$true)][string] $branch,
	[parameter(Mandatory=$true)][string] $build,
	[parameter(Mandatory=$true)][string] $image
) 

. ".\appveyor-util.ps1"

$build = Get-Build $branch $build

gci -Recurse *.tests.csproj | % { dotnet test $_ }

Invoke-Exe -cmd "docker" -args "push ${image}:$build"

if ($branch -eq "develop" -OR $branch -eq "master") {
	Invoke-Exe -cmd "docker" -args "push ${image}:$branch"
}
