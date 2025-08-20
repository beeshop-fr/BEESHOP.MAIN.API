# --- Build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore ./BEESHOP.MAIN.API/BEESHOP.MAIN.API.csproj
RUN dotnet build   ./BEESHOP.MAIN.API/BEESHOP.MAIN.API.csproj -c Release --no-restore
RUN dotnet publish ./BEESHOP.MAIN.API/BEESHOP.MAIN.API.csproj \
    -c Release --no-build -o /publish /p:UseAppHost=false

# --- Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
ENV DOTNET_EnableDiagnostics=0
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /publish/ ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "BEESHOP.MAIN.API.dll"]