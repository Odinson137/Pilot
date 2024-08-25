FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Pilot.Receiver/Pilot.Receiver.csproj", "Pilot.Receiver/"]
COPY ["Pilot.Contracts/Pilot.BaseContract/Pilot.BaseContract.csproj", "Pilot.Contracts/Pilot.BaseContract/"]
COPY ["Pilot.Identity/Pilot.Identity.csproj", "Pilot.Identity/"]
COPY ["Pilot.Messenger/Pilot.Messenger.csproj", "Pilot.Messenger/"]
COPY ["Pilot.Contracts/Pilot.SqrsControllerLibrary/Pilot.SqrsControllerLibrary.csproj", "Pilot.Contracts/Pilot.SqrsControllerLibrary/"]
COPY ["Pilot.Contracts/Pilot.InvalidationCacheRedisLibrary/Pilot.InvalidationCacheRedisLibrary.csproj", "Pilot.Contracts/Pilot.InvalidationCacheRedisLibrary/"]
RUN dotnet restore "Pilot.Receiver/Pilot.Receiver.csproj"
COPY . .
WORKDIR "/src/Pilot.Receiver"
RUN dotnet build "Pilot.Receiver.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Pilot.Receiver.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pilot.Receiver.dll"]
