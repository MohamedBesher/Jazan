namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateImageURLLenght : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AdvertisementImages", "ImageUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Categories", "CategoryImage", c => c.String(nullable: false));
            AlterColumn("dbo.TouristVisitImages", "ImageUrl", c => c.String(nullable: false));
            AlterStoredProcedure(
                "dbo.AdvertisementImage_Insert",
                p => new
                    {
                        ImageUrl = p.String(),
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
            
            AlterStoredProcedure(
                "dbo.AdvertisementImage_Update",
                p => new
                    {
                        Id = p.Int(),
                        ImageUrl = p.String(),
                        AdvertisementId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[AdvertisementImages]
                      SET [ImageUrl] = @ImageUrl, [AdvertisementId] = @AdvertisementId
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Category_Insert",
                p => new
                    {
                        CategoryNameAr = p.String(maxLength: 400),
                        CategoryNameEn = p.String(maxLength: 400),
                        CategoryImage = p.String(),
                        ParentId = p.Int(),
                        Status = p.Byte(),
                        CreateDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Categories]([CategoryNameAr], [CategoryNameEn], [CategoryImage], [ParentId], [Status], [CreateDate])
                      VALUES (@CategoryNameAr, @CategoryNameEn, @CategoryImage, @ParentId, @Status, @CreateDate)
                      
                      DECLARE @CategoryId int
                      SELECT @CategoryId = [CategoryId]
                      FROM [dbo].[Categories]
                      WHERE @@ROWCOUNT > 0 AND [CategoryId] = scope_identity()
                      
                      SELECT t0.[CategoryId]
                      FROM [dbo].[Categories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[CategoryId] = @CategoryId"
            );
            
            AlterStoredProcedure(
                "dbo.Category_Update",
                p => new
                    {
                        CategoryId = p.Int(),
                        CategoryNameAr = p.String(maxLength: 400),
                        CategoryNameEn = p.String(maxLength: 400),
                        CategoryImage = p.String(),
                        ParentId = p.Int(),
                        Status = p.Byte(),
                        CreateDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Categories]
                      SET [CategoryNameAr] = @CategoryNameAr, [CategoryNameEn] = @CategoryNameEn, [CategoryImage] = @CategoryImage, [ParentId] = @ParentId, [Status] = @Status, [CreateDate] = @CreateDate
                      WHERE ([CategoryId] = @CategoryId)"
            );
            
            AlterStoredProcedure(
                "dbo.TouristVisitImage_Insert",
                p => new
                    {
                        ImageUrl = p.String(),
                        MediaType = p.Int(),
                        TouristVisitId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[TouristVisitImages]([ImageUrl], [MediaType], [TouristVisitId])
                      VALUES (@ImageUrl, @MediaType, @TouristVisitId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TouristVisitImages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TouristVisitImages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.TouristVisitImage_Update",
                p => new
                    {
                        Id = p.Int(),
                        ImageUrl = p.String(),
                        MediaType = p.Int(),
                        TouristVisitId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[TouristVisitImages]
                      SET [ImageUrl] = @ImageUrl, [MediaType] = @MediaType, [TouristVisitId] = @TouristVisitId
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TouristVisitImages", "ImageUrl", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Categories", "CategoryImage", c => c.String(nullable: false, maxLength: 36));
            AlterColumn("dbo.AdvertisementImages", "ImageUrl", c => c.String(nullable: false, maxLength: 250));
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
