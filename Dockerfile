FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StudioManager.API/StudioManager.API.csproj", "StudioManager.API/"]
RUN dotnet restore "StudioManager.API/StudioManager.API.csproj"
COPY . .
WORKDIR "/src/StudioManager.API"
RUN dotnet build "StudioManager.API.csproj" -c release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StudioManager.API.csproj" -c release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudioManager.API.dll"]

EXPOSE 80
EXPOSE 8080
