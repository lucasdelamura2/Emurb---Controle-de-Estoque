CREATE DATABASE EstoqueEmurb
GO

USE EstoqueEmurb
GO

CREATE TABLE Pessoas
(
    idPessoa INT          NOT NULL PRIMARY KEY IDENTITY,
    nome     VARCHAR(75) NOT NULL,                          
    cpf_cnpj VARCHAR(20)  NOT NULL UNIQUE,                
    email    VARCHAR(50) NOT NULL  UNIQUE,                           
    telefone VARCHAR(20)  NOT NULL                       
)
GO

CREATE TABLE Funcionarios
(
    idFuncionario INT         NOT NULL PRIMARY KEY, 
    cargo         VARCHAR(50) NOT NULL,
    setor         VARCHAR(50) NOT NULL,
    FOREIGN KEY (idFuncionario) REFERENCES Pessoas(idPessoa) 
)
GO

CREATE TABLE Fornecedores
(
    idFornecedor       INT         NOT NULL PRIMARY KEY, 
    inscricao_estadual VARCHAR(20) NOT NULL,
    FOREIGN KEY (idFornecedor) REFERENCES Pessoas(idPessoa) 
)
GO

CREATE TABLE Produtos
(
    idProduto INT          NOT NULL PRIMARY KEY IDENTITY,
    nome      VARCHAR(100) NOT NULL UNIQUE, 
    descricao VARCHAR(255) NULL             
)
GO

CREATE TABLE OrdemEntrada
(
    idOrdEnt INT      NOT NULL PRIMARY KEY IDENTITY,
    fornId   INT      NOT NULL,
    dataEnt  DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (fornId) REFERENCES Fornecedores(idFornecedor),
    status VARCHAR(20) NOT NULL DEFAULT 'Aberta',
    CONSTRAINT CK_OrdemEntrada_Status CHECK (status IN ('Aberta', 'Concluída'))
)
GO

CREATE TABLE Lote
(
    idLote       INT           NOT NULL PRIMARY KEY IDENTITY,
    produtoId    INT           NOT NULL,
    OrdEntId     INT           NOT NULL,
    qtd          INT           NOT NULL CHECK (qtd > 0),
    preco        MONEY         NOT NULL CHECK (preco >= 0), 
    dataValidade DATETIME      NULL, 
    CONSTRAINT FK_Lote_Produtos FOREIGN KEY (produtoId) REFERENCES Produtos(idProduto),
    CONSTRAINT FK_Lote_OrdemEntrada FOREIGN KEY (OrdEntId) REFERENCES OrdemEntrada(idOrdEnt)
)
GO

CREATE TABLE Locais
(
    idLocal   INT          NOT NULL PRIMARY KEY IDENTITY,
    nome      VARCHAR(100) NOT NULL UNIQUE, 
    descricao VARCHAR(255) NULL
)
GO

CREATE TABLE Autorizados
(
    idAutorizado INT         NOT NULL PRIMARY KEY IDENTITY,
    funcao       VARCHAR(50) NOT NULL UNIQUE 
)
GO

CREATE TABLE Autorizacao
(
    idAutoriza   INT NOT NULL PRIMARY KEY IDENTITY, 
    autorizadoId INT NOT NULL,
    localId      INT NOT NULL,
    CONSTRAINT FK_Autorizacao_Autorizados FOREIGN KEY (autorizadoId) REFERENCES Autorizados(idAutorizado),
    CONSTRAINT FK_Autorizacao_Locais FOREIGN KEY (localId) REFERENCES Locais(idLocal),
    CONSTRAINT UQ_Autorizacao_AutorizadoLocal UNIQUE (autorizadoId, localId)
)
GO

CREATE TABLE OrdemSaida
(
    idOrdSai   INT      NOT NULL PRIMARY KEY IDENTITY,
    funcId     INT      NOT NULL, 
    autorizaId INT      NOT NULL,
    dataSaida  DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT FK_OrdemSaida_Funcionarios FOREIGN KEY (funcId) REFERENCES Pessoas(idPessoa), 
    CONSTRAINT FK_OrdemSaida_Autorizacao FOREIGN KEY (autorizaId) REFERENCES Autorizacao(idAutoriza),
    status VARCHAR(20) NOT NULL DEFAULT 'Aberta',
    CONSTRAINT CK_OrdemSaida_Status CHECK (status IN ('Aberta', 'Concluída'))
)
GO

CREATE TABLE ItensOS
(
    idItemOS INT NOT NULL PRIMARY KEY IDENTITY,
    ordsaiId INT NOT NULL,
    loteId   INT NOT NULL,
    qtd      INT NOT NULL CHECK (qtd > 0), 
    CONSTRAINT FK_ItensOS_OrdemSaida FOREIGN KEY (ordsaiId) REFERENCES OrdemSaida(idOrdSai),
    CONSTRAINT FK_ItensOS_Lote FOREIGN KEY (loteId) REFERENCES Lote(idLote),
)
GO

CREATE TABLE Usuarios
(
    idUsuario    INT           NOT NULL PRIMARY KEY IDENTITY,
    email        VARCHAR(100)  NOT NULL UNIQUE, 
    senha    VARCHAR(25)  NOT NULL,        
    funcionarioId INT          NULL,            
    isAdmin      BIT           NOT NULL DEFAULT 0, 
    isAtivo      BIT           NOT NULL DEFAULT 1, 
    CONSTRAINT FK_Usuarios_Pessoas FOREIGN KEY (funcionarioId) REFERENCES Pessoas(idPessoa)
)
GO

CREATE TABLE Estoque
(
    idEstoque       INT NOT NULL PRIMARY KEY IDENTITY,
    produtoId       INT NOT NULL UNIQUE, 
    quantidadeAtual INT NOT NULL DEFAULT 0 CHECK (quantidadeAtual >= 0), 
    CONSTRAINT FK_Estoque_Produtos FOREIGN KEY (produtoId) REFERENCES Produtos(idProduto)
)

GO
