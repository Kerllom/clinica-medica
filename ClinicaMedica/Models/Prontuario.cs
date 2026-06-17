using System;

namespace ClinicaMedica.Models
{
    // Modelo que representa a tabela "prontuario".
    // Guarda o histórico clínico (registros) de um paciente.
    
    public class Prontuario
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public string Descricao { get; set; }
        public DateTime DataRegistro { get; set; }
    }
}