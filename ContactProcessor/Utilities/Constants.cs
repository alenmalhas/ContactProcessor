using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactProcessor.Utilities
{
    public class Constants
    {
        internal const string AppConfigKey_fileUploadFolderRelativePath= "fileUploadFolderRelativePath";
        
        internal const string AppConfigKey_EmailShotFrom= "EmailShotFrom";
        internal const string AppConfigKey_EmailShotSubject = "EmailShotSubject";
        internal const string AppConfigKey_EmailShotBody = "EmailShotBody";

        internal const string AppConfigKey_SmtpHost = "SmtpHost";
        internal const string AppConfigKey_EmailSentMessage = "EmailSentMessage";

        internal const string IOC_NotifierEmail = "NotifierEmail";
        internal const string IOC_NotifierText = "NotifierText";
    }
}