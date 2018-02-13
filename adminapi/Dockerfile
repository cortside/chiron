FROM cortside/chiron-base

COPY src/build.json c:/www/
COPY publish/ c:/

ENV DLLNAME Chiron.Admin.WebApi.dll
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000
