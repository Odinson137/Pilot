FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Pilot.BackgroundJob/Pilot.BackgroundJob.csproj", "Pilot.BackgroundJob/"]
COPY ["Pilot.Contracts/Pilot.BaseContract/Pilot.BaseContract.csproj", "Pilot.Contracts/Pilot.BaseContract/"]
COPY ["Pilot.Contracts/Pilot.SqrsControllerLibrary/Pilot.SqrsControllerLibrary.csproj", "Pilot.Contracts/Pilot.SqrsControllerLibrary/"]
RUN dotnet restore "Pilot.BackgroundJob/Pilot.BackgroundJob.csproj"
COPY . .
WORKDIR "/src/Pilot.BackgroundJob"
RUN dotnet build "./Pilot.BackgroundJob.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Pilot.BackgroundJob.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pilot.BackgroundJob.dll"]
