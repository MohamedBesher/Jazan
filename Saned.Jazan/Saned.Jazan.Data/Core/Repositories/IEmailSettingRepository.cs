using Saned.Jazan.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface IEmailSettingRepository  : IBaseRepository<EmailSetting> , IDisposable
    {
        EmailSetting GetEmailSetting(string type);
    }
}
