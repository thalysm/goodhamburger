# Frontend - Blazor WebAssembly (.NET 10)

A interface do **Good Hamburger** é uma SPA de alta performance construída com Blazor.

## Pilares do Design
- **Estética Profissional**: Uso do MudBlazor para um sistema de grids e componentes limpo e moderno.
- **Responsividade Real**: Layout adaptável para atendentes (Dashboard) e clientes (Menu Público).
- **Usabilidade (UX)**: Fluxo de pedido "One-way", onde o sistema guia o usuário entre as categorias.

## Organização por Pastas (Next.js Style)
Mantemos o contexto agrupado para facilitar a escalabilidade:

- `Pages/Dashboard/`: Visão gerencial e resumo de vendas.
- `Pages/Orders/`: Fluxos de criação (`New`) e gestão (`Index`).
- `Pages/Menu/`: Página pública com layout aberto.

## Funcionalidades Inteligentes

### Busca Centralizada
Utilizamos um `SearchService` injetado via DI que coordena a barra de busca do topo com o conteúdo ativo:
- No **Painel**, filtra por ID do Pedido.
- No **Novo Pedido**, filtra por Nome do Item e **muda a aba de categoria** se o item for encontrado em outra seção do cardápio.

### Cálculos Dinâmicos
Toda a lógica de desconto é simulada no frontend para feedback instantâneo, mas a validação final e o cálculo oficial ocorrem na camada de domínio do backend, garantindo segurança total.

## Como Executar
```bash
cd frontend
dotnet run
```
O projeto subirá por padrão no endereço `http://localhost:5151`.
