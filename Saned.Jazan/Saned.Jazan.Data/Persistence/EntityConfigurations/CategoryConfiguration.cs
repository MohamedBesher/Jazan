

using Saned.Jazan.Data.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            HasKey(a => a.CategoryId);
            Property(n => n.CategoryId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(n => n.CategoryNameAr).HasMaxLength(400).IsOptional();
            Property(n => n.CategoryNameEn).HasMaxLength(400).IsOptional();
            Property(n => n.Status).HasColumnType("TINYINT").IsRequired();
            Property(n => n.CategoryImage).IsRequired();
            MapToStoredProcedures();
        }
    }
}
