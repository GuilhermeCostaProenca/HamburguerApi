# 🍔 HamburguerApi

Sistema completo de **gestão de lanchonete** com backend em **ASP.NET Core 8 + EF Core** e painel administrativo em **HTML/CSS/JS**.

---

## 📌 Funcionalidades

* CRUD de **Hambúrgueres**, **Ingredientes**, **Clientes**, **Acompanhamentos**, **Bebidas** e **Sobremesas**.
* Gestão de **Pedidos** com fluxo Kanban:

  * Recebido → Em Preparo → Pronto → Saiu para Entrega → Finalizado / Cancelado.
* Painel web simples (em `wwwroot/index.html`) para interagir com a API.
* Validações automáticas com **FluentValidation**.
* Documentação automática da API com **Swagger**.

---

## 🛠️ Tecnologias Utilizadas

* **ASP.NET Core 8**
* **Entity Framework Core 9** (SQL Server)
* **FluentValidation**
* **Swagger / Swashbuckle**
* **HTML + CSS + JavaScript** (painel administrativo)

---

## 📂 Estrutura do Projeto

```
HamburguerApi/
├── controllers/        # Endpoints REST
├── data/               # DbContext
├── dtos/               # Data Transfer Objects
├── mappings/           # Configurações de AutoMapper
├── models/             # Entidades do banco
├── validators/         # Regras de validação
├── wwwroot/            # Painel web (index.html + style.css)
├── Migrations/         # Migrações do EF Core
├── Program.cs          # Configuração da aplicação
└── HamburguerApi.csproj
```

---

## 🚀 Como Rodar

### Pré-requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* SQL Server

### Passos

```bash
# Clonar repositório
git clone https://github.com/SEU-USUARIO/HamburguerApi.git
cd HamburguerApi

# Restaurar pacotes
dotnet restore

# Criar banco e aplicar migrações
dotnet ef database update

# Rodar aplicação
dotnet run
```

Acesse em: [http://localhost:5000](http://localhost:5000)

* Swagger: [http://localhost:5000/swagger](http://localhost:5000/swagger)
* Painel Web: [http://localhost:5000/index.html](http://localhost:5000/index.html)

---

## 📊 Exemplos de JSON

### Criar Hambúrguer

```json
{
  "nome": "Cheeseburger",
  "descricao": "Pão, carne, queijo",
  "preco": 25.90,
  "ativo": true
}
```

### Criar Cliente

```json
{
  "nome": "Guilherme Costa",
  "telefone": "11999999999",
  "email": "gui@email.com"
}
```

### Criar Pedido

```json
{
  "clienteId": 1,
  "itens": [
    { "hamburguerId": 1, "qtde": 2, "precoUnit": 25.90, "removerIngredientesIds": [1] }
  ],
  "extras": [
    { "tipo": "Bebida", "nome": "Coca-Cola Lata", "qtde": 2, "precoUnit": 6.50 }
  ]
}
```

---

## ✅ Próximos Passos

* [ ] Implementar autenticação (JWT)
* [ ] Relatórios de vendas
* [ ] Melhorias no painel web (React/Next.js futuramente)

---

## 👨‍💻 Autor

**Guilherme Costa Proença**
📌 Sistemas de Informação — FIAP
🚀 Apaixonado por tecnologia, IA e desenvolvimento fullstack
