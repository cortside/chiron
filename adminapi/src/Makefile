DOTNETFRAMEWORK	:= c:/WINDOWS/Microsoft.NET/Framework/v4.0.30319
MSBUILD		:= $(DOTNETFRAMEWORK)/MSBuild.exe

ifeq ($(OS),Windows_NT)
    DNU := dnu.cmd
else
    DNU := dnu
endif


include ./*.mk ./sql/*.mk

all:


.DELETE_ON_ERROR:

all: build_db build

.PHONY: versionAssets 
versionAssets:

web:
	cd webapi && dnx web
	
consume:
	cd SavedSearch.Consumer && ./run.bat

publish:
	cd SavedSearch.Publisher && dnx run


restore:
	$(DNU) restore

	
.PHONY: build 
build: restore
	cd webapi && $(DNU) build
	cd WebAPI.Tests && $(DNU) build
	
precommit: build
	cd WebAPI.Tests && dnx test

#image: build
#	docker build -t liveauctioneers/savedserch:0 .
#	docker run -t -d -p 80:80 -p 5000:5000 -e APP:webapi  liveauctioneers/savedserch:0
#	docker ps

.PHONY: clean
clean:
	rm -rf artifacts
	rm -rf webapi/bin
	rm -rf WebAPI.Tests/bin

cleanlogs:
	rm -f ./logs/*.log

reallyclean: clean cleanlogs
	$(DNU) clear-http-cache
	
versionhtml:
	sed -e "s|##_REPO_##|$(shell basename $(CURDIR))|" -e "s/##_REVISION_##/$(shell git rev-parse HEAD)/" -e "s/##_DATE_##/$(shell date)/"  -e "s/##_COMPUTER_##/$(shell hostname)/" -e "s/##_USER_##/$(USERNAME)/" -e "s/##_BUILD_##/$(shell cat crons/timestamp.txt)/" version.thtml > webapi/wwwroot/version.html
	
apidocs:
	rm -f swagger.json
	rm -f httpdocs/api/index.html
	rm -f httpdocs/api/main.css
	swagger validate swagger.yaml
	cat swagger.yaml | ruby -ryaml -rjson -e 'puts JSON.pretty_generate(YAML::load(ARGF.read))' > swagger.json
	swagger validate swagger.json
#	bootprint swagger swagger.json webapi/wwwroot
#from package.json
#-    "bootprint": "^0.8.0",
#-    "bootprint-swagger": "^0.13.1",

serve:
	./serve.sh

deserve:
	-./docker-stop.sh
	-docker rm mysql
	-docker rm memcached
	-docker rm rabbitmq
	
install:
	dnvm upgrade -u -r mono
	dnvm upgrade -u -r coreclr -p
	dnvm list
	sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
	echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
	sudo apt-get update
	apt-get install mono-complete
	mozroots --import --machine --sync
	certmgr -ssl -m https://go.microsoft.com
	certmgr -ssl -m https://nugetgallery.blob.core.windows.net
	certmgr -ssl -m https://nuget.org
	dnu commands install Microsoft.Dnx.Watcher


