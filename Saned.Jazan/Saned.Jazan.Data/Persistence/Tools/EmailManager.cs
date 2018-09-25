using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtiltyManagemnt;

namespace Saned.Jazan.Data.Persistence.Tools
{
    public class EmailManager : IDisposable
    {
        readonly ApplicationDbContext _context;

        public EmailManager()
        {
            _context = new ApplicationDbContext();

        }

        public string SendActivationEmail(string messageTamplate, string toEmail, string messageBodyAr)
        {
            string result = "";

            EmailSettingRepository emailSettingRepository = new EmailSettingRepository();
            EmailSetting emailSettings = emailSettingRepository.GetEmailSetting(messageTamplate);

            result = Utilty.SendMail(emailSettings.Host, emailSettings.FromEmail, emailSettings.Password, toEmail, emailSettings.SubjectAr, messageBodyAr, "");

            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
