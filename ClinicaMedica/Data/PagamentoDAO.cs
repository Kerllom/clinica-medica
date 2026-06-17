using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ClinicaMedica.Models;

namespace ClinicaMedica.Data
{
    // DAO (Data Access Object) do Pagamento.
    // Responsável por toda a conversa com a tabela "pagamento" no banco.
    // Segue o mesmo padrão do PacienteDAO.

    public class PagamentoDAO
    {
        // INSERIR: adiciona um novo pagamento no banco.
        public void Inserir(Pagamento pagamento)
        {
            string sql = @"INSERT INTO pagamento
                               (valor, forma_pagamento, data_pagamento, status)
                           VALUES
                               (@valor, @forma_pagamento, @data_pagamento, @status)";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@valor", pagamento.Valor);
                cmd.Parameters.AddWithValue("@forma_pagamento", pagamento.FormaPagamento);
                cmd.Parameters.AddWithValue("@data_pagamento", pagamento.DataPagamento);
                cmd.Parameters.AddWithValue("@status", pagamento.Status);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(Pagamento pagamento)
        {
            string sql = @"UPDATE pagamento SET
                               valor = @valor,
                               forma_pagamento = @forma_pagamento,
                               data_pagamento = @data_pagamento,
                               status = @status
                           WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@valor", pagamento.Valor);
                cmd.Parameters.AddWithValue("@forma_pagamento", pagamento.FormaPagamento);
                cmd.Parameters.AddWithValue("@data_pagamento", pagamento.DataPagamento);
                cmd.Parameters.AddWithValue("@status", pagamento.Status);
                cmd.Parameters.AddWithValue("@id", pagamento.Id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            string sql = "DELETE FROM pagamento WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Pagamento BuscarPorId(int id)
        {
            string sql = "SELECT * FROM pagamento WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MontarPagamento(reader);
                    }
                }
            }

            return null;
        }

        public List<Pagamento> ListarTodos()
        {
            List<Pagamento> lista = new List<Pagamento>();
            string sql = "SELECT * FROM pagamento ORDER BY data_pagamento";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MontarPagamento(reader));
                    }
                }
            }

            return lista;
        }

        private Pagamento MontarPagamento(MySqlDataReader reader)
        {
            Pagamento pg = new Pagamento();

            pg.Id = Convert.ToInt32(reader["id"]);
            pg.Valor = Convert.ToDecimal(reader["valor"]);
            pg.FormaPagamento = reader["forma_pagamento"].ToString();
            pg.DataPagamento = Convert.ToDateTime(reader["data_pagamento"]);
            pg.Status = reader["status"].ToString();

            return pg;
        }
    }
}
