
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ClinicaMedica.Models;

namespace ClinicaMedica.Data
{
    /// <summary>
    /// Responsável pelo acesso ao banco de dados para a entidade Recepcionista.
    /// </summary>
    public class RecepcionistaDAO
    {
        /// <summary>
        /// Insere uma nova recepcionista no banco.
        /// </summary>
        public void Inserir(Recepcionista obj)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = @"INSERT INTO recepcionista (id_usuario, nome, cpf, endereco)
                               VALUES (@idUsuario, @nome, @cpf, @endereco)";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@nome", obj.Nome);
                    cmd.Parameters.AddWithValue("@cpf", obj.Cpf);
                    cmd.Parameters.AddWithValue("@endereco", obj.Endereco);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Atualiza os dados de uma recepcionista existente.
        /// </summary>
        public void Atualizar(Recepcionista obj)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = @"UPDATE recepcionista
                               SET id_usuario = @idUsuario,
                                   nome      = @nome,
                                   cpf       = @cpf,
                                   endereco  = @endereco
                               WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@nome", obj.Nome);
                    cmd.Parameters.AddWithValue("@cpf", obj.Cpf);
                    cmd.Parameters.AddWithValue("@endereco", obj.Endereco);
                    cmd.Parameters.AddWithValue("@id", obj.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Exclui uma recepcionista pelo ID.
        /// </summary>
        public void Excluir(int id)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "DELETE FROM recepcionista WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Busca uma recepcionista pelo ID.
        /// </summary>
        public Recepcionista BuscarPorId(int id)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "SELECT id, id_usuario, nome, cpf, endereco FROM recepcionista WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Recepcionista
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                Nome = reader.GetString("nome"),
                                Cpf = reader.GetString("cpf"),
                                Endereco = reader.GetString("endereco")
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Lista todas as recepcionistas cadastradas.
        /// </summary>
        public List<Recepcionista> ListarTodos()
        {
            var lista = new List<Recepcionista>();

            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "SELECT id, id_usuario, nome, cpf, endereco FROM recepcionista";

                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Recepcionista
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                            Nome = reader.GetString("nome"),
                            Cpf = reader.GetString("cpf"),
                            Endereco = reader.GetString("endereco")
                        });
                    }
                }
            }

            return lista;
        }
    }
}