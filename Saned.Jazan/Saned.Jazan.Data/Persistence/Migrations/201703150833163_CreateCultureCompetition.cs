namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCultureCompetition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CulturalCompetitionQuestionSponsors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CulturalCompetitionQuestionId = c.Int(nullable: false),
                        AdvertisementId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.AdvertisementId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.CulturalCompetitionQuestions", t => t.CulturalCompetitionQuestionId)
                .Index(t => t.CulturalCompetitionQuestionId)
                .Index(t => t.AdvertisementId)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.CulturalCompetitionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 500),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 128),
                        CulturalCompetitionQuestionId = c.Int(nullable: false),
                        IsWinner = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.CulturalCompetitionQuestions", t => t.CulturalCompetitionQuestionId)
                .Index(t => t.CreatedBy)
                .Index(t => t.CulturalCompetitionQuestionId);
            
            CreateTable(
                "dbo.CulturalCompetitionQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 250),
                        Question = c.String(nullable: false, maxLength: 800),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 128),
                        UpdatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        IsPublished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateStoredProcedure(
                "dbo.CulturalCompetitionQuestionSponsor_Insert",
                p => new
                    {
                        CulturalCompetitionQuestionId = p.Int(),
                        AdvertisementId = p.Int(),
                        CreatedOn = p.DateTime(),
                        CreatedBy = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[CulturalCompetitionQuestionSponsors]([CulturalCompetitionQuestionId], [AdvertisementId], [CreatedOn], [CreatedBy])
                      VALUES (@CulturalCompetitionQuestionId, @AdvertisementId, @CreatedOn, @CreatedBy)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CulturalCompetitionQuestionSponsors]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CulturalCompetitionQuestionSponsors] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CulturalCompetitionQuestionSponsor_Update",
                p => new
                    {
                        Id = p.Int(),
                        CulturalCompetitionQuestionId = p.Int(),
                        AdvertisementId = p.Int(),
                        CreatedOn = p.DateTime(),
                        CreatedBy = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[CulturalCompetitionQuestionSponsors]
                      SET [CulturalCompetitionQuestionId] = @CulturalCompetitionQuestionId, [AdvertisementId] = @AdvertisementId, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CulturalCompetitionQuestionSponsor_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CulturalCompetitionQuestionSponsors]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CulturalCompetitionAnswer_Insert",
                p => new
                    {
                        Value = p.String(maxLength: 500),
                        CreatedOn = p.DateTime(),
                        CreatedBy = p.String(maxLength: 128),
                        CulturalCompetitionQuestionId = p.Int(),
                        IsWinner = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[CulturalCompetitionAnswers]([Value], [CreatedOn], [CreatedBy], [CulturalCompetitionQuestionId], [IsWinner])
                      VALUES (@Value, @CreatedOn, @CreatedBy, @CulturalCompetitionQuestionId, @IsWinner)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CulturalCompetitionAnswers]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CulturalCompetitionAnswers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CulturalCompetitionAnswer_Update",
                p => new
                    {
                        Id = p.Int(),
                        Value = p.String(maxLength: 500),
                        CreatedOn = p.DateTime(),
                        CreatedBy = p.String(maxLength: 128),
                        CulturalCompetitionQuestionId = p.Int(),
                        IsWinner = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[CulturalCompetitionAnswers]
                      SET [Value] = @Value, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [CulturalCompetitionQuestionId] = @CulturalCompetitionQuestionId, [IsWinner] = @IsWinner
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CulturalCompetitionAnswer_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CulturalCompetitionAnswers]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CulturalCompetitionQuestion_Insert",
                p => new
                    {
                        Title = p.String(maxLength: 250),
                        Question = p.String(maxLength: 800),
                        CreatedOn = p.DateTime(),
                        CreatedBy = p.String(maxLength: 128),
                        UpdatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 128),
                        IsPublished = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[CulturalCompetitionQuestions]([Title], [Question], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [IsPublished])
                      VALUES (@Title, @Question, @CreatedOn, @CreatedBy, @UpdatedOn, @UpdatedBy, @IsPublished)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CulturalCompetitionQuestions]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CulturalCompetitionQuestions] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CulturalCompetitionQuestion_Update",
                p => new
                    {
                        Id = p.Int(),
                        Title = p.String(maxLength: 250),
                        Question = p.String(maxLength: 800),
                        CreatedOn = p.DateTime(),
                        CreatedBy = p.String(maxLength: 128),
                        UpdatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 128),
                        IsPublished = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[CulturalCompetitionQuestions]
                      SET [Title] = @Title, [Question] = @Question, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy, [IsPublished] = @IsPublished
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CulturalCompetitionQuestion_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CulturalCompetitionQuestions]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.CulturalCompetitionQuestion_Delete");
            DropStoredProcedure("dbo.CulturalCompetitionQuestion_Update");
            DropStoredProcedure("dbo.CulturalCompetitionQuestion_Insert");
            DropStoredProcedure("dbo.CulturalCompetitionAnswer_Delete");
            DropStoredProcedure("dbo.CulturalCompetitionAnswer_Update");
            DropStoredProcedure("dbo.CulturalCompetitionAnswer_Insert");
            DropStoredProcedure("dbo.CulturalCompetitionQuestionSponsor_Delete");
            DropStoredProcedure("dbo.CulturalCompetitionQuestionSponsor_Update");
            DropStoredProcedure("dbo.CulturalCompetitionQuestionSponsor_Insert");
            DropForeignKey("dbo.CulturalCompetitionQuestionSponsors", "CulturalCompetitionQuestionId", "dbo.CulturalCompetitionQuestions");
            DropForeignKey("dbo.CulturalCompetitionQuestionSponsors", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CulturalCompetitionAnswers", "CulturalCompetitionQuestionId", "dbo.CulturalCompetitionQuestions");
            DropForeignKey("dbo.CulturalCompetitionQuestions", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CulturalCompetitionQuestions", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CulturalCompetitionAnswers", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CulturalCompetitionQuestionSponsors", "AdvertisementId", "dbo.Advertisements");
            DropIndex("dbo.CulturalCompetitionQuestions", new[] { "UpdatedBy" });
            DropIndex("dbo.CulturalCompetitionQuestions", new[] { "CreatedBy" });
            DropIndex("dbo.CulturalCompetitionAnswers", new[] { "CulturalCompetitionQuestionId" });
            DropIndex("dbo.CulturalCompetitionAnswers", new[] { "CreatedBy" });
            DropIndex("dbo.CulturalCompetitionQuestionSponsors", new[] { "CreatedBy" });
            DropIndex("dbo.CulturalCompetitionQuestionSponsors", new[] { "AdvertisementId" });
            DropIndex("dbo.CulturalCompetitionQuestionSponsors", new[] { "CulturalCompetitionQuestionId" });
            DropTable("dbo.CulturalCompetitionQuestions");
            DropTable("dbo.CulturalCompetitionAnswers");
            DropTable("dbo.CulturalCompetitionQuestionSponsors");
        }
    }
}
