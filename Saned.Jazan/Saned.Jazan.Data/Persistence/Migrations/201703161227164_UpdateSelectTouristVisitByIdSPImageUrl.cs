namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSelectTouristVisitByIdSPImageUrl : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("TouristVisit_SelectById",
              p => new
              {
                  Id = p.Int()

              }, @" SELECT 
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
                    Description,
                    TouristVisitImages.ImageUrl as MediaUrl,
	                TouristVisitImages.MediaType,
	                TouristVisitImages.Id as TouristVisitImageId
                    FROM TouristVisits INNER JOIN AspNetUsers ON AspNetUsers.Id = TouristVisits.CreatedBy
	                inner join TouristVisitImages on TouristVisits.Id = TouristVisitImages.TouristVisitId
                    WHERE TouristVisits.Id = @Id");
        }
        
        public override void Down()
        {
        }
    }
}
