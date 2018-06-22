using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactProcessor.Models
{
    public class ContactViewModel
    {
        public ContactViewModel()
        {
            
        }

        public ContactViewModel(string s, string s1, string s2, string s3)
        {
            FirstName = s;
            LastName = s1;
            PhoneNumber = s2;
            Email = s3;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class NewContactViewModel : ContactViewModel
    {
        public NewContactViewModel()
        {
            
        }

        public NewContactViewModel(string s, string s1, string s2, string s3, string s4) : base(s, s1, s2, s3)
        {
            FirstName = s;
            LastName = s1;
            PhoneNumber = s2;
            Email = s3;
            FileName = s4;
        }

        public string FileName { get; set; }
    }
}