using ClosedXML.Excel;
using Domain.Core;
using System.Data;

namespace Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        public async Task<XLWorkbook> CreateFile(DataTable data)
        {
            var t = Task.Run(() =>
            {
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Proveedores");
                ws.Cell(2, 1).Value = "Lista de Proveedores";
                ws.Range(2, 1, 2, 8).Merge().AddToNamed("Titles");

                var table = ws.Cell(3, 1).InsertTable(data, true);

                table.Field("id").Name = "ID Proveedor";
                table.Field("nombre").Name = "Nombre";
                table.Field("fechaalta").Name = "Fecha Alta";
                table.Field("rfc").Name = "RFC";
                table.Field("direccion").Name = "Direccion";
                table.Field("activo").Name = "Activo";
                table.Field("fechacreacion").Name = "Fecha Creacion";
                table.Field("fechamodificacion").Name = "Fecha Modificacion";

                foreach (var item in table.Field("Activo").DataCells)
                {
                    item.Value = (item.Value.ToString().ToLower() == "true") ? "SI" : "NO";
                }
                // Prepare the style for the titles
                var titlesStyle = wb.Style;
                titlesStyle.Font.Bold = true;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titlesStyle.Fill.BackgroundColor = XLColor.Amber;

                // Format all titles in one shot
                wb.Range("Titles").Style = titlesStyle;

                return wb;
            });

            return await t;
        }
    }
}
