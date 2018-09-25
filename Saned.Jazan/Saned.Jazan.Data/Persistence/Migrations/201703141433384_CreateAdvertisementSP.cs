namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateAdvertisementSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("Advertisement_SelectById", p => new { Id = p.Int() }, @"SELECT * FROM Advertisements WHERE Id = @Id");

            CreateStoredProcedure("Advertisement_SelectPaging", p => new
            {
                PageNumber = p.Int(),
                PageSize = p.Int(),
                CategoryId = p.Int(defaultValueSql: "NULL")
            }, @"DECLARE @PageLowerBound INT  DECLARE @PageUpperBound INT 
                DECLARE @OverallCount INT 
                SET @PageLowerBound = @PageSize * (@PageNumber-1) + 1
                SET @PageUpperBound = @PageLowerBound + @PageSize 
    
                SET @OverallCount = (SELECT COUNT(Id) FROM Advertisements WHERE 
	            (@CategoryId IS NULL OR Advertisements.CategoryId  = @CategoryId ) )
    
    
     SELECT *, @OverallCount AS OverallCount  
	 FROM (SELECT Advertisements.Id ,
	              Advertisements.Name ,
				  Advertisements.CityName , 
				  Advertisements.ViewsCount ,
				  Advertisements.ImageUrl ,
				  Advertisements.PackageId , 
	              Packages.EnglishName AS PackageEnglishName ,
				  Packages.ArabicName AS PackageArabicName,
				  ROW_NUMBER() OVER(ORDER BY Advertisements.CreatedOn DESC) AS RowNumber 
	 FROM Advertisements INNER JOIN Packages ON Advertisements.PackageId = Packages.Id 
	 WHERE @CategoryId IS NULL OR Advertisements.CategoryId  = @CategoryId ) ADS
     WHERE ADS.RowNumber >= @PageLowerBound AND ADS.RowNumber < @PageUpperBound");
        }

        public override void Down()
        {
        }
    }
}
