-- Cria o banco de dados (se ainda não existir) e seleciona ele
CREATE DATABASE IF NOT EXISTS clinica_medica
    DEFAULT CHARACTER SET utf8mb4
    DEFAULT COLLATE utf8mb4_general_ci;
 
USE clinica_medica;
 
-- ---------------------------------------------------------------------
-- TABELA: usuario
-- Responsável pelo login e autenticação 
-- O campo "tipo" define o papel: paciente, medico, recepcionista, admin.
-- ---------------------------------------------------------------------
CREATE TABLE usuario (
    id      BIGINT       NOT NULL AUTO_INCREMENT,
    login   VARCHAR(50)  NOT NULL,
    senha   VARCHAR(255) NOT NULL,              -- armazenar criptografada 
    tipo    VARCHAR(20)  NOT NULL,              -- paciente / medico / recepcionista / admin
    CONSTRAINT pk_usuario PRIMARY KEY (id),
    CONSTRAINT uq_usuario_login UNIQUE (login)  -- não permite login duplicado
);
 
-- ---------------------------------------------------------------------
-- TABELA: especialidade
-- Catálogo de especialidades médicas (ex.: Cardiologia, Pediatria).
-- ---------------------------------------------------------------------
CREATE TABLE especialidade (
    id    BIGINT      NOT NULL AUTO_INCREMENT,
    nome  VARCHAR(80) NOT NULL,
    CONSTRAINT pk_especialidade PRIMARY KEY (id),
    CONSTRAINT uq_especialidade_nome UNIQUE (nome)
);
 
-- ---------------------------------------------------------------------
-- TABELA: paciente
-- Dados do paciente. Vincula-se a um usuário para login.
-- ---------------------------------------------------------------------
CREATE TABLE paciente (
    id               BIGINT       NOT NULL AUTO_INCREMENT,
    id_usuario       BIGINT       NOT NULL,
    nome             VARCHAR(100) NOT NULL,
    cpf              VARCHAR(14)  NOT NULL,     -- formato 000.000.000-00
    telefone         VARCHAR(20),              -- varchar: permite (), -, e zeros à esquerda
    endereco         VARCHAR(150),
    email            VARCHAR(100),
    data_nascimento  DATE,
    CONSTRAINT pk_paciente PRIMARY KEY (id),
    CONSTRAINT uq_paciente_cpf UNIQUE (cpf),
    CONSTRAINT fk_paciente_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuario (id)
);
 
-- ---------------------------------------------------------------------
-- TABELA: recepcionista
-- Funcionário que gerencia cadastros e agendamentos no balcão.
-- ---------------------------------------------------------------------
CREATE TABLE recepcionista (
    id          BIGINT       NOT NULL AUTO_INCREMENT,
    id_usuario  BIGINT       NOT NULL,
    nome        VARCHAR(100) NOT NULL,
    cpf         VARCHAR(14)  NOT NULL,
    endereco    VARCHAR(150),
    CONSTRAINT pk_recepcionista PRIMARY KEY (id),
    CONSTRAINT uq_recepcionista_cpf UNIQUE (cpf),
    CONSTRAINT fk_recepcionista_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuario (id)
);
 
-- ---------------------------------------------------------------------
-- TABELA: medico
-- Dados do médico. Cada médico possui uma especialidade.
-- ---------------------------------------------------------------------
CREATE TABLE medico (
    id                BIGINT       NOT NULL AUTO_INCREMENT,
    id_usuario        BIGINT       NOT NULL,
    id_especialidade  BIGINT       NOT NULL,
    nome              VARCHAR(100) NOT NULL,
    crm               VARCHAR(20)  NOT NULL,    -- varchar: aceita letras e UF (ex.: MG12345)
    CONSTRAINT pk_medico PRIMARY KEY (id),
    CONSTRAINT uq_medico_crm UNIQUE (crm),
    CONSTRAINT fk_medico_usuario
        FOREIGN KEY (id_usuario) REFERENCES usuario (id),
    CONSTRAINT fk_medico_especialidade
        FOREIGN KEY (id_especialidade) REFERENCES especialidade (id)
);
 
-- ---------------------------------------------------------------------
-- TABELA: pagamento
-- Registro do pagamento de uma consulta.
-- ---------------------------------------------------------------------
CREATE TABLE pagamento (
    id              BIGINT         NOT NULL AUTO_INCREMENT,
    valor           DECIMAL(10, 2) NOT NULL,
    forma_pagamento VARCHAR(30),              -- dinheiro / cartao / pix / convenio
    data_pagamento  DATE,
    status          VARCHAR(20)    NOT NULL DEFAULT 'pendente',
    CONSTRAINT pk_pagamento PRIMARY KEY (id)
);
 
-- ---------------------------------------------------------------------
-- TABELA: consulta
-- Entidade central do sistema 
-- Liga paciente, médico, data, horário e o pagamento gerado.
-- ---------------------------------------------------------------------
CREATE TABLE consulta (
    id            BIGINT      NOT NULL AUTO_INCREMENT,
    id_paciente   BIGINT      NOT NULL,
    id_medico     BIGINT      NOT NULL,
    id_pagamento  BIGINT,                       -- pode ser nulo até o pagamento ocorrer
    data          DATE        NOT NULL,
    horario       TIME        NOT NULL,
    status        VARCHAR(20) NOT NULL DEFAULT 'agendada', -- agendada/confirmada/cancelada/realizada
    CONSTRAINT pk_consulta PRIMARY KEY (id),
    CONSTRAINT fk_consulta_paciente
        FOREIGN KEY (id_paciente) REFERENCES paciente (id),
    CONSTRAINT fk_consulta_medico
        FOREIGN KEY (id_medico) REFERENCES medico (id),
    CONSTRAINT fk_consulta_pagamento
        FOREIGN KEY (id_pagamento) REFERENCES pagamento (id),
    -- REGRA RF05: impede dois agendamentos no mesmo médico, dia e horário
    CONSTRAINT uq_consulta_medico_horario UNIQUE (id_medico, data, horario)
);
 
-- ---------------------------------------------------------------------
-- TABELA: prontuario
-- Histórico clínico do paciente.
-- ---------------------------------------------------------------------
CREATE TABLE prontuario (
    id             BIGINT NOT NULL AUTO_INCREMENT,
    id_paciente    BIGINT NOT NULL,
    descricao      TEXT,                         -- texto livre do registro clínico
    data_registro  DATE,
    CONSTRAINT pk_prontuario PRIMARY KEY (id),
    CONSTRAINT fk_prontuario_paciente
        FOREIGN KEY (id_paciente) REFERENCES paciente (id)
);
 
-- =====================================================================
-- DADOS DE EXEMPLO (para testar o sistema)
-- =====================================================================
 
-- Especialidades
INSERT INTO especialidade (nome) VALUES
    ('Cardiologia'),
    ('Pediatria'),
    ('Dermatologia'),
    ('Ortopedia');
 
-- Usuário administrador de teste (login: admin / senha: 123456)
INSERT INTO usuario (login, senha, tipo) VALUES
    ('admin', '123456', 'admin');
 
-- Médico de exemplo (vinculado ao usuário admin e à Cardiologia)
-- Util para ja ter um medico na lista de agendamento ao testar.
INSERT INTO medico (id_usuario, id_especialidade, nome, crm) VALUES
    (1, 1, 'Dr. Joao Silva', 'CRM-MG 12345');
