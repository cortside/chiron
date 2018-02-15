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
