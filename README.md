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

### Test Reporting

This project uses **ExtentReports** to generate comprehensive HTML test reports after each test execution.

**Report Location:**
After running tests, the HTML report is automatically generated at:

```
hackernews-api-test/bin/Debug/net8.0/TestReports/TestReport_[timestamp].html
```

**Opening the Report:**
Simply open the HTML file in any web browser to view:

- Test execution summary with pass/fail statistics
- Detailed test results with logs and timestamps
- Test categories (regression, smoke)
- System/environment information
- Interactive dashboard with charts

**Report Features:**

- ✅ Pass/Fail status for each test
- 📝 Detailed test steps and logs
- 🏷️ Test categorization (regression, smoke)
- ⏱️ Execution time tracking
- 💻 System information (OS, .NET version, etc.)
- 📊 Visual dashboard with test statistics

## Key Dependencies

- Microsoft.NET.Test.Sdk
- Newtonsoft.Json
- MSTest
- ExtentReports (for HTML test reporting)

## Contributing

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Create a new Pull Request
