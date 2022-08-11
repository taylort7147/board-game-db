# Specify image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Install node
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_16.x | bash \
    && apt-get install nodejs -yq

# Set work dir
WORKDIR /app

# Copy project file
COPY BoardGameDB/*.csproj ./
RUN dotnet restore

# Copy files
COPY BoardGameDB ./

# Install node packages 
RUN npm install

# Run babel on react files
RUN npx babel wwwroot/js --out-dir wwwroot/js/react --presets react-app/prod

# Build app
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