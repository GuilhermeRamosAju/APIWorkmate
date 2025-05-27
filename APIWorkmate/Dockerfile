# Etapa 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copia apenas o csproj primeiro para restaurar as dependências
COPY APIWorkmate.csproj ./
RUN dotnet restore

# Copia o restante do código
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "APIWorkmate.dll"]
