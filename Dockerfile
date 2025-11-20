FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and project files
COPY hackernews-api-test.sln .
COPY hackernews-api-test/hackernews-api-test.csproj ./hackernews-api-test/

# Restore dependencies
RUN dotnet restore

# Copy the entire project
COPY . .

# Run tests
ENTRYPOINT ["dotnet", "test"]
