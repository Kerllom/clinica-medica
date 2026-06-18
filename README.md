# 🏥 Sistema de Clínica Médica

Sistema desktop para agendamento de consultas médicas, onde é possível
cadastrar pacientes e agendar consultas escolhendo especialidade, médico,
dia e horário. Projeto acadêmico desenvolvido para a disciplina de
Programação Orientada a Objetos.

---

## 👥 Equipe

| Integrante | Responsabilidade |
|-----------|------------------|
| Kerllote | Models e DAOs (Paciente, Consulta, Pagamento, Prontuário) + Services |
| Igor | Models e DAOs (Usuário, Especialidade, Médico, Recepcionista) |
| [Integrante 3] | Interface gráfica (Forms) |
| [Integrante 4] | [função] |
| [Integrante 5] | Integração e GitHub |

> Preencha os nomes reais da equipe.

---

## ✨ Funcionalidades

- Login de usuários (RF02)
- Cadastro de pacientes com validação de CPF e e-mail (RF01 / RNF05)
- Agendamento de consultas: especialidade → médico → dia → horário (RF03)
- Verificação de disponibilidade de horário (RF04 / RF05)
- Listagem e cancelamento de consultas (RF06 / RF08)

---

## 🛠️ Tecnologias

- **Linguagem:** C#
- **Interface gráfica:** Windows Forms (.NET)
- **Banco de dados:** MySQL
- **Acesso a dados:** ADO.NET (conector `MySql.Data`)
- **Arquitetura:** em camadas (Models → DAO → Services → Forms)

---

## 📁 Estrutura do projeto

```
ClinicaMedica/
├── Models/        Classes que espelham as tabelas
├── Data/          Conexão e DAOs (acesso ao banco)
├── Services/      Regras de negócio (agendamento, validações)
├── Forms/         Telas (interface gráfica)
├── Program.cs     Classe principal (Main)
├── database/      Script SQL do banco
└── docs/          Requisitos e diagramas
```

---

## ▶️ Como executar

### 1. Banco de dados
1. Instale o MySQL.
2. Execute o script `database/script_clinica.sql` para criar o banco
   `clinica_medica` e suas tabelas (ele já insere algumas especialidades
   e um usuário administrador para teste).

### 2. Configurar a conexão
No arquivo `Data/Conexao.cs`, ajuste o usuário e a senha do seu MySQL:
```
"Server=localhost;Database=clinica_medica;Uid=root;Pwd=suasenha;"
```

### 3. Executar o sistema
1. Abra o projeto no **Visual Studio 2022**.
2. Restaure o pacote NuGet `MySql.Data` (se necessário).
3. Pressione **F5** para rodar.

### 4. Login de teste
- **Usuário:** `admin`
- **Senha:** `123456`

---

## 📊 Diagramas

Os diagramas estão na pasta `docs/` (renderizam automaticamente no GitHub):

- [Casos de uso](docs/diagrama_caso_uso.md)
- [Classes](docs/diagrama_classes.md)
- [Entidade-Relacionamento](docs/diagrama_er.md)

## 📋 Requisitos

Os requisitos funcionais e não funcionais estão em [docs/requisitos.md](docs/requisitos.md).
