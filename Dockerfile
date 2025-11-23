# Use official lightweight .NET runtime build image for build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

RUN apt-get update && apt-get install -y libicu-dev gettext iputils-ping curl

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
RUN dotnet publish ./src/EngageGov.API/EngageGov.API.csproj -c Release -o /app/publish

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime

RUN groupadd -r appgroup && useradd -r -g appgroup appuser

RUN apt-get update && apt-get install -y libicu-dev iputils-ping curl

WORKDIR /app
COPY --from=build /app/publish .

# Set environment for ASP.NET Core
EXPOSE 5001
ENV ASPNETCORE_URLS http://+:5001
ENV DOTNET_RUNNING_IN_CONTAINER true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false

# Ensure non-root user owns the app folder
RUN chown -R appuser:appgroup /app
USER appuser

# Healthcheck
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 CMD ["/bin/sh", "-c", "wget -qO- http://localhost:80/health || exit 1"]

EXPOSE 80
ENTRYPOINT ["dotnet", "EngageGov.API.dll"]
