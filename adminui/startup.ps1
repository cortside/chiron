#show what network config looks like
ipconfig /all

& c:/load-config.ps1

$consul = "c:/consul.exe"
$args = "watch -http-addr=${env:consul}:8500 -type=keyprefix -prefix=config/files/${env:service}/${env:envtype}/ powershell -File c:\load-config.ps1"
start-process $consul $args

# start the service
cd c:/nginx; # nginx doesn't seem to like to be executed outside of its directory.
& c:/nginx/nginx.exe -g "daemon off;"