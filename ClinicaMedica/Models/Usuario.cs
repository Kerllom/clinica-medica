using System;


namespace ClinicaMedica.Models
{
    /// <summary>
    /// Representa umm usuário do sistema (médico, recepcionista, etc.).
    /// Espelha a tabela 'usuario' do banco de dados.
    /// </summary>
    public class Usuario
    {
        // Identificador único do usuário (chave primária)
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Tipo { get; set; }
    }
}
