namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTouristVisitsComponent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TouristVisits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 128),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        Description = c.String(maxLength: 600),
                        UpdatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.TouristVisitImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(nullable: false, maxLength: 250),
                        TouristVisitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TouristVisits", t => t.TouristVisitId)
                .Index(t => t.TouristVisitId);
            
            CreateStoredProcedure(
                "dbo.TouristVisit_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 250),
                        CreatedOn = p.DateTime(),
                        CreatedBy = p.String(maxLength: 128),
                        Latitude = p.String(),
                        Longitude = p.String(),
                        Description = p.String(maxLength: 600),
                        UpdatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[TouristVisits]([Name], [CreatedOn], [CreatedBy], [Latitude], [Longitude], [Description], [UpdatedOn], [UpdatedBy])
                      VALUES (@Name, @CreatedOn, @CreatedBy, @Latitude, @Longitude, @Description, @UpdatedOn, @UpdatedBy)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TouristVisits]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TouristVisits] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.TouristVisit_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 250),
                        CreatedOn = p.DateTime(),
                        CreatedBy = p.String(maxLength: 128),
                        Latitude = p.String(),
                        Longitude = p.String(),
                        Description = p.String(maxLength: 600),
                        UpdatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[TouristVisits]
                      SET [Name] = @Name, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [Latitude] = @Latitude, [Longitude] = @Longitude, [Description] = @Description, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TouristVisit_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[TouristVisits]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TouristVisitImage_Insert",
                p => new
                    {
                        ImageUrl = p.String(maxLength: 250),
                        TouristVisitId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[TouristVisitImages]([ImageUrl], [TouristVisitId])
                      VALUES (@ImageUrl, @TouristVisitId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TouristVisitImages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TouristVisitImages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.TouristVisitImage_Update",
                p => new
                    {
                        Id = p.Int(),
                        ImageUrl = p.String(maxLength: 250),
                        TouristVisitId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[TouristVisitImages]
                      SET [ImageUrl] = @ImageUrl, [TouristVisitId] = @TouristVisitId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.TouristVisitImage_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[TouristVisitImages]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.TouristVisitImage_Delete");
            DropStoredProcedure("dbo.TouristVisitImage_Update");
            DropStoredProcedure("dbo.TouristVisitImage_Insert");
            DropStoredProcedure("dbo.TouristVisit_Delete");
            DropStoredProcedure("dbo.TouristVisit_Update");
            DropStoredProcedure("dbo.TouristVisit_Insert");
            DropForeignKey("dbo.TouristVisits", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.TouristVisitImages", "TouristVisitId", "dbo.TouristVisits");
            DropForeignKey("dbo.TouristVisits", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.TouristVisitImages", new[] { "TouristVisitId" });
            DropIndex("dbo.TouristVisits", new[] { "UpdatedBy" });
            DropIndex("dbo.TouristVisits", new[] { "CreatedBy" });
            DropTable("dbo.TouristVisitImages");
            DropTable("dbo.TouristVisits");
        }
    }
}
