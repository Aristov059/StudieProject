using System.Data.SqlClient;

namespace WorldPC
{
    internal class DataBase
    {
        // Строка подключения к БД.

        SqlConnection SqlConnect = new SqlConnection(@"Data Source=DESKTOP-VLO3SHC\MYSQL; Initial Catalog = WorldPC; Integrated Security = True");

        public void OpenConnection()
        {
            if (SqlConnect.State == System.Data.ConnectionState.Closed)
            {
                SqlConnect.Open();
            }
        }

        public void CloseConnection()
        {
            if (SqlConnect.State == System.Data.ConnectionState.Open)
            {
                SqlConnect.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return SqlConnect;
        }
    }
}
