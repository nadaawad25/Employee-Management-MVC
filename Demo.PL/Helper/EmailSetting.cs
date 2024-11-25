//using Deno.DAL.Models;
//using System.Net;
//using System.Net.Mail;

//namespace Demo.PL.Helper
//{
//    public static class EmailSetting
//    {
//        public static void  SendEmail(Email email)
//        {
//            var Client = new SmtpClient("smtp.gmail.com", 587);
//            Client.EnableSsl = true;
//            Client.Credentials = new NetworkCredential("nadaawad112002@gmail.com", "pfgahdysjnyduupg");
//            Client.Send("nadaawad112002@gmail.com", email.To, email.Subject, email.Body);



//        }
//    }
//}
using Deno.DAL.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Demo.PL.Helper
{
    public static class EmailSetting
    {
        public static async Task SendEmailAsync(Email email)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;

                client.Credentials = new NetworkCredential("nadaawad112002@gmail.com", "pfgahdysjnyduupg");

                var mailMessage = new MailMessage("nadaawad112002@gmail.com", email.To, email.Subject, email.Body);

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
    }
}
