namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeDeleteAdvertisementImage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdvertisementImages", "AdvertisementId", "dbo.Advertisements");
            AddForeignKey("dbo.AdvertisementImages", "AdvertisementId", "dbo.Advertisements", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdvertisementImages", "AdvertisementId", "dbo.Advertisements");
            AddForeignKey("dbo.AdvertisementImages", "AdvertisementId", "dbo.Advertisements", "Id");
        }
    }
}
