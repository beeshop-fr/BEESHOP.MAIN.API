# �tape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore ./BEESHOP.MAIN.API.sln
RUN dotnet publish ./BEESHOP.MAIN.API/BEESHOP.MAIN.API.csproj -c Release -o /app/publish

# �tape de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "BEESHOP.MAIN.API.dll"]