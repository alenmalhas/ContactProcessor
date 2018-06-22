using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactProcessor.Utilities.EmailClient
{
    public interface IEmailClient
    {
        void Send(string from, string to, string subject, string body);
    }
}
