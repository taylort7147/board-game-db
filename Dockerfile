# Specify image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set work dir
WORKDIR /app

# Copy project file
COPY BoardGameDB/*.csproj ./
RUN dotnet restore

# Copy and build
COPY BoardGameDB ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0

# Set work dir
WORKDIR /app

# Copy build files to container
COPY --from=build-env /app/out .

# Environment
VOLUME [ "/data" ]
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:5001
ENV BGDB_CONNECTION_STRING="Data Source=/data/games.db;"

ENTRYPOINT ["dotnet", "BoardGameDB.dll"]