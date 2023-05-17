using System.Net.Mail;
using System.Net;
using DataBaseModel.Entity;
using System.Net.Mime;

namespace KursProjectDataBase.Services
{
    public class EmailService
    {
        public async Task SentReportAsync(List<Contract> data, string email)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("kursprojecttask5fantokin@gmail.com", "qqpytrfmzcjpaycn");
            smtpClient.EnableSsl = true;

            ReportService reportService = new ReportService();
            string path = "C:\\Users\\Yrulewet\\source\\repos\\LearcnCS\\DataBaseProject\\KursProjectDataBase\\KursProjectDataBase\\Services\\TemporalData\\report.xlsx";
            
            if (File.Exists(path)) File.Delete(path);

            FileStream objFileStream = File.Create(path);
            objFileStream.Close();

            File.WriteAllBytes(path, await reportService.GetDocument(data));
            
            Attachment bag = new Attachment(path, MediaTypeNames.Application.Octet);

            MailAddress from = new MailAddress("kursprojecttask5fantokin@gmail.com", "Отчёт");
            MailAddress to = new MailAddress(email);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Fantokin placements",
                Body = "Отчётные данные",
            };

            message.Attachments.Add(bag);

            smtpClient.Send(message);
            message.Attachments.Dispose();

        }

        public void SentTenant(Contract tenant) 
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("kursprojecttask5fantokin@gmail.com", "qqpytrfmzcjpaycn");
            smtpClient.EnableSsl = true;


            MailAddress from = new MailAddress("kursprojecttask5fantokin@gmail.com", "Информация о сделке");

            string type = tenant.IdPNavigation.IdType == 1 ? "Квартира" : "Дом";
            string payment = tenant.IdPay == 1 ? "Наличные" : "Безналичные";
            string body = $"Вами была успешно арендована квартира по адресу:" +
                $"\n\tРайон: {tenant.IdPNavigation.Area}" +
                $"\n\tУлица: {tenant.IdPNavigation.Street}" +
                $"\n\tНомер: {tenant.IdPNavigation.Number}" +
                $"\n\tТип помещения: {type}\nЭтаж: {tenant.IdPNavigation.Floor}" +
                $"\n\tКоличество м2: {tenant.IdPNavigation.Square}" +
                $"\n\tКоличество комнат: {tenant.IdPNavigation.Room}" +
                $"\n\tСтоимость {tenant.Paymentsize}" +
                $"\n\tСпособ оплаты: {payment}" +
                $"\nАрендодатель: {tenant.IdSNavigation.IdRNavigation.IdUNavigation.Name} {tenant.IdSNavigation.IdRNavigation.IdUNavigation.Surname}" +
                $"\nКонтактные данные: {tenant.IdSNavigation.IdRNavigation.IdUNavigation.Contact}";
                

            MailAddress to = new MailAddress(tenant.IdSNavigation.IdTNavigation.IdUNavigation.Contact!);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Fantokin placements",
                Body = body,
            };

            smtpClient.SendAsync(message, "token");
            message.Attachments.Dispose();
        }

        public void SentRenter(Contract renter)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("kursprojecttask5fantokin@gmail.com", "qqpytrfmzcjpaycn");
            smtpClient.EnableSsl = true;


            MailAddress from = new MailAddress("kursprojecttask5fantokin@gmail.com", "Информация о сделке");

            string type = renter.IdPNavigation.IdType == 1 ? "Квартира" : "Дом";
            string payment = renter.IdPay == 1 ? "Наличные" : "Безналичные";
            string body = $"Ваша квартира теперь арендуется по адресу:" +
                $"\n\tРайон: {renter.IdPNavigation.Area}" +
                $"\n\tУлица: {renter.IdPNavigation.Street}" +
                $"\n\tНомер: {renter.IdPNavigation.Number}" +
                $"\n\tТип помещения: {type}\nЭтаж: {renter.IdPNavigation.Floor}" +
                $"\n\tКоличество м2: {renter.IdPNavigation.Square}" +
                $"\n\tКоличество комнат: {renter.IdPNavigation.Room}" +
                $"\n\tОплата {renter.Paymentsize}" +
                $"\n\tСпособ оплаты: {payment}" +
                $"\nСъёмщик: {renter.IdSNavigation.IdTNavigation.IdUNavigation.Name} {renter.IdSNavigation.IdTNavigation.IdUNavigation.Surname}" +
                $"\nКонтактные данные: {renter.IdSNavigation.IdTNavigation.IdUNavigation.Contact}";

            MailAddress to = new MailAddress(renter.IdSNavigation.IdRNavigation.IdUNavigation.Contact!);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Fantokin placements",
                Body = body,
            };

            smtpClient.SendAsync(message, "token");
            message.Attachments.Dispose();
        }

        public void ChangeData(string to_message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("kursprojecttask5fantokin@gmail.com", "qqpytrfmzcjpaycn");
            smtpClient.EnableSsl = true;


            MailAddress from = new MailAddress("kursprojecttask5fantokin@gmail.com", "Изменение аккаунта");
            MailAddress to = new MailAddress(to_message!);


            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Fantokin placements",
                Body = "Ваши данные были изменены администратором",
            };

            smtpClient.SendAsync(message,null);
            message.Attachments.Dispose();
            Console.WriteLine("Отправлено: " + to_message);
        }
    }
}
