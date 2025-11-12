USE EstoqueEmurb
GO

INSERT INTO Pessoas (nome, cpf_cnpj, email, telefone) VALUES 
('Felipe Rughut', '111.111.111-11', 'felipe.rughut@emurb.com', '17-91111-1111'),
('Lucas Delamura', '222.222.222-22', 'lucas.delamura@emurb.com', '17-92222-2222'),
('Pedro Capelli', '333.333.333-33', 'pedro.capelli@emurb.com', '17-93333-3333'),
('João Candola', '444.444.444-44', 'joao.candola@emurb.com', '17-94444-4444');
GO
 
INSERT INTO Pessoas (nome, cpf_cnpj, email, telefone) VALUES 
('Construtudo Rio Preto', '12.345.678/0001-10', 'vendas@construtudo.com', '17-3211-1000'),
('LimpaFácil Higiene SJRP', '22.345.678/0001-20', 'contato@limpafacil.com', '17-3222-2000'),
('Cimentão Obras Rio Preto', '33.345.678/0001-30', 'cimento@cimento.com', '17-3233-3000');
GO
 
INSERT INTO Funcionarios (idFuncionario, cargo, setor) VALUES 
(1, 'Desenvolvedor Sr.', 'TI'),
(2, 'Gerente de TI', 'TI'),
(3, 'Almoxarife', 'Logística'),
(4, 'Coordenador', 'Administrativo');
GO
 
INSERT INTO Fornecedores (idFornecedor, inscricao_estadual) VALUES 
(5, '111.222.333.444'),
(6, '222.333.444.555'), 
(7, '333.444.555.666');
GO
 
INSERT INTO Produtos (nome, descricao) VALUES 
('Cimento CP-II 50kg', 'Saco de cimento Votoran para obras'),
('Vergalhão 10mm (1/4")', 'Barra de 12 metros para estrutura'),
('Tinta Branca Fosca 18L', 'Lata de tinta Suvinil para parede'),
('Papel Higiênico (Fardo)', 'Pacote com 64 rolos folha dupla'),
('Água Sanitária 5L', 'Galão de água sanitária Qboa'),
('Copo Descartável 200ml', 'Caixa com 1000 unidades');
GO
 
INSERT INTO OrdemEntrada (fornId, status, dataEnt) VALUES 
(7, 'Concluída', '2025-10-01'), 
(6, 'Concluída', '2025-10-02'),
(5, 'Aberta', '2025-10-05');  
GO
 
INSERT INTO Lote (produtoId, OrdEntId, qtd, preco, dataValidade) VALUES 
(1, 1, 100, 28.50, '2026-12-01');
GO
INSERT INTO Lote (produtoId, OrdEntId, qtd, preco) VALUES 
(4, 2, 50, 45.00), 
(5, 2, 30, 12.00); 
GO
INSERT INTO Lote (produtoId, OrdEntId, qtd, preco) VALUES 
(2, 3, 200, 40.00); 
GO
 
INSERT INTO Locais (nome, descricao) VALUES 
('Almoxarifado Central', 'Estoque principal da Emurb'),
('Obra Av. Bady Bassitt', 'Recapeamento da avenida'),
('Escritório Central', 'Materiais de consumo interno');
GO
 
INSERT INTO Autorizados (funcao) VALUES 
('Gerente de Obras'),
('Almoxarife Chefe'),
('Coordenador Administrativo');
GO
 
INSERT INTO Autorizacao (autorizadoId, localId) VALUES 
(2, 1), 
(1, 2), 
(3, 3), 
(1, 1); 
GO
 
INSERT INTO OrdemSaida (funcId, autorizaId, status, dataSaida) VALUES 
(3, 2, 'Concluída', '2025-10-10'),
(4, 3, 'Aberta', '2025-10-11');   
GO
 
INSERT INTO ItensOS (ordsaiId, loteId, qtd) VALUES 
(1, 1, 40), 
(1, 4, 100); 
GO
INSERT INTO ItensOS (ordsaiId, loteId, qtd) VALUES 
(2, 2, 5)
GO

INSERT INTO Usuarios (email, senha, funcionarioId, isAdmin, isAtivo) VALUES 
('felipe.rughut@emurb.com', 'senha123', 1, 1, 1),
('lucas.delamura@emurb.com', 'senha123', 2, 1, 1),
('pedro.capelli@emurb.com', 'senha123', 3, 0, 1),
('joao.candola@emurb.com', 'senha123', 4, 0, 1), 
('desativado@emurb.com', 'senha123', NULL, 0, 0);  
GO
 
INSERT INTO Estoque (produtoId, quantidadeAtual) VALUES 
(1, 60),
(2, 100),
(3, 0),
(4, 45),
(5, 30),
(6, 0);
GO