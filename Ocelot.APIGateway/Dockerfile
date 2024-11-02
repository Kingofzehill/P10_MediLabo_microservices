#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
#FIXRUN03 change EXPOSE 8080 for 80 and deactivate
#EXPOSE 80
#FIXRUN03 change EXPOSE 8081 for 443
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ocelot.APIGateway/OcelotAPIGateway.csproj", "Ocelot.APIGateway/"]
RUN dotnet restore "./Ocelot.APIGateway/OcelotAPIGateway.csproj"
COPY . .
WORKDIR "/src/Ocelot.APIGateway"
RUN dotnet build "./OcelotAPIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OcelotAPIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotAPIGateway.dll"]