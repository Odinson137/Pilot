FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Pilot.Tests/Pilot.Tests.csproj", "Pilot.Tests/"]
COPY ["Pilot.Contracts/Pilot.Contracts.csproj", "Pilot.Contracts/"]
COPY ["Pilot.Identity/Pilot.Identity.csproj", "Pilot.Identity/"]
COPY ["Pilot.Api/Pilot.Api.csproj", "Pilot.Api/"]
COPY ["Pilot.Receiver/Pilot.Receiver.csproj", "Pilot.Receiver/"]
RUN dotnet restore "Pilot.Tests/Pilot.Tests.csproj"
COPY . .
WORKDIR "/src/Pilot.Tests"
RUN dotnet build "Pilot.Tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pilot.Tests.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pilot.Tests.dll"]
