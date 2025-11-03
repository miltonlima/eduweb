using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcSaed.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserir modalidades iniciais com sobreposição eficiente
            migrationBuilder.Sql(@"
                INSERT INTO Modalidade (Id, Nome) VALUES 
                (1, 'Futebol'),
                (2, 'Basquete'),
                (3, 'Vôlei'),
                (4, 'Natação'),
                (5, 'Tênis'),
                (6, 'Atletismo'),
                (7, 'Judô'),
                (8, 'Karatê'),
                (9, 'Taekwondo'),
                (10, 'Boxe'),
                (11, 'Muay Thai'),
                (12, 'Jiu-Jitsu'),
                (13, 'Ginástica Artística'),
                (14, 'Ginástica Rítmica'),
                (15, 'Pilates'),
                (16, 'Yoga'),
                (17, 'Dança'),
                (18, 'Ballet'),
                (19, 'Capoeira'),
                (20, 'Xadrez'),
                (21, 'Ping Pong'),
                (22, 'Badminton'),
                (23, 'Handebol'),
                (24, 'Futsal'),
                (25, 'Ciclismo'),
                (26, 'Corrida')
                ON DUPLICATE KEY UPDATE 
                Nome = VALUES(Nome);
            ");

            // Inserir pessoas iniciais com sobreposição eficiente
            migrationBuilder.Sql(@"
                INSERT INTO Pessoa (Id, Nome, Nascimento, Cpf, Email, Matricula) VALUES 
                (1, 'Milton Francisco Mascara Lima', '1986-05-15', '113.009.827-30', 'milton.mlima@gmail.com', '202500000000001'),
                (2, 'Karolina Bastos Carvalho Lima', '1984-05-15', '097.635.197-80', 'karolbastos@gmail.com', '202500000000002'),
                (3, 'João Silva Santos', '1990-05-15', '123.456.789-01', 'joao.silva@email.com', '202500000000003'),
                (4, 'Maria Santos Oliveira', '1985-08-22', '987.654.321-02', 'maria.santos@email.com', '202500000000004'),
                (5, 'Pedro Oliveira Costa', '1992-03-10', '456.789.123-03', 'pedro.oliveira@email.com', '202500000000005'),
                (6, 'Ana Costa Ferreira', '1988-12-05', '789.123.456-04', 'ana.costa@email.com', '202500000000006'),
                (7, 'Carlos Ferreira Lima', '1995-07-18', '321.654.987-05', 'carlos.ferreira@email.com', '202500000000007'),
                (8, 'Fernanda Almeida Souza', '1991-03-25', '111.222.333-44', 'fernanda.almeida@email.com', '202500000000008'),
                (9, 'Roberto Silva Machado', '1987-11-12', '555.666.777-88', 'roberto.silva@email.com', '202500000000009'),
                (10, 'Juliana Pereira Castro', '1993-09-08', '999.888.777-66', 'juliana.pereira@email.com', '202500000000010'),
                (11, 'Marcos Antonio Ribeiro', '1989-01-30', '444.333.222-11', 'marcos.antonio@email.com', '202500000000011'),
                (12, 'Larissa Martins Vieira', '1994-06-14', '777.888.999-00', 'larissa.martins@email.com', '202500000000012'),
                (13, 'Diego Henrique Barbosa', '1986-04-22', '123.987.456-78', 'diego.henrique@email.com', '202500000000013'),
                (14, 'Camila Rodrigues Nascimento', '1992-12-03', '456.123.789-01', 'camila.rodrigues@email.com', '202500000000014'),
                (15, 'Thiago Santos Lopes', '1988-08-17', '789.456.123-45', 'thiago.santos@email.com', '202500000000015'),
                (16, 'Patricia Lima Cardoso', '1990-10-09', '654.321.987-32', 'patricia.lima@email.com', '202500000000016'),
                (17, 'Anderson Silva Moreira', '1985-02-28', '321.654.987-65', 'anderson.silva@email.com', '202500000000017'),
                (18, 'Vanessa Costa Andrade', '1996-07-11', '987.321.654-98', 'vanessa.costa@email.com', '202500000000018'),
                (19, 'Bruno Oliveira Freitas', '1991-05-04', '147.258.369-74', 'bruno.oliveira@email.com', '202500000000019'),
                (20, 'Isabela Ferreira Rocha', '1993-03-16', '963.852.741-85', 'isabela.ferreira@email.com', '202500000000020'),
                (21, 'Rafael Mendes Carvalho', '1989-09-23', '234.567.890-12', 'rafael.mendes@email.com', '202500000000021'),
                (22, 'Beatriz Sousa Pereira', '1994-11-07', '345.678.901-23', 'beatriz.sousa@email.com', '202500000000022'),
                (23, 'Lucas Henrique Santos', '1987-01-12', '456.789.012-34', 'lucas.henrique@email.com', '202500000000023'),
                (24, 'Gabriela Silva Costa', '1995-08-25', '567.890.123-45', 'gabriela.silva@email.com', '202500000000024'),
                (25, 'Felipe Rodrigues Lima', '1990-11-18', '678.901.234-56', 'felipe.rodrigues@email.com', '202500000000025'),
                (26, 'Amanda Pereira Dias', '1988-04-07', '789.012.345-67', 'amanda.pereira@email.com', '202500000000026'),
                (27, 'Rodrigo Alves Mendes', '1992-09-14', '890.123.456-78', 'rodrigo.alves@email.com', '202500000000027'),
                (28, 'Priscila Santos Barros', '1991-12-02', '901.234.567-89', 'priscila.santos@email.com', '202500000000028'),
                (29, 'Gustavo Lima Ferreira', '1986-06-29', '012.345.678-90', 'gustavo.lima@email.com', '202500000000029'),
                (30, 'Natália Costa Ribeiro', '1993-03-21', '123.456.789-01', 'natalia.costa@email.com', '202500000000030'),
                (31, 'Eduardo Silva Machado', '1989-10-15', '234.567.890-12', 'eduardo.silva@email.com', '202500000000031'),
                (32, 'Renata Oliveira Castro', '1994-07-08', '345.678.901-23', 'renata.oliveira@email.com', '202500000000032'),
                (33, 'Vinicius Pereira Lopes', '1987-02-24', '456.789.012-34', 'vinicius.pereira@email.com', '202500000000033'),
                (34, 'Carolina Ferreira Rocha', '1996-05-17', '567.890.123-45', 'carolina.ferreira@email.com', '202500000000034'),
                (35, 'Marcelo Santos Vieira', '1985-12-11', '678.901.234-56', 'marcelo.santos@email.com', '202500000000035'),
                (36, 'Letícia Almeida Cardoso', '1992-08-03', '789.012.345-67', 'leticia.almeida@email.com', '202500000000036'),
                (37, 'André Costa Nascimento', '1990-01-26', '890.123.456-78', 'andre.costa@email.com', '202500000000037'),
                (38, 'Jéssica Lima Andrade', '1988-11-19', '901.234.567-89', 'jessica.lima@email.com', '202500000000038'),
                (39, 'Daniel Rodrigues Freitas', '1991-04-12', '012.345.678-90', 'daniel.rodrigues@email.com', '202500000000039'),
                (40, 'Tatiane Silva Moreira', '1993-09-05', '123.456.789-01', 'tatiane.silva@email.com', '202500000000040'),
                (41, 'Henrique Oliveira Barbosa', '1986-06-28', '234.567.890-12', 'henrique.oliveira@email.com', '202500000000041'),
                (42, 'Aline Pereira Carvalho', '1995-03-13', '345.678.901-23', 'aline.pereira@email.com', '202500000000042')
                ON DUPLICATE KEY UPDATE 
                Nome = VALUES(Nome),
                Nascimento = VALUES(Nascimento),
                Cpf = VALUES(Cpf),
                Email = VALUES(Email),
                Matricula = VALUES(Matricula);
            ");

            // Inserir unidades iniciais com sobreposição eficiente
            migrationBuilder.Sql(@"
                INSERT INTO Unidade (Id, Nome, Descricao, Ativa, Endereco, TurmaId) VALUES 
                (1, 'Centro Esportivo Central', 'Unidade principal com quadras poliesportivas', 1, 'Rua das Flores, 123 - Centro', 1),
                (2, 'Complexo Aquático Norte', 'Unidade especializada em natação e esportes aquáticos', 1, 'Av. Atlântica, 456 - Zona Norte', 3),
                (3, 'Academia de Lutas Sul', 'Unidade focada em artes marciais e lutas', 1, 'Rua dos Guerreiros, 789 - Zona Sul', 2),
                (4, 'Ginásio Poliesportivo Leste', 'Grande ginásio para esportes coletivos', 1, 'Av. dos Esportes, 321 - Zona Leste', 4),
                (5, 'Centro de Bem-Estar Oeste', 'Unidade para atividades de relaxamento e condicionamento', 1, 'Rua da Paz, 654 - Zona Oeste', 1),
                (6, 'Quadra Descoberta Parque', 'Espaço ao ar livre para diversos esportes', 1, 'Parque Municipal s/n - Centro', 2),
                (7, 'Sala de Dança e Ginástica', 'Espaço dedicado à dança e ginástica artística', 1, 'Rua Artística, 987 - Bairro Cultural', 3),
                (8, 'Campo de Futebol Sociedade', 'Campo oficial para futebol e futsal', 1, 'Estrada do Futebol, 135 - Periferia', 4),
                (9, 'Estúdio de Yoga e Pilates', 'Ambiente tranquilo para práticas meditativas', 1, 'Alameda Zen, 246 - Jardim Botânico', 1),
                (10, 'Pista de Atletismo Municipal', 'Pista oficial para corrida e atletismo', 1, 'Complexo Esportivo Municipal - Av. Olímpica', 2)
                ON DUPLICATE KEY UPDATE 
                Nome = VALUES(Nome),
                Descricao = VALUES(Descricao),
                Ativa = VALUES(Ativa),
                Endereco = VALUES(Endereco),
                TurmaId = VALUES(TurmaId);
            ");

            // Inserir turmas iniciais com sobreposição eficiente
            migrationBuilder.Sql(@"
                INSERT INTO Turma (Id, Nome, Descricao, DataInicio, DataFim, Status, DataCriacao, UnidadeId) VALUES 
                (1, 'Futebol Iniciante', 'Turma para iniciantes no futebol', '2024-02-01', '2024-06-30', 1, NOW(), 1),
                (2, 'Basquete Avançado', 'Turma avançada de basquete', '2024-03-01', '2024-07-31', 1, NOW(), 4),
                (3, 'Natação Intermediário', 'Turma intermediária de natação', '2024-01-15', '2024-05-15', 1, NOW(), 2),
                (4, 'Vôlei Feminino', 'Turma de vôlei exclusiva feminina', '2024-04-01', '2024-08-31', 1, NOW(), 4),
                (5, 'Judô Infantil', 'Turma de judô para crianças de 7 a 12 anos', '2024-02-15', '2024-07-15', 1, NOW(), 3),
                (6, 'Karatê Adulto', 'Turma de karatê para adultos iniciantes', '2024-03-10', '2024-08-10', 1, NOW(), 3),
                (7, 'Taekwondo Graduação', 'Turma para graduados em taekwondo', '2024-01-20', '2024-06-20', 1, NOW(), 3),
                (8, 'Boxe Feminino', 'Turma de boxe exclusiva para mulheres', '2024-04-05', '2024-09-05', 1, NOW(), 3),
                (9, 'Muay Thai Avançado', 'Turma avançada de muay thai', '2024-02-28', '2024-07-28', 1, NOW(), 3),
                (10, 'Jiu-Jitsu Básico', 'Turma básica de jiu-jitsu brasileiro', '2024-03-15', '2024-08-15', 1, NOW(), 3),
                (11, 'Ginástica Artística Infantil', 'Ginástica artística para crianças', '2024-01-30', '2024-06-30', 1, NOW(), 7),
                (12, 'Ginástica Rítmica Juvenil', 'Ginástica rítmica para adolescentes', '2024-02-10', '2024-07-10', 1, NOW(), 7),
                (13, 'Pilates Terapêutico', 'Pilates focado em reabilitação', '2024-03-20', '2024-08-20', 1, NOW(), 9),
                (14, 'Yoga Matinal', 'Sessões de yoga pela manhã', '2024-01-05', '2024-06-05', 1, NOW(), 9),
                (15, 'Dança Contemporânea', 'Turma de dança contemporânea', '2024-04-12', '2024-09-12', 1, NOW(), 7),
                (16, 'Ballet Clássico', 'Ballet clássico para todas as idades', '2024-02-22', '2024-07-22', 1, NOW(), 7),
                (17, 'Capoeira Regional', 'Capoeira estilo regional', '2024-03-05', '2024-08-05', 1, NOW(), 6),
                (18, 'Xadrez Escolar', 'Turma de xadrez para estudantes', '2024-01-12', '2024-06-12', 1, NOW(), 5),
                (19, 'Ping Pong Competitivo', 'Treinamento competitivo de tênis de mesa', '2024-04-18', '2024-09-18', 1, NOW(), 5),
                (20, 'Badminton Duplas', 'Turma focada em jogos de duplas', '2024-02-08', '2024-07-08', 1, NOW(), 4),
                (21, 'Handebol Masculino', 'Handebol para equipe masculina', '2024-03-25', '2024-08-25', 1, NOW(), 4),
                (22, 'Futsal Feminino', 'Futsal exclusivo feminino', '2024-01-18', '2024-06-18', 1, NOW(), 8),
                (23, 'Ciclismo de Estrada', 'Treinamento para ciclismo de estrada', '2024-04-08', '2024-09-08', 1, NOW(), 10),
                (24, 'Corrida de Resistência', 'Treinamento para corridas de longa distância', '2024-02-14', '2024-07-14', 1, NOW(), 10),
                (25, 'Tênis Iniciante', 'Tênis para iniciantes adultos', '2024-03-12', '2024-08-12', 1, NOW(), 1),
                (26, 'Atletismo Juvenil', 'Atletismo para jovens de 14 a 18 anos', '2024-01-25', '2024-06-25', 1, NOW(), 10),
                (27, 'Futebol Feminino', 'Futebol exclusivo para mulheres', '2024-04-15', '2024-09-15', 1, NOW(), 8),
                (28, 'Basquete 3x3', 'Modalidade basquete 3 contra 3', '2024-02-18', '2024-07-18', 1, NOW(), 6),
                (29, 'Natação Infantil', 'Natação para crianças de 5 a 10 anos', '2024-03-08', '2024-08-08', 1, NOW(), 2),
                (30, 'Vôlei de Praia', 'Vôlei de praia ao ar livre', '2024-01-22', '2024-06-22', 1, NOW(), 6),
                (31, 'Hidroginástica Terceira Idade', 'Hidroginástica para idosos', '2024-04-02', '2024-09-02', 1, NOW(), 2),
                (32, 'Funcional Cross Training', 'Treinamento funcional intensivo', '2024-02-05', '2024-07-05', 1, NOW(), 5),
                (33, 'Dança de Salão', 'Dança de salão para casais', '2024-03-18', '2024-08-18', 1, NOW(), 7),
                (34, 'Esgrima Iniciante', 'Introdução à esgrima esportiva', '2024-01-08', '2024-06-08', 1, NOW(), 5)
                ON DUPLICATE KEY UPDATE 
                Nome = VALUES(Nome),
                Descricao = VALUES(Descricao),
                DataInicio = VALUES(DataInicio),
                DataFim = VALUES(DataFim),
                Status = VALUES(Status),
                UnidadeId = VALUES(UnidadeId);
            ");

            // Inserir inscrições iniciais com sobreposição eficiente - distribuição entre as turmas
            migrationBuilder.Sql(@"
                INSERT INTO InscricaoTurma (Id, PessoaId, TurmaId, DataInscricao) VALUES 
                (1, 1, 1, DATE_SUB(NOW(), INTERVAL 30 DAY)),
                (2, 2, 2, DATE_SUB(NOW(), INTERVAL 25 DAY)),
                (3, 3, 1, DATE_SUB(NOW(), INTERVAL 20 DAY)),
                (4, 4, 3, DATE_SUB(NOW(), INTERVAL 15 DAY)),
                (5, 5, 4, DATE_SUB(NOW(), INTERVAL 10 DAY)),
                (6, 6, 1, DATE_SUB(NOW(), INTERVAL 45 DAY)),
                (7, 7, 2, DATE_SUB(NOW(), INTERVAL 40 DAY)),
                (8, 8, 3, DATE_SUB(NOW(), INTERVAL 35 DAY)),
                (9, 9, 4, DATE_SUB(NOW(), INTERVAL 32 DAY)),
                (10, 10, 1, DATE_SUB(NOW(), INTERVAL 28 DAY)),
                (11, 11, 2, DATE_SUB(NOW(), INTERVAL 24 DAY)),
                (12, 12, 3, DATE_SUB(NOW(), INTERVAL 22 DAY)),
                (13, 13, 4, DATE_SUB(NOW(), INTERVAL 18 DAY)),
                (14, 14, 1, DATE_SUB(NOW(), INTERVAL 16 DAY)),
                (15, 15, 2, DATE_SUB(NOW(), INTERVAL 14 DAY)),
                (16, 16, 3, DATE_SUB(NOW(), INTERVAL 12 DAY)),
                (17, 17, 4, DATE_SUB(NOW(), INTERVAL 8 DAY)),
                (18, 18, 1, DATE_SUB(NOW(), INTERVAL 6 DAY)),
                (19, 19, 2, DATE_SUB(NOW(), INTERVAL 4 DAY)),
                (20, 20, 3, DATE_SUB(NOW(), INTERVAL 2 DAY)),
                (21, 21, 4, DATE_SUB(NOW(), INTERVAL 1 DAY)),
                (22, 22, 1, NOW()),
                (23, 23, 5, DATE_SUB(NOW(), INTERVAL 20 DAY)),
                (24, 24, 6, DATE_SUB(NOW(), INTERVAL 18 DAY)),
                (25, 25, 7, DATE_SUB(NOW(), INTERVAL 16 DAY)),
                (26, 26, 8, DATE_SUB(NOW(), INTERVAL 14 DAY)),
                (27, 27, 9, DATE_SUB(NOW(), INTERVAL 12 DAY)),
                (28, 28, 10, DATE_SUB(NOW(), INTERVAL 10 DAY)),
                (29, 29, 11, DATE_SUB(NOW(), INTERVAL 8 DAY)),
                (30, 30, 12, DATE_SUB(NOW(), INTERVAL 6 DAY)),
                (31, 31, 13, DATE_SUB(NOW(), INTERVAL 4 DAY)),
                (32, 32, 14, DATE_SUB(NOW(), INTERVAL 2 DAY)),
                (33, 33, 15, NOW()),
                (34, 34, 16, DATE_SUB(NOW(), INTERVAL 25 DAY)),
                (35, 35, 17, DATE_SUB(NOW(), INTERVAL 23 DAY)),
                (36, 36, 18, DATE_SUB(NOW(), INTERVAL 21 DAY)),
                (37, 37, 19, DATE_SUB(NOW(), INTERVAL 19 DAY)),
                (38, 38, 20, DATE_SUB(NOW(), INTERVAL 17 DAY)),
                (39, 39, 21, DATE_SUB(NOW(), INTERVAL 15 DAY)),
                (40, 40, 22, DATE_SUB(NOW(), INTERVAL 13 DAY)),
                (41, 41, 23, DATE_SUB(NOW(), INTERVAL 11 DAY)),
                (42, 42, 24, DATE_SUB(NOW(), INTERVAL 9 DAY)),
                (43, 1, 25, DATE_SUB(NOW(), INTERVAL 7 DAY)),
                (44, 2, 26, DATE_SUB(NOW(), INTERVAL 5 DAY)),
                (45, 3, 27, DATE_SUB(NOW(), INTERVAL 3 DAY)),
                (46, 4, 28, DATE_SUB(NOW(), INTERVAL 1 DAY)),
                (47, 5, 29, NOW()),
                (48, 6, 30, DATE_SUB(NOW(), INTERVAL 30 DAY)),
                (49, 7, 31, DATE_SUB(NOW(), INTERVAL 28 DAY)),
                (50, 8, 32, DATE_SUB(NOW(), INTERVAL 26 DAY)),
                (51, 9, 33, DATE_SUB(NOW(), INTERVAL 24 DAY)),
                (52, 10, 34, DATE_SUB(NOW(), INTERVAL 22 DAY))
                ON DUPLICATE KEY UPDATE 
                PessoaId = VALUES(PessoaId),
                TurmaId = VALUES(TurmaId),
                DataInscricao = VALUES(DataInscricao);
            ");

            // Inserir relações modalidade-turma com sobreposição eficiente
            migrationBuilder.Sql(@"
                INSERT INTO modalidade_turma (ModalidadeId, TurmaId) VALUES 
                (1, 1), -- Futebol -> Futebol Iniciante
                (2, 2), -- Basquete -> Basquete Avançado
                (4, 3), -- Natação -> Natação Intermediário
                (3, 4), -- Vôlei -> Vôlei Feminino
                (7, 5), -- Judô -> Judô Infantil
                (8, 6), -- Karatê -> Karatê Adulto
                (9, 7), -- Taekwondo -> Taekwondo Graduação
                (10, 8), -- Boxe -> Boxe Feminino
                (11, 9), -- Muay Thai -> Muay Thai Avançado
                (12, 10), -- Jiu-Jitsu -> Jiu-Jitsu Básico
                (13, 11), -- Ginástica Artística -> Ginástica Artística Infantil
                (14, 12), -- Ginástica Rítmica -> Ginástica Rítmica Juvenil
                (15, 13), -- Pilates -> Pilates Terapêutico
                (16, 14), -- Yoga -> Yoga Matinal
                (17, 15), -- Dança -> Dança Contemporânea
                (18, 16), -- Ballet -> Ballet Clássico
                (19, 17), -- Capoeira -> Capoeira Regional
                (20, 18), -- Xadrez -> Xadrez Escolar
                (21, 19), -- Ping Pong -> Ping Pong Competitivo
                (22, 20), -- Badminton -> Badminton Duplas
                (23, 21), -- Handebol -> Handebol Masculino
                (24, 22), -- Futsal -> Futsal Feminino
                (25, 23), -- Ciclismo -> Ciclismo de Estrada
                (26, 24), -- Corrida -> Corrida de Resistência
                (5, 25), -- Tênis -> Tênis Iniciante
                (6, 26), -- Atletismo -> Atletismo Juvenil
                (1, 27), -- Futebol -> Futebol Feminino
                (2, 28), -- Basquete -> Basquete 3x3
                (4, 29), -- Natação -> Natação Infantil
                (3, 30), -- Vôlei -> Vôlei de Praia
                (4, 31), -- Natação -> Hidroginástica Terceira Idade
                (6, 32), -- Atletismo -> Funcional Cross Training
                (17, 33), -- Dança -> Dança de Salão
                (6, 34) -- Atletismo -> Esgrima Iniciante
                ON DUPLICATE KEY UPDATE 
                ModalidadeId = VALUES(ModalidadeId),
                TurmaId = VALUES(TurmaId);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover dados em ordem reversa para respeitar foreign keys usando SQL direto
            migrationBuilder.Sql("DELETE FROM modalidade_turma WHERE TurmaId BETWEEN 1 AND 34;");
            migrationBuilder.Sql("DELETE FROM InscricaoTurma WHERE Id BETWEEN 1 AND 52;");
            migrationBuilder.Sql("DELETE FROM Turma WHERE Id BETWEEN 1 AND 34;");
            migrationBuilder.Sql("DELETE FROM Unidade WHERE Id BETWEEN 1 AND 10;");
            migrationBuilder.Sql("DELETE FROM Pessoa WHERE Id BETWEEN 1 AND 42;");
            migrationBuilder.Sql("DELETE FROM Modalidade WHERE Id BETWEEN 1 AND 26;");
        }
    }
}
