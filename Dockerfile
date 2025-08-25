# Multi-stage build for .NET applications supporting multi-project solutions
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy solution file first (if exists) for better layer caching
COPY *.sln* ./

# Copy all project files for restore
COPY **/*.csproj ./
COPY */*.csproj ./
COPY *.csproj ./

# Restore dependencies for entire solution
RUN dotnet restore || echo "No solution file found, restoring individual projects..."

# Copy all source code
COPY . .

# Find and validate startup project
RUN STARTUP_PROJECT="" && \
    # First, try to find Web API projects (most common)
    for proj in $(find . -name "*.csproj"); do \
        if grep -q "Microsoft.AspNetCore" "$proj" 2>/dev/null; then \
            STARTUP_PROJECT="$proj"; \
            break; \
        fi \
    done && \
    # If no Web API found, look for executable projects
    if [ -z "$STARTUP_PROJECT" ]; then \
        for proj in $(find . -name "*.csproj"); do \
            if grep -q "<OutputType>Exe</OutputType>" "$proj" 2>/dev/null; then \
                STARTUP_PROJECT="$proj"; \
                break; \
            fi \
        done \
    fi && \
    # Fallback to first project found
    if [ -z "$STARTUP_PROJECT" ]; then \
        STARTUP_PROJECT=$(find . -name "*.csproj" | head -1); \
    fi && \
    # Validate project exists
    if [ -z "$STARTUP_PROJECT" ] || [ ! -f "$STARTUP_PROJECT" ]; then \
        echo "Error: No valid .csproj files found"; \
        find . -name "*.csproj" -ls; \
        exit 1; \
    fi && \
    echo "Selected startup project: $STARTUP_PROJECT" && \
    # Build and publish the startup project
    dotnet publish "$STARTUP_PROJECT" -c Release -o /app/publish --no-restore --verbosity normal

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published application (this includes all required runtime files)
COPY --from=build-env /app/publish .

# Verify published files exist
RUN ls -la /app && \
    if [ ! -f /app/*.runtimeconfig.json ]; then \
        echo "Warning: No .runtimeconfig.json found, but continuing..."; \
    fi

# Create non-root user for security
RUN adduser --disabled-password --gecos '' --shell /bin/bash appuser && \
    chown -R appuser /app
USER appuser

# Expose standard port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_EnableDiagnostics=0

# Find main executable DLL and create startup script
RUN MAIN_DLL=$(find /app -maxdepth 1 -name "*.dll" -not -name "*.Views.dll" -not -name "*.PrecompiledViews.dll" | head -1) && \
    if [ -z "$MAIN_DLL" ]; then \
        echo "Error: No main DLL found in /app"; \
        ls -la /app; \
        exit 1; \
    fi && \
    echo "Main application DLL: $(basename $MAIN_DLL)" && \
    echo '#!/bin/bash' > /app/start.sh && \
    echo "exec dotnet \"$MAIN_DLL\" \"\$@\"" >> /app/start.sh && \
    chmod +x /app/start.sh

# Health check (works for both Web APIs and console apps)
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health 2>/dev/null || curl -f http://localhost:8080/ 2>/dev/null || exit 1

# Start the application
ENTRYPOINT ["/app/start.sh"]
