# Base image with runtime dependencies
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Development

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["DockerAssignment.csproj", "./"]
RUN dotnet restore

# Copy the rest of the application code and build it
COPY . .
RUN dotnet build "DockerAssignment.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DockerAssignment.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Entry point for the application
ENTRYPOINT ["dotnet", "DockerAssignment.dll"]
