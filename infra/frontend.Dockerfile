# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Paths relative to the root context (..)
COPY ["frontend/GoodHamburger.Frontend.csproj", "frontend/"]
COPY ["backend/src/GoodHamburger.Application/GoodHamburger.Application.csproj", "backend/src/GoodHamburger.Application/"]
COPY ["backend/src/GoodHamburger.Domain/GoodHamburger.Domain.csproj", "backend/src/GoodHamburger.Domain/"]

RUN dotnet restore "frontend/GoodHamburger.Frontend.csproj"

# Build and Publish
COPY frontend/ frontend/
COPY backend/src/GoodHamburger.Application/ backend/src/GoodHamburger.Application/
COPY backend/src/GoodHamburger.Domain/ backend/src/GoodHamburger.Domain/

WORKDIR /app/frontend
RUN dotnet publish "GoodHamburger.Frontend.csproj" -c Release -o /app/publish

# Runtime Stage (Nginx)
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
RUN rm -rf ./*
COPY --from=build /app/publish/wwwroot .

# Copy custom nginx config - this file will be in infra/ too
COPY infra/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
