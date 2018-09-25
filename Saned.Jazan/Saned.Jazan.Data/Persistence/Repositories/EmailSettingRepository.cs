using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class EmailSettingRepository : BaseRepository<EmailSetting>, IEmailSettingRepository
    {
        //private readonly ApplicationDbContext _context;

        //public EmailSettingRepository(ApplicationDbContext _context) : base(_context)
        //{
        //    _context = new ApplicationDbContext();
        //}
        public EmailSetting GetEmailSetting(string type)
        {
            return _context.EmailSettings.SingleOrDefault(u => u.EmailSettingType == type.Trim());
        }
    }
}
