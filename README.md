# Hacker News API Test Project

## Overview

This is a .NET project designed to test the Hacker News API, providing comprehensive unit tests for retrieving and processing Hacker News stories and comments.

## Project Structure

- `Clients/`: Contains API client implementations
  - `ApiClient.cs`: Base API client
  - `HackerNewsService.cs`: Specific implementation for Hacker News API interactions
- `Models/`: Data models for the project
  - `Story.cs`: Represents a Hacker News story
  - `Comment.cs`: Represents a Hacker News comment
- `Tests/`: Unit tests for the project
  - `TopStoriesTests.cs`: Tests for retrieving top stories
  - `BaseTest.cs`: Base test configuration

## Prerequisites

- .NET 8.0 SDK
- Visual Studio or Visual Studio Code

## Setup

1. Clone the repository
2. Restore NuGet packages:
   ```
   dotnet restore
   ```

## Running Tests

Execute the tests using the .NET CLI:

```
dotnet test
```

## Key Dependencies

- Microsoft.NET.Test.Sdk
- Newtonsoft.Json
- MSTest

## Contributing

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Create a new Pull Request
