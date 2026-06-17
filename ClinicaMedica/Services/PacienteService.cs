using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ClinicaMedica.Models;
using ClinicaMedica.Data;

namespace ClinicaMedica.Services
{
    // =================================================================
    // SERVICE do Paciente.
    // Camada de REGRAS DE NEGOCIO do cadastro de pacientes.
    // Cumpre o RNF05 (validacao dos dados de entrada): valida nome,
    // CPF e e-mail ANTES de gravar no banco, e impede CPF duplicado.
    // =================================================================
    public class PacienteService
    {
        private PacienteDAO pacienteDAO = new PacienteDAO();

        // -------------------------------------------------------------
        // CADASTRAR: valida os dados e, se tudo certo, insere.
        // -------------------------------------------------------------
        public void CadastrarPaciente(Paciente paciente)
        {
            // Aplica todas as validacoes (lanca Exception se algo falhar).
            Validar(paciente);

            // Regra: nao permitir dois pacientes com o mesmo CPF.
            Paciente existente = pacienteDAO.BuscarPorCpf(paciente.Cpf);
            if (existente != null)
                throw new Exception("Ja existe um paciente cadastrado com este CPF.");

            pacienteDAO.Inserir(paciente);
        }

        // -------------------------------------------------------------
        // ATUALIZAR: valida e atualiza um paciente existente.
        // -------------------------------------------------------------
        public void AtualizarPaciente(Paciente paciente)
        {
            Validar(paciente);

            // Permite o mesmo CPF se for do proprio paciente;
            // bloqueia se o CPF pertencer a OUTRO paciente.
            Paciente existente = pacienteDAO.BuscarPorCpf(paciente.Cpf);
            if (existente != null && existente.Id != paciente.Id)
                throw new Exception("Este CPF ja pertence a outro paciente.");

            pacienteDAO.Atualizar(paciente);
        }

        // -------------------------------------------------------------
        // EXCLUIR e consultas simples: repassam para o DAO.
        // -------------------------------------------------------------
        public void ExcluirPaciente(int id)
        {
            pacienteDAO.Excluir(id);
        }

        public Paciente BuscarPorId(int id)
        {
            return pacienteDAO.BuscarPorId(id);
        }

        public List<Paciente> ListarTodos()
        {
            return pacienteDAO.ListarTodos();
        }

        // -------------------------------------------------------------
        // VALIDAR (privado): concentra todas as regras de validacao.
        // Reaproveitado por Cadastrar e Atualizar.
        // -------------------------------------------------------------
        private void Validar(Paciente paciente)
        {
            // Nome obrigatorio.
            if (string.IsNullOrWhiteSpace(paciente.Nome))
                throw new Exception("O nome do paciente e obrigatorio.");

            // CPF obrigatorio e com 11 digitos.
            if (string.IsNullOrWhiteSpace(paciente.Cpf))
                throw new Exception("O CPF e obrigatorio.");

            // Remove pontos e tracos para contar so os numeros.
            string cpfNumeros = paciente.Cpf.Replace(".", "").Replace("-", "").Trim();
            if (cpfNumeros.Length != 11 || !SoNumeros(cpfNumeros))
                throw new Exception("CPF invalido. Deve conter 11 digitos.");

            // E-mail: se foi preenchido, precisa ter formato valido.
            if (!string.IsNullOrWhiteSpace(paciente.Email) && !EmailValido(paciente.Email))
                throw new Exception("E-mail invalido. Verifique o formato (exemplo: nome@dominio.com).");
        }

        // Verifica se a string contem apenas digitos numericos.
        private bool SoNumeros(string texto)
        {
            foreach (char c in texto)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        // Validacao basica de e-mail usando expressao regular.
        private bool EmailValido(string email)
        {
            // Padrao simples: algo@algo.algo
            string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, padrao);
        }
    }
}