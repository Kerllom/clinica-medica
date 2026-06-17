using System;

namespace ClinicaMedica.Models
{
    // Modelo que representa a tabela "consulta".
    // E a entidade central do sistema: liga paciente, medico,
    // data, horário e o pagamento gerado.
    public class Consulta
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int? IdPagamento { get; set; } // id_pagamento (FK -> pagamento) - pode ser nulo 
        public DateTime Data {  get; set; }
        public TimeSpan Horario { get; set; }
        public string Status { get; set; } // status: agendada/confirmada/cancelada/realizada
    }
}