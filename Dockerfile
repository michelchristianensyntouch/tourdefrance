# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for layer-cached restore
COPY TourDeFrance.slnx .
COPY TourDeFrance.Data/TourDeFrance.Data.csproj TourDeFrance.Data/
COPY TourDeFrance.Data.SqlServer/TourDeFrance.Data.SqlServer.csproj TourDeFrance.Data.SqlServer/
COPY TourDeFrance.Web/TourDeFrance.Web.csproj TourDeFrance.Web/
COPY TourDeFrance.Tests/TourDeFrance.Tests.csproj TourDeFrance.Tests/

RUN dotnet restore TourDeFrance.Web/TourDeFrance.Web.csproj

# Copy source and publish
COPY TourDeFrance.Data/ TourDeFrance.Data/
COPY TourDeFrance.Data.SqlServer/ TourDeFrance.Data.SqlServer/
COPY TourDeFrance.Web/ TourDeFrance.Web/

RUN dotnet publish TourDeFrance.Web/TourDeFrance.Web.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TourDeFrance.Web.dll"]
