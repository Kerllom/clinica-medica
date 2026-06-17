using System;

namespace ClinicaMedica.Models
{
    /// <summary>
    /// Representa um médico cadastrado na clínica
    /// </summary>
    public class Medico
    {
        // Identificador único do médico
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdEspecialidade { get; set; }
        public string Nome { get; set; }
        public string Crm { get; set; }
    }
}
