# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy solution and projects (Paths are relative to the build context)
COPY ["backend/src/GoodHamburger.Api/GoodHamburger.Api.csproj", "backend/src/GoodHamburger.Api/"]
COPY ["backend/src/GoodHamburger.Application/GoodHamburger.Application.csproj", "backend/src/GoodHamburger.Application/"]
COPY ["backend/src/GoodHamburger.Domain/GoodHamburger.Domain.csproj", "backend/src/GoodHamburger.Domain/"]
COPY ["backend/src/GoodHamburger.Infrastructure/GoodHamburger.Infrastructure.csproj", "backend/src/GoodHamburger.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "backend/src/GoodHamburger.Api/GoodHamburger.Api.csproj"

# Copy everything else and build
COPY backend/ backend/
RUN dotnet publish "backend/src/GoodHamburger.Api/GoodHamburger.Api.csproj" -c Release -o /out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /out .

RUN mkdir -p /app/data
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/GoodHamburger.db"

EXPOSE 8080
ENTRYPOINT ["dotnet", "GoodHamburger.Api.dll"]
