FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EnviroNotify.Dashboard/EnviroNotify.Dashboard.csproj", "EnviroNotify.Dashboard/"]
COPY ["EnviroNotify.Shared/EnviroNotify.Shared.csproj", "EnviroNotify.Shared/"]
RUN dotnet restore "EnviroNotify.Dashboard/EnviroNotify.Dashboard.csproj"
COPY . .
WORKDIR "/src/EnviroNotify.Dashboard"
RUN dotnet build "EnviroNotify.Dashboard.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EnviroNotify.Dashboard.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnviroNotify.Dashboard.dll"]
