# read anything from stdin that there might be (when call with consul watch)
$line = [Console]::In.ReadLine()

filter timestamp {"$(Get-Date -Format G): $_"}
$outfile = "c:\logs\$($env:SERVICE)-watch.log"

# get any configuration files from consul kv store
& c:/GetConfigurationFiles.ps1 | timestamp | out-file -filepath $outfile -append -encoding ASCII