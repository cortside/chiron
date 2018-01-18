FROM microsoft/aspnetcore:2.0.0-nanoserver
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

RUN mkdir logs

ENV CONSUL consul.service.consul

ENV CONSUL_VERSION 0.9.2

RUN Invoke-WebRequest ('https://releases.hashicorp.com/consul/{0}/consul_{0}_windows_amd64.zip' -f ${ENV:CONSUL_VERSION}) -outfile 'consul.zip'; \
	Expand-Archive consul.zip -DestinationPath .; \
	rm consul.zip;

COPY load-config.ps1 c:/
COPY getconfigurationfiles.ps1 c:/
COPY startup.ps1 c:/

CMD powershell -File startup.ps1