namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDataBaseTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryNameAr = c.String(maxLength: 400),
                        CategoryNameEn = c.String(maxLength: 400),
                        CategoryImage = c.String(nullable: false, maxLength: 36),
                        ParentId = c.Int(nullable: false),
                        Status = c.Byte(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Host = c.String(),
                        FromEmail = c.String(),
                        Password = c.String(),
                        SubjectAr = c.String(),
                        SubjectEn = c.String(),
                        MessageBodyAr = c.String(),
                        MessageBodyEn = c.String(),
                        EmailSettingType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArabicName = c.String(nullable: false, maxLength: 250),
                        EnglishName = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PackageFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageId = c.Int(nullable: false),
                        FeatureId = c.Int(nullable: false),
                        Period = c.Int(),
                        Quantity = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Features", t => t.FeatureId)
                .ForeignKey("dbo.Packages", t => t.PackageId)
                .Index(t => t.PackageId)
                .Index(t => t.FeatureId);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArabicName = c.String(nullable: false, maxLength: 250),
                        EnglishName = c.String(maxLength: 250),
                        Period = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedOn = c.DateTime(),
                        UpdatedBy = c.Guid(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsImages",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(maxLength: 36),
                        IsDefault = c.Boolean(nullable: false),
                        NewsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.News", t => t.NewsId, cascadeDelete: true)
                .Index(t => t.NewsId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 300),
                        Details = c.String(nullable: false),
                        PublishingDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 100),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SoicalMediaId = c.String(),
                        PhotoUrl = c.String(),
                        ConfirmedEmailToken = c.String(),
                        ResetPasswordlToken = c.String(),
                        IsSelfAdded = c.Boolean(),
                        IsApprove = c.Boolean(),
                        IsDeleted = c.Boolean(),
                        CreatedDate = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateStoredProcedure(
                "dbo.Category_Insert",
                p => new
                    {
                        CategoryNameAr = p.String(maxLength: 400),
                        CategoryNameEn = p.String(maxLength: 400),
                        CategoryImage = p.String(maxLength: 36),
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
            
            CreateStoredProcedure(
                "dbo.Category_Update",
                p => new
                    {
                        CategoryId = p.Int(),
                        CategoryNameAr = p.String(maxLength: 400),
                        CategoryNameEn = p.String(maxLength: 400),
                        CategoryImage = p.String(maxLength: 36),
                        ParentId = p.Int(),
                        Status = p.Byte(),
                        CreateDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Categories]
                      SET [CategoryNameAr] = @CategoryNameAr, [CategoryNameEn] = @CategoryNameEn, [CategoryImage] = @CategoryImage, [ParentId] = @ParentId, [Status] = @Status, [CreateDate] = @CreateDate
                      WHERE ([CategoryId] = @CategoryId)"
            );
            
            CreateStoredProcedure(
                "dbo.Category_Delete",
                p => new
                    {
                        CategoryId = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Categories]
                      WHERE ([CategoryId] = @CategoryId)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Category_Delete");
            DropStoredProcedure("dbo.Category_Update");
            DropStoredProcedure("dbo.Category_Insert");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.NewsImages", "NewsId", "dbo.News");
            DropForeignKey("dbo.PackageFeatures", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.PackageFeatures", "FeatureId", "dbo.Features");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.NewsImages", new[] { "NewsId" });
            DropIndex("dbo.PackageFeatures", new[] { "FeatureId" });
            DropIndex("dbo.PackageFeatures", new[] { "PackageId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.News");
            DropTable("dbo.NewsImages");
            DropTable("dbo.Packages");
            DropTable("dbo.PackageFeatures");
            DropTable("dbo.Features");
            DropTable("dbo.EmailSettings");
            DropTable("dbo.Clients");
            DropTable("dbo.Categories");
        }
    }
}
