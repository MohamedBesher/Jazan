namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAdvertisementComponent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdvertisementImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(nullable: false, maxLength: 250),
                        AdvertisementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.AdvertisementId)
                .Index(t => t.AdvertisementId);
            
            CreateTable(
                "dbo.Advertisements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        CityName = c.String(maxLength: 250),
                        CategoryId = c.Int(nullable: false),
                        PackageId = c.Int(nullable: false),
                        Description = c.String(maxLength: 1000),
                        ImageUrl = c.String(nullable: false, maxLength: 250),
                        MapLocation = c.String(maxLength: 250),
                        WorkingHours = c.Int(nullable: false),
                        Mobile = c.String(maxLength: 250),
                        Email = c.String(maxLength: 250),
                        WebSite = c.String(maxLength: 250),
                        Twitter = c.String(maxLength: 250),
                        FaceBook = c.String(maxLength: 250),
                        Instagram = c.String(maxLength: 250),
                        Snapchat = c.String(maxLength: 250),
                        IsApproved = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedOn = c.DateTime(),
                        UpdatedBy = c.Guid(),
                        ViewsCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Packages", t => t.PackageId)
                .Index(t => t.CategoryId)
                .Index(t => t.PackageId);
            
            CreateStoredProcedure(
                "dbo.AdvertisementImage_Insert",
                p => new
                    {
                        ImageUrl = p.String(maxLength: 250),
                        AdvertisementId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[AdvertisementImages]([ImageUrl], [AdvertisementId])
                      VALUES (@ImageUrl, @AdvertisementId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AdvertisementImages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AdvertisementImages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertisementImage_Update",
                p => new
                    {
                        Id = p.Int(),
                        ImageUrl = p.String(maxLength: 250),
                        AdvertisementId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[AdvertisementImages]
                      SET [ImageUrl] = @ImageUrl, [AdvertisementId] = @AdvertisementId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertisementImage_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[AdvertisementImages]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Advertisement_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 250),
                        CityName = p.String(maxLength: 250),
                        CategoryId = p.Int(),
                        PackageId = p.Int(),
                        Description = p.String(maxLength: 1000),
                        ImageUrl = p.String(maxLength: 250),
                        MapLocation = p.String(maxLength: 250),
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
                    @"INSERT [dbo].[Advertisements]([Name], [CityName], [CategoryId], [PackageId], [Description], [ImageUrl], [MapLocation], [WorkingHours], [Mobile], [Email], [WebSite], [Twitter], [FaceBook], [Instagram], [Snapchat], [IsApproved], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [ViewsCount])
                      VALUES (@Name, @CityName, @CategoryId, @PackageId, @Description, @ImageUrl, @MapLocation, @WorkingHours, @Mobile, @Email, @WebSite, @Twitter, @FaceBook, @Instagram, @Snapchat, @IsApproved, @IsActive, @CreatedOn, @CreatedBy, @UpdatedOn, @UpdatedBy, @ViewsCount)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Advertisements]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Advertisements] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
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
                        MapLocation = p.String(maxLength: 250),
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
                      SET [Name] = @Name, [CityName] = @CityName, [CategoryId] = @CategoryId, [PackageId] = @PackageId, [Description] = @Description, [ImageUrl] = @ImageUrl, [MapLocation] = @MapLocation, [WorkingHours] = @WorkingHours, [Mobile] = @Mobile, [Email] = @Email, [WebSite] = @WebSite, [Twitter] = @Twitter, [FaceBook] = @FaceBook, [Instagram] = @Instagram, [Snapchat] = @Snapchat, [IsApproved] = @IsApproved, [IsActive] = @IsActive, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy, [ViewsCount] = @ViewsCount
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Advertisement_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Advertisements]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Advertisement_Delete");
            DropStoredProcedure("dbo.Advertisement_Update");
            DropStoredProcedure("dbo.Advertisement_Insert");
            DropStoredProcedure("dbo.AdvertisementImage_Delete");
            DropStoredProcedure("dbo.AdvertisementImage_Update");
            DropStoredProcedure("dbo.AdvertisementImage_Insert");
            DropForeignKey("dbo.AdvertisementImages", "AdvertisementId", "dbo.Advertisements");
            DropForeignKey("dbo.Advertisements", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.Advertisements", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Advertisements", new[] { "PackageId" });
            DropIndex("dbo.Advertisements", new[] { "CategoryId" });
            DropIndex("dbo.AdvertisementImages", new[] { "AdvertisementId" });
            DropTable("dbo.Advertisements");
            DropTable("dbo.AdvertisementImages");
        }
    }
}
