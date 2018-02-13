. ".\appveyor-util.ps1"

gci -Recurse *.tests.csproj | % { dotnet test $_ }

Invoke-Exe -cmd "docker" -args "push ${ENV:IMAGENAME}:${ENV:APPVEYOR_BUILD_VERSION}"
Invoke-Exe -cmd "docker" -args "push ${ENV:IMAGENAME}:latest"