using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ClinicaMedica.Models;

namespace ClinicaMedica.Data
{
    /// <summary>
    /// Responsável pelo acesso ao banco de dados para a entidade Medico.
    /// </summary>
    public class MedicoDAO
    {
        /// Insere um novo médico no banco.
        public void Inserir(Medico obj)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = @"INSERT INTO medico (id_usuario, id_especialidade, nome, crm)
                               VALUES (@idUsuario, @idEspecialidade, @nome, @crm)";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@idEspecialidade", obj.IdEspecialidade);
                    cmd.Parameters.AddWithValue("@nome", obj.Nome);
                    cmd.Parameters.AddWithValue("@crm", obj.Crm);

                    cmd.ExecuteNonQuery();
                }
            }

        }

        /// <summary> 
        /// Atualiza os dados de um médico existente.
        /// </summary>
        public void Atualizar(Medico obj)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = @"UPDATE medico
                               SET id_usuario = @idUsuario,
                                   id_especialidade = @idEspecialidade,
                                   nome = @nome,
                                   crm  = @crm
                               WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@idEspecialidade", obj.IdEspecialidade);
                    cmd.Parameters.AddWithValue("@nome", obj.Nome);
                    cmd.Parameters.AddWithValue("@crm", obj.Crm);
                    cmd.Parameters.AddWithValue("@id", obj.Id);

                    cmd.ExecuteNonQuery();
                }

            }
        }

        /// <summary>
        /// Exclui um médico pelo ID
        /// </summary>
        public void Excluir(int id)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "DELETE FROM medico WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Busca um médico pelo ID.
        /// </summary>
        public Medico BuscarPorId(int id)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "SELECT id, id_usuario, id_especialidade, nome, crm FROM medico WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Medico
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                IdEspecialidade = Convert.ToInt32(reader["id_especialidade"]),
                                Nome = reader.GetString("nome"),
                                Crm = reader.GetString("crm")
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Lista todos os médicos cadastrados.
        /// </summary>
        public List<Medico> ListarTodos()
        {
            var lista = new List<Medico>();

            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "SELECT id, id_usuario, id_especialidade, nome, crm FROM medico";

                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Medico
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                            IdEspecialidade = Convert.ToInt32(reader["id_especialidade"]),
                            Nome = reader.GetString("nome"),
                            Crm = reader.GetString("crm")
                        });
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Lista todos os médicos de uma especialidade específica.
        /// Útil para os Forms filtrarem médicos por especialidade.
        /// </summary>
        public List<Medico> ListarPorEspecialidade(int idEspecialidade)
        {
            var lista = new List<Medico>();

            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "SELECT id, id_usuario, id_especialidade, nome, crm FROM medico WHERE id_especialidade = @idEspecialidade";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@idEspecialidade", idEspecialidade);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Medico
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                IdEspecialidade = Convert.ToInt32(reader["id_especialidade"]),
                                Nome = reader.GetString("nome"),
                                Crm = reader.GetString("crm")
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}
