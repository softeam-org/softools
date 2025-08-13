FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Softools.Projetos/Softools.Projetos.csproj", "Softools.Projetos/"]
COPY ["Softools.ServiceDefaults/Softools.ServiceDefaults.csproj", "Softools.ServiceDefaults/"]
RUN dotnet restore "Softools.Projetos/Softools.Projetos.csproj"
COPY . .
WORKDIR "/src/Softools.Projetos"
RUN dotnet build "./Softools.Projetos.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Softools.Projetos.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Softools.Projetos.dll"]
