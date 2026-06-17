using System;
using MySql.Data.MySqlClient;

namespace ClinicaMedica.Data
{
    // Classe responsável por fornecer a conexão com o banco MySQL.
    // Centraliza a "string de conexão" em um único lugar, para que
    // todas as classes de acesso a dados (DAO) usem a mesma fonte.

    public class Conexao
    {
        // String de conexão com o banco.
        private static readonly string stringConexao =
            "Server=localhost;" +
            "Database=clinica_medica;" +
            "Uid=root;" +
            "Pwd=";

        // Retorna um novo objeto de conexão (ainda fechado).
        public static MySqlConnection ObterConexao()
        {
            return new MySqlConnection(stringConexao);
        }
    }
}