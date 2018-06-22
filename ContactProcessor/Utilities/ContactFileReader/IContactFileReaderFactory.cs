using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactProcessor.Utilities.ContactFileReader
{
    public interface IContactFileReaderFactory
    {
        IContactFileReader Create();
    }
}
