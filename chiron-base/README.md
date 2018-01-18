# README #

### What is this repository for? ###

Source for building an image with the following:

* NanoServer
* dotnet CLI
* consul
* scripts to get config files from consul

### How do I get set up? ###

You just need docker installed.

### How to build an application deployable image from this? ###
Building an application deployable image from this image is as simple as:

* Copying the files into the container's c:/www directory.
* Declaring the environment variable "DLLNAME" to be executed by the dotnet cli.
* Exposing the port your application uses.

Sample dockerfile:

```
#!dockerfile

FROM spring2/base-service

COPY outputdir/ c:/www/

ENV DLLNAME mynetcoreapp.dll
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000
```

Run image:

```
#!cmd

docker run -d myimagename
```

Run image (with local configurations):

```
#!cmd run

docker run -d -e "configdir=c:\configs" -v c:\work\project\config:c:\configs myimagename
```

## Environment Variables ##
* **DLLNAME** - the name of the dll which the dotnet will execute the application from. Specify this in your dockerfile.
* **CONSUL** - the hostname of the consul server. Default: consul.service.consul
* **SERVICE** - the name of your service that will show up in Consul. This should be passed in from docker run.
* **ENVTYPE** - the name of the environment that will show up in Consul as a tag. This should be passed in from docker run.
* **CONFIGDIR** - if you want to run the container locally and point it to a volume for configuration, you can pass this environment variable in as part of the docker run command.

CONSUL, SERVICE and ENVTYPE is used for constructing the URL to get config files down from the Consul KVP store.

## Local Configuration ##
If you wanted to execute certain code, without pulling the source and compiling to run it, you can pull the docker image down and run it in a container. However, you will want to specify your own local configurations. When you specify the CONFIGDIR environment variable, the included startup.ps1 script will copy the contents of that directory into the C:\ directory. If your application lives in the c:\www folder of the container and you're expecting to copy a config.json file into it, you will want to make sure that your config file on the host to be [host config folder]\www\config.json.

### Who do I talk to? ###
Cort or Roland.