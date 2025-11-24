USE EstoqueEmurb
GO

create function getPessoa (@cpf_cnpj varchar(20))
returns int
as
begin
	return (select idPessoa from pessoas where cpf_cnpj = @cpf_cnpj)
end
go


create procedure cadFuncionario
(
	@nome     VARCHAR(75),                          
    @cpf_cnpj VARCHAR(20),                
    @email    VARCHAR(50),                           
    @telefone VARCHAR(20),
	@cargo    VARCHAR(50),
    @setor    VARCHAR(50)
)
as
begin
	IF EXISTS (SELECT 1 FROM Pessoas WHERE email = @email)
    BEGIN
        RETURN 3; 
    END
	begin try
		declare @codigo int
		begin tran
		set @codigo = isnull(dbo.getPessoa(@cpf_cnpj), 0)
		if @codigo = 0 
		begin
			insert into pessoas values (@nome, @cpf_cnpj, @email, @telefone)
			insert into funcionarios values (SCOPE_IDENTITY(), @cargo, @setor)
			commit 
			return 0 
		end
		else
		begin
			if not exists (select * from funcionarios where idFuncionario = @codigo)
			begin
				insert into funcionarios values (@codigo, @cargo, @setor)
				commit 
				return 0 
			end
			else
			begin
				rollback 
				return 1 
			end 
		end 
	end try 
	begin catch
		rollback
		raiserror ('Problemas no cadastro do cliente', 16, 1)
		return 2 
	end catch
end
go

create PROCEDURE alterFuncionario
(
    @idFuncionario INT,
    @nome VARCHAR(75),
    @cpf_cnpj VARCHAR(20),
    @email VARCHAR(50),
    @telefone VARCHAR(20),
    @cargo VARCHAR(50),
    @setor VARCHAR(50)
)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Pessoas WHERE email = @email AND idPessoa != @idFuncionario)
    BEGIN
        RETURN 3 
    END
    SET NOCOUNT ON
    BEGIN TRY
        BEGIN TRAN
        UPDATE Pessoas SET
            nome = @nome,
            cpf_cnpj = @cpf_cnpj,
            email = @email,
            telefone = @telefone
        WHERE idPessoa = @idFuncionario;
        UPDATE Funcionarios SET
            cargo = @cargo,
            setor = @setor
        WHERE idFuncionario = @idFuncionario
        COMMIT 
		RETURN 0
    END TRY
    BEGIN CATCH
        ROLLBACK 
        RAISERROR ('Problemas no cadastro do cliente', 16, 1)
		RETURN 2
    END CATCH
END
GO

CREATE PROCEDURE delFuncionario
(
    @idFuncionario INT
)
AS
BEGIN
    SET NOCOUNT ON
    IF EXISTS (SELECT 1 FROM OrdemSaida WHERE funcId = @idFuncionario) OR
       EXISTS (SELECT 1 FROM Usuarios WHERE funcionarioId = @idFuncionario)
    BEGIN
        RAISERROR('Este funcionário não pode ser excluído, pois está vinculado a Ordens de Saída ou é um Usuário do sistema.', 16, 1);
        RETURN 1 
    END
    BEGIN TRY
        BEGIN TRAN
        DELETE FROM Funcionarios
        WHERE idFuncionario = @idFuncionario
        DELETE FROM Pessoas
        WHERE idPessoa = @idFuncionario

        COMMIT TRAN
        RETURN 0
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        RAISERROR ('Problemas ao excluir o funcionário.', 16, 1)
        RETURN 2 
    END CATCH
END
GO

alter procedure cadFornecedor
(
    @nome     VARCHAR(75),                          
    @cpf_cnpj VARCHAR(20),                
    @email    VARCHAR(50),                           
    @telefone VARCHAR(20),
    @inscricao_estadual VARCHAR (20)
)
as
begin
	begin try

		declare @codigo int
		begin tran 
		set @codigo = isnull(dbo.getPessoa(@cpf_cnpj), 0)
		if @codigo = 0
		begin
			insert into pessoas values (@nome, @cpf_cnpj, @email, @telefone)
			insert into fornecedores values (SCOPE_IDENTITY(), @inscricao_estadual)
			commit 
			return 0 
		end
		else
		begin
			if not exists (select * from fornecedores where idFornecedor = @codigo)
			begin
				insert into fornecedores values (@codigo, @inscricao_estadual)
				commit 
				return 0 
			end
			else
			begin
				rollback 
				return 1
			end 
		end 
	end try 
	begin catch
		rollback
		return 2 
	end catch
end
go

CREATE PROCEDURE alterFornecedor
(
    @idFornecedor INT,
    @nome VARCHAR(75),
    @cpf_cnpj VARCHAR(20),
    @email VARCHAR(50),
    @telefone VARCHAR(20),
    @inscricao_estadual VARCHAR (20)
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Pessoas WHERE email = @email AND idPessoa != @idFornecedor)
    BEGIN
        RETURN 3
    END
    SET NOCOUNT ON
    BEGIN TRY
        BEGIN TRAN
        UPDATE Pessoas SET
            nome = @nome,
            cpf_cnpj = @cpf_cnpj,
            email = @email,
            telefone = @telefone
        WHERE idPessoa = @idFornecedor;
        UPDATE Fornecedores SET
            inscricao_estadual = @inscricao_estadual
        WHERE idFornecedor = @idFornecedor
        COMMIT 
	RETURN 0
    END TRY
    BEGIN CATCH
        ROLLBACK 
		RETURN 2
    END CATCH
END
GO

CREATE PROCEDURE delFornecedor
(
    @idFornecedor INT
)
AS
BEGIN
    SET NOCOUNT ON
    IF EXISTS (SELECT 1 FROM OrdemEntrada WHERE fornId = @idFornecedor)
    BEGIN
        RETURN 1
    END
    BEGIN TRY
        BEGIN TRAN
        DELETE FROM Fornecedores
        WHERE idFornecedor = @idFornecedor
        DELETE FROM Pessoas
        WHERE idPessoa = @idFornecedor
        COMMIT TRAN
        RETURN 0 
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN
        RETURN 2 
    END CATCH
END
GO

CREATE TRIGGER tr_lote
ON Lote 
FOR INSERT 
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProdutoId INT;
    DECLARE @QuantidadeInserida INT;
    SELECT @ProdutoId = produtoId, @QuantidadeInserida = qtd
    FROM inserted;
    IF EXISTS (SELECT 1 FROM Estoque WHERE produtoId = @ProdutoId)
    BEGIN
        UPDATE Estoque
        SET quantidadeAtual = quantidadeAtual + @QuantidadeInserida
        WHERE produtoId = @ProdutoId;
    END
    ELSE
    BEGIN
        INSERT INTO Estoque (produtoId, quantidadeAtual)
        VALUES (@ProdutoId, @QuantidadeInserida);
    END
END
GO

CREATE TRIGGER tr_ItensOS
ON ItensOS 
FOR INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ProdutoId INT;
    DECLARE @QuantidadeSaida INT;
    SELECT 
        @QuantidadeSaida = i.qtd,
        @ProdutoId = l.produtoId
    FROM 
        inserted i
    JOIN 
        Lote l ON i.loteId = l.idLote;
    BEGIN TRY
        UPDATE Estoque
        SET quantidadeAtual = quantidadeAtual - @QuantidadeSaida
        WHERE produtoId = @ProdutoId;
    END TRY
    BEGIN CATCH
        ROLLBACK
        RAISERROR ('Erro: Saldo de estoque insuficiente para esta saída. A operação foi cancelada.', 16, 1);
    END CATCH
END
GO

CREATE FUNCTION GetPessoaNome
(
    @IdPessoa INT
)
RETURNS VARCHAR(75)
AS
BEGIN
    DECLARE @Nome VARCHAR(75)
    SELECT @Nome = nome FROM Pessoas WHERE idPessoa = @IdPessoa
    RETURN @Nome
END
GO

CREATE FUNCTION GetUsuarioAdmin
(
    @IdUsuario INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @IsAdmin BIT
    SELECT @IsAdmin = isAdmin FROM Usuarios WHERE idUsuario = @IdUsuario
    RETURN @IsAdmin
END
GO

CREATE TRIGGER tr_ItensOS_Delete
ON ItensOS 
FOR DELETE 
AS
BEGIN
    SET NOCOUNT ON
    DECLARE @ProdutoId INT
    DECLARE @QuantidadeDevolvida INT
    SELECT 
        @QuantidadeDevolvida = d.qtd,
        @ProdutoId = l.produtoId
    FROM 
        deleted d
    JOIN 
        Lote l ON d.loteId = l.idLote
    UPDATE Estoque
    SET quantidadeAtual = quantidadeAtual + @QuantidadeDevolvida
    WHERE produtoId = @ProdutoId
END
GO

CREATE FUNCTION GetLotesDisponiveis ()
RETURNS TABLE
AS
RETURN
(
    SELECT 
        l.idLote,
        l.produtoId,
        p.nome NomeProduto,
        l.qtd QtdEntradaOriginal,
        dbo.GetSaldoLote(l.idLote) SaldoAtual 
    FROM Lote l
    JOIN Produtos p ON l.produtoId = p.idProduto
    WHERE dbo.GetSaldoLote(l.idLote) > 0
)
GO

CREATE FUNCTION GetEstoqueAtual () 
RETURNS TABLE 
AS 
RETURN 
( 
    SELECT  
        e.produtoId AS ID_Produto, 
        p.nome AS NomeProduto,
        e.quantidadeAtual AS SaldoAtual
    FROM 
        Estoque e 
    JOIN 
        Produtos p ON e.produtoId = p.idProduto
    WHERE 
        e.quantidadeAtual > 0 
) 
GO 


