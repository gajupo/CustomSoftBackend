using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
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

        public async Task<int> ExecuteNonQuerySPAsync(string procedureName, CancellationToken cancellationToken, params object[] parameters)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            using var tran = await conn.BeginTransactionAsync();

            using var cmd = new NpgsqlCommand(procedureName, conn, tran) { CommandType = CommandType.StoredProcedure };
            for (var i = 1; i <= parameters.Length; i++)
            {
                cmd.Parameters.Add(new() { Value = parameters[i - 1] });
            }

            var affectedRows = await cmd.ExecuteNonQueryAsync();

            tran.Commit();

            return affectedRows;
        }

        public async Task<DataSet> ExecuteQuerySPAsync(string procedureName, CancellationToken cancellationToken, params (string, object)[] parameters)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync(cancellationToken);
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

        public async Task<DataTable> ExecuteQueryFuncAsync(string funcName, CancellationToken cancellationToken, params object[] parameters)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync(cancellationToken);

                // Start a transaction as cursors require to run within a transaction in PostgreSQL
                using (var tran = await conn.BeginTransactionAsync())
                {
                    // Call the function which returns a cursor
                    using (var cmd = new NpgsqlCommand($"SELECT {funcName}", conn, tran))
                    {

                        for (var i = 1; i <= parameters.Length; i++)
                        {
                            cmd.Parameters.Add(new() { Value = parameters[i - 1] });
                        }

                        // Execute the function that returns the cursor
                        string cursorName = (string)await cmd.ExecuteScalarAsync();

                        // Fetch rows from the cursor
                        using (var cursorCmd = new NpgsqlCommand("FETCH ALL IN \"" + cursorName + "\"", conn))
                        {
                            cursorCmd.Transaction = tran;

                            using (var reader = await cursorCmd.ExecuteReaderAsync())
                            {
                                var dataTable = new DataTable();
                                dataTable.Load(reader);
                                
                                return dataTable;
                            }
                        }
                    } 
                }
            }
        }

        public async Task<int> ExecuteNonQueryFuncAsync(string funcName, CancellationToken cancellationToken, params object[] parameters)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync(cancellationToken);

                using var tran = await conn.BeginTransactionAsync();
                using var cmd = new NpgsqlCommand($"SELECT {funcName}", conn, tran);

                for (var i = 1; i <= parameters.Length; i++)
                {
                    cmd.Parameters.Add(new() { Value = parameters[i - 1] });
                }

                var returnedValue = await cmd.ExecuteScalarAsync();
                if (returnedValue != null)
                {
                    tran.Commit();
                    return (int)returnedValue;
                }
                else
                    return 0;
            }
        }
    }
}
