# Infraestrutura (Docker)

O **Good Hamburger** utiliza uma arquitetura baseada em containers para garantir que o ambiente de desenvolvimento seja idêntico ao de produção. Tudo o que é necessário para rodar o sistema está centralizado na pasta `infra/`.

## Estrutura da Pasta `infra/`
- **`docker-compose.yml`**: Arquivo mestre que orquestra os serviços.
- **`backend.Dockerfile`**: Receita para construir a imagem da API .NET 10.
- **`frontend.Dockerfile`**: Receita para compilar o Blazor e configurar o servidor Nginx.
- **`nginx.conf`**: Configuração do Nginx para suportar aplicações Single Page Application (SPA).

## Serviços Orquestrados

### 1. Backend (`goodhamburger-api`)
- **Base**: `aspnet:10.0`
- **Porta Exposta**: `5269` (interno 8080).
- **Persistência**: Utiliza um volume mapeado em `../data:/app/data` para garantir que o banco SQLite (`GoodHamburger.db`) não seja apagado ao destruir o container.
- **Inicialização**: O banco é criado e populado automaticamente no primeiro boot do container.

### 2. Frontend (`goodhamburger-web`)
- **Base**: `nginx:alpine`
- **Porta Exposta**: `5151` (interno 80).
- **Servidor Web**: Os arquivos estáticos do Blazor WASM são servidos pelo Nginx.
- **Proxy**: O frontend está configurado para se comunicar com a API via endereço estático ou gateway de rede.

## Rede Local (Docker Network)
Ambos os containers estão na rede `goodhamburger-network`. Isso permite que eles se comuniquem usando nomes de serviço (ex: o frontend pode chamar `http://backend:8080/api/orders`).

## Guia de Operação

### Subir o ambiente completo
```bash
cd infra
docker-compose up --build
```

### Rodar em modo "detached" (segundo plano)
```bash
docker-compose up -d
```

### Derrubar e limpar volumes
```bash
docker-compose down -v
```

## Considerações de Produção
- O arquivo `docker-compose` atual está configurado para facilitar o desenvolvimento. Em produção, recomenda-se o uso de segredos (Docker Secrets) para strings de conexão e o uso de HTTPS via certificados SSL no Nginx.
