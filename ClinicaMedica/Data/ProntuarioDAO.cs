using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ClinicaMedica.Models;

namespace ClinicaMedica.Data
{
    
    // DAO (Data Access Object) do Prontuario.
    // Responsavel por toda a conversa com a tabela "prontuario" no banco.
    // Segue o mesmo padrao do PacienteDAO e do PagamentoDAO.
    
    public class ProntuarioDAO
    {
       
        // INSERIR: adiciona um novo registro de prontuario no banco.// -------------------------------------------------------------
        public void Inserir(Prontuario prontuario)
        {
            string sql = @"INSERT INTO prontuario
                               (id_paciente, descricao, data_registro)
                           VALUES
                               (@id_paciente, @descricao, @data_registro)";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id_paciente", prontuario.IdPaciente);
                cmd.Parameters.AddWithValue("@descricao", prontuario.Descricao);
                cmd.Parameters.AddWithValue("@data_registro", prontuario.DataRegistro);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        // ATUALIZAR: altera um registro de prontuario ja existente.
        
        public void Atualizar(Prontuario prontuario)
        {
            string sql = @"UPDATE prontuario SET
                               id_paciente = @id_paciente,
                               descricao = @descricao,
                               data_registro = @data_registro
                           WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id_paciente", prontuario.IdPaciente);
                cmd.Parameters.AddWithValue("@descricao", prontuario.Descricao);
                cmd.Parameters.AddWithValue("@data_registro", prontuario.DataRegistro);
                cmd.Parameters.AddWithValue("@id", prontuario.Id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        // EXCLUIR: remove um registro de prontuario pelo seu id.
        
        public void Excluir(int id)
        {
            string sql = "DELETE FROM prontuario WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        // BUSCAR POR ID: retorna um unico prontuario, ou null se nao achar.
        
        public Prontuario BuscarPorId(int id)
        {
            string sql = "SELECT * FROM prontuario WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MontarProntuario(reader);
                    }
                }
            }

            return null;
        }

        
        // LISTAR TODOS: retorna uma lista com todos os prontuarios.// -------------------------------------------------------------
        public List<Prontuario> ListarTodos()
        {
            List<Prontuario> lista = new List<Prontuario>();
            string sql = "SELECT * FROM prontuario ORDER BY data_registro DESC";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MontarProntuario(reader));
                    }
                }
            }

            return lista;
        }

        
        // LISTAR POR PACIENTE: retorna o historico de um paciente
        // especifico. Util para a tela que mostra o prontuario de
        // um paciente. (Metodo extra, alem dos 5 padrao.)
        
        public List<Prontuario> ListarPorPaciente(int idPaciente)
        {
            List<Prontuario> lista = new List<Prontuario>();
            string sql = @"SELECT * FROM prontuario
                           WHERE id_paciente = @id_paciente
                           ORDER BY data_registro DESC";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id_paciente", idPaciente);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MontarProntuario(reader));
                    }
                }
            }

            return lista;
        }

        
        // METODO AUXILIAR (privado): le uma linha do banco (reader) e
        // monta um objeto Prontuario.
        
        private Prontuario MontarProntuario(MySqlDataReader reader)
        {
            Prontuario pr = new Prontuario();

            pr.Id = Convert.ToInt32(reader["id"]);
            pr.IdPaciente = Convert.ToInt32(reader["id_paciente"]);
            pr.Descricao = reader["descricao"].ToString();
            pr.DataRegistro = Convert.ToDateTime(reader["data_registro"]);

            return pr;
        }
    }
}