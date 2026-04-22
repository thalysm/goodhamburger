# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "frontend/GoodHamburger.Frontend.csproj"
RUN dotnet publish "frontend/GoodHamburger.Frontend.csproj" -c Release -o /app/publish /p:StaticWebAssetProjectMode=Standalone

# Runtime Stage (Nginx)
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
RUN rm -rf ./*

# Copia o conteúdo da wwwroot gerada pelo publish para a raiz do Nginx
COPY --from=build /app/publish/wwwroot .

# Copy custom nginx config
COPY infra/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
