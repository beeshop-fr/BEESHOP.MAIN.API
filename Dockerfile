# --- Build Stage ---  
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build  
WORKDIR /src  

# Copy project files  
COPY ./BEESHOP.MAIN.API/BEESHOP.MAIN.API.csproj ./BEESHOP.MAIN.API/  
RUN dotnet restore ./BEESHOP.MAIN.API/BEESHOP.MAIN.API.csproj  

# Copy the rest of the source code and build  
COPY . .  
RUN dotnet build ./BEESHOP.MAIN.API/BEESHOP.MAIN.API.csproj -c Release --no-restore  

# Publish the application  
RUN dotnet publish ./BEESHOP.MAIN.API/BEESHOP.MAIN.API.csproj \  
   -c Release --no-build -o /publish /p:UseAppHost=false  

# --- Runtime Stage ---  
FROM mcr.microsoft.com/dotnet/aspnet:8.0  
WORKDIR /app  

# Environment variables for performance and configuration  
ENV DOTNET_EnableDiagnostics=0  
ENV ASPNETCORE_URLS=http://+:8080  

# Copy the published output from the build stage  
COPY --from=build /publish/ ./  

# Expose the application port  
EXPOSE 8080  

# Set the entry point for the application  
ENTRYPOINT ["dotnet", "BEESHOP.MAIN.API.dll"]
