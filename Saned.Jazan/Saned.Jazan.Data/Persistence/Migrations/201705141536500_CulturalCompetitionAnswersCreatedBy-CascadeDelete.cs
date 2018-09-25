namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CulturalCompetitionAnswersCreatedByCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CulturalCompetitionAnswers", "CreatedBy", "dbo.AspNetUsers");
            AddForeignKey("dbo.CulturalCompetitionAnswers", "CreatedBy", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CulturalCompetitionAnswers", "CreatedBy", "dbo.AspNetUsers");
            AddForeignKey("dbo.CulturalCompetitionAnswers", "CreatedBy", "dbo.AspNetUsers", "Id");
        }
    }
}
