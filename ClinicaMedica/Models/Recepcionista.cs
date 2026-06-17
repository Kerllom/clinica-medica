using System;

namespace ClinicaMedica.Models
{
    /// <summary>
    /// Representa uma recepcionista da clínica.
    /// Espelha a tabela 'recepcionista' do banco de dados.
    /// </summary>
    public class Recepcionista
    {
        // Identificador único da recepcionista
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }

    }
}
