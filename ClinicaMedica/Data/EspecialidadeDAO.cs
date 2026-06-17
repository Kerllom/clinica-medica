
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ClinicaMedica.Models;

namespace ClinicaMedica.Data
{
    /// <summary>
    /// Responsável pelo acesso ao banco de dados para a entidade Especialidade.
    /// </summary>
    public class EspecialidadeDAO
    {
        /// <summary>
        /// Insere uma nova especialidade no banco.
        /// </summary>
        public void Inserir(Especialidade obj)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "INSERT INTO especialidade (nome) VALUES (@nome)";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", obj.Nome);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Atualiza o nome de uma especialidade existente.
        /// </summary>
        public void Atualizar(Especialidade obj)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "UPDATE especialidade SET nome = @nome WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", obj.Nome);
                    cmd.Parameters.AddWithValue("@id", obj.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Exclui uma especialidade pelo ID.
        /// </summary>
        public void Excluir(int id)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "DELETE FROM especialidade WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Busca uma especialidade pelo ID.
        /// </summary>
        public Especialidade BuscarPorId(int id)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "SELECT id, nome FROM especialidade WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Especialidade
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nome = reader.GetString("nome")
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Lista todas as especialidades cadastradas.
        /// </summary>
        public List<Especialidade> ListarTodos()
        {
            var lista = new List<Especialidade>();

            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "SELECT id, nome FROM especialidade";

                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Especialidade
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nome = reader.GetString("nome")
                        });
                    }
                }
            }

            return lista;
        }
    }
}