# Кыс-Кыс — интернет-магазин для питомцев

ASP.NET Core 8 (Razor Pages) + SQLite (EF Core). Адаптивная вёрстка, корзина, регистрация и вход, личный кабинет, оформление заказа, отзывы.

**Сайт:** http://zoomagaz.runasp.net/ — деплой настроен через GitHub Actions, каждый push в `main` выкладывает новую версию.

## Локальный запуск
```bash
dotnet run
```
База создаётся сама в `App_Data/kyskys.db`.

## Загрузка на GitHub
```bash
git init -b main
git add .
git commit -m "Initial commit"
git remote add origin https://github.com/<логин>/<репозиторий>.git
git push -u origin main
```

## Деплой на MonsterASP.NET

### 1. Сайт на хостинге
1. Зарегистрируйтесь на monsterasp.net и создайте сайт (.NET 8 или новее).
2. Если домена нет, сайт будет на бесплатном поддомене `*.runasp.net`.
3. На бесплатном тарифе HTTPS включается вручную через тикет в поддержку.

### 2. Автодеплой через GitHub Actions (Web Deploy)
В панели хостинга откройте сайт и найдите данные Web Deploy (или скачайте publish profile).
В репозитории: Settings → Secrets and variables → Actions → New repository secret:

| Секрет | Что вписать |
|---|---|
| `WEBSITE_NAME` | имя сайта (`siteXXXXX`) из карточки **Visual Studio access** |
| `SERVER_COMPUTER_NAME` | адрес Web Deploy, например `https://siteXXXXX.siteasp.net:8172` |
| `SERVER_USERNAME` | имя пользователя Web Deploy (обычно совпадает с `siteXXXXX`) |
| `SERVER_PASSWORD` | пароль Web Deploy |

После этого каждый push в `main` собирает и выкладывает сайт. Запустить вручную: вкладка Actions → Deploy to MonsterASP.NET → Run workflow.

### Запасные варианты
- **Visual Studio:** скачайте publish profile в панели → Publish → Import profile.
- **FTP/SFTP:** `dotnet publish -c Release -o publish`, затем загрузите содержимое `publish` в корневую папку сайта (перед загрузкой положите туда `app_offline.htm`, после — удалите).
- **Деплой из GitHub в панели:** подключите репозиторий в панели хостинга; проверьте там, как она собирает проект.

## Важно
- База и ключи авторизации лежат в `App_Data/` на сервере; деплой их не удаляет (`DoNotDeleteRule`). Папка должна быть доступна на запись.
- Корзина хранится в памяти сервера: при перезапуске приложения она очищается.
- Не коммитьте `App_Data/` и `*.db` (уже в `.gitignore`).
- Бесплатный тариф не отправляет e-mail.
