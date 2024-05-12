#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["./FinancialAccountingServer.csproj", "FinancialAccountingServer/"]
#RUN dotnet restore "./FinancialAccountingServer/FinancialAccountingServer.csproj"
#COPY . .
#WORKDIR "/src/FinancialAccountingServer"
#RUN dotnet build "./FinancialAccountingServer.csproj" -c $BUILD_CONFIGURATION -o /app/build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем csproj файл в корневую директорию
COPY ["FinancialAccountingServer.csproj", "./"]

# Восстанавливаем зависимости
RUN dotnet restore "FinancialAccountingServer.csproj"

# Копируем все остальные файлы проекта
COPY . .

# Переходим в каталог проекта
WORKDIR "/src"

# Собираем проект
RUN dotnet build "FinancialAccountingServer.csproj" -c $BUILD_CONFIGURATION -o /app/build




FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FinancialAccountingServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialAccountingServer.dll"]