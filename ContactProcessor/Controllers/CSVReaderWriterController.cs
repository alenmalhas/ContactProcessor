using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ContactProcessor.Models;
using ContactProcessor.Utilities;

namespace ContactProcessor.Controllers
{
    public class CSVReaderWriterController : Controller
    {
        private readonly IEmailClient _emailClient;

        public CSVReaderWriterController(IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        [HttpGet]
        public ActionResult Read()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var splitfilename = fileName.Split('.').ToArray();

                fileName = splitfilename[0] + Guid.NewGuid() + '.' + splitfilename[1];

                file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName));

                var reader = new StreamReader(Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName));

                var model = new DisplayInputViewModel();

                model.FileName = fileName;
                model.Contacts = new List<ContactViewModel>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    model.Contacts.Add(new ContactViewModel(values[0], values[1], values[2], values[3]));
                }

                reader.Close();

                return View("DisplayInput", model);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Write(string filename)
        {
            var model = new NewContactViewModel("", "", "", "", filename);
            return View(model);
        }

        [HttpPost]
        public ActionResult Write(NewContactViewModel model)
        {
            System.IO.File.AppendAllText(Path.Combine(Server.MapPath("~/App_Data/Uploads"), model.FileName),
                model.FirstName + "," + model.LastName + "," + model.PhoneNumber + "," + model.Email + "\n");

            var reader = new StreamReader(Path.Combine(Server.MapPath("~/App_Data/Uploads"), model.FileName));

            var data = new DisplayInputViewModel();

            data.FileName = model.FileName;
            data.Contacts = new List<ContactViewModel>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                data.Contacts.Add(new ContactViewModel(values[0], values[1], values[2], values[3]));
            }

            reader.Close();

            return View("DisplayInput", data);
        }

        public ActionResult Process(string filename)
        {
            var reader = new StreamReader(Path.Combine(Server.MapPath("~/App_Data/Uploads"), filename));

            var data = new DisplayInputViewModel();

            data.FileName = filename;
            data.Contacts = new List<ContactViewModel>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (values[3].Contains("@"))
                {
                    var to = values[3];
                    var from = "youremail@yourservername.com";
                    var subject = "Automated Email";
                    var body = @"Hi, You are received this as you exist in my contacts list.";
                    /*
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = "Automated Email";
                    message.Body = @"Hi, You are received this as you exist in my contacts list.";
                    SmtpClient client = new SmtpClient("yourservername");

                    client.UseDefaultCredentials = true;

                    client.Send(message);
                    */
                    _emailClient.Send(from, to, subject, body);
                }
                else if (values[2].Contains("07"))
                {
                    //TODO: send text message
                }
            }

            return Content("Email Sent");
        }
    }
}