using Saned.Jazan.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    /// <summary>
    /// Configure Package Enitiy
    /// </summary>
    class PackageConfiguration : EntityTypeConfiguration<Package>
    {
        public PackageConfiguration()
        {
            Property(x => x.ArabicName).IsRequired().HasMaxLength(250);
            Property(x => x.Period).IsOptional();
            Property(x => x.CreatedOn).IsRequired();
            Property(x => x.CreatedBy).IsRequired();
            Property(x => x.UpdatedOn).IsOptional();
            Property(x => x.UpdatedBy).IsOptional();
        }
    }
}
