if (!$Env:APPVEYOR_BUILD_NUMBER) { $Env:APPVEYOR_BUILD_NUMBER = "0" }
if (!$ENV:APPVEYOR_BUILD_VERSION) { $ENV:APPVEYOR_BUILD_VERSION = "1.0.0" }
if (!$ENV:DOCKER_PUSH) { $ENV:DOCKER_PUSH = $true }
if (!$ENV:APPVEYOR_REPO_BRANCH) { $ENV:APPVEYOR_REPO_BRANCH = "local" }

write-host "Build: $env:APPVEYOR_BUILD_NUMBER"
write-host "Version: $env:APPVEYOR_BUILD_VERSION"
write-host "Docker Push: $env:DOCKER_PUSH"


$BuildObject = New-Object -TypeName psobject        
$Build = New-Object -TypeName psobject
$builditems = [ordered] @{
	"version" = ""
	"timestamp" = ""
}

$builditems.version = "1.0.0"
$builditems.timestamp = (Get-Date).ToUniversalTime().ToString("u")

Foreach ( $item in $builditems.Keys ) {
	 $build | Add-Member -MemberType NoteProperty -Name $item  -Value $builditems.$item
}

$BuildObject | Add-Member -MemberType NoteProperty -Name build -Value $build
$BuildObject| convertto-json -depth 5 | out-file build.json -force 

cp build.json catalog/src/build.json
cp build.json adminapi/src/build.json
cp build.json auth/build.json
cp build.json customer-registration-api/src/build.json

$sw = [Diagnostics.Stopwatch]::StartNew()

# db
cd db;
$ENV:IMAGENAME = 'cortside/chiron-db'
./build.ps1;
cd ..;

# docker-base
cd chiron-base;
$ENV:IMAGENAME = 'cortside/chiron-base'
./appveyor-build.ps1;
cd ..;

# auth
cd auth;
$ENV:IMAGENAME = 'cortside/chiron-auth'
./appveyor-build.ps1 $ENV:APPVEYOR_REPO_BRANCH $ENV:APPVEYOR_BUILD_VERSION $ENV:IMAGENAME;
cd ..;

# registration
cd customer-registration-api;
$ENV:IMAGENAME = 'cortside/chiron-customer-registration-api'
./appveyor-build.ps1 $ENV:APPVEYOR_REPO_BRANCH $ENV:APPVEYOR_BUILD_VERSION $ENV:IMAGENAME;
cd ..;

## customerui
#cd customerui;
#$ENV:IMAGENAME = 'cortside/chiron-customerui'
#./build.ps1
#cd ..;

# catalog
cd catalog;
$ENV:IMAGENAME = 'cortside/chiron-catalog'
./build.ps1
cd ..;

# admin api
cd adminapi;
$ENV:IMAGENAME = 'cortside/chiron-adminapi'
./build.ps1
cd ..;

# admin ui
cd adminui;
$ENV:IMAGENAME = 'cortside/chiron-adminui'
./build.ps1
cd ..;

$sw.Stop()
Write-Host "Build completed in $($sw.Elapsed.TotalMinutes) minutes"
