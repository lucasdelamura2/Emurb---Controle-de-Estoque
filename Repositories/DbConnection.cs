using Microsoft.Data.SqlClient;

namespace EmurbEstoque.Repositories
{
        public abstract class DbConnection : IDisposable
    {
        protected SqlConnection conn;
        public DbConnection(string? connStr)
        {
            conn = new SqlConnection(connStr);
            conn.Open();
        }
        public void Dispose()
        {
            conn.Close();
        }
    }
}