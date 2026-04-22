# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy solution and projects (Paths are relative to the build context)
COPY ["src/GoodHamburger.Api/GoodHamburger.Api.csproj", "src/GoodHamburger.Api/"]
COPY ["src/GoodHamburger.Application/GoodHamburger.Application.csproj", "src/GoodHamburger.Application/"]
COPY ["src/GoodHamburger.Domain/GoodHamburger.Domain.csproj", "src/GoodHamburger.Domain/"]
COPY ["src/GoodHamburger.Infrastructure/GoodHamburger.Infrastructure.csproj", "src/GoodHamburger.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/GoodHamburger.Api/GoodHamburger.Api.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "src/GoodHamburger.Api/GoodHamburger.Api.csproj" -c Release -o /out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /out .

RUN mkdir -p /app/data
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/GoodHamburger.db"

EXPOSE 8080
ENTRYPOINT ["dotnet", "GoodHamburger.Api.dll"]
