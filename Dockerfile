FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BPN.ECommerce.Api/BPN.ECommerce.Api.csproj", "BPN.ECommerce.Api/"]
COPY ["BPN.ECommerce.Application/BPN.ECommerce.Application.csproj", "BPN.ECommerce.Application/"]
COPY ["BPN.ECommerce.Domain/BPN.ECommerce.Domain.csproj", "BPN.ECommerce.Domain/"]
COPY ["BPN.ECommerce.Infrastructure/BPN.ECommerce.Infrastructure.csproj", "BPN.ECommerce.Infrastructure/"]
RUN dotnet restore "BPN.ECommerce.Api/BPN.ECommerce.Api.csproj"
COPY . .
WORKDIR "/src/BPN.ECommerce.Api"
RUN dotnet build "BPN.ECommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BPN.ECommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BPN.ECommerce.Api.dll"]

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD curl --fail http://localhost/health || exit 1