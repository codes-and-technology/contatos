--DROP DATABASE Fiap_Fase1_TechChallenge_Contatos;
CREATE DATABASE Fiap_Fase1_TechChallenge_Contatos
GO
USE Fiap_Fase1_TechChallenge_Contatos;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [TelefoneRegiao] (
    [Id] uniqueidentifier NOT NULL,
    [CodigoArea] smallint NOT NULL,
    [DataCriacao] DATETIME NOT NULL,
    CONSTRAINT [PK_TelefoneRegiao] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Contato] (
    [Id] uniqueidentifier NOT NULL,
    [Nome] VARCHAR(250) NOT NULL,
    [NumeroTelefone] VARCHAR(9) NOT NULL,
    [Email] VARCHAR(250) NOT NULL,
    [PhoneRegionId] uniqueidentifier NULL,
    [DataCriacao] DATETIME NOT NULL,
    CONSTRAINT [PK_Contato] PRIMARY KEY ([Id]),
    CONSTRAINT [IdArea] FOREIGN KEY ([PhoneRegionId]) REFERENCES [TelefoneRegiao] ([Id])
);
GO

CREATE INDEX [IX_Contato_PhoneRegionId] ON [Contato] ([PhoneRegionId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241110150038_InitialCreate', N'8.0.10');
GO

COMMIT;
GO
