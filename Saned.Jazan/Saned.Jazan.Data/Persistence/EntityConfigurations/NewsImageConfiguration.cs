using Saned.Jazan.Data.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class NewsImageConfiguration : EntityTypeConfiguration<NewsImage>
    {
        public NewsImageConfiguration()
        {
            HasKey(a => a.ImageId);
            Property(n => n.ImageId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(nm => nm.IsDefault).IsRequired();
     
            HasRequired<News>(s => s.News)
             .WithMany(s => s.NewsImages);
        }
    }
}
