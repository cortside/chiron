$root = 'http://rabbitmq:15672/api/definitions'
$user = "admin"
$pass= "password"
$secpasswd = ConvertTo-SecureString $pass -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential($user, $secpasswd)

$json = '{"rabbit_version":"3.6.10","users":[{"name":"admin","password_hash":"NPmtWGPmuXKMcJ7TPRNTUcPN9OHv6KaghIfaJpG10Ys4xUTp","hashing_algorithm":"rabbit_password_hashing_sha256","tags":"administrator"}],"vhosts":[{"name":"/"}],"permissions":[{"user":"admin","vhost":"/","configure":".*","write":".*","read":".*"}],"parameters":[],"global_parameters":[{"name":"cluster_name","value":"rabbit@bc95ece2a648"}],"policies":[],"queues":[{"name":"auth.queue","vhost":"/","durable":false,"auto_delete":false,"arguments":{}}],"exchanges":[{"name":"registration","vhost":"/","type":"topic","durable":true,"auto_delete":false,"internal":false,"arguments":{}}],"bindings":[{"source":"registration","vhost":"/","destination":"auth.queue","destination_type":"queue","routing_key":"#","arguments":{}}]}'

try {
	$result = Invoke-RestMethod $root -Method Post -Body $json -ContentType 'application/json' -Credential $credential
} catch {
    # Dig into the exception to get the Response details.
    # Note that value__ is not a typo.
    Write-Host "StatusCode:" $_.Exception.Response.StatusCode.value__ 
    Write-Host "StatusDescription:" $_.Exception.Response.StatusDescription
}