# Good Hamburger System 🍔

Sistema completo de gestão de pedidos para a lanchonete **Good Hamburger**, focado em experiência gourmet e eficiência operacional.

## 🚀 Tecnologias Utilizadas

### Backend
- **Framework**: .NET 10
- **Arquitetura**: Clean Architecture
- **Banco de Dados**: SQLite (com EF Core)
- **Validação**: FluentValidation
- **Documentação**: OpenAPI com **Scalar**
- **Testes**: Unidade, Integração e E2E (xUnit, Moq, Bogus, FluentAssertions)

### Frontend
- **Framework**: Blazor WebAssembly (.NET 10)
- **UI Components**: MudBlazor
- **Estruturação**: Padrão de rotas baseado em pastas (Next.js Style)
- **Estado**: SearchService para busca global reativa

## 🛠️ Como Executar

### Pré-requisitos
- .NET 10 SDK

### Backend
1. Navegue até a pasta do backend:
   ```bash
   cd backend
   ```
2. Restaure e execute:
   ```bash
   dotnet run --project src/GoodHamburger.Api
   ```
3. Acesse a documentação da API em: `http://localhost:5269/scalar/v1`

### Frontend
1. Navegue até a pasta do frontend e execute `dotnet run`.
2. O app estará disponível em `http://localhost:5151`.

### Testes Automatizados
1. Navegue até a pasta do backend e execute:
   ```bash
   dotnet test
   ```

---
### 🐳 Rodando com Docker
Se preferir rodar tudo em containers:
```bash
cd infra
docker-compose up --build
```

## 📖 Documentação Completa
Para detalhes técnicos, consulte a pasta `/docs`:
- [[index|Sumário da Documentação]]
- [[Infrastructure|Detalhes da Infraestrutura (Docker)]]

## 💡 Principais Diferenciais
- **Infra as Code**: Ambiente 100% reprodutível via Docker na pasta `/infra`.
- **Busca Híbrida**: Localização de pedidos por ID e itens por nome com troca automática de categoria.
- **Cálculos Reativos**: Descontos de combo aplicados em tempo real no frontend e validados no backend.

---
## Próximos Passos:

- [] Utilizar banco de dados relacional (PostgreSQL, MySQL, etc)
- [] Adicionar autenticação e autorização (OAuth, JWT, etc)
- [] Paginação de pedidos para não sobrecarregar a aplicação
