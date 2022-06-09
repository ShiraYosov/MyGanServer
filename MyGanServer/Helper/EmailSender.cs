using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using SendGridLib;


namespace MyGanServer.Helper
{
    public class EmailSender
    {
        public static async void SendEmail2(string subject, string body, string to, string toName, string from, string fromName, string pswd, string smtpUrl)
        {
           await MailSender.SendEmail(fromName, to, toName, subject, body, "");
        }
    }
}
