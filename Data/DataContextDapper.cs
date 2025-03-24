using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace CarInventory.Data
{
  class DataContextDapper
  {
    private string _connectionString;
    public DataContextDapper(IConfiguration config)
    {
      _connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public IEnumerable<T> LoadData<T>(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.Query<T>(sql);
    }

    public T LoadDataSingle<T>(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.QuerySingle<T>(sql);
    }

    public int ExecuteSql(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.Execute(sql);
    }

    public bool ExecuteSqlBool(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.Execute(sql) > 0;
    }
  }
}