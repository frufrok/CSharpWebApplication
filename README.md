# CSharpWebApplication

## Урок 1. ASP.NET база

### Задание

Доработайте контроллер, дополнив его возможностью удалять группы и продукты, а также задавать цены. Для каждого типа ответа создайте свою модель.

### Решение

Созданы сущности и контекст базы данных.

Выполнена миграция командой:

```bash
dotnet ef migrations add InitialCreate --context ProductContext
```

Изменения внесены в базу данных командой:
```bash
dotnet ef database update
```

База данных успешно создана:
![Таблицы БД PostgreSQL](https://downloader.disk.yandex.ru/preview/57fdd9082d9dd6bdf5458a41ae0316fdd814b653b609ce46351c2fce4ef9b8d2/66b53430/2WerxUfO1mgUR2UYFACGWWhLhZ4UFVG9ahxXkuwD7ueF7yVj5TKyXMo2IpwaZ5FkC21uvXXJL40b405OgmaktA%3D%3D?uid=0&filename=2024-08-08_20-08-41.png&disposition=inline&hash=&limit=0&content_type=image%2Fpng&owner_uid=0&tknv=v2&size=2048x2048)

Созданы контроллеры для продуктов и категорий.