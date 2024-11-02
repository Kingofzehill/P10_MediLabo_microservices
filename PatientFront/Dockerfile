#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
#FIXRUN03 change EXPOSE 8080 for 80 and desactivate
#EXPOSE 80
#FIXRUN03 change EXPOSE 8081 for 443
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PatientFront/PatientFront.csproj", "PatientFront/"]
# Not working ==> Projet references management: https://www.docker.com/blog/dockerfiles-now-support-multiple-build-contexts/
COPY ["PatientBack.API/PatientBackAPI.csproj", "PatientBack.API/"]
#COPY –from=PatientBackAPI . /src
COPY ["PatientDiabeteRiskBackAPI/PatientDiabeteRiskBackAPI.csproj", "PatientDiabeteRiskBackAPI/"]
#COPY –from=PatientDiabeteRiskBackAPI . /src
COPY ["PatientNoteBackAPI/PatientNoteBackAPI.csproj", "PatientNoteBackAPI/"]
#COPY –from=PatientNoteBackAPI . /src
RUN dotnet restore "./PatientFront/PatientFront.csproj"
COPY . .
WORKDIR "/src/PatientFront"
RUN dotnet build "./PatientFront.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PatientFront.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientFront.dll"]