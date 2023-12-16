using API.Models;
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
        /*public static List<Report> LoadReports(string query, Report data)
        {
            return Connection.Query<Report, Project, Report>(query, (account, bank) => { account._Bank = bank; return account; }, new { CardNumber = cardNumber }).ToList().First();
        }*/
        public static List<T> LoadData<T, V>(string query, T data, Func<T, V, T> func)
        {
            return Connection.Query<T, V, T>(query, func, data).ToList();
        }
    }
}
