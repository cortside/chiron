$consul = $env:CONSUL
$service = $env:SERVICE
$envtype = $env:ENVTYPE
$kvstem = "config/files/${service}/${envtype}"
$kvroot = "http://${consul}:8500/v1/kv/${kvstem}"

"Attempting to get configuration files from: ${kvroot}/?keys"
try {
	$keys = Invoke-RestMethod "${kvroot}/?keys"
} catch {
	"no keys found for: ${kvroot}"
	$_.Exception.Response.StatusCode.Value__
}

foreach ($key in $keys) {
	if (!$key.EndsWith("/")) {
		"Found: $key"
		$file = $key.Substring($kvstem.Length)

		#make sure folder exists before outputing file 
		mkdir $(split-path $file) -Force
		
		"Creating $file"
		invoke-webrequest "${kvroot}${file}?raw" -UseBasicParsing -Outfile "${file}"
		"$((Get-Item $file).Length) bytes written"
	}
}
