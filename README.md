# ReelfyAPI

ReelfyAPI é uma API desenvolvida em ASP.NET Core para autenticação e gerenciamento de usuários.

## Funcionalidades

- Registro de novos usuários
- Listagem de usuários cadastrados

## Endpoints

### GET `/api/AuthApi/GetAllUsers`

Retorna todos os usuários.

### POST `/api/AuthApi/Register`

Registra um novo usuário.

```json
{
  "email": "string",
  "age": 0,
  "password": "string"
}

```
## Como executar?
#### Clone o repositório:

```bash

git clone https://github.com/seu-usuario/reelfyapi.git
cd reelfyapi
```
