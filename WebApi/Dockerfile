FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /WebApi

COPY . ./

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

ENV TZ=America/Sao_Paulo \
    ASPNETCORE_URLS=http://*:5000

RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

WORKDIR /api

COPY --from=build-env /WebApi/out .

ENTRYPOINT ["dotnet", "WebApi.dll"]
