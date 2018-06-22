using ContactProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactProcessor.Utilities.Notifier
{
    public class NotifierText : INotifier
    {
        public bool CanNotify(ContactViewModel model)
        {
            return model.PhoneNumber.Contains("07");
        }

        public void Notify(ContactViewModel model)
        {
            //TODO: send text message
        }
    }
}