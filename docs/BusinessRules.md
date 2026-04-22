# Regras de Negócio - Good Hamburger

## 1. Composição do Pedido
- Um pedido pode conter no máximo **um item de cada tipo** (um Sanduíche, um Acompanhamento e uma Bebida).
- O sistema bloqueia duplicatas de tipo (ex: dois sanduíches no mesmo pedido).

## 2. Política de Descontos (Combos)
Os descontos são aplicados automaticamente sobre o subtotal:
- **Combo Completo (20% OFF)**: Sanduíche + Acompanhamento + Bebida.
- **Combo Clássico (15% OFF)**: Sanduíche + Bebida.
- **Combo Dobradinha (10% OFF)**: Sanduíche + Acompanhamento.

## 3. Validações de Dados (Input)
Para que um pedido seja aceito, ele deve passar pelos seguintes critérios:
- **Nome do Cliente**: Obrigatório, mínimo de 1 caractere, máximo de 100.
- **Itens do Pedido**: Mínimo de 1 item selecionado, máximo de 10 (conforme limite de tipos).
- **Consistência**: Todos os IDs de itens devem existir no cardápio ativo.

## 4. Gestão de Status
- Um pedido nasce como `Pendente`.
- Pode progredir para `Em Andamento`, `Entrega` e `Finalizado`.
