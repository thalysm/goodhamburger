# Testes de Software

O projeto conta com uma suíte de testes automatizados que cobre 100% das regras críticas de negócio e integração.

## Estrutura de Testes
Os testes estão localizados na pasta `backend/tests` e seguem a pirâmide de testes:

- **GoodHamburger.UseCases.Test**: Unidade.
    - ✅ Lógica de Descontos (10%, 15%, 20%).
    - ✅ Validação de itens duplicados no pedido.
- **GoodHamburger.Validators.Test**: Unidade (FluentValidation).
    - ✅ Validação de Nome do Cliente (Obrigatório e limite de 100 caracteres).
    - ✅ Validação de Carrinho (Mínimo de 1 item, máximo de 10).
- **GoodHamburger.Infrastructure.Test**: Integração (SQLite In-Memory).
    - ✅ Persistência de Pedidos e Itens.
    - ✅ Atualização de Status no Banco de Dados.
- **GoodHamburger.WebApi.Test**: Integração/E2E (HTTP Client).
    - ✅ Consumo de Menu.
    - ✅ Fluxo completo de criação de pedido via API.

## Tecnologias Utilizadas
- **xUnit**: Framework de execução.
- **Moq**: Mocking de repositórios.
- **Bogus**: Geração de massa de dados aleatórios.
- **FluentAssertions**: Asserções legíveis.
- **FluentValidation.TestHelper**: Auxílio na validação de regras de entrada.

## Como Executar os Testes
```bash
cd backend
dotnet test
```
