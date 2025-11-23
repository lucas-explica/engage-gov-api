# Use official lightweight .NET runtime build image for build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# Install necessary packages for building on Alpine
RUN apk add --no-cache icu-libs libintl

WORKDIR /src

# copy csproj and restore as distinct layers
COPY src/EngageGov.API/*.csproj ./EngageGov.API/
COPY src/EngageGov.Application/*.csproj ./EngageGov.Application/
COPY src/EngageGov.Domain/*.csproj ./EngageGov.Domain/
COPY src/EngageGov.Infrastructure/*.csproj ./EngageGov.Infrastructure/
COPY tests/EngageGov.IntegrationTests/*.csproj ./tests/EngageGov.IntegrationTests/
COPY tests/EngageGov.UnitTests/*.csproj ./tests/EngageGov.UnitTests/

RUN dotnet restore ./EngageGov.API/EngageGov.API.csproj

# copy everything else and build
COPY . .
WORKDIR /src
RUN dotnet publish ./src/EngageGov.API/EngageGov.API.csproj -c Release -o /app/publish --no-restore

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime

# Create a non-root user
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

# Install runtime dependencies (icu for globalization if needed)
RUN apk add --no-cache icu-libs

WORKDIR /app
COPY --from=build /app/publish .

# Set environment for ASP.NET Core
ENV ASPNETCORE_URLS http://+:80
ENV DOTNET_RUNNING_IN_CONTAINER true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false

# Ensure non-root user owns the app folder
RUN chown -R appuser:appgroup /app
USER appuser

# Healthcheck
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 CMD ["/bin/sh", "-c", "wget -qO- http://localhost:80/health || exit 1"]

EXPOSE 80
ENTRYPOINT ["dotnet", "EngageGov.API.dll"]
