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

$build = $ENV:APPVEYOR_BUILD_VERSION

# copy the scripts to setup the db
rm .\sql\  -recurse
mkdir sql > $null
cp ../auth/src/sql/table/*.sql ./sql
cp ../catalog/src/sql/*.sql ./sql

Invoke-Exe -cmd "docker" -args "build -t ${ENV:IMAGENAME}:${build} ."
Invoke-Exe -cmd "docker" -args "tag ${ENV:IMAGENAME}:$build ${ENV:IMAGENAME}:latest"


Invoke-Exe -cmd "docker" -args "images" | select-string ${ENV:IMAGENAME}
if ($ENV:DOCKER_PUSH -eq $true) {
	Invoke-Exe -cmd "docker" -args "push ${ENV:IMAGENAME}:${ENV:APPVEYOR_BUILD_VERSION}"
	Invoke-Exe -cmd "docker" -args "push ${ENV:IMAGENAME}:latest"
} else {
	"Skipping docker push"
}

