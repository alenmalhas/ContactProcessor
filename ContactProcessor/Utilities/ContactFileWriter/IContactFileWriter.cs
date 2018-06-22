using ContactProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactProcessor.Utilities.ContactFileWriter
{
    public interface IContactFileWriter : IDisposable
    {
        void AddContact(string fileFullPath, ContactViewModel contact);
    }
}
