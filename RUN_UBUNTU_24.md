# Запуск Cynosura.Studio / Cygnus.Studio на Ubuntu 24.04 — пошаговая инструкция

Репозиторий: `/srv/GitHub/Cynosura.Studio` (ветка `development`).
Это инструкция по **запуску**, установка компонентов описана в `INSTALL_UBUNTU_24.md`.

## Быстрый порядок запуска (режим host dev)

**Первый запуск** — пункты **1 → 2 → 4**:

| Шаг | Пункт | Что делает |
|---|---|---|
| 1 | §1 | проверить окружение (версии dotnet/node/docker/git) |
| 2 | §2 | `dotnet restore` + `dotnet build` + `npm ci` — скачать пакеты и собрать |
| 3 | §4.1–4.6 | RabbitMQ (один раз) → общий путь БД → Web → Worker (создаст БД) → Frontend → проверка |

**Последующие запуски** — только **§4** (с 4.1 по 4.6):

- §1, §2, §3 повторять **не нужно** (`dotnet run` сам пересобирает код; `npm ci` — только если удалён `node_modules`).
- §4.1 (RabbitMQ) — повторять **не нужно**: контейнер работает в фоне; только проверить `docker ps` (а после ребута — `docker start mq`).
- §4.2 (`export ConnectionStrings__DefaultConnection=...`) — повторять **в каждом новом терминале**.
- Worker на первом запуске создаёт `cynosura.db`; на последующих миграции уже применены.
- Хочешь без открытых терминалов — используй **§4.7** (запуск в фоне через `nohup ... &`).

---

## 0. Состав и порты

| Компонент | Назначение | Host dev | Docker compose |
|---|---|---|---|
| `Cynosura.Studio.Web` | Backend API (ASP.NET Core 8) | `http://localhost:43000` | `http://localhost:5000` |
| `Cynosura.Studio.Worker` | Фоновые задачи (Quartz), выполняет миграции БД | запускается рядом | внутри `worker` |
| `Cynosura.Studio.Web.Ng` | Frontend (Angular 13) | `http://localhost:4300` | `http://localhost:5010` |
| RabbitMQ | Очередь сообщений | `localhost:5672` (UI `15672`) | `localhost:5672` (UI `15672`) |
| СУБД | **SQLite** (файл `cynosura.db`) | файл в репозитории | общий volume `/data` |

Учётные данные RabbitMQ: `Manager` / `Qwerty123!`.
Пароль dev-сертификата: `qwe123`.

---

## 1. Проверка окружения

```bash
dotnet --version      # 8.0.x
node --version        # v16.x (Angular 13 совместим с 16, не с 18+)
npm --version         # 8.x
docker --version
docker compose version
git --version
```

Node 16 — обязательное требование (в репозитории лежит `.nvmrc`):

```bash
nvm install 16 && nvm use 16
```

---

## 2. Восстановление зависимостей и сборка

### 2.1. .NET restore

> nuget.org периодически нестабилен (500/504). Если `dotnet restore` зависает в ретраях — используйте зеркало (пункт 2.2).

```bash
cd /srv/GitHub/Cygnus.Studio
dotnet restore Cynosura.Studio.sln
```

### 2.2. Если nuget.org недоступен — через зеркало Huawei

```bash
cat > /tmp/nuget.huawei.config <<'EOF'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="huawei" value="https://mirrors.huaweicloud.com/repository/nuget/v3/index.json" protocolVersion="3" />
  </packageSources>
</configuration>
EOF

dotnet restore Cynosura.Studio.sln --configfile /tmp/nuget.huawei.config
```

### 2.3. Сборка

```bash
dotnet build Cynosura.Studio.sln --no-restore
# Ожидаемо: Build succeeded, 0 Error(s). Предупреждения про "unused import" в .proto — безвредны.
```

### 2.4. Frontend

```bash
cd Cynosura.Studio.Web.Ng
npm ci
```

---

## 3. Dev-сертификат (только для docker compose)

Сертификат `volumes/cert/mycert.pfx` не хранится в git — генерируется локально:

```bash
cd /srv/GitHub/Cygnus.Studio
mkdir -p volumes/cert
openssl req -x509 -newkey rsa:2048 -nodes \
  -keyout /tmp/mycert.key -out /tmp/mycert.crt \
  -days 3650 -subj "/CN=localhost"
openssl pkcs12 -export \
  -out volumes/cert/mycert.pfx \
  -inkey /tmp/mycert.key -in /tmp/mycert.crt \
  -passout pass:qwe123
rm -f /tmp/mycert.key /tmp/mycert.crt
```

Проверка:

```bash
openssl pkcs12 -in volumes/cert/mycert.pfx -info -noout -passin pass:qwe123
```

---

## 4. Запуск: режим 1 — host dev (приложение без Docker)

Удобно для разработки. В Docker нужен только RabbitMQ.

### 4.1. RabbitMQ (запускается один раз)

RabbitMQ работает как фоновый Docker-контейнер: стартуешь **один раз**, дальше он живёт сам (не привязан к терминалу). При каждом запуске приложения перезапускать его не нужно — достаточно проверить, что контейнер работает.

```bash
docker run -d --name mq \
  -p 5672:5672 -p 15672:15672 \
  -e RABBITMQ_DEFAULT_USER=Manager \
  -e RABBITMQ_DEFAULT_PASS='Qwerty123!' \
  --restart unless-stopped \
  rabbitmq:3.8-management
```

Проверка, что RabbitMQ запущен:

```bash
docker ps --filter name=mq
# должен быть статус "Up"
```

Если контейнер остановился (после ребута или `docker stop mq`) — просто поднять:

```bash
docker start mq
```

Либо сервис `mq` из compose (тоже один раз):

```bash
docker compose -f docker-compose.yaml -f docker-compose.Development.yaml up -d mq
```

Флаг `--restart unless-stopped` заставляет контейнер подниматься автоматически после перезагрузки машины.

### 4.2. Общий файл БД (важно!)

И `web`, и `worker` должны смотреть на **один** файл SQLite. Задайте абсолютный путь:

```bash
export ConnectionStrings__DefaultConnection='Data Source=/srv/GitHub/Cygnus.Studio/cynosura.db'
```

### 4.3. Backend (терминал 1)

```bash
cd /srv/GitHub/Cygnus.Studio
dotnet run --project Cynosura.Studio.Web
# http://localhost:43000  (Swagger UI в приложении не включён, см. раздел 4.6)
```

### 4.4. Worker — создаёт/обновляет БД (терминал 2)

Worker при старте выполняет миграции SQLite (`MigrateAsync`) и запускает фоновые задачи:

```bash
cd /srv/GitHub/Cygnus.Studio
dotnet run --project Cynosura.Studio.Worker
```

### 4.5. Frontend (терминал 3)

```bash
cd /srv/GitHub/Cygnus.Studio/Cynosura.Studio.Web.Ng
npm run start
# http://localhost:4300
```

`src/assets/config.json` уже содержит `apiBaseUrl: http://localhost:43000` — правки не требуются.

> **Удалённый доступ (VS Code Remote-SSH):** `ng serve` слушает только `127.0.0.1`, поэтому из локального браузера `http://localhost:4300` не откроется. Либо пробрось порт 4300 в панели **Ports** VS Code, либо запусти `npm run start -- --host 0.0.0.0` и открывай по IP сервера.

### 4.6. Проверка

- Backend (анонимный smoke-запрос): `curl http://localhost:43000/api/Properties` → JSON `{...}`.
  > Важно: `/swagger/index.html` вернёт 404 — Swagger UI в приложении **не включён** (пакет `Swashbuckle.AspNetCore` подключён в `.csproj`, но `AddSwaggerGen`/`UseSwaggerUI` нигде не вызываются). Поэтому проверка идёт по реальному endpoint, а не по Swagger.
- RabbitMQ UI: `http://localhost:15672` (`Manager` / `Qwerty123!`)
- UI: `http://localhost:4300`

### 4.7. Запуск в фоне (чтобы закрыть терминалы)

Процессы из разделов 4.3–4.5 привязаны к открытым терминалам: закроешь терминал — процесс остановится. Чтобы освободить терминалы, но оставить приложение работающим, запускай через `nohup ... &`.

> Порядок важен: сначала Worker (применит миграции), затем Web и Frontend.

```bash
cd /srv/GitHub/Cygnus.Studio
export ConnectionStrings__DefaultConnection='Data Source=/srv/GitHub/Cygnus.Studio/cynosura.db'

# Worker (миграции + фоновые задачи)
nohup dotnet run --project Cynosura.Studio.Worker > /tmp/worker.log 2>&1 &

# Web (API)
nohup dotnet run --project Cynosura.Studio.Web > /tmp/web.log 2>&1 &

# Frontend (Angular dev-server)
cd Cynosura.Studio.Web.Ng
nohup npm run start -- --host 0.0.0.0 > /tmp/ng.log 2>&1 &
```

После этого терминалы можно закрывать. Логи — `/tmp/worker.log`, `/tmp/web.log`, `/tmp/ng.log`.

**Проверка и управление:**

```bash
# что запущено
pgrep -af "Cynosura.Studio.Web|Cynosura.Studio.Worker|ng serve"

# логи
tail -f /tmp/web.log
tail -f /tmp/worker.log
tail -f /tmp/ng.log

# остановить
pkill -f "Cynosura.Studio.Web"
pkill -f "Cynosura.Studio.Worker"
pkill -f "ng serve"
```

> Примечание: `nohup` не переживает перезагрузку машины. Для постоянного запуска (автозапуск после ребута) используйте `systemd`-юниты или `docker compose up -d` (раздел 5).

---

## 5. Запуск: режим 2 — docker compose (всё в контейнерах)

```bash
cd /srv/GitHub/Cygnus.Studio

# 1. сертификат (раздел 3)

# 2. сборка и запуск
docker compose -f docker-compose.yaml -f docker-compose.Development.yaml up -d --build

# 3. статус
docker compose -f docker-compose.yaml -f docker-compose.Development.yaml ps
```

Сервисы: `web` (API, 5000), `webng` (Angular+nginx, 5010), `worker`, `mq` (5672/15672). SQL Server больше **не используется** — данные в общем volume `data` (`/data/cynosura.db`).

### Проверка

```bash
curl http://localhost:5000/api/Properties
curl -I http://localhost:5010/
curl -u Manager:Qwerty123! http://localhost:15672/api/overview
```

### Логи и остановка

```bash
docker compose -f docker-compose.yaml -f docker-compose.Development.yaml logs -f web worker
docker compose -f docker-compose.yaml -f docker-compose.Development.yaml down
# с удалением данных (volume data/static/mq):
docker compose -f docker-compose.yaml -f docker-compose.Development.yaml down -v
```

---

## 6. Тесты

```bash
cd /srv/GitHub/Cygnus.Studio

# быстрый прогон без functional-тестов (они требуют сеть и NuGet)
dotnet test Cynosura.Studio.sln --filter "FullyQualifiedName!~FunctionalTests"

# конкретный проект
dotnet test Tests/Cynosura.Studio.Generator.Tests/Cynosura.Studio.Generator.Tests.csproj
```

> Примечание: `Tests/Cynosura.Studio.Core.UnitTests` в текущей версии содержит 0 тестов — «No test matches» для него это норма.

---

## 7. Публикация (production-сборка)

```bash
cd /srv/GitHub/Cygnus.Studio

# со сборкой Angular-фронта (npm ci + ng build --configuration production)
dotnet publish Cynosura.Studio.Web/Cynosura.Studio.Web.csproj -c Release -o ./out

# без сборки фронта (когда фронт собирается отдельно, например в образе webng)
dotnet publish Cynosura.Studio.Web/Cynosura.Studio.Web.csproj -c Release -o ./out -p:SkipSpaBuild=true
```

---

## 8. Troubleshooting

| Симптом | Причина / решение |
|---|---|
| `dotnet restore` висит в ретраях `500/504` | nuget.org нестабилен — использовать зеркало (раздел 2.2) |
| `MSB3202 ... tests/...csproj was not found` | старый `.sln` — обновить/проверить регистр путей (`Tests` vs `tests`) |
| Worker падает с `no such table` | БД не мигрирована — worker выполняет миграции при старте; убедиться, что web и worker используют **один** `ConnectionStrings__DefaultConnection` |
| `database is locked` | SQLite, одновременная запись web/worker — перезапустить worker, либо не запускать web и worker одновременно на запись |
| Worker/web видят разные данные | относительный путь `Data Source=cynosura.db` разрешается от рабочей папки каждого проекта — задать абсолютный путь (раздел 4.2) или общий volume в compose |
| Фронт не видит API | проверить `apiBaseUrl` в `assets/config.json` (host dev = `http://localhost:43000`, compose = `http://localhost:5000`) |
| RabbitMQ: нет соединения | в compose — `rabbitmq://mq`, в host dev — `rabbitmq://localhost`; проверить учётку `Manager`/`Qwerty123!` |
| `openssl pkcs12` не читает `mycert.pfx` | файл повреждён/закоммичен как текст — перегенерировать (раздел 3), не коммитить `.pfx` |
| Angular-сборка ругается на `--prod` | использовать `--configuration production` (флаг `--prod` удалён в Angular CLI 14) |

---

## См. также

- `PLAN_ADAPTATION_UBUNTU24.md` — полный план адаптации и реестр проблем.
- `INSTALL_UBUNTU_24.md` — установка компонентов.
- `INSTALL_COMPONENTS_UBUNTU_24.md`, `INSTALL_COMPONENTS_UBUNTU_24_ROOT.md` — детальная установка окружения.
