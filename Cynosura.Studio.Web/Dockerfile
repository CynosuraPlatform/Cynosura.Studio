FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY Cynosura.Studio.Core/*.csproj ./Cynosura.Studio.Core/
COPY Cynosura.Studio.Data/*.csproj ./Cynosura.Studio.Data/
COPY Cynosura.Studio.Infrastructure/*.csproj ./Cynosura.Studio.Infrastructure/
COPY Cynosura.Studio.Web/*.csproj ./Cynosura.Studio.Web/

RUN dotnet restore Cynosura.Studio.Web/Cynosura.Studio.Web.csproj

COPY Cynosura.Studio.Core/. ./Cynosura.Studio.Core/
COPY Cynosura.Studio.Data/. ./Cynosura.Studio.Data/
COPY Cynosura.Studio.Infrastructure/. ./Cynosura.Studio.Infrastructure/
COPY Cynosura.Studio.Web/. ./Cynosura.Studio.Web/

RUN dotnet publish Cynosura.Studio.Web/Cynosura.Studio.Web.csproj -c Release -o ./out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 5000

ENTRYPOINT ["dotnet", "Cynosura.Studio.Web.dll"]