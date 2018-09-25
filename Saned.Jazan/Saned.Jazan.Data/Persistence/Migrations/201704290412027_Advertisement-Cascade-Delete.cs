namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertisementCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Advertisements", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Advertisements", "PackageId", "dbo.Packages");
            AddForeignKey("dbo.Advertisements", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
            AddForeignKey("dbo.Advertisements", "PackageId", "dbo.Packages", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advertisements", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.Advertisements", "CategoryId", "dbo.Categories");
            AddForeignKey("dbo.Advertisements", "PackageId", "dbo.Packages", "Id");
            AddForeignKey("dbo.Advertisements", "CategoryId", "dbo.Categories", "CategoryId");
        }
    }
}
