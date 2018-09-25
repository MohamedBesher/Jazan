namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateADSmodel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Advertisements", "CreatedBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Advertisements", "UpdatedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.Advertisements", "CreatedBy");
            CreateIndex("dbo.Advertisements", "UpdatedBy");
            AddForeignKey("dbo.Advertisements", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Advertisements", "UpdatedBy", "dbo.AspNetUsers", "Id");
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
                        CreatedBy = p.String(maxLength: 128),
                        UpdatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 128),
                        ViewsCount = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Advertisements]
                      SET [Name] = @Name, [CityName] = @CityName, [CategoryId] = @CategoryId, [PackageId] = @PackageId, [Description] = @Description, [ImageUrl] = @ImageUrl, [Latitude] = @Latitude, [Longitude] = @Longitude, [WorkingHours] = @WorkingHours, [Mobile] = @Mobile, [Email] = @Email, [WebSite] = @WebSite, [Twitter] = @Twitter, [FaceBook] = @FaceBook, [Instagram] = @Instagram, [Snapchat] = @Snapchat, [IsApproved] = @IsApproved, [IsActive] = @IsActive, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy, [ViewsCount] = @ViewsCount
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advertisements", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Advertisements", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Advertisements", new[] { "UpdatedBy" });
            DropIndex("dbo.Advertisements", new[] { "CreatedBy" });
            AlterColumn("dbo.Advertisements", "UpdatedBy", c => c.Guid());
            AlterColumn("dbo.Advertisements", "CreatedBy", c => c.Guid(nullable: false));
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
