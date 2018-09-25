namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterTouristVisitsSelectPaged : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER PROCEDURE [dbo].[TouristVisit_SelectPaged]
    @PageNumber [int],
    @PageSize [int],
    @UserId [nvarchar](128) = NULL
AS
BEGIN
     DECLARE @PageLowerBound INT DECLARE @PageUpperBound INT 
    DECLARE @OverallCount INT 
    SET @PageLowerBound = @PageSize * (@PageNumber-1) + 1
    SET @PageUpperBound = @PageLowerBound + @PageSize 
    
    SET @OverallCount = (SELECT COUNT(Id) FROM TouristVisits WHERE 
    	                        (@UserId IS NULL OR TouristVisits.CreatedBy  = @UserId ) )
    
    
    SELECT *, @OverallCount AS OverallCount  
    	             FROM (SELECT TouristVisits.Id ,
    	                          TouristVisits.Name ,
	(SELECT  ISNULL(AVG(TotalDegree),0)  FROM [dbo].[RatingUsers] 
    WHERE RelatedId = TouristVisits.Id AND RelatedType = 2 ) AS Rating,
	(SELECT ISNULL(SUM([Views].[Count]),0)  FROM [dbo].[Views] 
    WHERE RelatedId = TouristVisits.Id AND [RelatedTypeId] = 2  ) AS [Views],
								  TouristVisits.Description , 
    								  TouristVisits.CityName,
    								  TouristVisits.VisitDate,
    								  TouristVisits.ImageUrl,
    				              ROW_NUMBER() OVER(ORDER BY TouristVisits.CreatedOn DESC) AS RowNumber 
    	             FROM TouristVisits 
    	             WHERE @UserId IS NULL OR TouristVisits.CreatedBy  = @UserId ) TV
    WHERE TV.RowNumber >= @PageLowerBound AND TV.RowNumber < @PageUpperBound
END");
        }
        
        public override void Down()
        {
        }
    }
}
