FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
#RUN dotnet dev-certs https
#RUN update-ca-certificates
COPY . .
RUN dotnet restore "StudioManager.API/StudioManager.API.csproj"

COPY . .
WORKDIR "/src/StudioManager.API"
RUN dotnet build -c release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudioManager.API.dll"]