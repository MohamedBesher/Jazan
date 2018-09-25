namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AlterTouristVisit_SelectById : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("TouristVisit_SelectById",
                p => new
                {
                    Id = p.Int()

                }, @"DECLARE @images VARCHAR(MAX)
                    SELECT @images = COALESCE(@images + ',', '') +  CONVERT(VARCHAR(12),[ImageUrl])
                    FROM [TouristVisitImages]
                    WHERE [TouristVisitId] = @Id AND MediaType = 1
                    ORDER BY [ImageUrl] 

	                DECLARE @youtube VARCHAR(MAX)
                    SELECT @youtube = COALESCE(@youtube + ',', '') +  CONVERT(VARCHAR(12),[ImageUrl])
                    FROM [TouristVisitImages]
                    WHERE [TouristVisitId] = @Id AND MediaType = 2
                    ORDER BY [ImageUrl] 

                    SELECT 
                    TouristVisits.Id,
                    TouristVisits.Name,
    	                TouristVisits.VisitDate,
                    TouristVisits.ImageUrl,
    	                TouristVisits.CityName,
                    CreatedOn,
                    CreatedBy,
                    AspNetUsers.UserName AS UserName,
                    Latitude,
                    Longitude,
                    Description,@images as Images , @youtube as YouTube
    
                    FROM TouristVisits INNER JOIN AspNetUsers ON AspNetUsers.Id = TouristVisits.CreatedBy
                    WHERE TouristVisits.Id = @Id");

        }

        public override void Down()
        {
           
        }
    }
}
