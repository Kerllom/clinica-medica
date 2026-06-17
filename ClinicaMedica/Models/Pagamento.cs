using System;

namespace ClinicaMedica.Models
{
    // Modelo que representa a tabela "pagamento".
    // Guarda os dados do pagamento de uma consulta.

    public class Pagamento
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string FormaPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
        public string Status {  get; set; }
    }
}