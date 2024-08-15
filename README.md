# CSharpWebApplication

## ���� 1. ASP.NET ����

### �������

����������� ����������, �������� ��� ������������ ������� ������ � ��������, � ����� �������� ����. ��� ������� ���� ������ �������� ���� ������.

### �������

������� �������� � �������� ���� ������.

��������� �������� ��������:

```bash
dotnet ef migrations add InitialCreate --context ProductContext
```

��������� ������� � ���� ������ ��������:
```bash
dotnet ef database update
```

���� ������ ������� �������.

������� ����������� ��� ��������� � ���������.

## ���� 2. ������ � ������� (CSV + �������), ������� � �����������

### �������

- ����������� ����������, ���������� � ��� ����� �������� CSV-����� � ��������.
- ����������� ����������, ���������� ��������� ���� �� ����������� ������ ���. �������� ��� ��������� �� ������.
- ���������� ������ ����������� ��� ������ � ����� ������ � ���������������� ���� ����������.

### �������

- ��������� ProductController � �������� ����� GetProductsCSV().
- ��������� CategoryController � ����������� ����������� ������, ��������� ������ GetCacheStatistics() � GetCacheStatisticsUrl(), ��� ��� ������ ���������� ������ �� ����, ����������� �� �����.
- ������ ����������� ���������� � appsettings.json. � Program.cs � ������� 28-35 ��������� �������� ������������. � ������������ �������� �� ���������� ����� �������� ������������ � ������������.

## ���� 3. GraphQL � �������������� �����������

### �������

�������� ��������� ������ ����������� ������� ���������� � ������� �� ������/��������. ���������� � ���� ������ ����������� API � GraphQL.
���������� API-Gateway ��� API ������� ������ � API-������� �� ������ ������.

### �������

- ������� ��������� �� 4 �������:
	- CSharpWebApplication � ������������ ������, � ������� ���������� API ������ � ���������� � �����������.
	- SharedModels ��� �������� ����� ������� ���� ������ � � ���������. � ���� ������ ���������� �������������� ����� �� CSharpWebApplication.
	- ProductStorageAPI � ����� ������, � ������� ���������� API ������ � �������������� ��������� �� �������.
	- Gateway � ������ API Gateway.
- ��� ����� ������� ���������� ����� ��. ������ ����������� ������������� � appsettings.json �������� ��� CSharpWebApplication � ProductStorageAPI.
- CSharpWebApplication � ProductStorageAPI ����������� �� ������ ������. ��� � ��� �������� Swagger.
- Gateway ���������� API �� ������ ��������, ����������� ����� ���� 7201.
- ��� ������� ProductStorageAPI ���������� ������ ����� GraphQL (). ����� ���������� ������ � �� ����� ���������� � GraphQL ��������� � ����� ProductStorageRepository.

������ ������� ��� GraphQL:

```
{
  storages{
    id,
    name,
    description
  }
}
```

������ ������� ��� GraphQL:

```
mutation addStorage{
  addStorage(input: {
    name: "GraphQL �����",
    description : "����� ������ ����� GraphQL"
  })
  {
    storage{
      id,
      name,
      description
    }
  }
}
```
	