# TodoApi — ASP.NET Core + SQLite + Docker

API REST simples (CRUD de tarefas) em ASP.NET Core 8, com SQLite via EF Core
**Migrations**, empacotada em Docker, com os ficheiros do projeto e da base
de dados montados como **volumes** (persistem fora do container).

## Estrutura

```
.
├── .config/dotnet-tools.json         # tool manifest local (dotnet-ef)
├── .devcontainer/devcontainer.json   # ambiente de dev/debug dentro do Docker (F5 no VS Code)
├── .vscode/launch.json, tasks.json   # debug F5 no VS Code
├── src/TodoApi/
│   ├── Migrations/                   # EF Core Migrations
│   └── ...                           # código da API
├── data/                             # ficheiro app.db do modo produção
├── Dockerfile                        # 3 stages: build, runtime (prod), dev (hot reload)
├── docker-compose.yml                # modo produção
└── docker-compose.dev.yml            # modo desenvolvimento (hot reload)
```

## Três formas de correr o projeto

### 1) Produção — `docker-compose.yml`

Build otimizado (multi-stage), sem código fonte montado, só a base de dados
como volume.

```bash
docker compose up --build
```

### 2) Desenvolvimento com hot reload — `docker-compose.dev.yml`

Usa a imagem do SDK, corre `dotnet watch run` dentro do container, e monta
**todo o código fonte** (`src/TodoApi`) como volume — editas no host (VS
Code, Rider, etc.), o container deteta a alteração e recompila/reinicia
sozinho. Não é debug com breakpoints (para isso, vai à opção 3), mas é ótimo
para correr a API em loop rápido de edição.

```bash
docker compose -f docker-compose.dev.yml up --build
```

A primeira vez demora um pouco mais (restaura os pacotes NuGet para o
volume `nuget_cache`); as seguintes são rápidas.

### 3) Debug com breakpoints no VS Code — Dev Containers

Esta é a forma de ter "tudo dentro do Docker" e ainda debugar com
breakpoints como se fosse local.

**Pré-requisitos:** Docker a correr + extensão **Dev Containers**
(`ms-vscode-remote.remote-containers`) no VS Code.

1. Abre a pasta do projeto no VS Code.
2. `Ctrl+Shift+P` → **"Dev Containers: Reopen in Container"**.
   - Monta automaticamente o projeto como volume em `/workspace` dentro do
     container e corre `dotnet tool restore && dotnet restore`.
3. Painel **Run and Debug** (`Ctrl+Shift+D`) → **F5**
   (".NET Core Launch (TodoApi)").
4. Coloca breakpoints em qualquer `.cs` — funcionam normalmente.
5. API em `http://localhost:5000`, Swagger em `http://localhost:5000/swagger`.

## Swagger

O Swagger UI está sempre ativo (`/swagger`), independentemente do
`ASPNETCORE_ENVIRONMENT`. Antes só estava ativo em `Development`, e como o
`docker-compose.yml` de produção corre com `ASPNETCORE_ENVIRONMENT=Production`,
o Swagger dava 404. Se quiseres voltar a escondê-lo em produção, em
`Program.cs` troca:

```csharp
app.UseSwagger();
app.UseSwaggerUI();
```

por:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

## EF Core Migrations

A base de dados já não é criada com `EnsureCreated()` — agora usa
**Migrations** (pasta `src/TodoApi/Migrations`), aplicadas automaticamente
no arranque com `db.Database.Migrate()`. A migration inicial (`InitialCreate`)
já está incluída e cria a tabela `TodoItems`.

### Adicionar uma nova migration (quando alterares os modelos)

Dentro do Dev Container (ou em qualquer ambiente com o SDK do .NET instalado):

```bash
# só na primeira vez (instala o dotnet-ef como tool local do projeto)
dotnet tool restore

# depois de alterares um modelo / o DbContext:
cd src/TodoApi
dotnet ef migrations add NomeDaMigration -o Migrations

# (opcional — não é obrigatório, porque o Program.cs já chama Migrate()
# automaticamente no arranque da aplicação)
dotnet ef database update
```

> Nota: `dotnet ef` só funciona num ambiente com o .NET SDK completo — ou
> seja, dentro do Dev Container (opção 3) ou `docker-compose.dev.yml`
> (opção 2), nunca na imagem de produção (que só tem o runtime).

## Endpoints

| Método | Rota              | Descrição              |
|--------|-------------------|-------------------------|
| GET    | `/api/todo`       | Lista todas as tarefas |
| GET    | `/api/todo/{id}`  | Obtém uma tarefa       |
| POST   | `/api/todo`       | Cria uma tarefa        |
| PUT    | `/api/todo/{id}`  | Atualiza uma tarefa    |
| DELETE | `/api/todo/{id}`  | Remove uma tarefa      |

Exemplo de corpo para `POST /api/todo`:

```json
{
  "title": "Comprar café",
  "isComplete": false
}
```
