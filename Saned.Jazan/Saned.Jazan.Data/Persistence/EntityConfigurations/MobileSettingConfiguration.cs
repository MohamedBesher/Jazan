using Saned.Jazan.Data.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    class MobileSettingConfiguration : EntityTypeConfiguration<MobileSetting>
    {
        public MobileSettingConfiguration()
        {
            Property(x => x.Value).HasMaxLength(250);
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.SettingType).IsRequired();
            MapToStoredProcedures();
        }
    }
}
