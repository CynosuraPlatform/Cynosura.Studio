# Установка компонентов для Cygnus.Studio на Ubuntu 24.04

**Версия:** 1.0  
**Дата:** 25.11.2026  
**Назначение:** пошаговая установка компонентов; код и конфигурация репозитория не изменяются  
**Репозиторий:** `/srv/GitHub/Cygnus.Studio`  
**Связанный план:** `PLAN_ADAPTATION_UBUNTU24.md`

> Это инструкция только по установке окружения. Запуск приложения выполняется после исправления P0-проблем из плана v2. Иначе `dotnet build` и `docker compose up` ожидаемо упадут.

---

## 0. Что устанавливаем и что не устанавливаем

| Компонент | Нужен | Вариант |
|---|---|---|
| .NET SDK 8 | Да | пакет Ubuntu `dotnet-sdk-8.0` |
| Node.js 16 | Да, для Angular 13 | через `nvm` |
| Docker Engine | Да | официальный репозиторий Docker |
| Docker Compose v2 | Да | пакет `docker-compose-plugin` |
| Git | Да | `apt` |
| OpenSSL | Да, для dev-сертификата | обычно уже есть |
| SQL Server | **Нет** | приложение использует SQLite |
| RabbitMQ | **Нет на хост** | запускается в Docker |
| Node.js из `apt` | Нежелательно | может быть Node 18+ |

Ориентировочное время: 15–30 минут + перелогин после добавления пользователя в группу `docker`.

---

## 1. Подготовка системы

Проверьте версию и архитектуру:

```bash
cat /etc/os-release
uname -m
free -h
df -h /
```

Ожидается Ubuntu 24.04 и `x86_64`. Для комфортной Docker-сборки желательно:

- RAM: от 4 ГБ, лучше 8 ГБ;
- свободное место: от 15 ГБ;
- `sudo`-доступ.

Обновите индексы и установите базовые утилиты:

```bash
sudo apt update
sudo apt install -y \
  curl wget ca-certificates gnupg lsb-release \
  git unzip jq openssl
```

`sudo apt upgrade -y` делать не обязательно; если делаете, лучше перезагрузиться после.

---

## 2. Установка .NET 8 SDK

### Вариант A — рекомендуемый, из репозитория Ubuntu

Ubuntu 24.04 содержит .NET 8 в штатном фиде:

```bash
sudo apt install -y dotnet-sdk-8.0
```

Проверка:

```bash
dotnet --version
dotnet --list-sdks
dotnet --info
```

Ожидается `8.0.x` и путь вида `/usr/lib/dotnet/sdk/8.0.x`.

### Если пакет не найден

Включите `universe`:

```bash
sudo add-apt-repository universe
sudo apt update
sudo apt install -y dotnet-sdk-8.0
```

### Вариант B — фид Microsoft

Используйте, только если пакета Ubuntu нет или нужна конкретная версия:

```bash
wget https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb \
  -O /tmp/packages-microsoft-prod.deb
sudo dpkg -i /tmp/packages-microsoft-prod.deb
rm /tmp/packages-microsoft-prod.deb
sudo apt update
sudo apt install -y dotnet-sdk-8.0
```

Не смешивайте оба источника без необходимости. Проверить, откуда ставится пакет:

```bash
apt-cache policy dotnet-sdk-8.0
```

Опционально отключите телеметрию:

```bash
echo 'export DOTNET_CLI_TELEMETRY_OPTOUT=1' >> ~/.bashrc
echo 'export DOTNET_NOLOGO=1' >> ~/.bashrc
```

---

## 3. Установка Node.js 16 через nvm

Angular 13 в этом проекте рассчитан на Node 16, и Dockerfile фронтенда использует `node:16-alpine`. Node 16 уже EOL, но для текущей итерации это наименее рискованный вариант. Обновление Angular/Node — отдельная задача.

Установите `nvm`:

```bash
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.7/install.sh | bash
```

Подхватите `nvm` в текущей сессии:

```bash
export NVM_DIR="$HOME/.nvm"
[ -s "$NVM_DIR/nvm.sh" ] && . "$NVM_DIR/nvm.sh"
```

Установите Node 16 и сделайте его версией по умолчанию:

```bash
nvm install 16
nvm alias default 16
nvm use 16
```

Проверка:

```bash
node --version
npm --version
which node
```

Ожидается `v16.20.x` и `npm 8.x`.

Если в новом терминале `nvm` не найден:

```bash
source ~/.nvm/nvm.sh
```

### Альтернатива Node 18+

Возможна, но для Angular 13 может потребоваться:

```bash
export NODE_OPTIONS=--openssl-legacy-provider
```

После этого обязательно проверить `npm ci && npm run build`. Для этого репозитория рекомендуется пока оставить Node 16.

---

## 4. Установка Docker Engine и Docker Compose v2

### 4.1. Удалить старые версии, если были

```bash
sudo apt remove -y docker docker-engine docker.io containerd runc 2>/dev/null || true
```

### 4.2. Добавить официальный репозиторий Docker

```bash
sudo apt update
sudo apt install -y ca-certificates curl gnupg

sudo install -m 0755 -d /etc/apt/keyrings
sudo rm -f /etc/apt/keyrings/docker.gpg

curl -fsSL https://download.docker.com/linux/ubuntu/gpg \
  | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg

echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "$VERSION_CODENAME") stable" \
  | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt update
```

### 4.3. Установить Docker и Compose plugin

```bash
sudo apt install -y \
  docker-ce docker-ce-cli containerd.io \
  docker-buildx-plugin docker-compose-plugin
```

### 4.4. Включить Docker

```bash
sudo systemctl enable --now docker
```

### 4.5. Добавить пользователя в группу docker

```bash
sudo usermod -aG docker $USER
```

Применить группу без перезагрузки:

```bash
newgrp docker
```

Либо выйдите из сессии и войдите снова.

### 4.6. Проверка

```bash
docker --version
docker compose version
docker run --rm hello-world
```

Ожидается успешный запуск `hello-world` и `Docker Compose version v2.x`.

Важно: дальше запускайте `docker compose` **без `sudo`**, иначе `${HOME}` станет `/root`, и пути к сертификатам/кэшу будут другими.

---

## 5. Настройка Git

Проверьте версию:

```bash
git --version
```

Настройте identity — это нужно не только для коммитов, но и для `cyn upgrade`/`GitMerge`, которые делают `git commit`:

```bash
git config --global user.name "Your Name"
git config --global user.email "you@example.com"
git config --global init.defaultBranch master
```

Проверка:

```bash
git config --global --list
```

Почему `master`: текущий `GitMerge` делает `git checkout master`. Если нужен `main`, потребуется правка кода — иначе `cyn upgrade` и часть тестов могут сломаться.

---

## 6. Дополнительные утилиты (опционально)

Для диагностики:

```bash
sudo apt install -y tree htop
```

Для проверки Excel-экспорта (EPPlus/System.Drawing) иногда упоминают `libgdiplus`:

```bash
sudo apt install -y libgdiplus fontconfig fonts-dejavu-core
```

Но на .NET 8 `System.Drawing.Common` официально не поддерживает Unix, поэтому `libgdiplus` не гарантирует работу EPPlus 5.1.0. Это лучше проверить отдельно после запуска приложения.

SQL Server и RabbitMQ на хост ставить не нужно:

- SQL Server в этом проекте не используется;
- RabbitMQ поднимается контейнером `mq`.

---

## 7. Проверка ресурсов перед сборкой

```bash
free -h
df -h /
nproc
```

Рекомендуется:

- свободное место: не меньше 15 ГБ;
- RAM: от 4 ГБ;
- 2+ vCPU.

Если RAM мало, можно добавить swap. Пример для 4 ГБ:

```bash
sudo fallocate -l 4G /swapfile
sudo chmod 600 /swapfile
sudo mkswap /swapfile
sudo swapon /swapfile
echo '/swapfile none swap sw 0 0' | sudo tee -a /etc/fstab
```

Это опционально.

---

## 8. Пост-установочная подготовка репозитория

Это уже не установка компонентов, но без этого запуск не проверить.

### 8.1. Перейти в репозиторий

```bash
cd /srv/GitHub/Cygnus.Studio
```

### 8.2. Сгенерировать dev-сертификат для JWT

Текущий `volumes/cert/mycert.pfx` повреждён, поэтому его нужно перегенерировать. Пароль в compose — `qwe123`.

```bash
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

Команда должна завершиться без ошибок. Учтите: это изменит рабочий файл в репозитории; окончательно его судьбу решает этап 4 плана v2.

### 8.3. Восстановить NuGet-пакеты

```bash
dotnet restore Cynosura.Studio.Web/Cynosura.Studio.Web.csproj
dotnet restore Cynosura.Studio.Worker/Cynosura.Studio.Worker.csproj
```

### 8.4. Установить npm-зависимости

```bash
cd Cynosura.Studio.Web.Ng
npm ci
```

Если `npm ci` падает из-за версии Node:

```bash
nvm use 16
node --version
npm ci
```

### 8.5. Что пока не запускать

`dotnet build Cynosura.Studio.sln` и `docker compose up` пока ожидаемо упадут из-за P0-проблем:

- `tests`/`Tests` в `.sln`;
- отсутствующий `Cynosura.Studio.Generator` в Dockerfile;
- `npm` внутри `dotnet publish`;
- конфликт `Kestrel:EndPoints` и `ASPNETCORE_URLS`;
- отсутствующие mount'ы и SQLite/worker.

Это следующие этапы плана v2, а не ошибки установки.

---

## 9. Итоговый чек-лист установки

| Проверка | Команда | Ожидаемо |
|---|---|---|
| Ubuntu | `cat /etc/os-release` | 24.04 |
| Архитектура | `uname -m` | `x86_64` |
| .NET SDK | `dotnet --list-sdks` | `8.0.x` |
| Node | `node --version` | `v16.20.x` |
| npm | `npm --version` | `8.x` |
| Docker | `docker --version` | `Docker version 2x.x` |
| Compose | `docker compose version` | `v2.x` |
| Docker daemon | `docker run --rm hello-world` | успешно |
| Git | `git --version` | 2.x |
| Git identity | `git config --global --list` | name/email/master |
| OpenSSL | `openssl version` | 3.x |
| Сертификат | `openssl pkcs12 ...` | без ошибок |
| npm-зависимости | `npm ci` | успешно |
| NuGet restore | `dotnet restore ...Web.csproj` | успешно |
| Ресурсы | `free -h`, `df -h /` | RAM ≥ 4 ГБ, диск ≥ 15 ГБ |

---

## 10. Типичные проблемы

### `E: Unable to locate package dotnet-sdk-8.0`

```bash
sudo add-apt-repository universe
sudo apt update
sudo apt install -y dotnet-sdk-8.0
```

Если не помогло — вариант B с фидом Microsoft.

### `dotnet: command not found`

Проверьте:

```bash
ls /usr/bin/dotnet
ls /usr/lib/dotnet
```

Иногда требуется перелогиниться или проверить `PATH`.

### `nvm: command not found`

```bash
source ~/.nvm/nvm.sh
```

И проверьте, что блок с `NVM_DIR` есть в `~/.bashrc`.

### `npm ERR! engine` или ошибки Angular

```bash
nvm use 16
node --version
rm -rf node_modules
npm ci
```

### `permission denied while trying to connect to the Docker daemon`

```bash
sudo usermod -aG docker $USER
newgrp docker
```

Или перелогиньтесь.

### `docker compose: command not found`

Проверьте пакет:

```bash
sudo apt install -y docker-compose-plugin
docker compose version
```

Не ставьте старый `docker-compose` через `apt`/`pip`.

### `openssl pkcs12` не читает `mycert.pfx`

Файл повреждён. Перегенерируйте по командам из 8.2.

### `WARN: The "USERPROFILE" variable is not set`

Это текущий compose до этапа 3 плана v2. Для установки не критично; при запуске будет исправлено.

### Нет места на диске

```bash
docker system df
docker system prune -f
df -h /
```

`docker system prune` удаляет неиспользуемые образы/контейнеры; будьте осторожны.

---

## 11. Что дальше

После установки компонентов и проверок из раздела 9 установка считается завершённой. Следующий шаг — исправления P0 из плана v2:

1. `.sln` и `.csproj`;
2. Dockerfile'ы и `SkipSpaBuild`;
3. порты/`Kestrel`;
4. сертификаты и compose;
5. SQLite/worker, RabbitMQ, storage;
6. затем `dotnet build`, `docker compose build`, `docker compose up`.

---

## 12. История изменений

| Дата | Версия | Описание |
|---|---|---|
| 25.11.2026 | 1.0 | Первая версия инструкции по установке компонентов |
