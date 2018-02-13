#show what network config looks like
ipconfig /all

ping db
ping rabbitmq

$i=0; do { $i++; start-sleep -Seconds 2; Write-Host "checking service"; $r = .\TestPort.ps1 -ComputerName rabbitmq -Port 15672 -Protocol TCP; Write-Host "returned $r";} while (!$r -And ($i -lt 15))
$i=0; do { $i++; start-sleep -Seconds 2; Write-Host "checking service"; $r = .\TestPort.ps1 -ComputerName db -Port 1433 -Protocol TCP; Write-Host "returned $r";} while (!$r -And ($i -lt 15))

if($env:configdir) {
	write-host "Loading local configs from: $env:configdir"
	$from = $env:configdir.replace("/", "\")
	if($from.endswith("\") -ne $true) { $from += "\" }
	$from += "*"
	cp $from c:\ -recurse -force
} else {
	write-host "Local configs not found, loading configs from consul (${env:consul}) with key 'config/files/${env:service}/${env:envtype}/'"
	& c:/load-config.ps1

	$consul = "c:/consul.exe"
	$args = "watch -http-addr=${env:consul}:8500 -type=keyprefix -prefix=config/files/${env:service}/${env:envtype}/ powershell -File c:\load-config.ps1"
	start-process $consul $args
}

c:\UpdateDefinitions.ps1

# start the service
cd c:\www
dotnet $env:DLLNAME