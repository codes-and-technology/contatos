# Contatos

Este é um projeto de exemplo para demonstrar como criar uma aplicação de gerenciamento de contatos utilizando a tecnologia .NET.

## Instalação

1. Clone o repositório:

git clone https://github.com/danielmorine/contatos.git

2. Instale as dependências:

cd contatos
dotnet restore

## Configuração do Banco de Dados

Este projeto utiliza o Entity Framework Core para gerenciar o banco de dados. Para criar e atualizar o banco de dados, siga as instruções abaixo:

### Criar Migration (Caso alterado objetos do banco de dados )

Para criar uma migração, execute o seguinte comando no Package Manager Console:

Add-Migration {NOME_DO_MIGRATION} -StartupProject RegionalContacts.Infrastructure

Isso criará uma nova migração com o nome {NOME_DO_MIGRATION} com base no contexto do banco de dados configurado no projeto de infraestrutura.

### Atualizar Banco de Dados

Para aplicar as migrações e atualizar o banco de dados, execute o seguinte comando no Package Manager Console:

Update-Database {NOME_MIGRATION} -StartupProject RegionalContacts.Infrastructure -Connection "Data Source=localhost;Initial Catalog=Fiap_Fase1_TechChallenge_Contatos;User ID={USUARIO};Password={SENHA};Trust Server Certificate=True;"

Isso aplicará todas as migrações pendentes e atualizará o banco de dados para a versão mais recente.

## Execução 

No Visual Studio, clique em Docker-Compose


Isso iniciará todos os serviços necessários para o projeto, incluindo o banco de dados e a aplicação.

