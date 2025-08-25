# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy solution and project files
COPY BBS.sln ./
COPY BBS.Api/BBS.Api.csproj BBS.Api/
COPY BBS.Application/BBS.Application.csproj BBS.Application/
COPY BBS.Domain/BBS.Domain.csproj BBS.Domain/
COPY BBS.Infrastructure/BBS.Infrastructure.csproj BBS.Infrastructure/

RUN dotnet restore

# copy all source and publish
COPY . .
RUN dotnet publish BBS.Api/BBS.Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "BBS.Api.dll"]
