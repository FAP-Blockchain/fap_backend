# --- Base image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Fap.Api/Fap.Api.csproj", "Fap.Api/"]
COPY ["Fap.Domain/Fap.Domain.csproj", "Fap.Domain/"]
COPY ["Fap.Infrastructure/Fap.Infrastructure.csproj", "Fap.Infrastructure/"]
RUN dotnet restore "Fap.Api/Fap.Api.csproj"

COPY . .
WORKDIR "/src/Fap.Api"
RUN dotnet build "Fap.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fap.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fap.Api.dll"]
