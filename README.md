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
![Таблицы БД PostgreSQL](https://s975sas.storage.yandex.net/rdisk/d40cb20eee41532a675797f2ba2424a4b90dd9ee4bb223759d1155a8bc3b5f7a/66b63996/MmrzcZdFqBsuAaoM02nEuyqtljD0IYqkpCTLCXBCHdT5Ih0AeIyxLwVd7L5kNCgDuHDybA7PcpqgmU7GhfF-3w==?uid=0&filename=2024-08-08_20-08-41.png&disposition=attachment&hash=U4ErQYMVt36SFjN%2BBj%2BqAEX4/kfw/NT2EG7fvY3pnYMc2VQh9IEU6cRSIzgkR/3cq/J6bpmRyOJonT3VoXnDag%3D%3D&limit=0&content_type=image%2Fpng&owner_uid=360618779&fsize=45642&hid=f8aff2893ae99ea242efa4f2a78a05fb&media_type=image&tknv=v2&ts=61f4208311180&s=300b124d84f0df90393722d051edc6be585da3e3b29793f322ba26d56739a649&pb=U2FsdGVkX19eXdG_Ad1XmdB_onpjzjEBTpA0RCGh1Rlw8YT4zSADE0qCU8u8aVdfXqZKm5w1N-fU9mgGkOx7sLomphaIvJay6DJngFBpKkw)

Созданы контроллеры для продуктов и категорий.