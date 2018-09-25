namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateAdvertisementModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advertisements", "Latitude", c => c.String(maxLength: 250));
            AddColumn("dbo.Advertisements", "Longitude", c => c.String(maxLength: 250));
            DropColumn("dbo.Advertisements", "MapLocation");
            AlterStoredProcedure(
                "dbo.Advertisement_Insert",
                p => new
                {
                    Name = p.String(maxLength: 250),
                    CityName = p.String(maxLength: 250),
                    CategoryId = p.Int(),
                    PackageId = p.Int(),
                    Description = p.String(maxLength: 1000),
                    ImageUrl = p.String(maxLength: 250),
                    Latitude = p.String(maxLength: 250),
                    Longitude = p.String(maxLength: 250),
                    WorkingHours = p.Int(),
                    Mobile = p.String(maxLength: 250),
                    Email = p.String(maxLength: 250),
                    WebSite = p.String(maxLength: 250),
                    Twitter = p.String(maxLength: 250),
                    FaceBook = p.String(maxLength: 250),
                    Instagram = p.String(maxLength: 250),
                    Snapchat = p.String(maxLength: 250),
                    IsApproved = p.Boolean(),
                    IsActive = p.Boolean(),
                    CreatedOn = p.DateTime(),
                    CreatedBy = p.Guid(),
                    UpdatedOn = p.DateTime(),
                    UpdatedBy = p.Guid(),
                    ViewsCount = p.Int(),
                },
                body:
                    @"INSERT [dbo].[Advertisements]([Name], [CityName], [CategoryId], [PackageId], [Description], [ImageUrl], [Latitude], [Longitude], [WorkingHours], [Mobile], [Email], [WebSite], [Twitter], [FaceBook], [Instagram], [Snapchat], [IsApproved], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [ViewsCount])
                      VALUES (@Name, @CityName, @CategoryId, @PackageId, @Description, @ImageUrl, @Latitude, @Longitude, @WorkingHours, @Mobile, @Email, @WebSite, @Twitter, @FaceBook, @Instagram, @Snapchat, @IsApproved, @IsActive, @CreatedOn, @CreatedBy, @UpdatedOn, @UpdatedBy, @ViewsCount)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Advertisements]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Advertisements] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );

            AlterStoredProcedure(
                "dbo.Advertisement_Update",
                p => new
                {
                    Id = p.Int(),
                    Name = p.String(maxLength: 250),
                    CityName = p.String(maxLength: 250),
                    CategoryId = p.Int(),
                    PackageId = p.Int(),
                    Description = p.String(maxLength: 1000),
                    ImageUrl = p.String(maxLength: 250),
                    Latitude = p.String(maxLength: 250),
                    Longitude = p.String(maxLength: 250),
                    WorkingHours = p.Int(),
                    Mobile = p.String(maxLength: 250),
                    Email = p.String(maxLength: 250),
                    WebSite = p.String(maxLength: 250),
                    Twitter = p.String(maxLength: 250),
                    FaceBook = p.String(maxLength: 250),
                    Instagram = p.String(maxLength: 250),
                    Snapchat = p.String(maxLength: 250),
                    IsApproved = p.Boolean(),
                    IsActive = p.Boolean(),
                    CreatedOn = p.DateTime(),
                    CreatedBy = p.Guid(),
                    UpdatedOn = p.DateTime(),
                    UpdatedBy = p.Guid(),
                    ViewsCount = p.Int(),
                },
                body:
                    @"UPDATE [dbo].[Advertisements]
                      SET [Name] = @Name, [CityName] = @CityName, [CategoryId] = @CategoryId, [PackageId] = @PackageId, [Description] = @Description, [ImageUrl] = @ImageUrl, [Latitude] = @Latitude, [Longitude] = @Longitude, [WorkingHours] = @WorkingHours, [Mobile] = @Mobile, [Email] = @Email, [WebSite] = @WebSite, [Twitter] = @Twitter, [FaceBook] = @FaceBook, [Instagram] = @Instagram, [Snapchat] = @Snapchat, [IsApproved] = @IsApproved, [IsActive] = @IsActive, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy, [ViewsCount] = @ViewsCount
                      WHERE ([Id] = @Id)"
            );

            AlterStoredProcedure("Advertisement_SelectPaging", p => new
            {
                PageNumber = p.Int(),
                PageSize = p.Int(),
                CategoryId = p.Int(defaultValueSql: "NULL"),
                UserId = p.String(defaultValueSql: "NULL", maxLength: 128)

            }, @"    DECLARE @PageLowerBound INT  DECLARE @PageUpperBound INT 
    DECLARE @OverallCount INT 
    SET @PageLowerBound = @PageSize * (@PageNumber-1) + 1
    SET @PageUpperBound = @PageLowerBound + @PageSize 
    
    SET @OverallCount = (SELECT COUNT(Id) FROM Advertisements WHERE 
    	            (@CategoryId IS NULL OR Advertisements.CategoryId  = @CategoryId ) AND 
    	            (@UserId IS NULL OR Advertisements.CreatedBy  = @UserId ) )
    
    
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
    	 WHERE ( @CategoryId IS NULL OR Advertisements.CategoryId  = @CategoryId ) AND 
    	       ( @UserId IS NULL OR Advertisements.CreatedBy  = @UserId ) ) ADS
    WHERE ADS.RowNumber >= @PageLowerBound AND ADS.RowNumber < @PageUpperBound");

        }

        public override void Down()
        {
            AddColumn("dbo.Advertisements", "MapLocation", c => c.String(maxLength: 250));
            DropColumn("dbo.Advertisements", "Longitude");
            DropColumn("dbo.Advertisements", "Latitude");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
