# Requisitos do Sistema

## Requisitos Funcionais (RF)

| Código | Requisito |
|--------|-----------|
| RF01 | O sistema deve permitir o cadastro de pacientes (nome, CPF, telefone, endereço, e-mail, data de nascimento). |
| RF02 | O sistema deve permitir login com autenticação por usuário e senha. |
| RF03 | O sistema deve permitir agendar consulta selecionando especialidade, médico, dia e horário. |
| RF04 | O sistema deve verificar a disponibilidade do horário antes de confirmar o agendamento. |
| RF05 | O sistema deve impedir o agendamento de dois pacientes no mesmo médico/horário. |
| RF06 | O sistema deve permitir cancelar uma consulta agendada. |
| RF07 | O sistema deve permitir remarcar uma consulta. |
| RF08 | O sistema deve listar as consultas agendadas do paciente. |
| RF09 | O administrador deve poder cadastrar, editar e remover médicos e especialidades. |
| RF10 | O sistema deve registrar a situação da consulta (agendada, confirmada, cancelada, realizada). |

## Requisitos Não Funcionais (RNF)

| Código | Requisito |
|--------|-----------|
| RNF01 | A interface deve ser intuitiva e de fácil utilização (usabilidade). |
| RNF02 | O sistema deve responder a uma busca de horários em até 3 segundos (desempenho). |
| RNF03 | As senhas devem ser armazenadas de forma criptografada (segurança). |
| RNF04 | O sistema deve utilizar banco de dados relacional MySQL. |
| RNF05 | O sistema deve validar os dados de entrada (ex.: CPF e e-mail em formato válido). |
| RNF06 | O sistema deve estar disponível durante o horário de funcionamento da clínica. |
| RNF07 | O código deve seguir padrões de indentação e estar comentado (manutenibilidade). |

> Observação: o RNF03 (criptografia de senha) está previsto como melhoria;
> na versão atual a senha é comparada em texto puro para simplificar a demonstração.
