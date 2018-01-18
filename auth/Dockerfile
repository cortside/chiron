FROM cortside/chiron-base

COPY publish/ c:/
COPY build.json c:/www/

ENV DLLNAME c7.auth.dll
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000