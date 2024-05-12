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

### Criar Migration

Para criar uma migração, execute o seguinte comando no Package Manager Console:

Add-Migration CriacaoTabelas -StartupProject RegionalContacts.Infrastructure

Isso criará uma nova migração com o nome "CriacaoTabelas" com base no contexto do banco de dados configurado no projeto de infraestrutura.

### Atualizar Banco de Dados

Para aplicar as migrações e atualizar o banco de dados, execute o seguinte comando no Package Manager Console:

Update-Database CriacaoTabelas -StartupProject RegionalContacts.Infrastructure -Connection "Data Source=localhost;Initial Catalog=Fiap_Fase1_TechChallenge_Contatos;User ID=sa;Password=sql@123456;Trust Server Certificate=True;"

Isso aplicará todas as migrações pendentes e atualizará o banco de dados para a versão mais recente.

## Execução em Ambiente Docker

Este projeto é configurado para ser executado em um ambiente Docker Windows no modelo Linux. Certifique-se de ter o Docker instalado e em execução na sua máquina. Você pode iniciar o ambiente Docker executando o seguinte comando:

docker-compose up

Isso iniciará todos os serviços necessários para o projeto, incluindo o banco de dados e a aplicação.

## Contribuição

Contribuições são bem-vindas! Se você deseja contribuir para este projeto, siga estas etapas:

1. Faça um fork do projeto.
2. Crie uma branch para a sua nova funcionalidade (git checko
