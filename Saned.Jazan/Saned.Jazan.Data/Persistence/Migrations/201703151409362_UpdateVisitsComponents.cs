namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVisitsComponents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TouristVisits", "CityName", c => c.String(nullable: false, maxLength: 250));
            AddColumn("dbo.TouristVisits", "VisitDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.TouristVisits", "ImageUrl", c => c.String());
            AlterColumn("dbo.TouristVisits", "Description", c => c.String(maxLength: 1000));
            AlterStoredProcedure(
                "dbo.TouristVisit_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 250),
                        CityName = p.String(maxLength: 250),
                        VisitDate = p.DateTime(),
                        CreatedOn = p.DateTime(),
                        CreatedBy = p.String(maxLength: 128),
                        Latitude = p.String(),
                        Longitude = p.String(),
                        Description = p.String(maxLength: 1000),
                        UpdatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 128),
                        ImageUrl = p.String(),
                    },
                body:
                    @"INSERT [dbo].[TouristVisits]([Name], [CityName], [VisitDate], [CreatedOn], [CreatedBy], [Latitude], [Longitude], [Description], [UpdatedOn], [UpdatedBy], [ImageUrl])
                      VALUES (@Name, @CityName, @VisitDate, @CreatedOn, @CreatedBy, @Latitude, @Longitude, @Description, @UpdatedOn, @UpdatedBy, @ImageUrl)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[TouristVisits]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[TouristVisits] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.TouristVisit_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(maxLength: 250),
                        CityName = p.String(maxLength: 250),
                        VisitDate = p.DateTime(),
                        CreatedOn = p.DateTime(),
                        CreatedBy = p.String(maxLength: 128),
                        Latitude = p.String(),
                        Longitude = p.String(),
                        Description = p.String(maxLength: 1000),
                        UpdatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 128),
                        ImageUrl = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[TouristVisits]
                      SET [Name] = @Name, [CityName] = @CityName, [VisitDate] = @VisitDate, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [Latitude] = @Latitude, [Longitude] = @Longitude, [Description] = @Description, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy, [ImageUrl] = @ImageUrl
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TouristVisits", "Description", c => c.String(maxLength: 600));
            DropColumn("dbo.TouristVisits", "ImageUrl");
            DropColumn("dbo.TouristVisits", "VisitDate");
            DropColumn("dbo.TouristVisits", "CityName");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
