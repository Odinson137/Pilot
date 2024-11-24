FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Set ASPNETCORE_ENVIRONMENT in the base stage
ENV ASPNETCORE_ENVIRONMENT=RELEASE

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Pilot.Identity/Pilot.Identity.csproj", "Pilot.Identity/"]
COPY ["Pilot.Contracts/Pilot.BaseContract/Pilot.BaseContract.csproj", "Pilot.Contracts/Pilot.BaseContract/"]
RUN dotnet restore "Pilot.Identity/Pilot.Identity.csproj"
COPY . .
WORKDIR "/src/Pilot.Identity"
RUN dotnet build "Pilot.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Pilot.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pilot.Identity.dll"]
