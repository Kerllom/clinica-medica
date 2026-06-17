using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ClinicaMedica.Models;

namespace ClinicaMedica.Data
{
    
    // DAO (Data Access Object) do Paciente.
    // Responsavel por toda a conversa com a tabela "paciente" no banco:
    // inserir, atualizar, excluir, buscar por id e listar todos.
    
    public class PacienteDAO
    {
        
        // INSERIR: adiciona um novo paciente no banco.
        
        public void Inserir(Paciente paciente)
        {
            // Comando SQL com parametros (@nome, @cpf...) para evitar
            // SQL Injection. Nunca concatenar valores direto na string.
            string sql = @"INSERT INTO paciente
                               (id_usuario, nome, cpf, telefone, endereco, email, data_nascimento)
                           VALUES
                               (@id_usuario, @nome, @cpf, @telefone, @endereco, @email, @data_nascimento)";

            // O 'using' garante que a conexao seja fechada automaticamente.
            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                // Preenche cada parametro com o valor vindo do objeto.
                cmd.Parameters.AddWithValue("@id_usuario", paciente.IdUsuario);
                cmd.Parameters.AddWithValue("@nome", paciente.Nome);
                cmd.Parameters.AddWithValue("@cpf", paciente.Cpf);
                cmd.Parameters.AddWithValue("@telefone", paciente.Telefone);
                cmd.Parameters.AddWithValue("@endereco", paciente.Endereco);
                cmd.Parameters.AddWithValue("@email", paciente.Email);
                cmd.Parameters.AddWithValue("@data_nascimento", paciente.DataNascimento);

                con.Open();              
                cmd.ExecuteNonQuery();   
            }
        }

        
        // ATUALIZAR: altera os dados de um paciente ja existente.
        
        public void Atualizar(Paciente paciente)
        {
            string sql = @"UPDATE paciente SET
                               nome = @nome,
                               cpf = @cpf,
                               telefone = @telefone,
                               endereco = @endereco,
                               email = @email,
                               data_nascimento = @data_nascimento
                           WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@nome", paciente.Nome);
                cmd.Parameters.AddWithValue("@cpf", paciente.Cpf);
                cmd.Parameters.AddWithValue("@telefone", paciente.Telefone);
                cmd.Parameters.AddWithValue("@endereco", paciente.Endereco);
                cmd.Parameters.AddWithValue("@email", paciente.Email);
                cmd.Parameters.AddWithValue("@data_nascimento", paciente.DataNascimento);
                cmd.Parameters.AddWithValue("@id", paciente.Id); // identifica QUAL paciente atualizar

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        // EXCLUIR: remove um paciente pelo seu id.
        
        public void Excluir(int id)
        {
            string sql = "DELETE FROM paciente WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        // BUSCAR POR ID: retorna um unico paciente, ou null se nao achar.
        
        public Paciente BuscarPorId(int id)
        {
            string sql = "SELECT * FROM paciente WHERE id = @id";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                // ExecuteReader e usado em SELECT, pois retorna dados.
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // se encontrou um registro
                    {
                        return MontarPaciente(reader);
                    }
                }
            }

            return null; // nao encontrou nenhum paciente com esse id
        }

        
        // LISTAR TODOS: retorna uma lista com todos os pacientes.
        
        public List<Paciente> ListarTodos()
        {
            List<Paciente> lista = new List<Paciente>();
            string sql = "SELECT * FROM paciente ORDER BY nome";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // percorre todos os registros
                    {
                        lista.Add(MontarPaciente(reader));
                    }
                }
            }

            return lista;
        }

        
        // BUSCAR POR CPF: retorna o paciente com aquele CPF, ou null.
        // Usado pelo PacienteService para impedir CPF duplicado.
        
        public Paciente BuscarPorCpf(string cpf)
        {
            string sql = "SELECT * FROM paciente WHERE cpf = @cpf";

            using (MySqlConnection con = Conexao.ObterConexao())
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@cpf", cpf);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MontarPaciente(reader);
                    }
                }
            }

            return null; // nenhum paciente com esse CPF
        }

        // METODO AUXILIAR (privado): le uma linha do banco (reader) e
        // monta um objeto Paciente. Evita repetir esse codigo em
        // BuscarPorId e ListarTodos.

        private Paciente MontarPaciente(MySqlDataReader reader)
        {
            Paciente p = new Paciente();

            p.Id = Convert.ToInt32(reader["id"]);
            p.IdUsuario = Convert.ToInt32(reader["id_usuario"]);
            p.Nome = reader["nome"].ToString();
            p.Cpf = reader["cpf"].ToString();
            p.Telefone = reader["telefone"].ToString();
            p.Endereco = reader["endereco"].ToString();
            p.Email = reader["email"].ToString();
            p.DataNascimento = Convert.ToDateTime(reader["data_nascimento"]);

            return p;
        }
    }
}