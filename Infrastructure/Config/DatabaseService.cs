using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> ExecuteNonQueryAsync(string procedureName, params (string, object)[] parameters)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(procedureName, conn) { CommandType = CommandType.StoredProcedure })
                {
                    foreach (var (paramName, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(paramName, value ?? DBNull.Value);
                    }

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<DataSet> ExecuteQueryAsync(string procedureName, params (string, object)[] parameters)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(procedureName, conn) { CommandType = CommandType.StoredProcedure })
                {
                    foreach (var (paramName, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(paramName, value ?? DBNull.Value);
                    }

                    var adapter = new NpgsqlDataAdapter(cmd);
                    var result = new DataSet();
                    adapter.Fill(result);
                    return result;
                }
            }
        }
    }
}
