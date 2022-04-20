using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace MyGanServer.Helper
{
    public class EmailSender
    {
        public static void SendEmail(string subject, string body, string to, string toName, string from, string fromName, string pswd, string smtpUrl)
        {
            var fromAddress = new MailAddress(from, fromName);
            var toAddress = new MailAddress(to, toName);
            string fromPassword = pswd;

            var smtp = new SmtpClient
            {
                Host = smtpUrl,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
