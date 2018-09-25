namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTouristVisitsImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TouristVisitImages", "MediaType", c => c.Int(nullable: false));
            AlterStoredProcedure(
                "dbo.TouristVisitImage_Insert",
                p => new
                    {
                        ImageUrl = p.String(maxLength: 250),
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
                        ImageUrl = p.String(maxLength: 250),
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
            DropColumn("dbo.TouristVisitImages", "MediaType");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
