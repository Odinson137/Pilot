# Этап сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта и восстанавливаем зависимости
COPY ["Pilot.Identity/Pilot.Identity.csproj", "Pilot.Identity/"]
RUN dotnet restore "Pilot.Identity/Pilot.Identity.csproj"

# Копируем все остальные файлы и выполняем сборку
COPY . .
WORKDIR "/src/Pilot.Identity"
RUN dotnet build "Pilot.Identity.csproj" -c Release -o /app/build

# Этап публикации приложения
FROM build AS publish
RUN dotnet publish "Pilot.Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап сборки конечного образа
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем файлы из предыдущего этапа
COPY --from=publish /app/publish .

# Определяем порт, который должен быть открыт приложением
EXPOSE 80

# Указываем точку входа в приложение
ENTRYPOINT ["dotnet", "Pilot.Identity.dll"]
