using ContactProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactProcessor.Utilities.ContactFileReader
{
    public interface IContactFileReader : IDisposable
    {
        IEnumerable<ContactViewModel> GetContacts(string fileFullPath);
    }
}
