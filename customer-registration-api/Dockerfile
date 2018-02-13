FROM cortside/chiron-base

COPY publish/ c:/
COPY build.json c:/www/
COPY rabbit_chiron.json c:/
COPY UpdateDefinitions.ps1 c:/
COPY startup.ps1 c:/

ENV DLLNAME Chiron.Registration.Customer.WebApi.dll
ENV ASPNETCORE_URLS http://+:5001
EXPOSE 5001
