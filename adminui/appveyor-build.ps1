. ".\appveyor-util.ps1"

Invoke-Exe -cmd "docker" -args "build -t ${ENV:IMAGENAME}:${ENV:APPVEYOR_BUILD_VERSION} ."
Invoke-Exe -cmd "docker" -args "tag ${ENV:IMAGENAME}:${ENV:APPVEYOR_BUILD_VERSION} ${ENV:IMAGENAME}:latest"

Invoke-Exe -cmd "docker" -args "images"
Invoke-Exe -cmd "docker" -args "push ${ENV:IMAGENAME}:${ENV:APPVEYOR_BUILD_VERSION}"
Invoke-Exe -cmd "docker" -args "push ${ENV:IMAGENAME}:latest"