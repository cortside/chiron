FROM cortside/nginx
SHELL ["powershell", "-Command"]
WORKDIR c:/

RUN mkdir logs
ENV CONSUL consul.service.consul

COPY load-config.ps1 c:/
COPY getconfigurationfiles.ps1 c:/
COPY startup.ps1 c:/
COPY nginx.conf.example c:/nginx/conf/nginx.conf

ENV CONSUL_VERSION 0.8.2

#RUN $AllProtocols = [System.Net.SecurityProtocolType]'Ssl3,Tls,Tls11,Tls12'; \
#	[System.Net.ServicePointManager]::SecurityProtocol = $AllProtocols; \
#	Invoke-WebRequest ('https://releases.hashicorp.com/consul/{0}/consul_{0}_windows_amd64.zip' -f ${ENV:CONSUL_VERSION}) -outfile 'consul.zip'; \
#	Expand-Archive consul.zip -DestinationPath .; \
#	rm consul.zip;

RUN Invoke-WebRequest ('https://releases.hashicorp.com/consul/{0}/consul_{0}_windows_amd64.zip' -f ${ENV:CONSUL_VERSION}) -outfile 'consul.zip'; \
	Expand-Archive consul.zip -DestinationPath .; \
	rm consul.zip;

	
COPY dist/ c:/www/

CMD powershell -File c:/startup.ps1