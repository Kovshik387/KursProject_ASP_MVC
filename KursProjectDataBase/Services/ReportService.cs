namespace KursProjectDataBase.Services
{
    using DataBaseModel.Entity;
    using OfficeOpenXml;
    public interface IDocumentContract : System.IDisposable
    {
        public abstract Task<byte[]> GetDocument(List<Contract> data);
    }
    public class ReportService : IDocumentContract
    {
        protected ILogger<ReportService> Logger { get; set; } = default!;
        public void Dispose() => this.Logger.LogInformation("report Disposed");
        
        public async Task<byte[]> GetDocument(List<Contract> data)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var excelDocument = new ExcelPackage();
            var workSheet = excelDocument.Workbook.Worksheets.Add("Съёмщики");

            workSheet.DefaultRowHeight = 12;
            workSheet.DefaultColWidth = 20;
            for (var index = 1; index <= 5; index++) workSheet.Column(index).AutoFit();

            workSheet.Cells[1, 1].Value = "Имя"; workSheet.Cells[1, 2].Value = "Фамилия";
            workSheet.Cells[1, 3].Value = "Контактн"; workSheet.Cells[1, 4].Value = "Пол";
            workSheet.Cells[1, 5].Value = "Рейтинг";

            workSheet.Row(1).Style.Font.Bold = true; workSheet.Row(1).Height = 20;

            
            for (var index = 0; index < data.Count; index++)
            {
                var formated = data[index].IdSNavigation.IdTNavigation.IdUNavigation;
                workSheet.Cells[index + 2, 1].Value = formated.Name;
                workSheet.Cells[index + 2, 2].Value = formated.Surname;
                workSheet.Cells[index + 2, 3].Value = formated.Contact;
                workSheet.Cells[index + 2, 4].Value = formated.Sex;
                workSheet.Cells[index + 2, 5].Value = data[index].IdSNavigation.IdTNavigation.Rating;
            }


            var workSheetRenter = excelDocument.Workbook.Worksheets.Add("Арендодатели");
            workSheetRenter.DefaultRowHeight = 12;
            workSheetRenter.DefaultColWidth = 20;
            for (var index = 1; index <= 5; index++) workSheetRenter.Column(index).AutoFit();

            workSheetRenter.Cells[1, 1].Value = "Имя"; workSheetRenter.Cells[1, 2].Value = "Фамилия";
            workSheetRenter.Cells[1, 3].Value = "Контактн"; workSheetRenter.Cells[1, 4].Value = "Пол";
            workSheetRenter.Cells[1, 5].Value = "Лицензия";

            workSheetRenter.Row(1).Style.Font.Bold = true; workSheetRenter.Row(1).Height = 20;


            for (var index = 0; index < data.Count; index++)
            {
                var formated = data[index].IdSNavigation.IdRNavigation.IdUNavigation;
                workSheetRenter.Cells[index + 2, 1].Value = formated.Name;
                workSheetRenter.Cells[index + 2, 2].Value = formated.Surname;
                workSheetRenter.Cells[index + 2, 3].Value = formated.Contact;
                workSheetRenter.Cells[index + 2, 4].Value = formated.Sex;
                workSheetRenter.Cells[index + 2, 5].Value = data[index].IdSNavigation.IdRNavigation.License.ToString();
            }

            var workSheetPlacement = excelDocument.Workbook.Worksheets.Add("Помещения");
            workSheetPlacement.DefaultRowHeight = 12;
            workSheetPlacement.DefaultColWidth = 20;
            for (var index = 1; index <= 10; index++) workSheetPlacement.Column(index).AutoFit();

            workSheetPlacement.Cells[1, 1].Value = "Арендодатель"; workSheetPlacement.Cells[1, 2].Value = "Этаж";
            workSheetPlacement.Cells[1, 3].Value = "Площадь м2"; workSheetPlacement.Cells[1, 4].Value = "Кол-во комнат";
            workSheetPlacement.Cells[1, 5].Value = "Район"; workSheetPlacement.Cells[1, 6].Value = "Улица";
            workSheetPlacement.Cells[1, 6].Value = "Номер"; workSheetPlacement.Cells[1, 8].Value = "Тип помещения";
            workSheetPlacement.Cells[1, 9].Value = "Стоимость";

            workSheetPlacement.Row(1).Style.Font.Bold = true; workSheetPlacement.Row(1).Height = 20;


            for (var index = 0; index < data.Count; index++)
            {
                var formated = data[index].IdPNavigation;
                workSheetPlacement.Cells[index + 2, 1].Value = formated.IdRNavigation.IdUNavigation.Name;
                workSheetPlacement.Cells[index + 2, 2].Value = formated.Floor;
                workSheetPlacement.Cells[index + 2, 3].Value = formated.Square;
                workSheetPlacement.Cells[index + 2, 4].Value = formated.Room;
                workSheetPlacement.Cells[index + 2, 5].Value = formated.Area;
                workSheetPlacement.Cells[index + 2, 6].Value = formated.Street;
                workSheetPlacement.Cells[index + 2, 7].Value = formated.Number;
                workSheetPlacement.Cells[index + 2, 8].Value = formated.IdType == 1 ? "Квартира" : "Дом";
                workSheetPlacement.Cells[index + 2, 9].Value = data[index].Paymentsize;
            }

            var workSheetContract = excelDocument.Workbook.Worksheets.Add("Контракты");
            workSheetContract.DefaultRowHeight = 12;
            workSheetContract.DefaultColWidth = 20;
            for (var index = 1; index <= 10; index++) workSheetPlacement.Column(index).AutoFit();

            workSheetContract.Cells[1, 1].Value = "Арендодатель"; workSheetContract.Cells[1, 2].Value = "Съёмщик";
            workSheetContract.Cells[1, 3].Value = "Этаж"; workSheetContract.Cells[1, 4].Value = "Площадь м2"; 
            workSheetContract.Cells[1, 5].Value = "Кол-во комнат"; workSheetContract.Cells[1, 6].Value = "Район";
            workSheetContract.Cells[1, 7].Value = "Улица"; workSheetContract.Cells[1, 8].Value = "Номер"; 
            workSheetContract.Cells[1, 9].Value = "Тип"; workSheetContract.Cells[1, 10].Value = "Стоимость";
            workSheetContract.Cells[1, 11].Value = "Способ оплаты";

            workSheetContract.Row(1).Style.Font.Bold = true; workSheetContract.Row(1).Height = 20;


            for (var index = 0; index < data.Count; index++)
            {
                if (data[index].IdSNavigation.IdT == null) continue;
                var formated = data[index].IdPNavigation;
                workSheetContract.Cells[index + 2, 1].Value = formated.IdRNavigation.IdUNavigation.Name;
                workSheetContract.Cells[index + 2, 2].Value = data[index].IdSNavigation.IdTNavigation.IdUNavigation.Name;
                workSheetContract.Cells[index + 2, 3].Value = formated.Floor;
                workSheetContract.Cells[index + 2, 4].Value = formated.Square;
                workSheetContract.Cells[index + 2, 5].Value = formated.Room;
                workSheetContract.Cells[index + 2, 6].Value = formated.Area;
                workSheetContract.Cells[index + 2, 7].Value = formated.Street;
                workSheetContract.Cells[index + 2, 8].Value = formated.Number;
                workSheetContract.Cells[index + 2, 9].Value = formated.IdType == 1 ? "Квартира" : "Дом";
                workSheetContract.Cells[index + 2, 10].Value = data[index].Paymentsize;
                workSheetContract.Cells[index + 2, 11].Value = data[index].IdPay == 1 ? "Наличные" : "Безналичные";
            }

            Console.WriteLine("Created");
            return await excelDocument.GetAsByteArrayAsync();
        }
    }
}
