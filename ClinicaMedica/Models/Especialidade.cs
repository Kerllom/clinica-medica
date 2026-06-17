using System;

namespace ClinicaMedica.Models
{
    /// <summary>
    /// Representa uma especialidade médica (ex.: Cardiologia, Pediatria).
    /// Espelha a tabela 'especialidade' do banco de dados.
    /// </summary>
    public class Especialidade
    {
        // Identificador único da especialidade
        public int Id { get; set; }
        
        public string Nome { get; set; }
    }
}
