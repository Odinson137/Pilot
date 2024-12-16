FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Pilot.Capability/Pilot.Capability.csproj", "Pilot.Capability/"]
COPY ["Pilot.Contracts/Pilot.BaseContract/Pilot.BaseContract.csproj", "Pilot.Contracts/Pilot.BaseContract/"]
COPY ["Pilot.Contracts/Pilot.SqrsControllerLibrary/Pilot.SqrsControllerLibrary.csproj", "Pilot.Contracts/Pilot.SqrsControllerLibrary/"]
COPY ["Pilot.Contracts/Pilot.InvalidationCacheRedisLibrary/Pilot.InvalidationCacheRedisLibrary.csproj", "Pilot.Contracts/Pilot.InvalidationCacheRedisLibrary/"]
RUN dotnet restore "Pilot.Capability/Pilot.Capability.csproj"
COPY . .
WORKDIR "/src/Pilot.Capability"
RUN dotnet build "Pilot.Capability.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Pilot.Capability.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pilot.Capability.dll"]
