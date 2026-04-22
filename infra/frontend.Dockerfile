# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "frontend/GoodHamburger.Frontend.csproj"
RUN dotnet publish "frontend/GoodHamburger.Frontend.csproj" -c Release -o /app/publish

# Runtime Stage (Nginx)
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
RUN rm -rf ./*

# Tenta copiar da wwwroot, mas se não existir ou estiver vazia, copia da raiz do publish
COPY --from=build /app/publish/wwwroot .
COPY --from=build /app/publish .

# Garante que a pasta _framework esteja no lugar certo (alguns builds do .NET 10 mudam isso)
COPY --from=build /app/publish/wwwroot/_framework ./_framework
COPY --from=build /app/publish/_framework ./_framework

# Copy custom nginx config
COPY infra/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
