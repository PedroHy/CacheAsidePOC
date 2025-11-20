# 📚 CacheAsidePOC

Este repositório contém uma POC criada para estudar, testar e entender como implementar o padrão de cache Cache-Aside em uma API .NET minimalista, utilizando:

- Redis como cache distribuído

- PostgreSQL como banco principal

- StackExchange.Redis como cliente Redis

- Minimal APIs (ASP.NET Core)

- Cache distribuído via IDistributedCache

O objetivo aqui não é construir um sistema completo, mas sim demonstrar na prática como o padrão Cache-Aside funciona dentro do ciclo de leitura e escrita de dados, e como integrar isso com Redis de forma simples.

## 🚀 Objetivo da POC
- Entender o fluxo básico do pattern Cache-Aside:

  - Leitura tenta o cache → Se não existir, consulta o banco, popula o cache e retorna.

  - Escrita atualiza o banco e invalida o cache relacionado.

- Criar uma estrutura mínima, mas funcional, para testar comportamento real.

- Explorar uso de Redis via IDistributedCache no ASP.NET Core.

- Analisar acertos e problemas comuns (cache miss, invalidação, timeout, etc.).

## 🧠 Tecnologias utilizadas

- .NET 8 / Minimal APIs

- StackExchange.Redis

- Redis (Docker)

- PostgreSQL (Docker)

- Dapper (DAO simples para consultas)

## 📦 Arquitetura da POC

A POC tem três partes principais:

1. Services

 - ```ClientesQueryService``` implementa Cache-Aside no método de listagem.

 - ```ClienteCreateService``` invalida o cache no fluxo de criação.

2. CachingService

 - Wrapper simples sobre IDistributedCache.

 - Responsável por Get, Set, Remove.

3. Minimal APIs

 - Endpoints para listar clientes e criar novos.

 - Usam os services que aplicam o padrão.

## 🔥 Como rodar localmente

Subir Redis + Postgres com Docker:

```bash
  docker compose up -d
```

Executar a API:

```bash 
  dotnet run
```

Acessar o Swagger: ```http://localhost:5000/swagger```

### 📌 Observação

Este projeto não é um template de produção — é uma POC voltada para estudo explícito do padrão Cache-Aside e de como integrá-lo com .NET, Redis e Postgres.
