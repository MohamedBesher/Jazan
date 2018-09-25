namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTouristVisitsSP : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("TouristVisit_SelectPaged", p => new
            {
                PageNumber = p.Int(),
                PageSize = p.Int(),
                UserId = p.String(defaultValueSql: "NULL", maxLength: 128)
            }, @" DECLARE @PageLowerBound INT DECLARE @PageUpperBound INT 
    DECLARE @OverallCount INT 
    SET @PageLowerBound = @PageSize * (@PageNumber-1) + 1
    SET @PageUpperBound = @PageLowerBound + @PageSize 
    
    SET @OverallCount = (SELECT COUNT(Id) FROM TouristVisits WHERE 
    	                        (@UserId IS NULL OR TouristVisits.CreatedBy  = @UserId ) )
    
    
    SELECT *, @OverallCount AS OverallCount  
    	             FROM (SELECT TouristVisits.Id ,
    	                          TouristVisits.Name ,
								  TouristVisits.CityName,
								  TouristVisits.VisitDate,
								  TouristVisits.ImageUrl,
    				              ROW_NUMBER() OVER(ORDER BY TouristVisits.CreatedOn DESC) AS RowNumber 
    	             FROM TouristVisits 
    	             WHERE @UserId IS NULL OR TouristVisits.CreatedBy  = @UserId ) TV
    WHERE TV.RowNumber >= @PageLowerBound AND TV.RowNumber < @PageUpperBound");


            AlterStoredProcedure("TouristVisit_SelectById", P => new { Id = P.Int() }, @"  DECLARE @results VARCHAR(MAX)
    SELECT @results = COALESCE(@results + ',', '') +  CONVERT(VARCHAR(12),[ImageUrl])
    FROM [TouristVisitImages]
    WHERE [TouristVisitId] = @Id
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
    Description,@results as Images
    
    FROM TouristVisits INNER JOIN AspNetUsers ON AspNetUsers.Id = TouristVisits.CreatedBy
    WHERE TouristVisits.Id = @Id");
        }
        
        public override void Down()
        {
            AlterStoredProcedure("TouristVisit_SelectPaged", p => new
            {
                PageNumber = p.Int(),
                PageSize = p.Int(),
                UserId = p.String(defaultValueSql: "NULL", maxLength: 128)
            }, @"DECLARE @PageLowerBound INT  DECLARE @PageUpperBound INT 
                DECLARE @OverallCount INT 
                SET @PageLowerBound = @PageSize * (@PageNumber-1) + 1
                SET @PageUpperBound = @PageLowerBound + @PageSize 
    
                SET @OverallCount = (SELECT COUNT(Id) FROM TouristVisits WHERE 
    	                        (@UserId IS NULL OR TouristVisits.CreatedBy  = @UserId ) )
    
    
                SELECT *, @OverallCount AS OverallCount  
    	             FROM (SELECT TouristVisits.Id ,
    	                          TouristVisits.Name ,
    				              (SELECT TOP 1  TouristVisitImages.ImageUrl FROM TouristVisitImages WHERE TouristVisitId = TouristVisits.Id) AS ImageUrl
    				              ,ROW_NUMBER() OVER(ORDER BY TouristVisits.CreatedOn DESC) AS RowNumber 
    	             FROM TouristVisits 
    	             WHERE @UserId IS NULL OR TouristVisits.CreatedBy  = @UserId ) TV
                WHERE TV.RowNumber >= @PageLowerBound AND TV.RowNumber < @PageUpperBound");


            AlterStoredProcedure("TouristVisit_SelectById", P => new { Id = P.Int() }, @"DECLARE @results VARCHAR(MAX)
                                SELECT @results = COALESCE(@results + ',', '') +  CONVERT(VARCHAR(12),[ImageUrl])
                                FROM [TouristVisitImages]
                                WHERE [TouristVisitId] = @Id
                                ORDER BY [ImageUrl] 

                                SELECT 
                                 TouristVisits.Id,
                                 TouristVisits.Name,
                                 CreatedOn,
                                 CreatedBy,
                                 AspNetUsers.UserName AS UserName,
                                 Latitude,
                                 Longitude,
                                 Description,@results as Images
  
                                FROM TouristVisits INNER JOIN AspNetUsers ON AspNetUsers.Id = TouristVisits.CreatedBy
                                WHERE TouristVisits.Id = @Id");
        }
    }
}
