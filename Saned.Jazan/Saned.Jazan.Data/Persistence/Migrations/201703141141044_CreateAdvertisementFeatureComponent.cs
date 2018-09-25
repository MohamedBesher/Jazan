namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAdvertisementFeatureComponent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdvertisementFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdvertisementId = c.Int(nullable: false),
                        FeatureId = c.Int(nullable: false),
                        Period = c.Int(),
                        Quantity = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.AdvertisementId)
                .ForeignKey("dbo.Features", t => t.FeatureId)
                .Index(t => t.AdvertisementId)
                .Index(t => t.FeatureId);
            
            CreateStoredProcedure(
                "dbo.AdvertisementFeature_Insert",
                p => new
                    {
                        AdvertisementId = p.Int(),
                        FeatureId = p.Int(),
                        Period = p.Int(),
                        Quantity = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[AdvertisementFeatures]([AdvertisementId], [FeatureId], [Period], [Quantity])
                      VALUES (@AdvertisementId, @FeatureId, @Period, @Quantity)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AdvertisementFeatures]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AdvertisementFeatures] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertisementFeature_Update",
                p => new
                    {
                        Id = p.Int(),
                        AdvertisementId = p.Int(),
                        FeatureId = p.Int(),
                        Period = p.Int(),
                        Quantity = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[AdvertisementFeatures]
                      SET [AdvertisementId] = @AdvertisementId, [FeatureId] = @FeatureId, [Period] = @Period, [Quantity] = @Quantity
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.AdvertisementFeature_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[AdvertisementFeatures]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.AdvertisementFeature_Delete");
            DropStoredProcedure("dbo.AdvertisementFeature_Update");
            DropStoredProcedure("dbo.AdvertisementFeature_Insert");
            DropForeignKey("dbo.AdvertisementFeatures", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.AdvertisementFeatures", "AdvertisementId", "dbo.Advertisements");
            DropIndex("dbo.AdvertisementFeatures", new[] { "FeatureId" });
            DropIndex("dbo.AdvertisementFeatures", new[] { "AdvertisementId" });
            DropTable("dbo.AdvertisementFeatures");
        }
    }
}
