## Chiron

https://en.wikipedia.org/wiki/Chiron

A sample implementation of a microservices based architecture.

With major contributions from:
* Roland Kwong
* Rui Fang Li
* Joe Shull
* and others that had been through Spring2...

To note:
* each folder in this repo really should be it's own repo and is only aggregated in a single repo for each of working on it and pulling it down to play

How to run:
* Set HOSTS file entry for chiron.docker.local pointing to the docker host that this is running on (i.e. 127.0.0.1)
* from PS shell, run ./build.ps1
* use docker compose to start up all services
    * docker-compose.exe up -d
* open broswer to http://chiron.docker.local:8000/adminui

There are 3 test users setup in the db with the following roles (all passwords are `test`:
* test -> customer
* clerk -> clerk
* admin -> admin
