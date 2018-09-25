using Saned.Jazan.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class RefreshTokenConfigurations : EntityTypeConfiguration<RefreshToken>
    {
        public RefreshTokenConfigurations()
        {
            Property(u => u.ProtectedTicket).IsRequired();
            Property(u => u.Subject).IsRequired().HasMaxLength(50);
            Property(u => u.ClientId).IsRequired() .HasMaxLength(100);
        }
    }
}
