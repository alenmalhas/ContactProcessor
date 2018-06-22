using ContactProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactProcessor.Utilities.Notifier
{
    public interface INotifier
    {
        bool CanNotify(ContactViewModel model);
        void Notify(ContactViewModel model);
    }
}
