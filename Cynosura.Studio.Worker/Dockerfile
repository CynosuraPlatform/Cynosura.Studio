FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY Cynosura.Studio.Core/*.csproj ./Cynosura.Studio.Core/
COPY Cynosura.Studio.Data/*.csproj ./Cynosura.Studio.Data/
COPY Cynosura.Studio.Infrastructure/*.csproj ./Cynosura.Studio.Infrastructure/
COPY Cynosura.Studio.Worker/*.csproj ./Cynosura.Studio.Worker/

RUN dotnet restore Cynosura.Studio.Worker/Cynosura.Studio.Worker.csproj

COPY Cynosura.Studio.Core/. ./Cynosura.Studio.Core/
COPY Cynosura.Studio.Data/. ./Cynosura.Studio.Data/
COPY Cynosura.Studio.Infrastructure/. ./Cynosura.Studio.Infrastructure/
COPY Cynosura.Studio.Worker/. ./Cynosura.Studio.Worker/

RUN dotnet publish Cynosura.Studio.Worker/Cynosura.Studio.Worker.csproj -c Release -o ./out

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Cynosura.Studio.Worker.dll"]