# Plataforma de Inventário Comercial — Desafio Full Stack

Módulo de gestão de catálogo de produtos e categorias desenvolvido com **ASP.NET Core Web API (backend)** e **Nuxt 3 / Vue 3 (frontend)**, conectado ao banco de dados relacional **PostgreSQL** via **Entity Framework Core**.

---

## 🛠️ Tecnologias e Ferramentas

- **Backend**: .NET 9 (ASP.NET Core Web API)
- **Banco de Dados**: PostgreSQL 15 & Entity Framework Core (EF Core)
- **Frontend**: Nuxt 3 (Vue 3, Composition API, Axios)
- **Orquestração**: Docker & Docker Compose

---

## 📋 Pré-requisitos

Para executar o projeto localmente, certifique-se de possuir instalado:

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 20+](https://nodejs.org/)
- [PostgreSQL](https://www.postgresql.org/) (caso for executar o banco localmente)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (caso for executar via Docker Compose)

---

## 🐳 Execução via Docker Compose (Recomendado)

Esta é a maneira mais simples e rápida de rodar toda a aplicação (Banco de Dados + API + Frontend) sem precisar instalar os SDKs locais.

1. Na raiz do projeto, execute o comando:
   ```bash
   docker compose up --build
   ```
2. O Docker criará três containers:
   - **PostgreSQL**: Rodando na porta `5432`
   - **API (.NET)**: Rodando na porta `5265` (mapeada para a porta interna `8080`)
   - **Frontend (Nuxt 3)**: Rodando na porta `3000`
3. As migrations serão aplicadas automaticamente no banco de dados durante a inicialização do container da API.
4. Acesse o sistema em seu navegador:
   - **Frontend**: [http://localhost:3000](http://localhost:3000)
   - **API (Swagger/Endpoints)**: [http://localhost:5265/api/produtos](http://localhost:5265/api/produtos)

Para parar os containers e limpar os volumes:
```bash
docker compose down -v
```

---

## 💻 Execução Local (Manual)

### 1. Banco de Dados
Certifique-se de que o PostgreSQL está rodando localmente na porta padrão `5432` e configure uma base de dados chamada `zdzcode_db`.
* Caso prefira rodar apenas o banco de dados no Docker, você pode subir o serviço do PostgreSQL com:
  ```bash
  docker compose up postgres -d
  ```

### 2. Executando o Backend (.NET API)
1. Acesse o diretório da API:
   ```bash
   cd backend/src/ZDZCode.API
   ```
2. Restaure as dependências e aplique as migrations no banco:
   ```bash
   dotnet ef database update
   ```
3. Execute a API:
   ```bash
   dotnet run
   ```
   A API estará rodando por padrão em:
   - HTTP: `http://localhost:5265`
   - HTTPS: `https://localhost:7050`

### 3. Executando o Frontend (Nuxt 3)
1. Acesse o diretório do frontend:
   ```bash
   cd frontend
   ```
2. Instale as dependências:
   ```bash
   npm install
   ```
3. Inicie o servidor de desenvolvimento:
   ```bash
   npm run dev
   ```
   O frontend estará acessível em:
   - [http://localhost:3000](http://localhost:3000)

---

## ⚙️ Variáveis de Ambiente

As seguintes variáveis podem ser configuradas no arquivo `docker-compose.yml` ou exportadas no terminal:

| Variável | Descrição | Valor Padrão |
|----------|-----------|--------------|
| `ASPNETCORE_ENVIRONMENT` | Define o ambiente da API | `Development` (`Docker` no Docker Compose) |
| `AllowedOrigins__Nuxt` | Define a origem permitida para CORS | `http://localhost:3000` |
| `NUXT_PUBLIC_API_BASE` | Define a URL base da API para o frontend | `http://localhost:5265` |
