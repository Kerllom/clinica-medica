using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ClinicaMedica.Models;

namespace ClinicaMedica.Data
{
    /// <summary>
    /// Responsável pelo acesso ao banco de dados para a entidade Usuario.
    /// Operações: Inserir, Atualizar, Excluir, BuscarPorId, ListarTodos.
    /// </summary>
    public class UsuarioDAO
    {
        /// <summary>
        /// Insere um novo usuário no banco de dados.
        /// </summary>
        public void Inserir(Usuario obj)
        {
            // Abre a conexão usando o bloco 'using' para garantir o fechamento automático
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                // Comando SQL com parâmetros para evitar SQL Injection
                string sql = "INSERT INTO usuario (login, senha, tipo) VALUES (@login, @senha, @tipo)";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    // Vincula os parâmetros aos valores do objeto
                    cmd.Parameters.AddWithValue("@login", obj.Login);
                    cmd.Parameters.AddWithValue("@senha", obj.Senha);
                    cmd.Parameters.AddWithValue("@tipo", obj.Tipo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente no banco.
        /// </summary>
        public void Atualizar(Usuario obj)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "UPDATE usuario SET login = @login, senha = @senha, tipo = @tipo WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@login", obj.Login);
                    cmd.Parameters.AddWithValue("@senha", obj.Senha);
                    cmd.Parameters.AddWithValue("@tipo", obj.Tipo);
                    cmd.Parameters.AddWithValue("@id", obj.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Exclui um usuário do banco pelo seu ID.
        /// </summary>
        public void Excluir(int id)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "DELETE FROM usuario WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Busca um único usuário pelo ID.
        /// Retorna null se não encontrado.
        /// </summary>
        public Usuario BuscarPorId(int id)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "SELECT id, login, senha, tipo FROM usuario WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Monta e retorna o objeto preenchido com os dados do banco
                            return new Usuario
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Login = reader.GetString("login"),
                                Senha = reader.GetString("senha"),
                                Tipo = reader.GetString("tipo")
                            };
                        }
                    }
                }
            }

            return null; // Não encontrado
        }

        /// <summary>
        /// Retorna a lista de todos os usuários cadastrados.
        /// </summary>
        public List<Usuario> ListarTodos()
        {
            var lista = new List<Usuario>();

            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sql = "SELECT id, login, senha, tipo FROM usuario";

                using (var cmd = new MySqlCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuario
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Login = reader.GetString("login"),
                            Senha = reader.GetString("senha"),
                            Tipo = reader.GetString("tipo")
                        });
                    }
                }
            }

            return lista;
        }
    }
}