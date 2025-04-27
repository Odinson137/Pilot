FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Set ASPNETCORE_ENVIRONMENT in the base stage
ENV ASPNETCORE_ENVIRONMENT=RELEASE

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Pilot.Api/Pilot.Api.csproj", "Pilot.Api/"]
COPY ["Pilot.Contracts/Pilot.BaseContract/Pilot.BaseContract.csproj", "Pilot.Contracts/Pilot.BaseContract/"]
COPY ["Pilot.Messenger/Pilot.Messenger.csproj", "Pilot.Messenger/"]
COPY ["Pilot.Contracts/Pilot.SqrsControllerLibrary/Pilot.SqrsControllerLibrary.csproj", "Pilot.Contracts/Pilot.SqrsControllerLibrary/"]
RUN dotnet restore "Pilot.Api/Pilot.Api.csproj"
COPY . .
WORKDIR "/src/Pilot.Api"
RUN dotnet build "Pilot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Pilot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# The environment variable from the base stage is inherited here
ENTRYPOINT ["dotnet", "Pilot.Api.dll"]
