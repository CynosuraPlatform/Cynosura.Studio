# Установка компонентов для Cygnus.Studio на Ubuntu 24.04 (версия для root)

**Версия:** 1.0-root  
**Дата:** 25.11.2026  
**Назначение:** пошаговая установка компонентов при работе **полностью под root**; код и конфигурация репозитория не изменяются  
**Репозиторий:** `/srv/GitHub/Cygnus.Studio`  
**Связанные документы:** `PLAN_ADAPTATION_UBUNTU24.md`, `INSTALL_COMPONENTS_UBUNTU_24.md`

> Все команды в этом файле выполняются от пользователя `root`. `sudo` не используется.
> Запуск приложения выполняется после исправления P0-проблем из плана v2. Иначе `dotnet build` и `docker compose up` ожидаемо упадут.

---

## 0. Что устанавливаем и что не устанавливаем

| Компонент | Нужен | Вариант |
|---|---|---|
| .NET SDK 8 | Да | пакет Ubuntu `dotnet-sdk-8.0` |
| Node.js 16 | Да, для Angular 13 | через `nvm` (в `/root/.nvm`) |
| Docker Engine | Да | официальный репозиторий Docker |
| Docker Compose v2 | Да | пакет `docker-compose-plugin` |
| Git | Да | `apt` |
| OpenSSL | Да, для dev-сертификата | обычно уже есть |
| SQL Server | **Нет** | приложение использует SQLite |
| RabbitMQ | **Нет на хост** | запускается в Docker |
| Node.js из `apt` | Нежелательно | может быть Node 18+ |

Ориентировочное время: 15–30 минут. Перелогин после установки Docker **не нужен**, так как root уже имеет доступ к Docker-сокету.

---

## 1. Подготовка системы

Проверьте, что вы действительно root, и посмотрите параметры системы:

```bash
whoami
id -u
echo "$HOME"

cat /etc/os-release
uname -m
free -h
df -h /
```

Ожидается:

- `whoami` → `root`;
- `id -u` → `0`;
- `$HOME` → `/root`;
- Ubuntu 24.04 и `x86_64`.

Для комфортной Docker-сборки желательно:

- RAM: от 4 ГБ, лучше 8 ГБ;
- свободное место: от 15 ГБ.

Обновите индексы и установите базовые утилиты:

```bash
apt update
apt install -y \
  curl wget ca-certificates gnupg lsb-release \
  git unzip jq openssl software-properties-common
```

`software-properties-common` нужен для `add-apt-repository` в разделе 2.

`apt upgrade -y` делать не обязательно; если делаете, лучше перезагрузиться после.

### Если `sudo` в системе отсутствует

Это нормально для root-сессии. Просто игнорируйте `sudo` в любых сторонних инструкциях:

```bash
# вместо: sudo apt update
apt update
```

---

## 2. Установка .NET 8 SDK

### Вариант A — рекомендуемый, из репозитория Ubuntu

Ubuntu 24.04 содержит .NET 8 в штатном фиде:

```bash
apt install -y dotnet-sdk-8.0
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
add-apt-repository universe
apt update
apt install -y dotnet-sdk-8.0
```

### Вариант B — фид Microsoft

Используйте, только если пакета Ubuntu нет или нужна конкретная версия:

```bash
wget https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb \
  -O /tmp/packages-microsoft-prod.deb
dpkg -i /tmp/packages-microsoft-prod.deb
rm /tmp/packages-microsoft-prod.deb
apt update
apt install -y dotnet-sdk-8.0
```

Не смешивайте оба источника без необходимости. Проверить, откуда ставится пакет:

```bash
apt-cache policy dotnet-sdk-8.0
```

Опционально отключите телеметрию (запишется в `/root/.bashrc`):

```bash
echo 'export DOTNET_CLI_TELEMETRY_OPTOUT=1' >> /root/.bashrc
echo 'export DOTNET_NOLOGO=1' >> /root/.bashrc
```

### Особенности root

- NuGet-кэш будет в `/root/.nuget/packages`.
- Глобальные .NET-инструменты (`dotnet tool install -g`) будут в `/root/.dotnet/tools`.
- Добавьте этот каталог в `PATH` (см. раздел 5.1).

---

## 3. Установка Node.js 16 через nvm

Angular 13 в этом проекте рассчитан на Node 16. Node 16 уже EOL, но для текущей итерации это наименее рискованный вариант. Обновление Angular/Node — отдельная задача.

### 3.1. Установить nvm в `/root/.nvm`

```bash
export NVM_DIR="/root/.nvm"
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.7/install.sh | bash
```

Установщик допишет блок инициализации в `/root/.bashrc`.

### 3.2. Подхватить nvm в текущей сессии

```bash
export NVM_DIR="/root/.nvm"
[ -s "$NVM_DIR/nvm.sh" ] && . "$NVM_DIR/nvm.sh"
```

### 3.3. Установить Node 16

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

Ожидается `v16.20.x`, `npm 8.x` и путь вида `/root/.nvm/versions/node/v16.20.x/bin/node`.

### 3.4. Если в новом терминале nvm не найден

```bash
export NVM_DIR="/root/.nvm"
. "$NVM_DIR/nvm.sh"
```

Проверьте, что в `/root/.bashrc` есть блок:

```bash
export NVM_DIR="$HOME/.nvm"
[ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"
```

Поскольку `$HOME=/root`, это указывает на правильный каталог.

### 3.5. `npm ci` под root

npm может запускать lifecycle-скрипты от `nobody`, из-за чего возможны `EACCES` в `node_modules`. Если это произойдёт:

```bash
npm ci --unsafe-perm
```

или разово:

```bash
npm config set unsafe-perm true
npm ci
```

Это допустимо для локальной dev-машины, но не для публичного CI.

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
apt remove -y docker docker-engine docker.io containerd runc 2>/dev/null || true
```

### 4.2. Добавить официальный репозиторий Docker

```bash
apt update
apt install -y ca-certificates curl gnupg

install -m 0755 -d /etc/apt/keyrings
rm -f /etc/apt/keyrings/docker.gpg

curl -fsSL https://download.docker.com/linux/ubuntu/gpg \
  | gpg --dearmor -o /etc/apt/keyrings/docker.gpg
chmod a+r /etc/apt/keyrings/docker.gpg

echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "$VERSION_CODENAME") stable" \
  | tee /etc/apt/sources.list.d/docker.list > /dev/null

apt update
```

### 4.3. Установить Docker и Compose plugin

```bash
apt install -y \
  docker-ce docker-ce-cli containerd.io \
  docker-buildx-plugin docker-compose-plugin
```

### 4.4. Включить Docker

```bash
systemctl enable --now docker
```

Если systemd недоступен (контейнер, WSL без systemd):

```bash
service docker start
# или
dockerd > /var/log/dockerd.log 2>&1 &
```

### 4.5. Группа docker — не нужна

Под root **не выполняйте** `usermod -aG docker` и `newgrp docker`: они не дают практического эффекта, root и так имеет доступ к сокету.

Если случайно выполнили — ничего страшного, просто лишняя запись в `/etc/group`.

### 4.6. Проверка

```bash
docker --version
docker compose version
docker run --rm hello-world
```

Ожидается успешный запуск `hello-world` и `Docker Compose version v2.x`.

### 4.7. Важно про `${HOME}` в compose

Поскольку вы работаете под root, при `docker compose` переменная `${HOME}` = `/root`.

Следствия:

- `${HOME}/.aspnet/https` → `/root/.aspnet/https`;
- если позже оставите HTTPS-профиль, сертификат `aspnet.pfx` нужно класть именно в `/root/.aspnet/https/`;
- текущий `docker-compose.Development.yaml` использует `${USERPROFILE}` — под root она не задана. Это исправляется в этапе 3 плана v2.

---

## 5. Настройка Git и PATH

### 5.1. Git identity

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

Файл будет создан в `/root/.gitconfig`.

Почему `master`: текущий `GitMerge` делает `git checkout master`. Если нужен `main`, потребуется правка кода — иначе `cyn upgrade` и часть тестов могут сломаться.

### 5.2. PATH для .NET global tools

```bash
echo 'export PATH="$PATH:/root/.dotnet/tools"' >> /root/.bashrc
export PATH="$PATH:/root/.dotnet/tools"
```

Проверка после установки CLI:

```bash
which cyn
```

---

## 6. Дополнительные утилиты (опционально)

Для диагностики:

```bash
apt install -y tree htop
```

Для проверки Excel-экспорта (EPPlus/System.Drawing) иногда упоминают `libgdiplus`:

```bash
apt install -y libgdiplus fontconfig fonts-dejavu-core
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
fallocate -l 4G /swapfile
chmod 600 /swapfile
mkswap /swapfile
swapon /swapfile
echo '/swapfile none swap sw 0 0' | tee -a /etc/fstab
```

Это опционально.

---

## 8. Пост-установочная подготовка репозитория

Это уже не установка компонентов, но без этого запуск не проверить. Все команды — от root, без `sudo`.

### 8.1. Перейти в репозиторий

```bash
cd /srv/GitHub/Cygnus.Studio
```

Если git ругается `detected dubious ownership` (например, репозиторий принадлежал другому пользователю), добавьте исключение:

```bash
git config --global --add safe.directory /srv/GitHub/Cygnus.Studio
git config --global --add safe.directory /srv/GitHub/AIreadme
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

Команда должна завершиться без ошибок. Файл будет принадлежать `root:root`. Контейнеры по умолчанию тоже работают под root, поэтому читать смогут.

Учтите: это изменит рабочий файл в репозитории; окончательно его судьбу решает этап 4 плана v2.

### 8.3. Восстановить NuGet-пакеты

```bash
dotnet restore Cynosura.Studio.Web/Cynosura.Studio.Web.csproj
dotnet restore Cynosura.Studio.Worker/Cynosura.Studio.Worker.csproj
```

Кэш будет в `/root/.nuget/packages`.

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

Если падает с `EACCES` при lifecycle-скриптах:

```bash
npm ci --unsafe-perm
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

## 9. Итоговый чек-лист установки (root)

| Проверка | Команда | Ожидаемо |
|---|---|---|
| Пользователь | `whoami` | `root` |
| HOME | `echo "$HOME"` | `/root` |
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

## 10. Типичные проблемы (root)

### `sudo: command not found`

Не ошибка. В root-сессии `sudo` не нужен — уберите его из команды.

### `E: Unable to locate package dotnet-sdk-8.0`

```bash
add-apt-repository universe
apt update
apt install -y dotnet-sdk-8.0
```

Если не помогло — вариант B с фидом Microsoft.

### `dotnet: command not found`

Проверьте:

```bash
ls /usr/bin/dotnet
ls /usr/lib/dotnet
echo "$PATH"
```

### `nvm: command not found`

```bash
export NVM_DIR="/root/.nvm"
. "$NVM_DIR/nvm.sh"
```

Проверьте блок `NVM_DIR` в `/root/.bashrc`.

### `npm ERR! engine` или ошибки Angular

```bash
nvm use 16
node --version
rm -rf node_modules
npm ci --unsafe-perm
```

### `EACCES` в `node_modules` под root

```bash
npm config set unsafe-perm true
npm ci
```

Или удалите каталог и повторите:

```bash
rm -rf node_modules
npm ci --unsafe-perm
```

### `docker compose: command not found`

```bash
apt install -y docker-compose-plugin
docker compose version
```

Не ставьте старый `docker-compose` через `apt`/`pip`.

### `systemctl: command not found` / `System has not been booted with systemd`

Если Ubuntu запущена как контейнер без systemd:

```bash
service docker start
# или
dockerd > /var/log/dockerd.log 2>&1 &
```

### `permission denied while trying to connect to the Docker daemon`

Под root это маловероятно, но проверьте, что демон запущен:

```bash
systemctl status docker
# или
service docker status
```

### `openssl pkcs12` не читает `mycert.pfx`

Файл повреждён. Перегенерируйте по командам из 8.2.

### `WARN: The "USERPROFILE" variable is not set`

Это текущий compose до этапа 3 плана v2. Для установки не критично; при запуске будет исправлено. Под root дополнительно помните, что `${HOME}` = `/root`.

### `detected dubious ownership in repository`

```bash
git config --global --add safe.directory /srv/GitHub/Cygnus.Studio
git config --global --add safe.directory /srv/GitHub/AIreadme
```

### Нет места на диске

```bash
docker system df
docker system prune -f
df -h /
```

`docker system prune` удаляет неиспользуемые образы/контейнеры; будьте осторожны.

---

## 11. Root-риски и что делать при смене пользователя

### 11.1. Всё принадлежит root

После выполнения инструкции под root будут принадлежать `root:root`:

- `/root/.nvm`, `/root/.nuget`, `/root/.dotnet`;
- `node_modules`, `obj`, `bin` в репозитории;
- `volumes/cert/mycert.pfx`;
- файлы, созданные приложением.

Пока вы работаете только под root — это нормально.

### 11.2. Если позже понадобится обычный пользователь

Если сейчас всё сделать под root, а потом запускать приложение под обычным пользователем, возможны:

- `node_modules`, `obj`, `bin`, сертификат принадлежат root;
- git выдаёт `detected dubious ownership`;
- nvm/Node и `dotnet` global tools доступны только root.

Варианты:

**A. Продолжать работать под root.** Приемлемо для выделенной dev-машины, но не для shared-сервера.

**B. Создать отдельного пользователя сразу:**

```bash
useradd -m -s /bin/bash cygnus
usermod -aG docker cygnus
chown -R cygnus:cygnus /srv/GitHub/Cygnus.Studio /srv/GitHub/AIreadme
```

И дальше выполнять инструкцию `INSTALL_COMPONENTS_UBUNTU_24.md` под `cygnus` (там есть `sudo` и `newgrp`).

**C. Передать существующие файлы новому пользователю позже:**

```bash
chown -R cygnus:cygnus /srv/GitHub/Cygnus.Studio /srv/GitHub/AIreadme
rm -rf /srv/GitHub/Cygnus.Studio/Cynosura.Studio.Web.Ng/node_modules
```

После этого `npm ci` и `dotnet restore` нужно выполнить заново под новым пользователем.

### 11.3. Не смешивайте пользователей на одних файлах

Главное правило: один и тот же рабочий каталог, `node_modules`, `obj/bin` и `volumes` должен использоваться одним пользователем. Иначе появятся `EACCES`, `dubious ownership` и странные ошибки сборки.

---

## 12. Что дальше

После установки компонентов и проверок из раздела 9 установка считается завершённой. Следующий шаг — исправления P0 из плана v2:

1. `.sln` и `.csproj`;
2. Dockerfile'ы и `SkipSpaBuild`;
3. порты/`Kestrel`;
4. сертификаты и compose;
5. SQLite/worker, RabbitMQ, storage;
6. затем `dotnet build`, `docker compose build`, `docker compose up`.

---

## 13. История изменений

| Дата | Версия | Описание |
|---|---|---|
| 25.11.2026 | 1.0 | Первая версия инструкции по установке компонентов |
| 25.11.2026 | 1.0-root | Версия для выполнения всех шагов под root: без `sudo`, `NVM_DIR=/root/.nvm`, без `usermod`/`newgrp`, добавлены root-риски и `npm --unsafe-perm` |
