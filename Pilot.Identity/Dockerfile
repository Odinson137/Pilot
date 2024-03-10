FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Pilot.Identity/Pilot.Identity.csproj", "Pilot.Identity/"]
RUN dotnet restore "Pilot.Identity/Pilot.Identity.csproj"
COPY . .
WORKDIR "/src/Pilot.Identity"
RUN dotnet build "Pilot.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pilot.Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pilot.Identity.dll"]
