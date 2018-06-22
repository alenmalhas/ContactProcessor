using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ContactProcessor.Utilities
{
    public interface IEmailClient
    {
        void Send(string from, string to, string subject, string body);
    }
    public class EmailClient : IEmailClient
    {
        private IConfigManager _configManager;
        public EmailClient(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public void Send(string from, string to, string subject, string body)
        {
            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;//"Automated Email";
            message.Body = body;//@"Hi, You are received this as you exist in my contacts list.";
            var serverName = _configManager.Get("yourservername");
            SmtpClient client = new SmtpClient(_configManager.Get("yourservername"));

            client.UseDefaultCredentials = true;

            client.Send(message);
        }
    }
}