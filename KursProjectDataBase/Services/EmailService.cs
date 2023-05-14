using System.Net.Mail;
using System.Net;

namespace KursProjectDataBase.Services
{
    public class EmailService
    {
        public string? To_Message { get; set; } = "pumb00sable@gmail.com";
        // Отправка файла json на почту
        public void MessageSend()
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("kursprojecttask5fantokin@gmail.com", "qqpytrfmzcjpaycn");
            smtpClient.EnableSsl = true;


            MailAddress from = new MailAddress("kursprojecttask5fantokin@gmail.com", "Изменение аккаунта");
            MailAddress to = new MailAddress(To_Message!);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Fantokin placements",
                Body = "Ваши данные были изменены администратором",
            };

            smtpClient.SendAsync(message,"d");
            message.Attachments.Dispose();
            Console.WriteLine("Отправлено: " + To_Message);
        }
    }
}
