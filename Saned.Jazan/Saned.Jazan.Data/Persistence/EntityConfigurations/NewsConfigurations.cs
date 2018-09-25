using Saned.Jazan.Data.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class NewsConfigurations : EntityTypeConfiguration<News>
    {
        public NewsConfigurations()
        {
            HasKey(a => a.Id);
            Property(n => n.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(n => n.Title).HasMaxLength(300).IsRequired();
            Property(n => n.Details).IsRequired();
        }
    }
}
