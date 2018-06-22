using ContactProcessor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactProcessor.Models
{
    public class ContactViewModel
    {
        public ContactViewModel() { }
        
        public ContactViewModel(string firstName = "", string lastName = "", string phoneNumber = "", string email = "", string filename = "")
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            FileName = filename;
        }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string FileName { get; set; }

        public override string ToString()
        {
            var contentToAppend = FirstName + "," + LastName + "," + PhoneNumber + "," + Email + "\n";
            return contentToAppend;
        }
    }
    
}