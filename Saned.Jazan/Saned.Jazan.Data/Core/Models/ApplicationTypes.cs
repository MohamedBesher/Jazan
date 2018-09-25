using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Models
{
    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    };

    public enum AccountTypeEnum
    {
        User = 1,
        Administrator = 4,
    };

    public enum AccountStatusEnum
    {
        New = 1,
        Active = 2,
        UnActive = 3,
    };
}
