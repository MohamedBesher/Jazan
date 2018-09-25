namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CulturalCompetitionQuestionCascadeDeletetrue : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CulturalCompetitionAnswers", "CulturalCompetitionQuestionId", "dbo.CulturalCompetitionQuestions");
            DropForeignKey("dbo.CulturalCompetitionQuestionSponsors", "CulturalCompetitionQuestionId", "dbo.CulturalCompetitionQuestions");
            AddForeignKey("dbo.CulturalCompetitionAnswers", "CulturalCompetitionQuestionId", "dbo.CulturalCompetitionQuestions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CulturalCompetitionQuestionSponsors", "CulturalCompetitionQuestionId", "dbo.CulturalCompetitionQuestions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CulturalCompetitionQuestionSponsors", "CulturalCompetitionQuestionId", "dbo.CulturalCompetitionQuestions");
            DropForeignKey("dbo.CulturalCompetitionAnswers", "CulturalCompetitionQuestionId", "dbo.CulturalCompetitionQuestions");
            AddForeignKey("dbo.CulturalCompetitionQuestionSponsors", "CulturalCompetitionQuestionId", "dbo.CulturalCompetitionQuestions", "Id");
            AddForeignKey("dbo.CulturalCompetitionAnswers", "CulturalCompetitionQuestionId", "dbo.CulturalCompetitionQuestions", "Id");
        }
    }
}
