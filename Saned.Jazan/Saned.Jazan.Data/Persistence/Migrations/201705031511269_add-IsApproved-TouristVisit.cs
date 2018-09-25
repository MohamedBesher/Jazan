namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsApprovedTouristVisit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TouristVisitImages", "TouristVisitId", "dbo.TouristVisits");
            AddColumn("dbo.TouristVisits", "IsApproved", c => c.Boolean(nullable: false));
            AddForeignKey("dbo.TouristVisitImages", "TouristVisitId", "dbo.TouristVisits", "Id", cascadeDelete: true);
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
                        IsApproved = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[TouristVisits]([Name], [CityName], [VisitDate], [CreatedOn], [CreatedBy], [Latitude], [Longitude], [Description], [UpdatedOn], [UpdatedBy], [ImageUrl], [IsApproved])
                      VALUES (@Name, @CityName, @VisitDate, @CreatedOn, @CreatedBy, @Latitude, @Longitude, @Description, @UpdatedOn, @UpdatedBy, @ImageUrl, @IsApproved)
                      
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
                        IsApproved = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[TouristVisits]
                      SET [Name] = @Name, [CityName] = @CityName, [VisitDate] = @VisitDate, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [Latitude] = @Latitude, [Longitude] = @Longitude, [Description] = @Description, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy, [ImageUrl] = @ImageUrl, [IsApproved] = @IsApproved
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TouristVisitImages", "TouristVisitId", "dbo.TouristVisits");
            DropColumn("dbo.TouristVisits", "IsApproved");
            AddForeignKey("dbo.TouristVisitImages", "TouristVisitId", "dbo.TouristVisits", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
