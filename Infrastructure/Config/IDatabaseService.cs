using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config
{
    public interface IDatabaseService
    {
        Task<int> ExecuteNonQueryAsync(string procedureName, params (string, object)[] parameters);
        Task<DataSet> ExecuteQueryAsync(string procedureName, params (string, object)[] parameters);
    }
}
