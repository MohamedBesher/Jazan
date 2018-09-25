namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterAdvertisementSelectById : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("Advertisement_SelectById",
                p => new
                {
                    Id = p.Int()

                }, @"SELECT Advertisements.* ,
	                 AdvertisementImages.ImageUrl AS AdvertisementImageUrl , 
	                 AdvertisementImages.Id AS  AdvertisementImageId
	                 FROM Advertisements LEFT JOIN AdvertisementImages 
	                 ON Advertisements.Id = AdvertisementImages.AdvertisementId WHERE Advertisements.Id = @Id");
        }
        
        public override void Down()
        {

            AlterStoredProcedure("Advertisement_SelectById",
                p => new
                {
                    Id = p.Int()

                }, @"SELECT * FROM Advertisements WHERE Id = @Id");
        }
    }
}
