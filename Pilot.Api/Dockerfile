# Этап сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта и восстанавливаем зависимости
COPY ["Pilot.Api/Pilot.Api.csproj", "Pilot.Api/"]
COPY ["Pilot.Contracts/Pilot.Contracts.csproj", "Pilot.Contracts/"]
RUN dotnet restore "Pilot.Api/Pilot.Api.csproj"

# Копируем все остальные файлы и выполняем сборку
COPY . .
WORKDIR "/src/Pilot.Api"
RUN dotnet build "Pilot.Api.csproj" -c Release -o /app/build

# Этап публикации приложения
FROM build AS publish
RUN dotnet publish "Pilot.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап сборки конечного образа
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем файлы из предыдущего этапа
COPY --from=publish /app/publish .

# Определяем порты, которые должны быть открыты приложением
EXPOSE 80
EXPOSE 443

# Указываем точку входа в приложение
ENTRYPOINT ["dotnet", "Pilot.Api.dll"]
