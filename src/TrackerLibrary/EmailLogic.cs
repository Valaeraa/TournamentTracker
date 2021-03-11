using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace TrackerLibrary
{
    public static class EmailLogic
    {
        public static void SendEmail(string to, string subject, string body)
        {
            SendEmail(new List<string> { to }, new List<string>(), subject, body);
        }

        public static void SendEmail(List<string> to, List<string> bcc, string subject, string body)
        {
            MailAddress fromMailAddress = new MailAddress(GlobalConfig.AppKeyLookup("senderEmail"), GlobalConfig.AppKeyLookup("senderDisplayName"));

            MailMessage mail = new MailMessage();

            foreach (var email in to)
            {
                mail.To.Add(email); 
            }

            foreach (var email in bcc)
            {
                mail.Bcc.Add(email);
            }

            mail.From = fromMailAddress;
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(GlobalConfig.AppKeyLookup("smtpServer"),
                int.Parse(GlobalConfig.AppKeyLookup("smtpPort")));

            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(GlobalConfig.AppKeyLookup("smtpUsername"),
                GlobalConfig.AppKeyLookup("smtpPassword"));
            client.EnableSsl = bool.Parse(GlobalConfig.AppKeyLookup("smtpEnableSsl"));

            client.Send(mail);
        }
    }
}
