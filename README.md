# SafeNetAPI

A API integra um controle de usuário e um salvamento em banco das requisições utilizadas pela IA do repositório https://github.com/ViniCosta1/saap-ai-model-unimar, salvando em um banco de dados SQL Server.

## Tecnologias

ASP.NET Core 8

Entity Framework Core

SQL Server 

ASP.NET Identity

Swagger

## Endpoints das requisições

```bash
POST /api/CreateRequest -> salva os dados de uma requisição no banco
GET /api/ListRequest -> lista todas as requisições salvas com base no usuário
```

## Endpoints dos usuários

```bash
POST /registrar ->  cria um usuário e junto um token
POST /login -> entra no sistema
```

# Como utilizar

```bash
# Clone o repositório
git clone https://github.com/ViniCarvalho71/SafeNetAPI/
cd SafeNetAPI

# Configure o appsettings.json com sua connection string, se necessário

# Restaure os pacotes
dotnet restore

# Rode as migrações
dotnet ef database update

# Rode a aplicação
dotnet run
```

*Para utilizar qualquer endpoint caso não esteja autenticado. Adicione no Authorization: Bearer {seu_token} na requisição para utilizá-la de forma remota em seu código

