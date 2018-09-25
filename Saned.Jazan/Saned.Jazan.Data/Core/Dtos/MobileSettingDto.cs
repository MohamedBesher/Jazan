using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Dtos
{
    public class MobileSettingDto
    {
        public int Id { get; set; }

        public string SettingTypeName { get; set; }

        public int SettingTypeId { get; set; }

        public string Value { get; set; }
    }
}
