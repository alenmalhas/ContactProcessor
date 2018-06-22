using ContactProcessor.Utilities.ConfigManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ContactProcessor.Utilities.EmailClient
{
    public class EmailClient : IEmailClient
    {
        private IConfigManager _configManager;
        public EmailClient(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public void Send(string from, string to, string subject, string body)
        {
            using (MailMessage message = new MailMessage(from, to))
            using (SmtpClient client = new SmtpClient(_configManager.Get(Constants.AppConfigKey_SmtpHost)))
            {
                message.Subject = subject;
                message.Body = body;

                client.UseDefaultCredentials = true;
                client.Send(message);
            }
        }
    }
}