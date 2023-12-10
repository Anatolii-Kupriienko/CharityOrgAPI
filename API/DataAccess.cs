using Dapper;
using System.Data.SqlClient;
using System.Security.Principal;

namespace API
{
    public class DataAccess
    {
        private static readonly string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kursovaDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private static SqlConnection Connection;

        static DataAccess()
        {
            Connection = new(ConnectionString);
        }
        public static int UpdateData<T>(string query, T data)
        {
            return Connection.Execute(query, data);
        }
        public static int UpdateData<T>(string query, T[] data)
        {
            return Connection.Execute(query, data);
        }

        public static int InsertData<T>(string query, T data)
        {
            return Connection.Execute(query, data);
        }
        public static List<T> LoadData<T>(string query, T? data)
        {
            if (data != null)
                return Connection.Query<T>(query, data).ToList();
            else
                return Connection.Query<T>(query).ToList();
        }
        public static List<T> GetDataById<T>(string query, int id)
        {
            return Connection.Query<T>(query, new { Id = id }).ToList();
        }
    }
}
