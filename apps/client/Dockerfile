# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project file and restore dependencies
COPY client.csproj ./
COPY src ./src
RUN dotnet restore

# Build the application
RUN dotnet build --configuration Release --output /app/build

# Runtime image for the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "client-release.dll"]
