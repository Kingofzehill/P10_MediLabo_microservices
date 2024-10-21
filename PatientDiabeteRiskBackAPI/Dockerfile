#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8086
EXPOSE 8087

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PatientDiabeteRiskBackAPI/PatientDiabeteRiskBackAPI.csproj", "PatientDiabeteRiskBackAPI/"]
COPY ["PatientBack.API/PatientBackAPI.csproj", "PatientBack.API/"]
COPY ["PatientNoteBackAPI/PatientNoteBackAPI.csproj", "PatientNoteBackAPI/"]
RUN dotnet restore "./PatientDiabeteRiskBackAPI/PatientDiabeteRiskBackAPI.csproj"
COPY . .
WORKDIR "/src/PatientDiabeteRiskBackAPI"
RUN dotnet build "./PatientDiabeteRiskBackAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PatientDiabeteRiskBackAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientDiabeteRiskBackAPI.dll"]