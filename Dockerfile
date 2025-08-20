# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore ./BEESHOP.MAIN.API.sln
RUN dotnet build   ./BEESHOP.MAIN.API.sln -c Release --no-restore
RUN dotnet publish ./BEESHOP.MAIN.API/BEESHOP.MAIN.API.csproj -c Release -o /app/publish

# Étape de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
ENV DOTNET_EnableDiagnostics=0

COPY --from=build /src/BEESHOP.MAIN.API/bin/Release/net8.0/publish ./

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BEESHOP.MAIN.API.dll"]