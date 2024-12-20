FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["AgileMetricsServer/AgileMetricsServer.csproj", "AgileMetricsServer/"]
RUN dotnet restore "AgileMetricsServer/AgileMetricsServer.csproj"
COPY . .
WORKDIR "/src/AgileMetricsServer"
RUN dotnet build "AgileMetricsServer.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "AgileMetricsServer.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgileMetricsServer.dll"]
