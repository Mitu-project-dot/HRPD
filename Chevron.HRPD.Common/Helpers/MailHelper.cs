using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;

namespace Chevron.HRPD.Common.Helpers
{
    public static class MailHelper
    {
        static MailHelper()
        {
            if (ConfigurationManager.GetSection("system.net/mailSettings") == null)
            {
                throw new ConfigurationErrorsException("The mail settings have not been configured");
            }
        }

        /// <summary>
        /// Sends an email synchronously
        /// </summary>
        public static void Send(MailMessage mail)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.Send(mail);
            }
        }

        /// <summary>
        /// Sends an email asynchronously
        /// </summary>
        public static void SendAsync(MailMessage mail)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.SendAsync(mail, null);

                client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
            }
        }

        static void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error != null)
            {
                throw e.Error;
            }
        }

        public static void Send(string to, string subject, string body)
        {
            Send(to, subject, body, false);
        }

        public static void Send(string to, string subject, string body, bool isBodyHtml)
        {
            Send(new List<string> { to }, subject, body, isBodyHtml);
        }

        public static void Send(List<string> to, string subject, string body)
        {
            Send(to, subject, body, false);
        }

        public static void Send(List<string> to, string subject, string body, bool isBodyHtml)
        {
            MailMessage message = new MailMessage();

            foreach (string address in to)
            {
                message.To.Add(address);
            }

            message.Subject = subject;

            message.Body = body;

            message.IsBodyHtml = isBodyHtml;

            Send(message);
        }
    }
}
