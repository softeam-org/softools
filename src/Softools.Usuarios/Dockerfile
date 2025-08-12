FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Softools.Usuarios/Softools.Usuarios.csproj", "Softools.Usuarios/"]
COPY ["Softools.ServiceDefaults/Softools.ServiceDefaults.csproj", "Softools.ServiceDefaults/"]
RUN dotnet restore "Softools.Usuarios/Softools.Usuarios.csproj"
COPY . .
WORKDIR "/src/Softools.Usuarios"
RUN dotnet build "./Softools.Usuarios.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Softools.Usuarios.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Softools.Usuarios.dll"]
