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

Function Get-Build {
	Param(
		[parameter(Mandatory=$true)][string] $branch,
		[parameter(Mandatory=$true)][string] $build		
	) 
	if ($branch -ne "master") {
		$s = $build.Split(".")
		$major = $s[0]
		$minor = $s[1]
		$build = "${major}.${minor}-${branch}"
	}
	return $build;
}