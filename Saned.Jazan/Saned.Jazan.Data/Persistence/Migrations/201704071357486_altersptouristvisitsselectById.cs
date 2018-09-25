namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class altersptouristvisitsselectById : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER PROCEDURE [dbo].[TouristVisit_SelectById]
    @Id [int]
AS
BEGIN
    SELECT 
    (SELECT  ISNULL(AVG(TotalDegree),0)  FROM [dbo].[RatingUsers] 
    WHERE RelatedId = TouristVisits.Id AND RelatedType = 2 ) AS Rating,
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
    	                left join TouristVisitImages on TouristVisits.Id = TouristVisitImages.TouristVisitId
    WHERE TouristVisits.Id = @Id
END");
        }
        
        public override void Down()
        {
        }
    }
}
