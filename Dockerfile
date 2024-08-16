FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app/
COPY ProductStorageAPI/ProductStorageAPI.csproj ./ProductStorageAPI/
COPY SharedModels/SharedModels.csproj ./SharedModels/
WORKDIR /app/ProductStorageAPI/
RUN dotnet restore
WORKDIR /app/
COPY ./ProductStorageAPI ./ProductStorageAPI
COPY ./SharedModels ./SharedModels

WORKDIR /app/ProductStorageAPI/
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app/ProductStorageAPI/
EXPOSE 80
COPY --from=build-env /app/ProductStorageAPI/out .
ENTRYPOINT ["dotnet", "/app/ProductStorageAPI/ProductStorageAPI.dll"]