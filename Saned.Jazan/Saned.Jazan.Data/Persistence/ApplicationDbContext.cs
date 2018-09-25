using Microsoft.AspNet.Identity.EntityFramework;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Persistence.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        #region DbSets
        public DbSet<EmailSetting> EmailSettings { get; set; }
        public DbSet<News> NewsSet { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageFeature> PackageFeatures { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<NewsImage> NewsImages { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementImage> AdvertisementImages { get; set; }
        public DbSet<AdvertisementFeature> AdvertisementFeatures { get; set; }
        public DbSet<MobileSetting> MobileSettings { get; set; }
        public DbSet<CulturalCompetitionQuestion> CulturalCompetitionQuestions { get; set; }
        public DbSet<CulturalCompetitionAnswer> CulturalCompetitionAnswers { get; set; }
        public DbSet<CulturalCompetitionQuestionSponsor> CulturalCompetitionQuestionSponsors { get; set; }

        public DbSet<TouristVisit> TouristVisits { get; set; }

        public DbSet<TouristVisitImage> TouristVisitImages { get; set; }

        #endregion
        //static string connnTemp = "Data Source=.;Initial Catalog=Saned_Jazan;Integrated Security=True";//"Saned_Jazan"

        public ApplicationDbContext() : base("Saned_Jazan")
        {
            this.Configuration.LazyLoadingEnabled = false;

        }
        public static ApplicationDbContext Create()
        {
            
            return new ApplicationDbContext();
        }
        //public ApplicationDbContext()
        //            : base(connnTemp, throwIfV1Schema: false)
        //{/*"Saned_Jazan"*/
        //    Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        //    Configuration.ProxyCreationEnabled = false;
        //}

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}

        //public virtual void Commit()
        //{
        //    base.SaveChanges();
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new NewsConfigurations());
            modelBuilder.Configurations.Add(new NewsImageConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new PackageConfiguration());
            modelBuilder.Configurations.Add(new FeatureConfiguration());
            modelBuilder.Configurations.Add(new PackageFeatureConfiguration());
            modelBuilder.Configurations.Add(new RefreshTokenConfigurations());
            modelBuilder.Configurations.Add(new ClientConfigurations());
            modelBuilder.Configurations.Add(new AdvertisementConfiguration());
            modelBuilder.Configurations.Add(new AdvertisementImageConfiguration());
            modelBuilder.Configurations.Add(new AdvertisementFeatureConfiguration());
            modelBuilder.Configurations.Add(new MobileSettingConfiguration());
            modelBuilder.Configurations.Add(new CulturalCompetitionQuestionConfiguration());
            modelBuilder.Configurations.Add(new CulturalCompetitionAnswerConfiguration());
            modelBuilder.Configurations.Add(new CulturalCompetitionQuestionSponsorsConfiguration());
            modelBuilder.Configurations.Add(new TouristVisitConfiguration());
            modelBuilder.Configurations.Add(new TouristVisitImagesConfigurations());
            //modelBuilder.Entity<NewsImage>().HasKey(x => x.ImageId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
