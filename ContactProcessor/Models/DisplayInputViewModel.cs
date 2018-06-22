using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactProcessor.Controllers;

namespace ContactProcessor.Models
{
    public class DisplayInputViewModel
    {
        public string FileName { get; set; }
        public List<ContactViewModel> Contacts { get; set; }

    }
}