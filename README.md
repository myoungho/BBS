# BBS

Simple bulletin board REST API built with ASP.NET Core and EF Core.

## Features
- Create posts
- List posts with comments
- Add comments to posts

## Running
1. Ensure SQL Server is available and update the connection string in `appsettings.json` if needed.
2. Restore and run the API:
   ```bash
   dotnet build
   dotnet run --project BBS.Api
   ```

## Docker
Build and run the API using Docker:
```bash
docker build -t bbs-api .
docker run -p 8080:8080 bbs-api
```
