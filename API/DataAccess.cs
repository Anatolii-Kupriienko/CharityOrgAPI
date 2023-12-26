using API.Models;
using Dapper;
using System.Data.SqlClient;
using System.Security.Principal;

namespace API
{
    public class DataAccess
    {
        private static readonly string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kursovaDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        public static int UpdateData(string query, object data)
        {
            SqlConnection connection = new(ConnectionString);
            return connection.Execute(query, data);
        }

        public static List<T> LoadData<T>(string query, object? data)
        {
            SqlConnection connection = new(ConnectionString);
            if (data != null)
                return connection.Query<T>(query, data).ToList();
            else
                return connection.Query<T>(query).ToList();
        }
        public static List<T> LoadData<T, V>(string query, object data, Func<T, V, T> func)
        {
            SqlConnection connection = new(ConnectionString);
            return connection.Query(query, func, data).ToList();

        }
        public static List<T> LoadData<T, V, U>(string query, object data, Func<T, V, U, T> func)
        {
            SqlConnection connection = new(ConnectionString);
            return connection.Query(query, func, data).ToList();
        }
    }
}
