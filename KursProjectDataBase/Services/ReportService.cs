using DataBaseModel.Entity;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursProjectDataBase.Services
{
    public class ReportService
    {
        public void ExportExcel(Tuple<Tenant,Renter> data)
        {
            _Application app = new Application();
            _Workbook workbook = app.Workbooks.Add(Type.Missing);
            _Worksheet worksheet = null;

            app.Visible = false;

/*            worksheet = workbook.Sheets["Съёмщики"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from gridView";*/
/*
            for (int i = 1; i < dataGrid.Columns.Count + 1; i++) worksheet.Cells[1, i] = dataGrid.Columns[i - 1].HeaderText;

            for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataGrid.Rows[i].Cells[j].Value.ToString();
                }
            }*/

            workbook.SaveAs("Ou",
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange);

            app.Quit();
        }
    }
}