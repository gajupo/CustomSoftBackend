using ClosedXML.Excel;
using System.Data;

namespace Domain.Core
{
    public interface IExcelService
    {
        Task<XLWorkbook> CreateFile(DataTable data);
    }
}
