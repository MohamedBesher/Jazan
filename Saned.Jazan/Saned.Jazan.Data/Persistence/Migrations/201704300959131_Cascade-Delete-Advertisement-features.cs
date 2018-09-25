namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeDeleteAdvertisementfeatures : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdvertisementFeatures", "AdvertisementId", "dbo.Advertisements");
            DropForeignKey("dbo.CulturalCompetitionQuestionSponsors", "AdvertisementId", "dbo.Advertisements");
            AddForeignKey("dbo.AdvertisementFeatures", "AdvertisementId", "dbo.Advertisements", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CulturalCompetitionQuestionSponsors", "AdvertisementId", "dbo.Advertisements", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CulturalCompetitionQuestionSponsors", "AdvertisementId", "dbo.Advertisements");
            DropForeignKey("dbo.AdvertisementFeatures", "AdvertisementId", "dbo.Advertisements");
            AddForeignKey("dbo.CulturalCompetitionQuestionSponsors", "AdvertisementId", "dbo.Advertisements", "Id");
            AddForeignKey("dbo.AdvertisementFeatures", "AdvertisementId", "dbo.Advertisements", "Id");
        }
    }
}
