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
        Task<int> ExecuteNonQuerySPAsync(string procedureName, CancellationToken cancellationToken, params object[] parameters);
        Task<DataTable> ExecuteQueryFuncAsync(string funcName, CancellationToken cancellationToken, params (string, object)[] parameters);
        Task<int> ExecuteNonQueryFuncAsync(string funcName, CancellationToken cancellationToken, params object[] parameters);
    }
}
