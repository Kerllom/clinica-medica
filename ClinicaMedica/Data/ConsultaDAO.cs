using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ClinicaMedica.Models;

namespace ClinicaMedica.Data
{
   
    // DAO (Data Access Object) da Consulta.
    // E o DAO mais importante: a consulta e a entidade central do
    // sistema. Possui dois pontos especiais em relacao aos outros:
    //   1) o campo IdPagamento pode ser nulo (DBNull);
    //   2) tem um metodo extra para verificar disponibilidade de horario.
    
    public class ConsultaDAO
    {
       
        // INSERIR: agenda uma nova consulta.
       
        public void Inserir(Consulta consulta)
        {
            string sql = @"INSERT INTO consulta
                               (id_paciente, id_medico, id_pagamento, data, horario, status)
                           VALUES
                               (@id_paciente, @id_medico, @id_pagamento, @data, @horario, @status)";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id_paciente", consulta.IdPaciente);
                cmd.Parameters.AddWithValue("@id_medico", consulta.IdMedico);

                // TRATAMENTO DO CAMPO NULO:
                // se IdPagamento estiver vazio (null), enviamos DBNull.Value,
                // que e o "nulo" entendido pelo banco. Caso contrario, o valor.
                if (consulta.IdPagamento.HasValue)
                    cmd.Parameters.AddWithValue("@id_pagamento", consulta.IdPagamento.Value);
                else
                    cmd.Parameters.AddWithValue("@id_pagamento", DBNull.Value);

                cmd.Parameters.AddWithValue("@data", consulta.Data);
                cmd.Parameters.AddWithValue("@horario", consulta.Horario);
                cmd.Parameters.AddWithValue("@status", consulta.Status);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

       
        // ATUALIZAR: altera uma consulta existente (ex.: mudar status
        // para "cancelada" ou "realizada", ou vincular um pagamento).
       
        public void Atualizar(Consulta consulta)
        {
            string sql = @"UPDATE consulta SET
                               id_paciente = @id_paciente,
                               id_medico = @id_medico,
                               id_pagamento = @id_pagamento,
                               data = @data,
                               horario = @horario,
                               status = @status
                           WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id_paciente", consulta.IdPaciente);
                cmd.Parameters.AddWithValue("@id_medico", consulta.IdMedico);

                if (consulta.IdPagamento.HasValue)
                    cmd.Parameters.AddWithValue("@id_pagamento", consulta.IdPagamento.Value);
                else
                    cmd.Parameters.AddWithValue("@id_pagamento", DBNull.Value);

                cmd.Parameters.AddWithValue("@data", consulta.Data);
                cmd.Parameters.AddWithValue("@horario", consulta.Horario);
                cmd.Parameters.AddWithValue("@status", consulta.Status);
                cmd.Parameters.AddWithValue("@id", consulta.Id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

       
        // EXCLUIR: remove uma consulta pelo seu id.
        // (Em geral prefere-se mudar o status para "cancelada", mas o
        //  metodo de exclusao fica disponivel.)
       
        public void Excluir(int id)
        {
            string sql = "DELETE FROM consulta WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

       
        // BUSCAR POR ID: retorna uma unica consulta, ou null se nao achar.
       
        public Consulta BuscarPorId(int id)
        {
            string sql = "SELECT * FROM consulta WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MontarConsulta(reader);
                    }
                }
            }

            return null;
        }

        
        // LISTAR TODOS: retorna todas as consultas.
       
        public List<Consulta> ListarTodos()
        {
            List<Consulta> lista = new List<Consulta>();
            string sql = "SELECT * FROM consulta ORDER BY data, horario";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MontarConsulta(reader));
                    }
                }
            }

            return lista;
        }

        
        // LISTAR POR PACIENTE: consultas de um paciente (RF08).
        
        public List<Consulta> ListarPorPaciente(int idPaciente)
        {
            List<Consulta> lista = new List<Consulta>();
            string sql = @"SELECT * FROM consulta
                           WHERE id_paciente = @id_paciente
                           ORDER BY data, horario";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id_paciente", idPaciente);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MontarConsulta(reader));
                    }
                }
            }

            return lista;
        }

        
        // VERIFICAR DISPONIBILIDADE (RF04/RF05): retorna TRUE se o
        // horario estiver LIVRE para aquele medico, naquele dia e hora.
        // Conta quantas consultas ja existem nesse slot; se for zero,
        // o horario esta disponivel.
        
        public bool HorarioDisponivel(int idMedico, DateTime data, TimeSpan horario)
        {
            string sql = @"SELECT COUNT(*) FROM consulta
                           WHERE id_medico = @id_medico
                             AND data = @data
                             AND horario = @horario
                             AND status <> 'cancelada'";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id_medico", idMedico);
                cmd.Parameters.AddWithValue("@data", data);
                cmd.Parameters.AddWithValue("@horario", horario);

                con.Open();
                // ExecuteScalar retorna um unico valor (o resultado do COUNT).
                int quantidade = Convert.ToInt32(cmd.ExecuteScalar());

                return quantidade == 0; // 0 consultas no slot = horario livre
            }
        }

       
        // METODO AUXILIAR (privado): le uma linha do banco (reader) e
        // monta um objeto Consulta. Trata o campo id_pagamento que
        // pode vir nulo do banco.
       
        private Consulta MontarConsulta(MySqlDataReader reader)
        {
            Consulta c = new Consulta();

            c.Id = Convert.ToInt32(reader["id"]);
            c.IdPaciente = Convert.ToInt32(reader["id_paciente"]);
            c.IdMedico = Convert.ToInt32(reader["id_medico"]);

            // LEITURA DO CAMPO NULO:
            // se a coluna id_pagamento estiver vazia no banco, mantemos null;
            // caso contrario, convertemos para int.
            if (reader["id_pagamento"] == DBNull.Value)
                c.IdPagamento = null;
            else
                c.IdPagamento = Convert.ToInt32(reader["id_pagamento"]);

            c.Data = Convert.ToDateTime(reader["data"]);
            c.Horario = (TimeSpan)reader["horario"];
            c.Status = reader["status"].ToString();

            return c;
        }
    }
}