using System;

namespace ClinicaMedica.Models
{
    // Modelo que representa a tabela "paciente".
    // Cada propriedade corresponde a uma coluna do banco de dados.

    public class Paciente
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}