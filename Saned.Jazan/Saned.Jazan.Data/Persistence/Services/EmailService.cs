using Microsoft.AspNet.Identity;
using Saned.Jazan.Data.Persistence.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Persistence.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return configSendGridasync(message);
        }
        private Task configSendGridasync(IdentityMessage message)
        {
            EmailManager mngMail = new EmailManager();
            string str = mngMail.SendActivationEmail(message.Subject, message.Destination, message.Body);

            return Task.FromResult(0);
        }
    }

}
