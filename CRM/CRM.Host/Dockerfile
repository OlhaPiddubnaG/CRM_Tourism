FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CRM.Host/CRM.Host.csproj", "CRM.Host/"]
COPY ["CRM.DataAccess/CRM.DataAccess.csproj", "CRM.DataAccess/"]
COPY ["CRM.Domain/CRM.Domain.csproj", "CRM.Domain/"]
RUN dotnet restore "CRM.Host/CRM.Host.csproj"
COPY . .
WORKDIR "/src/CRM.Host"
RUN dotnet build "CRM.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CRM.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRM.Host.dll"]
