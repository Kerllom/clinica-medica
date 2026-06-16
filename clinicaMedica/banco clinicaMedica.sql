create database clinicaMedica;
use clinicaMedica;

create table usuarios(
id_usuarios int auto_increment primary key,
login varchar(120) not null,
senha varchar(150) not null,
tipo varchar(100) not null
);

create table especialidade(
id_especialidade int auto_increment primary key,
nome varchar(70) not null
);

create table pacientes(
id_pacientes int auto_increment primary key,
nome varchar(200) not null,
cpf varchar(14) not null,
telefone varchar(20) not null,
endereco text,
email varchar(150) not null,
data_nascimento date not null,
id_usuarios int not null,
foreign key(id_usuarios) references usuarios(id_usuarios)
);

create table recepcionista(
id_recepcionista int auto_increment primary key,
nome varchar(170) not null,
cpf varchar(14) not null,
id_usuarios int not null,
foreign key(id_usuarios) references usuarios(id_usuarios)
);

create table medicos(
id_medicos int auto_increment primary key,
nome varchar(200) not null,
crm varchar(20) not null,
id_usuarios int not null,
id_especialidade int not null,
foreign key(id_usuarios) references usuarios(id_usuarios),
foreign key(id_especialidade) references especialidade(id_especialidade)
);

create table consulta(
id_consulta int auto_increment primary key,
data_hora datetime not null,
status varchar(20) not null,
id_pacientes int not null,
id_medicos int not null,
foreign key(id_pacientes) references pacientes(id_pacientes),
foreign key(id_medicos) references medicos(id_medicos)
);

create table pagamentos(
id_pagamentos int auto_increment primary key,
valor decimal(10,2) not null,
forma_pagamento varchar(50) not null,
data_pagamento date not null,
status varchar(60) not null,
id_consulta int not null,
foreign key(id_consulta) references consulta(id_consulta)
);

create table prontuario(
id_prontuario int auto_increment primary key,
descricao text,
data_registro datetime not null,
id_pacientes int not null,
id_consulta int not null,
foreign key(id_pacientes) references pacientes(id_pacientes),
foreign key(id_consulta) references consulta(id_consulta)
);

