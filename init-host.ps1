docker run --restart always -d -p 5050:5050 `
        -e DOCKER_REMOTE_API='tcp://172.17.160.1:2375' `
        -e DOCKERDASH_USER='admin' `
        -e DOCKERDASH_PASSWORD='changeme' `
        --name dockerdash `
        cortside/dockerdash
		
docker run -d --name seq -p 5341:5341 cortside/seq