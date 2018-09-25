namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStartDateAndEndDateToAdvertisementAndAdvertisementFeatures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvertisementFeatures", "StartDate", c => c.DateTime());
            AddColumn("dbo.AdvertisementFeatures", "EndDate", c => c.DateTime());
            AddColumn("dbo.Advertisements", "StartDate", c => c.DateTime());
            AddColumn("dbo.Advertisements", "EndDate", c => c.DateTime());
            DropColumn("dbo.Advertisements", "ViewsCount");
            AlterStoredProcedure(
                "dbo.AdvertisementFeature_Insert",
                p => new
                    {
                        AdvertisementId = p.Int(),
                        FeatureId = p.Int(),
                        Period = p.Int(),
                        Quantity = p.Int(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[AdvertisementFeatures]([AdvertisementId], [FeatureId], [Period], [Quantity], [StartDate], [EndDate])
                      VALUES (@AdvertisementId, @FeatureId, @Period, @Quantity, @StartDate, @EndDate)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AdvertisementFeatures]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AdvertisementFeatures] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.AdvertisementFeature_Update",
                p => new
                    {
                        Id = p.Int(),
                        AdvertisementId = p.Int(),
                        FeatureId = p.Int(),
                        Period = p.Int(),
                        Quantity = p.Int(),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[AdvertisementFeatures]
                      SET [AdvertisementId] = @AdvertisementId, [FeatureId] = @FeatureId, [Period] = @Period, [Quantity] = @Quantity, [StartDate] = @StartDate, [EndDate] = @EndDate
                      WHERE ([Id] = @Id)"
            );
            
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
                        CreatedBy = p.String(maxLength: 128),
                        UpdatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 128),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Advertisements]([Name], [CityName], [CategoryId], [PackageId], [Description], [ImageUrl], [Latitude], [Longitude], [WorkingHours], [Mobile], [Email], [WebSite], [Twitter], [FaceBook], [Instagram], [Snapchat], [IsApproved], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StartDate], [EndDate])
                      VALUES (@Name, @CityName, @CategoryId, @PackageId, @Description, @ImageUrl, @Latitude, @Longitude, @WorkingHours, @Mobile, @Email, @WebSite, @Twitter, @FaceBook, @Instagram, @Snapchat, @IsApproved, @IsActive, @CreatedOn, @CreatedBy, @UpdatedOn, @UpdatedBy, @StartDate, @EndDate)
                      
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
                        CreatedBy = p.String(maxLength: 128),
                        UpdatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 128),
                        StartDate = p.DateTime(),
                        EndDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Advertisements]
                      SET [Name] = @Name, [CityName] = @CityName, [CategoryId] = @CategoryId, [PackageId] = @PackageId, [Description] = @Description, [ImageUrl] = @ImageUrl, [Latitude] = @Latitude, [Longitude] = @Longitude, [WorkingHours] = @WorkingHours, [Mobile] = @Mobile, [Email] = @Email, [WebSite] = @WebSite, [Twitter] = @Twitter, [FaceBook] = @FaceBook, [Instagram] = @Instagram, [Snapchat] = @Snapchat, [IsApproved] = @IsApproved, [IsActive] = @IsActive, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy, [StartDate] = @StartDate, [EndDate] = @EndDate
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Advertisements", "ViewsCount", c => c.Int(nullable: false));
            DropColumn("dbo.Advertisements", "EndDate");
            DropColumn("dbo.Advertisements", "StartDate");
            DropColumn("dbo.AdvertisementFeatures", "EndDate");
            DropColumn("dbo.AdvertisementFeatures", "StartDate");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
