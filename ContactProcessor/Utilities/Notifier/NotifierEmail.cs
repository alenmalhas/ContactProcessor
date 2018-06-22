using ContactProcessor.Models;
using ContactProcessor.Utilities.ConfigManager;
using ContactProcessor.Utilities.EmailClient;
using ContactProcessor.Utilities.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactProcessor.Utilities.Notifier
{
    public class NotifierEmail : INotifier
    {
        private readonly IEmailClient _emailClient;
        private readonly IConfigManager _configManager;
        private readonly ILogger _logger;

        public NotifierEmail(IConfigManager configManager, IEmailClient emailClient, ILogger logger)
        {
            _configManager = configManager;
            _emailClient = emailClient;
            _logger = logger;
        }

        public bool CanNotify(ContactViewModel model)
        {
            var canNotify = !string.IsNullOrEmpty(model.Email)
                && model.Email.Contains("@");

            return canNotify;
        }

        public void Notify(ContactViewModel model)
        {
            SendEmail(model);
        }

        private void SendEmail(ContactViewModel contact)
        {
            var to = contact.Email;
            var from = _configManager.Get(Constants.AppConfigKey_EmailShotFrom);
            var subject = _configManager.Get(Constants.AppConfigKey_EmailShotSubject);
            var body = _configManager.Get(Constants.AppConfigKey_EmailShotBody);

            try
            {
                _emailClient.Send(from, to, subject, body);
                _logger.Log($"Email sent from: {from}, to: {to}", LogLevel.Info);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occured while sending email to contact: {contact}. Exception details: {ex.Message}", LogLevel.Error);
            }
        }
    }
}