FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY Cynosura.Studio.Core/*.csproj ./Cynosura.Studio.Core/
COPY Cynosura.Studio.Data/*.csproj ./Cynosura.Studio.Data/
COPY Cynosura.Studio.Web/*.csproj ./Cynosura.Studio.Web/

RUN dotnet restore Cynosura.Studio.Web/Cynosura.Studio.Web.csproj

COPY Cynosura.Studio.Core/. ./Cynosura.Studio.Core/
COPY Cynosura.Studio.Data/. ./Cynosura.Studio.Data/
COPY Cynosura.Studio.Web/. ./Cynosura.Studio.Web/

RUN dotnet publish Cynosura.Studio.Web/Cynosura.Studio.Web.csproj -c Release -o ../out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Cynosura.Studio.Web.dll"]