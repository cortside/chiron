$ErrorActionPreference = 'Stop'; 
$ProgressPreference = 'SilentlyContinue';

Function check-result {
        if ($LastExitCode -ne 0) {
                $e = [char]27
                $start = "$e[1;31m"
                $end = "$e[m"
                $text = "ERROR: Exiting with error code $LastExitCode"
                Write-Host "$start$text$end"
                return $false
        }
        return $true
}

Function Invoke-Exe {
Param(
    [parameter(Mandatory=$true)][string] $cmd,
    [parameter(Mandatory=$true)][string] $args

)
        Write-Host "Executing: `"$cmd`" --% $args"
        Invoke-Expression "& `"$cmd`" $args"
        $result = check-result
        if (!$result) {
                throw "ERROR executing EXE"
        }
}


Function Build-Image {
Param(
    [parameter(Mandatory=$true)][string] $image,
    [parameter(Mandatory=$true)][string] $dockerfile
)

	Invoke-Exe -cmd "docker" -args "build -t ${image}:${build} -f ${dockerfile} ."
	Invoke-Exe -cmd "docker" -args "tag ${image}:$build ${image}:$branch"
	Invoke-Exe -cmd "docker" -args "images" | select-string ${image}

	if ($ENV:DOCKER_PUSH -eq $true) {
		Invoke-Exe -cmd "docker" -args "push ${image}:$build"
		Invoke-Exe -cmd "docker" -args "push ${image}:$branch"
	} else {
		"Skipping docker push"
	}	
}

$build = $ENV:APPVEYOR_BUILD_VERSION
if (!$build) {
	$build="0.0.0"
}
$branch = $ENV:APPVEYOR_REPO_BRANCH
if (!$branch) {
	$branch="local"
}

# only create real version for "master" build, otherwise use maj.min-branch
if ($branch -ne "master") {
	$s = $build.Split(".")
	$major = $s[0]
	$minor = $s[1]
	$build = "${major}.${minor}-${branch}"
}

"Building $build on $branch"
Invoke-Exe -cmd "docker" -args "pull cortside/nginx:latest"

"build base image"
Build-Image -image "cortside/chiron-adminui" -dockerfile "Dockerfile"
