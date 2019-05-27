FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
WORKDIR /app

COPY *.sln .
COPY Cynosura.Studio.Core/*.csproj ./Cynosura.Studio.Core/
COPY Cynosura.Studio.Data/*.csproj ./Cynosura.Studio.Data/
COPY Cynosura.Studio.Worker/*.csproj ./Cynosura.Studio.Worker/

RUN dotnet restore Cynosura.Studio.Worker/Cynosura.Studio.Worker.csproj

COPY Cynosura.Studio.Core/. ./Cynosura.Studio.Core/
COPY Cynosura.Studio.Data/. ./Cynosura.Studio.Data/
COPY Cynosura.Studio.Worker/. ./Cynosura.Studio.Worker/

RUN sed -i "s|c:\\\temp\\\Cynosura.Studio\\\|/my/logs/|g" Cynosura.Studio.Worker/nlog.config
RUN sed -i "s|c:\\\temp\\\|/my/logs/|g" Cynosura.Studio.Worker/nlog.config

RUN dotnet publish Cynosura.Studio.Worker/Cynosura.Studio.Worker.csproj -c Release -o ../out

FROM mcr.microsoft.com/dotnet/core/runtime:2.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Cynosura.Studio.Worker.dll"]