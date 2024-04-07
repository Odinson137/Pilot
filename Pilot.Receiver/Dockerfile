# Этап сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проектов и восстанавливаем зависимости
COPY ["Pilot.Receiver/Pilot.Receiver.csproj", "Pilot.Receiver/"]
COPY ["Pilot.Contracts/Pilot.Contracts.csproj", "Pilot.Contracts/"]
RUN dotnet restore "Pilot.Receiver/Pilot.Receiver.csproj"

# Копируем все остальные файлы и выполняем сборку
COPY . .
WORKDIR "/src/Pilot.Receiver"
RUN dotnet build "Pilot.Receiver.csproj" -c Release -o /app/build

# Этап публикации приложения
FROM build AS publish
RUN dotnet publish "Pilot.Receiver.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап сборки конечного образа
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Определяем порты, которые должны быть открыты приложением
EXPOSE 80

# Указываем точку входа в приложение
ENTRYPOINT ["dotnet", "Pilot.Receiver.dll"]
