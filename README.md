# Projeto Wake Commerce

### resumo
```
O Projeto Wake Commerce é uma API REST desenvolvida em .NET6 para gerenciamento de produtos. Ela permite listar, buscar, criar, atualizar e excluir produtos de um banco de dados.
```
### Tecnologias Utilizadas
```
.NET 6

ASP.NET Core

Entity Framework Core

SQL Server

xUnit

Moq
```
### Configuração do Projeto
```
1. Clonar o Repositório

git clone https://github.com/seuusuario/WakeCommerceAPI.git
cd WakeCommerceAPI

2. Configurar o Banco de Dados

Edite o arquivo appsettings.json e configure a string de conexão para o banco de dados:

"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=WakeCommerceDB;User Id=SEU_USUARIO;Password=SUA_SENHA;"
}

3. Executar Migrações do Banco de Dados
dotnet ef migrations add InitialCreate 
dotnet ef database update

4. Executar a API

Pode ser compilada dentro do visual studio ou utilizando o comando CLI dotnet run

A API estará disponível em https://localhost:5001 ou http://localhost:5000.
```

### Testes
```
Executar Testes Unitários e de Integração

Pode ser compilado pelo visual studio ou utilizando o comando CLI dotnet test

Se ocorrer erro relacionado ao arquivo testhost.deps.json, certifique-se de adicionar PreserveCompilationContext ao arquivo .csproj:

<PropertyGroup>
  <PreserveCompilationContext>true</PreserveCompilationContext>
</PropertyGroup>
```