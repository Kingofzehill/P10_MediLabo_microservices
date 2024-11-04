#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# https://docs.docker.com/build/concepts/dockerfile/

# Defines a base for your image (from Docker Images library).
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
# Sets the working directory.
WORKDIR /app
#Docker Port exposed. FIXRUN03 change EXPOSE 8080 for 80 and desactivate
#EXPOSE 80
#FIXRUN03 change EXPOSE 8081 for 443
EXPOSE 443

# Build stage.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Defines project directories to copy for build.
COPY ["PatientFront/PatientFront.csproj", "PatientFront/"]
COPY ["PatientBack.API/PatientBackAPI.csproj", "PatientBack.API/"]
COPY ["PatientDiabeteRiskBackAPI/PatientDiabeteRiskBackAPI.csproj", "PatientDiabeteRiskBackAPI/"]
COPY ["PatientNoteBackAPI/PatientNoteBackAPI.csproj", "PatientNoteBackAPI/"]
# Restore project tools and dependencies ==> https://learn.microsoft.com/fr-fr/dotnet/core/tools/dotnet-restore
RUN dotnet restore "./PatientFront/PatientFront.csproj"
COPY . .
WORKDIR "/src/PatientFront"
# Build project and all dependencies ==> https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-build 
RUN dotnet build "./PatientFront.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Publishes the application and its dependencies to a folder for deployment to a hosting system. ==> https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish
RUN dotnet publish "./PatientFront.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage. Runtime environment.
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT serves as starting point for a the Docker container’s runtime
ENTRYPOINT ["dotnet", "PatientFront.dll"]