$env:DNX_BUILD_VERSION=$env:APPVEYOR_BUILD_NUMBER
dotnet --info
('{{"build":{{"version":"{0}","timestamp":"{1}"}}}}' -f $env:APPVEYOR_BUILD_VERSION, (get-date -f G)) | out-file build.json -Encoding ascii

npm install -g bower
npm install -g node-sass
src/WebApi/; bower install; 
node-sass --output-style expanded src/WebApi/wwwroot/styles/site.scss src/WebApi/wwwroot/styles/site.css
node-sass --output-style compressed src/WebApi/wwwroot/styles/site.scss src/WebApi/wwwroot/styles/site.min.css
./appveyor-build.ps1 $ENV:APPVEYOR_REPO_BRANCH $ENV:APPVEYOR_BUILD_VERSION $ENV:IMAGENAME

dotnet pack src\WebApi --no-build -o ..\..\artifacts

./appveyor-test.ps1 $ENV:APPVEYOR_REPO_BRANCH $ENV:APPVEYOR_BUILD_VERSION $ENV:IMAGENAME
