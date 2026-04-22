# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all project files first for restore
COPY ["frontend/GoodHamburger.Frontend.csproj", "frontend/"]
COPY ["backend/src/GoodHamburger.Application/GoodHamburger.Application.csproj", "backend/src/GoodHamburger.Application/"]
COPY ["backend/src/GoodHamburger.Domain/GoodHamburger.Domain.csproj", "backend/src/GoodHamburger.Domain/"]

RUN dotnet restore "frontend/GoodHamburger.Frontend.csproj"

# Copy the rest of the source code
COPY . .

# Publish from the frontend directory
WORKDIR /src/frontend
RUN dotnet publish "GoodHamburger.Frontend.csproj" -c Release -o /app/publish

# Runtime Stage (Nginx)
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
RUN rm -rf ./*
COPY --from=build /app/publish/wwwroot .

# Copy custom nginx config
COPY infra/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
