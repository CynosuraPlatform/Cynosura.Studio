# План адаптации Cygnus.Studio / Cynosura.Studio под Ubuntu 24.04

**Версия:** 2.2  
**Дата:** 25.11.2026  
**Статус:** план; код и конфигурация репозитория не изменялись  
**Исходный репозиторий:** `/srv/GitHub/Cygnus.Studio`  
**Документация:** `/srv/GitHub/AIreadme/Cygnus/`

---

## 0. Что изменено в версии 2.0

Версия 1.0 была проверена против фактического состояния репозитория. Основные исправления:

1. Приложение использует **SQLite**, а не SQL Server. SQL Server-контейнер в compose не используется.
2. Добавлены блокеры, без которых `dotnet build` и `docker compose up` не работают:
   - регистр `tests`/`Tests` в `.sln`;
   - отсутствующий проект `Cynosura.Studio.Generator` в Dockerfile;
   - `npm`-сборка фронтенда внутри `dotnet publish`;
   - конфликт `Kestrel:EndPoints` и `ASPNETCORE_URLS`;
   - повреждённый `mycert.pfx`;
   - отсутствующие mount'ы сертификата, SQLite и `wwwroot/static`;
   - `rabbitmq://localhost` внутри контейнеров.
3. Разведены два режима запуска: **host dev** и **docker compose**, с единой таблицей портов и URL.
4. Добавлены этапы проверки git/CLI/тестов и CI.
5. Чек-лист переписан так, чтобы его пункты действительно проверяли результат.

### 0.1. Что изменено в версии 2.1

Добавлен разбор мины в target `PublishRunWebpack` (`--prod`, `--outputPath`, обратные слэши) с проверкой на фактическом окружении:

- новый пункт **P1-13** в реестре проблем;
- новый раздел **2.4** с результатами проверок и инструкцией по исправлению;
- расширен **этап 2**: полный исправленный target, вынос флагов в `package.json`, проверка `dotnet publish`;
- обновлены раздел 5 (список изменяемых файлов) и чек-лист приёмки.

Важное уточнение по фактам (проверено, см. 2.4): флаг `--prod` в Angular CLI **13** ещё работает как deprecated shorthand для `--configuration production`; удалён он в **14**. То есть это не текущий блокер, а мина на обновление Angular. Текущий же блокер `dotnet publish` на Linux — обратные слэши в `SpaRoot`/`Exec WorkingDirectory` (P0-2).

### 0.2. Что изменено в версии 2.2 (выполнение плана)

План выполнен на фактическом окружении (`/srv/GitHub/Cygnus.Studio`, ветка `development`). Итоги проверок и найденные недочёты зафиксированы ниже.

**Что сделано (по этапам):**

1. `.sln`: `tests\` → `Tests\` (P0-1).
2. `Web.csproj`: кросс-платформенный `SpaRoot`, обратные слэши в путях убраны, target `PublishRunWebpack` получил `Condition` и использует `npm ci` + `npm run build:prod` (P0-2, P0-5, P1-4, P1-13).
3. `Web.Ng/package.json`: скрипт `build:prod`, `engines.node` (P1-3).
4. Dockerfile Web/Worker: добавлен `Cynosura.Studio.Generator`, в Web — `-p:SkipSpaBuild=true` (P0-3, P0-4); WebNg: `--configuration production` (P1-13).
5. compose: удалён `db`, добавлен healthcheck `mq` + `condition: service_healthy`, общие volume `data`/`static`, убран порт 5001 и Kestrel-cert переменные, `Messaging__ConnectionUrl=rabbitmq://mq`, лишние порты RabbitMQ (4369/5671/15671) убраны (P0-6, P0-9, P0-10, P0-11, P1-11).
6. appsettings: убран `Kestrel:EndPoints`, redirect на `4300`, storage URL/path на `http://localhost:43000/static` и `wwwroot/static` (P0-6, P0-12, P0-13, P0-14).
7. `volumes/webng/config.json`: `http://localhost:5000` (P0-15).
8. Сертификат перегенерирован; `.gitattributes` — binary-правила; `.gitignore` — cert/emails/static/NgApp (P0-8).
9. `Worker/Program.cs`: миграции SQLite до запуска (P0-10).
10. `GitMerge.cs`: `checkout master` → `checkout -`, проверка exit code, `commit --allow-empty` (P1-5).
11. `azure-pipelines.yml`: ubuntu-24.04, build/test, pack `--output`, убран пустой NuGetToolInstaller (P1-7, P1-8).
12. BOM убран из `docker-compose.Development.yaml` и `.dockerignore` (P2).
13. `global.json`, `.nvmrc` (этапы 1, 8).

**Результаты проверок:**

| Проверка | Команда | Результат |
|---|---|---|
| Сборка решения | `dotnet build Cynosura.Studio.sln --no-restore` | 0 ошибок, 0 ошибок (протобуф-варнинги «unused import» — безвредны) |
| Тесты (без functional) | `dotnet test --filter "FullyQualifiedName!~FunctionalTests"` | 38 passed, 0 failed |
| Сборка фронта | `npm run build:prod` | exit 0, `NgApp` создан |
| Публикация (без SPA) | `dotnet publish -p:SkipSpaBuild=true` | exit 0 |
| Публикация (со сборкой SPA) | `dotnet publish` (полный) | exit 0, `NgApp` в output |
| Compose конфиг | `docker compose ... config` | exit 0 |
| Сертификат | `openssl pkcs12 ... -passin pass:${CERT_PASSWORD}` | валиден |
| Синтаксис CLI | `grep -rn -- "--prod"` / `"--outputPath"` | пусто |

**Найденные недочёты / уточнения:**

- **GitMerge (P1-5)**: `git merge` при конфликтах возвращает ненулевой код — это штатно (результат обрабатывается в `Sync()`), поэтому проверка exit code не должна касаться merge. Также обязателен `commit --allow-empty`: при отсутствии изменений обычный commit падает с «nothing to commit» (тесты `MergeDirectoryAsync_NoChange`/`_Delete`).
- **`Cynosura.Studio.Core.UnitTests` содержит 0 тестов** (пустая папка `Requests`) — для него `dotnet test` выводит «No test matches». Не блокер, но `dotnet test Cynosura.Studio.sln` не проверяет Core-логику юнит-тестами.
- **BOM** есть не только в `docker-compose.Development.yaml` и `.dockerignore` (убраны), но и в обоих `appsettings.Development.json` и в `Web.csproj` (безвреден, оставлен как есть).
- **nuget.org нестабилен** (NU1900 504 при проверке vulnerability data). Restore/build/test выполнялись с зеркалом `mirrors.huaweicloud.com` (см. 2.4 / шаг «Как поправить»).

---

## 1. Проверенная архитектура

| Компонент | Фактическое состояние | Подтверждение |
|---|---|---|
| Backend | .NET 8, ASP.NET Core | `TargetFramework=net8.0` |
| Frontend | Angular 13.3.12 | `package.json`, `angular.json` |
| СУБД | **SQLite** (`Data Source=cynosura.db`) | `Data.csproj`, `Startup.cs`, `Worker/Program.cs` |
| Очередь | RabbitMQ, MassTransit | `Messaging` в `appsettings.Development.json` |
| Docker | web, webng, worker, db, mq | `docker-compose.yaml` |
| CI | Azure Pipelines, только упаковка CLI | `azure-pipelines.yml` |
| CLI | .NET global tool `cyn` | `Cynosura.Studio.CliTool.csproj` |

### 1.1. Важное решение по СУБД

В этой версии плана **оставляем SQLite**. Переход на SQL Server — отдельная задача, требующая миграций, строк подключения и проверки EF-провайдера. Сервис `db` из compose удаляется, так как приложение его не использует.

---

## 2. Реестр проблем

### 2.1. Блокеры (P0)

| # | Файл / место | Проблема | Что делать |
|---|---|---|---|
| P0-1 | `Cynosura.Studio.sln:26` | Путь `tests\...`, реальный каталог `Tests/`. На Linux регистрозависим. | Исправить на `Tests\...`; проверить все project paths. |
| P0-2 | `Cynosura.Studio.Web/Cynosura.Studio.Web.csproj:8,42-43` | `SpaRoot` и `Exec WorkingDirectory` содержат `\`; на Linux это не разделитель. | Перейти на `/` и/или `$(MSBuildThisFileDirectory)`. |
| P0-3 | `Cynosura.Studio.Web/Dockerfile:5-15` | Не копируется `Cynosura.Studio.Generator`, хотя `Core` на него ссылается. | Добавить copy csproj и исходников Generator. |
| P0-4 | `Cynosura.Studio.Worker/Dockerfile:5-15` | То же: отсутствует Generator. | Добавить copy Generator. |
| P0-5 | `Cynosura.Studio.Web.csproj:40-54` | При `dotnet publish` запускается `npm install`/`npm run build` в отсутствующем каталоге; в SDK-образе нет Node. | Добавить `Condition="'$(SkipSpaBuild)' != 'true'"` и передавать `-p:SkipSpaBuild=true` в Docker. |
| P0-6 | `appsettings.json:38-43`, `docker-compose.Development.yaml:5` | `Kestrel:EndPoints` задаёт `43000`, а compose — `ASPNETCORE_URLS=http://+:5000`. Kestrel endpoints имеют приоритет. | Убрать `Kestrel:EndPoints` из общего `appsettings.json`; порт задавать через `ASPNETCORE_URLS`/launchSettings. |
| P0-7 | `docker-compose.Development.yaml:6,16-17` | `Jwt__CertificatePath` указывает на `/my/cert/mycert.pfx`, но mount `./volumes/cert:/my/cert` отсутствует. | Вернуть mount; задать `Jwt__CertificatePassword`. |
| P0-8 | `volumes/cert/mycert.pfx` | Файл повреждён: содержит UTF-8 replacement characters, `openssl pkcs12` не читает. | Перегенерировать сертификат; добавить binary-правила в `.gitattributes`. |
| P0-9 | `docker-compose.yaml:18-19`, `docker-compose.Development.yaml:26-33` | SQL Server-контейнер не используется приложением. | Удалить `db` из compose и из плана. |
| P0-10 | `appsettings.json`, `Worker/appsettings.json` | `Data Source=cynosura.db` — относительный путь. В контейнерах web/worker получают разные БД; worker падает на пустой БД. | Общий volume `/data`, `Data Source=/data/cynosura.db`; worker выполняет миграции. |
| P0-11 | `Web/appsettings.Development.json:20-23`, `Worker/appsettings.Development.json` | `rabbitmq://localhost` внутри контейнера указывает на сам контейнер, а не на `mq`. | В compose задать `Messaging__ConnectionUrl=rabbitmq://mq`. |
| P0-12 | `Worker/appsettings.json:15` | `../../../../Cynosura.Studio.Web/wwwroot/static` уходит в `/Cynosura.Studio.Web/...`. | Исправить на `../Cynosura.Studio.Web/wwwroot/static` для host dev; в compose — `/app/wwwroot/static`. |
| P0-13 | `Web/appsettings.json:49`, `Worker/appsettings.json:14` | `FileSystemStorage.Url` указывает на несуществующие порты `42001`/`46457`. | Привести к `http://localhost:43000/static` (host) и `http://localhost:5000/static` (compose). |
| P0-14 | `Web/appsettings.json:14-15` | Redirect URI `4200`, а Angular dev server — `4300`. | Исправить на `4300`; compose переопределяет на `5010`. |
| P0-15 | `volumes/webng/config.json` | `apiBaseUrl: https://localhost:5001`, хотя backend в dev работает по HTTP на `5000`. | Исправить на `http://localhost:5000`. |

### 2.2. Важные проблемы (P1)

| # | Место | Проблема | Что делать |
|---|---|---|---|
| P1-1 | `PLAN` / `INSTALL` / `README` | Противоречивые порты и утверждение про SQL Server. | Синхронизировать документы с таблицей из раздела 3.3. |
| P1-2 | `INSTALL_UBUNTU_24.md:7` | «Ubuntu 24.04 не включает .NET 8» — неточно. | Использовать `sudo apt install dotnet-sdk-8.0` или явно обосновать фид Microsoft. |
| P1-3 | `README.md:7`, `INSTALL:25-34` | Node 16 EOL; версия не зафиксирована. | Добавить `.nvmrc`, `engines`; зафиксировать версию. |
| P1-4 | `README.md`, `INSTALL:112` | `npm install` вместо `npm ci`. | Использовать `npm ci` при наличии lock-файла. |
| P1-5 | `Cynosura.Studio.Generator/Merge/GitMerge.cs:84` | Предполагается ветка `master`; exit code git-команд не проверяется. | Возвращаться на исходную ветку через `git checkout -` (не хардкодить имя); проверять exit code для init/add/checkout/commit; `commit --allow-empty` (при отсутствии изменений обычный commit падает); для `merge` конфликт (≠0) — штатно, exit code не проверять. |
| P1-6 | `Tests/Cynosura.Studio.CliTool.FunctionalTests/NewCommandTest.cs` | Тест скачивает `Cynosura.Template` из NuGet, требует сеть. | Пометить как integration / исключать из быстрого прогона. |
| P1-7 | `azure-pipelines.yml:12-21` | `dotnet pack` кладёт пакет в `./nupkg`, а push ищет в `$(Build.ArtifactStagingDirectory)`; пустой `versionSpec`. | Передавать `--output $(Build.ArtifactStagingDirectory)`; заполнить/убрать `NuGetToolInstaller`. |
| P1-8 | `azure-pipelines.yml` | Нет сборки/тестов Web/Worker/Docker; смена image ничего не проверяет. | Добавить build/test, при необходимости docker build. |
| P1-9 | `Cynosura.Studio.Infrastructure` | EPPlus 5.1.0 использует `System.Drawing`; на Linux возможны проблемы с Excel-экспортом. | Проверить экспорт; при необходимости `libgdiplus`/шрифты или обновить EPPlus. |
| P1-10 | Docker images | `node:16-alpine`, `nginx:1.14.1-alpine`, `rabbitmq:3.8-management` устарели/EOL. | Зафиксировать как техдолг; обновление — отдельной задачей. |
| P1-11 | `docker-compose` | Нет healthcheck у `mq`; `depends_on` не ждёт готовности. | Добавить healthcheck и `condition: service_healthy`. |
| P1-12 | `docker-compose.Development.yaml` | `${HOME}` при запуске через `sudo` станет `/root`. | Запускать compose от пользователя; либо явно задавать `HOME`. |
| P1-13 | `Cynosura.Studio.Web/Cynosura.Studio.Web.csproj:42` | `npm run build -- --prod --outputPath=...`: `--prod` deprecated с CLI 12 и удалён в CLI 14, `--outputPath` (camelCase) в CLI 14 запрещён. Сейчас (CLI 13.3.10) работает, при обновлении Angular `dotnet publish` упадёт. | Заменить на `--configuration production` и `--output-path`; лучше вынести флаги в `package.json` (`build:prod`). См. 2.4. |

### 2.3. Мелочи (P2)

- BOM в начале `docker-compose.Development.yaml` и `.dockerignore`.
- Устаревший `version: '3'` в compose.
- `favicon.ico` также содержит replacement characters — добавить `*.ico binary`.
- `mcr.microsoft.com/mssql/server:2019-latest` не работает на ARM64 (но SQL Server мы удаляем).
- `appsettings.Development.json`: относительные `EmailSender.PickupFolder` (`../../emails`, `../../../../../emails`) стоит привести к `../emails`.
- `dotnet tool install -g` требует `$HOME/.dotnet/tools` в `PATH`.

### 2.4. Мина в `PublishRunWebpack`: `--prod`, `--outputPath`, обратные слэши

Текущий target в `Cynosura.Studio.Web/Cynosura.Studio.Web.csproj`:

```xml
<Target Name="PublishRunWebpack" AfterTargets="ComputeFilesToPublish">
  <Exec WorkingDirectory="$(SpaRoot)" Command="npm install" />
  <Exec WorkingDirectory="$(SpaRoot)" Command="npm run build -- --prod --outputPath=../Cynosura.Studio.Web/NgApp" />
  <ItemGroup>
    <DistFiles Include="NgApp\**" />
    ...
```

#### Что проверено фактически

Окружение: Ubuntu 24.04, .NET SDK 8.0.134, Angular CLI 13.3.10, Angular 13.3.12, Node 16.20.2.

| Проверка | Как проверяли | Результат |
|---|---|---|
| Флаг `--prod` | `npx ng build --prod --outputPath=/tmp/ngtest` | **Работает**, exit 0. `--prod` присутствует в `ng build --help` как «Shorthand for --configuration=production» |
| camelCase `--outputPath` | та же команда | **Работает** в CLI 13 |
| Обратный слэш в `WorkingDirectory` | MSBuild `Exec WorkingDirectory="..\dir\"` | **Падает**: `MSB6003 ... DirectoryNotFoundException` |
| Обратный слэш в `Include` | `DistFiles Include="NgApp\**"` | **Работает**: MSBuild нормализует `\` → `/` |

#### Выводы

1. **`--prod` — не текущий блокер.** В Angular CLI 13 он ещё поддерживается; deprecated с CLI 12, удалён в **CLI 14** (breaking change 14.0.0: «Deprecated option `--prod` has been removed from all builders»). Пока проект на 13.3.12, publish из-за него не падает. Но при обновлении Angular publish сломается с `Error: Unknown argument: prod`.
2. **`--outputPath` — такая же отложенная мина.** В CLI 14 переписан парсер аргументов, camelCase-опции не поддерживаются. Правильное имя — `--output-path`.
3. **Настоящий текущий блокер — обратные слэши** в `SpaRoot` и `Exec WorkingDirectory` (P0-2). На Linux `Exec` падает с `MSB6003` до вызова npm, то есть `dotnet publish` не доходит до сборки фронтенда.
4. `npm install` вместо `npm ci` (P1-4) делает publish невоспроизводимым: команда может изменить `package-lock.json` и подтянуть другие версии.
5. Нет `Condition="'$(SkipSpaBuild)' != 'true'"` (P0-5): нельзя собрать backend в SDK-образе, где нет Node.

#### Как поправить

Шаг 1. Вынести флаги Angular CLI из `.csproj` в `package.json` — тогда синтаксис CLI живёт в одном месте и не зависит от MSBuild:

```json
"scripts": {
  "build": "ng build",
  "build:prod": "ng build --configuration production --output-path=../Cynosura.Studio.Web/NgApp"
}
```

Шаг 2. Исправить target: кросс-платформенный `SpaRoot`, `npm ci`, вызов скрипта, условие пропуска:

```xml
<PropertyGroup>
  <!-- без обратных слэшей: на Linux '\' не разделитель пути -->
  <SpaRoot>$(MSBuildThisFileDirectory)../Cynosura.Studio.Web.Ng/</SpaRoot>
</PropertyGroup>

<Target Name="PublishRunWebpack"
        AfterTargets="ComputeFilesToPublish"
        Condition="'$(SkipSpaBuild)' != 'true'">
  <Exec WorkingDirectory="$(SpaRoot)" Command="npm ci" />
  <Exec WorkingDirectory="$(SpaRoot)" Command="npm run build:prod" />

  <ItemGroup>
    <DistFiles Include="NgApp\**" />
    <ResolvedFileToPublish Include="@(DistFiles->'%(FullPath)')" Exclude="@(ResolvedFileToPublish)">
      <RelativePath>%(DistFiles.Identity)</RelativePath>
      <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
      <ExcludeFromSingleFile>true</ExcludeFromSingleFile>
    </ResolvedFileToPublish>
  </ItemGroup>
</Target>
```

`DistFiles Include="NgApp\**"` можно оставить как есть: MSBuild на Linux нормализует обратные слэши в `Include` (проверено). Менять обязательно только пути, которые передаются в shell (`WorkingDirectory`, аргументы `Exec`).

Шаг 3. Если фронтенд собирается отдельно (CI, отдельный образ `webng`), публиковать backend без него:

```bash
dotnet publish Cynosura.Studio.Web/Cynosura.Studio.Web.csproj -c Release -o ./out -p:SkipSpaBuild=true
```

Шаг 4. Проверить результат:

```bash
cd /srv/GitHub/Cygnus.Studio

# 1. publish со сборкой фронта
dotnet publish Cynosura.Studio.Web/Cynosura.Studio.Web.csproj -c Release -o /tmp/pubtest
ls /tmp/pubtest/NgApp/main.*.js

# 2. publish без сборки фронта
dotnet publish Cynosura.Studio.Web/Cynosura.Studio.Web.csproj -c Release -o /tmp/pubtest2 -p:SkipSpaBuild=true
test ! -d /tmp/pubtest2/NgApp && echo "SkipSpaBuild работает"

# 3. в репозитории не осталось устаревшего синтаксиса
grep -rn -- "--prod\b" --include=*.csproj --include=*.json --include=*.yml . | grep -v node_modules
grep -rn -- "--outputPath" --include=*.csproj --include=*.json --include=*.yml . | grep -v node_modules
```

Обе команды `grep` должны ничего не выводить.

---

## 3. Целевое состояние

### 3.1. Host dev (без Docker)

| Что | Значение |
|---|---|
| Backend | `dotnet run --project Cynosura.Studio.Web`, `http://localhost:43000` |
| Frontend | `cd Cynosura.Studio.Web.Ng && npm ci && npm run start`, `http://localhost:4300` |
| БД | SQLite, единый файл, путь задаётся `ConnectionStrings__DefaultConnection` |
| RabbitMQ | `rabbitmq://localhost` (контейнер `mq` с проброшенным 5672) |
| File storage | `Cynosura.Studio.Web/wwwroot/static` |
| Git | настроены `user.name`, `user.email`, `init.defaultBranch=master` |

### 3.2. Docker compose (dev)

| Сервис | Порт | Назначение |
|---|---|---|
| `web` | `5000` | Backend API, HTTP |
| `webng` | `5010` | Angular + nginx |
| `mq` | `5672`, `15672` | RabbitMQ, management UI |
| `worker` | — | Фоновые задачи |

`db` (SQL Server) удаляется. HTTPS в dev-профиле не используем: это упрощает сертификаты и убирает конфликт с `Kestrel:EndPoints`. Если HTTPS нужен, он оформляется отдельным профилем (`docker-compose.Https.yaml`) и отдельным этапом.

### 3.3. Единая таблица портов и URL

| Режим | Backend API | Frontend | RabbitMQ UI | File storage URL |
|---|---|---|---|---|
| Host dev | `http://localhost:43000` | `http://localhost:4300` | `http://localhost:15672` | `http://localhost:43000/static` |
| Docker compose | `http://localhost:5000` | `http://localhost:5010` | `http://localhost:15672` | `http://localhost:5000/static` |

---

## 4. Пошаговый план

### Этап 0. Базовая проверка до изменений

Выполнить и сохранить результат:

```bash
cd /srv/GitHub/Cygnus.Studio

dotnet --version
node --version
npm --version
docker --version
docker compose version
git --version

dotnet build Cynosura.Studio.sln
dotnet test --no-build
docker compose -f docker-compose.yaml -f docker-compose.Development.yaml config
```

Ожидаемо: сборка/тесты/`docker compose` могут упасть на P0-1…P0-5. Это фиксируется как baseline, а не как неожиданность.

### Этап 1. Solution и сборка

1. `Cynosura.Studio.sln:26`: `tests\...` → `Tests\...`.
2. Проверить все пути в `.sln` на соответствие регистру каталогов.
3. В `Cynosura.Studio.Web.csproj` заменить обратные слэши в путях, которые не являются `ProjectReference`:
   - `SpaRoot`;
   - `Exec WorkingDirectory`;
   - `Protobuf Include`;
   - `DistFiles Include`;
   - `None Update`.
4. Для `SpaRoot` использовать кросс-платформенный вариант:

```xml
<SpaRoot>$(MSBuildThisFileDirectory)../Cynosura.Studio.Web.Ng</SpaRoot>
```

5. Добавить `global.json` с фиксацией .NET 8:

```json
{
  "sdk": {
    "version": "8.0.100",
    "rollForward": "latestFeature"
  }
}
```

**Проверка:** `dotnet build Cynosura.Studio.sln` завершается с кодом 0.

### Этап 2. Docker build: web и worker

1. В `Cynosura.Studio.Web/Dockerfile` добавить:

```dockerfile
COPY Cynosura.Studio.Generator/*.csproj ./Cynosura.Studio.Generator/
...
COPY Cynosura.Studio.Generator/. ./Cynosura.Studio.Generator/
```

2. То же — в `Cynosura.Studio.Worker/Dockerfile`.
3. В `Cynosura.Studio.Web.csproj` обернуть SPA-сборку условием:

```xml
<Target Name="PublishRunWebpack"
        AfterTargets="ComputeFilesToPublish"
        Condition="'$(SkipSpaBuild)' != 'true'">
  <Exec WorkingDirectory="$(SpaRoot)" Command="npm ci" />
  <Exec WorkingDirectory="$(SpaRoot)"
        Command="npm run build -- --configuration production --output-path=$(MSBuildThisFileDirectory)../Cynosura.Studio.Web/NgApp" />
  ...
</Target>
```

   Обратите внимание: `--configuration production` и `--output-path` (kebab-case), а не `--prod` / `--outputPath`. Предпочтительно вообще вынести флаги в `package.json` (`build:prod`) и вызывать `npm run build:prod`, чтобы `.csproj` не зависел от синтаксиса Angular CLI. Подробный разбор, почему это мина, и проверки — раздел 2.4.

4. В `Cynosura.Studio.Web/Dockerfile`:

```dockerfile
RUN dotnet publish Cynosura.Studio.Web/Cynosura.Studio.Web.csproj \
    -c Release -o ./out -p:SkipSpaBuild=true
```

5. Проверить, что в build-контексте есть `Cynosura.Studio.Generator` и он не исключён `.dockerignore`.

**Проверка:** `docker compose -f docker-compose.yaml -f docker-compose.Development.yaml build web worker` завершается успешно; дополнительно — `dotnet publish` на хосте (шаг 4 в разделе 2.4).

### Этап 3. Порты и HTTP/HTTPS

1. Удалить секцию `Kestrel:EndPoints` из `Cynosura.Studio.Web/appsettings.json`, чтобы не конфликтовать с `ASPNETCORE_URLS`.
2. Host dev: порт `43000` остаётся в `Properties/launchSettings.json`.
3. Compose: `ASPNETCORE_URLS: 'http://+:5000'`.
4. Удалить из compose порт `5001`, `ASPNETCORE_Kestrel__Certificates__Default__*` и mount `${USERPROFILE}/.aspnet/https`, если HTTPS не нужен.
5. Если HTTPS всё же нужен — отдельный профиль:
   - заменить `${USERPROFILE}` на `${HOME}`;
   - сгенерировать `aspnet.pfx`;
   - добавить `Kestrel__EndPoints__Https__Url=https://+:5001`;
   - обновить `webng/config.json` на `https://localhost:5001`;
   - описать доверие к self-signed сертификату.

**Проверка:** `docker compose ... up -d web`, затем `curl -I http://localhost:5000/swagger/index.html`.

### Этап 4. Сертификаты

1. Удалить повреждённый `volumes/cert/mycert.pfx` из индекса git:

```bash
git rm --cached volumes/cert/mycert.pfx
```

2. Добавить в `.gitattributes`:

```gitattributes
*.pfx binary
*.ico binary
*.png binary
*.jpg binary
*.woff binary
*.woff2 binary
```

3. Добавить в `.gitignore` (если ещё нет):

```gitignore
volumes/cert/*.pfx
```

4. Генерировать dev-сертификат локально:

```bash
mkdir -p volumes/cert
openssl req -x509 -newkey rsa:2048 -nodes \
  -keyout /tmp/mycert.key -out /tmp/mycert.crt \
  -days 3650 -subj "/CN=localhost"
openssl pkcs12 -export \
  -out volumes/cert/mycert.pfx \
  -inkey /tmp/mycert.key -in /tmp/mycert.crt \
  -passout pass:${CERT_PASSWORD}
rm -f /tmp/mycert.key /tmp/mycert.crt
```

5. В compose вернуть mount и пароль:

```yaml
Jwt__CertificatePath: '/my/cert/mycert.pfx'
Jwt__CertificatePassword: '${CERT_PASSWORD}'
volumes:
  - ./volumes/cert:/my/cert:ro
```

6. Проверить, нужен ли `Jwt__Authority`. В старой версии compose он был:

```yaml
Jwt__Authority: 'http://localhost:5000'
```

Если JWT-валидация без него не работает — вернуть.

7. Восстановить `favicon.ico` из исходного репозитория/сгенерировать заново; после добавления `*.ico binary` проверить, что файл не содержит `EF BF BD`.

**Проверка:**

```bash
openssl pkcs12 -in volumes/cert/mycert.pfx -info -noout -passin pass:${CERT_PASSWORD}
```

### Этап 5. SQLite, общая БД и worker

1. Удалить `db` из `docker-compose.yaml` и `docker-compose.Development.yaml`; удалить `depends_on: db` и volume `db`.
2. Добавить named volume `data` и монтировать в `web` и `worker` как `/data`.
3. Задать обоим сервисам:

```yaml
ConnectionStrings__DefaultConnection: 'Data Source=/data/cynosura.db'
```

4. В `Worker/Program.cs` до `host.Run()` выполнить миграции:

```csharp
var host = CreateHostBuilder(args).Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    await db.Database.MigrateAsync();
}

await host.RunAsync();
```

`Main` сделать `async Task Main`. Это нужно, потому что `WorkerInfoShedulerService.StartAsync` сразу читает `WorkerInfos`, и без таблиц worker падает.

5. Предусмотреть повтор/ожидание миграции, если web и worker стартуют одновременно (SQLite lock). Достаточно retry с задержкой; либо запускать worker после успешного старта web.
6. Host dev: задавать `ConnectionStrings__DefaultConnection` с абсолютным путём, например:

```bash
export ConnectionStrings__DefaultConnection="Data Source=/srv/GitHub/Cygnus.Studio/cynosura.db"
```

7. В `Worker/appsettings.json` не оставлять `../../../../...`; для host dev использовать `../Cynosura.Studio.Web/wwwroot/static` (см. этап 7).

**Проверка:** после `docker compose up -d` в логах worker нет `no such table`, а файл `/data/cynosura.db` существует и переживает `docker compose restart`.

### Этап 6. RabbitMQ

1. В compose для `web` и `worker`:

```yaml
Messaging__ConnectionUrl: 'rabbitmq://mq'
Messaging__Username: 'Manager'
Messaging__Password: '${RABBITMQ_PASSWORD}'
```

2. В `docker-compose.yaml` добавить healthcheck:

```yaml
mq:
  image: rabbitmq:3.8-management
  hostname: "mq"
  healthcheck:
    test: ["CMD", "rabbitmq-diagnostics", "-q", "ping"]
    interval: 10s
    timeout: 5s
    retries: 10
```

3. Для `web` и `worker`:

```yaml
depends_on:
  mq:
    condition: service_healthy
```

4. Host dev оставляет `rabbitmq://localhost`, так как `mq` пробрасывает 5672 на хост.

**Проверка:** `curl -u Manager:${RABBITMQ_PASSWORD} http://localhost:15672/api/overview`; в логах web/worker нет ошибок подключения к RabbitMQ.

### Этап 7. File storage

1. В `Cynosura.Studio.Web/appsettings.json`:

```json
"FileSystemStorage": {
  "Url": "http://localhost:43000/static",
  "Path": "wwwroot/static"
}
```

2. В `Cynosura.Studio.Worker/appsettings.json`:

```json
"FileSystemStorage": {
  "Url": "http://localhost:43000/static",
  "Path": "../Cynosura.Studio.Web/wwwroot/static"
}
```

3. В compose добавить общий volume:

```yaml
volumes:
  - static:/app/wwwroot/static
```

4. В `web`:

```yaml
FileSystemStorage__Path: 'wwwroot/static'
FileSystemStorage__Url: 'http://localhost:5000/static'
```

5. В `worker`:

```yaml
FileSystemStorage__Path: '/app/wwwroot/static'
FileSystemStorage__Url: 'http://localhost:5000/static'
```

6. Проверить, что web отдаёт `/static` из `wwwroot/static`, а worker пишет в тот же volume.

**Проверка:** загрузить файл через UI, скачать его, перезапустить контейнеры и убедиться, что файл на месте.

### Этап 8. Frontend и Node

1. Зафиксировать Node 16 для текущей итерации (совместимость с Angular 13 и Dockerfile):

```bash
nvm install 16
nvm use 16
```

2. Добавить `.nvmrc`:

```text
16
```

3. В `package.json` добавить `engines`:

```json
"engines": {
  "node": ">=16.10 <17"
}
```

4. Использовать `npm ci` вместо `npm install`.
5. В `Cynosura.Studio.Web.Ng/Dockerfile`:

```dockerfile
RUN npm ci
...
RUN npm run build -- --configuration production --output-path=dist
```

6. `volumes/webng/config.json`:

```json
{
  "apiBaseUrl": "http://localhost:5000"
}
```

7. В `Cynosura.Studio.Web/appsettings.json` исправить redirect URI на `4300`; compose оставляет `5010`.
8. Если всё же выбирается Node 18+, добавить в окружение:

```bash
export NODE_OPTIONS=--openssl-legacy-provider
```

и отдельно проверить сборку Angular 13.

**Проверка:** `cd Cynosura.Studio.Web.Ng && npm ci && npm run build` завершается успешно.

### Этап 9. Git, CLI и тесты

1. В документацию добавить настройку git:

```bash
git config --global user.name "Your Name"
git config --global user.email "you@example.com"
git config --global init.defaultBranch master
```

2. Либо (предпочтительно) исправить `GitMerge`:
   - определять текущую ветку через `git branch --show-current`;
   - проверять exit code каждой git-команды и выбрасывать исключение при ошибке.

3. Для `cyn` добавить в `PATH`:

```bash
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc
source ~/.bashrc
```

4. Functional-тест `NewCommandTest` требует сеть и скачивает `Cynosura.Template`. Для быстрого прогона:

```bash
dotnet test --filter "FullyQualifiedName!~FunctionalTests"
```

Полный прогон — отдельно, с доступом к NuGet.

**Проверка:** `cyn info`; `cyn new TestSolution` в temp-каталоге; `dotnet test --filter ...`.

### Этап 10. CI

1. Заменить `vmImage` на `ubuntu-24.04`.
2. Исправить `dotnet pack`, чтобы пакет попадал в staging:

```yaml
- task: DotNetCoreCLI@2
  displayName: dotnet pack
  inputs:
    command: 'pack'
    packagesToPack: 'Cynosura.Studio.CliTool/*.csproj'
    arguments: '--output $(Build.ArtifactStagingDirectory)'
    versioningScheme: 'off'
```

3. Заполнить `NuGetToolInstaller` версией (например, `6.x`) или убрать задачу.
4. Добавить проверки, если CI должен валидировать адаптацию:

```yaml
- task: DotNetCoreCLI@2
  displayName: dotnet build
  inputs:
    command: 'build'
    projects: 'Cynosura.Studio.sln'

- task: DotNetCoreCLI@2
  displayName: dotnet test
  inputs:
    command: 'test'
    projects: 'Cynosura.Studio.sln'
    arguments: '--filter "FullyQualifiedName!~FunctionalTests"'
```

5. При необходимости добавить `docker compose build` и `npm ci && npm run build`.
6. Если CI в эту итерацию не входит — явно указать это в плане и не создавать ложного впечатления, что смена `vmImage` что-то проверяет.

### Этап 11. Документация

Обновить согласованно:

- `README.md`;
- `INSTALL_UBUNTU_24.md`;
- `PLAN_ADAPTATION_UBUNTU24.md`.

Обязательные темы:

1. SQLite, а не SQL Server.
2. Таблица портов host dev / compose.
3. Генерация `mycert.pfx` и пароль.
4. `npm ci`, Node 16, `.nvmrc`.
5. `git config` для `cyn`.
6. `docker compose` команды, `logs`, `down`, `down -v`.
7. Проверка `openssl pkcs12`.
8. Troubleshooting: `Kestrel:EndPoints`, CORS, RabbitMQ, пустая БД worker.
9. `PATH` для `dotnet tools`.

### Этап 12. Финальная проверка

См. раздел 6.

---

## 5. Список изменяемых файлов

| Файл | Изменение |
|---|---|
| `Cynosura.Studio.sln` | `tests` → `Tests`; проверка регистра |
| `Cynosura.Studio.Web/Cynosura.Studio.Web.csproj` | кросс-платформенные пути (`SpaRoot` без `\`), `SkipSpaBuild`, `npm ci`, вызов `npm run build:prod` вместо `--prod`/`--outputPath` |
| `Cynosura.Studio.Web/Dockerfile` | copy Generator, `-p:SkipSpaBuild=true` |
| `Cynosura.Studio.Worker/Dockerfile` | copy Generator |
| `Cynosura.Studio.Worker/Program.cs` | миграции SQLite до запуска scheduler |
| `Cynosura.Studio.Web/appsettings.json` | убрать `Kestrel:EndPoints`; redirect 4300; storage URL/path |
| `Cynosura.Studio.Worker/appsettings.json` | storage path/URL |
| `Cynosura.Studio.Web/appsettings.Development.json` | `EmailSender.PickupFolder` (опционально) |
| `Cynosura.Studio.Worker/appsettings.Development.json` | `EmailSender.PickupFolder` (опционально) |
| `docker-compose.yaml` | удалить `db`; healthcheck `mq`; volumes |
| `docker-compose.Development.yaml` | порты 5000/5010; env; cert mount; data/static volumes; без HTTPS |
| `volumes/webng/config.json` | `http://localhost:5000` |
| `volumes/cert/mycert.pfx` | удалить из git, генерировать локально |
| `.gitattributes` | binary-правила для pfx/ico/шрифтов |
| `.gitignore` | `volumes/cert/*.pfx`, `emails/`, `wwwroot/static/` |
| `global.json` | новый файл, фиксация .NET 8 |
| `.nvmrc` | новый файл, Node 16 |
| `Cynosura.Studio.Web.Ng/package.json` | `engines`; скрипт `build:prod` (`--configuration production --output-path`) |
| `Cynosura.Studio.Web.Ng/Dockerfile` | `npm ci`, production build |
| `Cynosura.Studio.Generator/Merge/GitMerge.cs` | ветка и exit code (P1) |
| `azure-pipelines.yml` | ubuntu-24.04, pack output, build/test |
| `README.md` | команды, порты, SQLite, certs, git |
| `INSTALL_UBUNTU_24.md` | синхронизация с планом |
| `PLAN_ADAPTATION_UBUNTU24.md` | этот документ |

---

## 6. Чек-лист приёмки

### Сборка и тесты

- [x] `dotnet --version` → 8.0.x.
- [x] `dotnet build Cynosura.Studio.sln` → код 0.
- [x] `dotnet test --filter "FullyQualifiedName!~FunctionalTests"` → код 0.
- [x] `cd Cynosura.Studio.Web.Ng && npm ci && npm run build` → код 0.
- [x] `dotnet publish Cynosura.Studio.Web/Cynosura.Studio.Web.csproj -c Release -o /tmp/pubtest` → код 0, в `/tmp/pubtest/NgApp` есть `main.*.js`.
- [x] `dotnet publish ... -p:SkipSpaBuild=true` → код 0, каталог `NgApp` не собирается.
- [x] `grep -rn -- "--prod\b" --include=*.csproj --include=*.json --include=*.yml . | grep -v node_modules` → пусто.

### Конфигурация

- [x] `docker compose -f docker-compose.yaml -f docker-compose.Development.yaml config` → без ошибок.
- [x] `openssl pkcs12 -in volumes/cert/mycert.pfx -info -noout -passin pass:${CERT_PASSWORD}` → без ошибок.
- [x] В `git status` нет случайных бинарных изменений.
- [x] `Kestrel:EndPoints` не конфликтует с `ASPNETCORE_URLS`.

### Docker

- [ ] `docker compose ... build` → код 0.
- [ ] `docker compose ... up -d` → все сервисы запущены.
- [ ] `docker compose ... ps` → `web`, `webng`, `worker`, `mq` в состоянии `running`/`healthy`.
- [ ] `curl -I http://localhost:5000/swagger/index.html` → 200/301/302.
- [ ] `curl -I http://localhost:5010/` → 200.
- [ ] `curl -u Manager:${RABBITMQ_PASSWORD} http://localhost:15672/api/overview` → 200.
- [ ] В логах worker нет `no such table`, `database is locked`, ошибок RabbitMQ.
- [ ] Файл `/data/cynosura.db` существует; после `docker compose restart` данные сохраняются.
- [ ] Загруженный файл доступен по `/static/...` и сохраняется после перезапуска.

### Функциональность

- [ ] UI открывается на `http://localhost:5010/`.
- [ ] API отвечает; CORS не блокирует запросы с `5010`.
- [ ] Если используется auth — redirect URI совпадает с фактическим адресом UI.
- [ ] `cyn info`, `cyn new` работают.
- [ ] `cyn upgrade` не падает из-за ветки `main`/git identity.

---

## 7. Риски, допущения и что не входит в план

### Риски

1. SQLite — однопользовательская/однопроцессная БД; для dev этого достаточно, но конкурентная запись web/worker требует аккуратности.
2. Node 16 EOL; Angular 13 старый. Это осознанный компромисс текущей итерации.
3. Удаление SQL Server-контейнера меняет привычную схему compose; решение зафиксировано в разделе 1.1.
4. Приватный ключ `mycert.pfx` не должен попадать в git; история git может содержать старый повреждённый файл.
5. `Kestrel:EndPoints` легко снова включить и получить конфликт с `ASPNETCORE_URLS`.
6. На ARM64 SQL Server-контейнер не запустится; после удаления `db` это неактуально.
7. Functional-тесты зависят от NuGet и сети.

### Допущения

- Целевая машина — x86_64 Ubuntu 24.04.
- Docker Compose v2 (`docker compose`), не старый `docker-compose`.
- Для dev достаточно HTTP; HTTPS — отдельный профиль.
- Шаблон `Cynosura.Template` доступен в NuGet.

### Не входит в план

- Миграция на SQL Server/PostgreSQL.
- Обновление Angular 13 → актуальная версия.
- Обновление .NET 8 → .NET 9/10.
- Production-развёртывание, reverse proxy, HTTPS-харденинг.
- Полный редизайн CI/CD.
- Обновление `node:16-alpine`, `nginx:1.14.1-alpine`, `rabbitmq:3.8-management`.
- Рефакторинг генератора beyond P1-5.

---

## 8. История изменений

| Дата | Версия | Автор | Описание |
|---|---|---|---|
| 24.11.2026 | 1.0 | Cline | Начальный план адаптации под Ubuntu 24.04 |
| 25.11.2026 | 2.0 | OpenCode | Исправлена архитектура (SQLite), добавлены блокеры Docker/портов/сертификатов, переписан чек-лист |
| 25.11.2026 | 2.1 | OpenCode | Раздел 2.4 и P1-13: `--prod`/`--outputPath` в `PublishRunWebpack`, проверки на живом окружении, инструкция по исправлению |
| 25.11.2026 | 2.2 | OpenCode | План выполнен на живом окружении; уточнены GitMerge (merge-конфликты, `--allow-empty`), отмечены 0 тестов в Core.UnitTests и BOM в appsettings.Development |
