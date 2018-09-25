using Saned.Jazan.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class ClientConfigurations : EntityTypeConfiguration<Client>
    {
        public ClientConfigurations()
        {
            Property(u => u.Secret).IsRequired();
            Property(u => u.Name).IsRequired().HasMaxLength(100);
            Property(u => u.AllowedOrigin).HasMaxLength(100);
        }
    }
}
