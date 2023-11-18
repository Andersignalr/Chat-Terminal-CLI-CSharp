// using Microsoft.Data.Sqlite => Este namespace fornece classes para trabalhar com o SQLite
// usando o provedor de dados SQLite da Microsoft.
using Microsoft.Data.Sqlite;
// using SQLitePCL => é necessário para usar o Batteries.Init() e é necessário para usar o SQLite
using SQLitePCL;


namespace UTF16BoxDrawing
{
    public static class Database
    {
        private static SqliteConnection Open()
        {
            Batteries.Init();

            SqliteConnection connection = new SqliteConnection("Data Source=mensagens.db;");

            connection.Open();

            return connection;
        }

        public static void Initialize()
        {
            using (SqliteConnection connection = Open())
            {
                string createTableQuery = "CREATE TABLE IF NOT EXISTS Mensagens(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "Content TEXT, RemetentId TEXT, CreatedAt TEXT);";
                using (SqliteCommand createTableCommand = new SqliteCommand(createTableQuery, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }

        public static int SalvarMensagem(Mensagem mensagem)
        {
            int generatedId = -1;

            using (SqliteConnection connection = Open())
            {
                string insertQuery = "INSERT INTO Mensagens (Content, RemetentId, CreatedAt) VALUES (@Content, @RemetentId, @CreatedAt);";
                using (SqliteCommand insertCommand = new SqliteCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@Content", mensagem.Content);
                    insertCommand.Parameters.AddWithValue("@RemetentId", mensagem.RemetentId);
                    insertCommand.Parameters.AddWithValue("@CreatedAt", mensagem.CreatedAt);
                    insertCommand.ExecuteNonQuery();

                    string selectIdQuery = "SELECT last_insert_rowid();";

                    using (SqliteCommand selectIdCommand = new SqliteCommand(selectIdQuery, connection))
                    {
                        object? result = selectIdCommand.ExecuteScalar();

                        if(result == null || !int.TryParse(result.ToString(), out generatedId))
                        {
                            throw new Exception("id recebido é inválido");
                        }
                    }
                }
            }
            return generatedId;
        }

        public static List<Mensagem> GetMensagens()
        {
            List<Mensagem> mensagens = new List<Mensagem>();
            using (SqliteConnection connection = Open())
            {
                string selectQuery = "SELECT * FROM Mensagens;";
                using (SqliteCommand selectCommand = new SqliteCommand(selectQuery, connection))
                {
                    using (SqliteDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mensagens.Add(new Mensagem
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Content = reader["Content"].ToString(),
                                RemetentId = reader["RemetentId"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                            });
                        }
                    }
                }
            }

            return mensagens;
        }

        public static void Close(SqliteConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
