namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCustomeCultureCompetition : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CulturalCompetitionQuestion_SelectActiveQuestion", @"SELECT TOP 1 Id ,  Title , Question   FROM CulturalCompetitionQuestions 
WHERE CulturalCompetitionQuestions.IsPublished = 1");

            CreateStoredProcedure("CulturalCompetitionAnswers_SelectWinnerUsers", @"SELECT AspNetUsers.Id , AspNetUsers.Name , AspNetUsers.PhotoUrl FROM  AspNetUsers 
INNER JOIN CulturalCompetitionAnswers ON AspNetUsers.Id = CulturalCompetitionAnswers.CreatedBy
INNER JOIN CulturalCompetitionQuestions ON CulturalCompetitionQuestions.Id =  CulturalCompetitionAnswers.CulturalCompetitionQuestionId
WHERE CulturalCompetitionAnswers.IsWinner = 1 
AND CulturalCompetitionQuestions.IsPublished = 0
AND CulturalCompetitionQuestions.Id = (SELECT TOP 1 Id FROM CulturalCompetitionQuestions WHERE IsPublished = 0 ORDER BY CreatedDate DESC)");

            CreateStoredProcedure("CulturalCompetitionQuestionSponsors_SelectSponsors", @"SELECT Advertisements.Id , Advertisements.Name , Advertisements.ImageUrl FROM CulturalCompetitionQuestionSponsors INNER JOIN Advertisements ON 
CulturalCompetitionQuestionSponsors.AdvertisementId = Advertisements.Id WHERE CulturalCompetitionQuestionId  = 
(
SELECT TOP 1 Id  FROM CulturalCompetitionQuestions 
WHERE CulturalCompetitionQuestions.IsPublished = 1
)");
        }
        
        public override void Down()
        {
            DropStoredProcedure("CulturalCompetitionQuestion_SelectActiveQuestion");
            DropStoredProcedure("CulturalCompetitionAnswers_SelectWinnerUsers");
            DropStoredProcedure("CulturalCompetitionQuestionSponsors_SelectSponsors");
        }
    }
}
