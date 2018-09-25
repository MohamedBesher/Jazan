namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSelectMainBannerAdvertisement_StoredProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE PROCEDURE SelectMainBannerAdvertisement
                    AS
                    BEGIN
	                    SET NOCOUNT ON;

	                    DECLARE @featureids NVARCHAR(MAX)
	                    SET @featureids = N'1'
                        SELECT
                        top 10  Advertisements.Id as 'AdvertisementId',
	                    Advertisements.ImageUrl as 'AdvertisementImageUrl',
                        ROW_NUMBER() OVER(ORDER BY newid())  AS RowNumber
                        From Advertisements
                        inner join Packages on Advertisements.PackageId = Packages.Id
                        inner join AspNetUsers on AspNetUsers.Id = Advertisements.CreatedBy
	                    inner join Categories on Categories.CategoryId = Advertisements.CategoryId
                        where (Advertisements.IsActive = 1 and IsApproved = 1) AND
                    (@featureids is null or(SELECT

                    STUFF((SELECT ', ' + CAST(AdvertisementFeatures.FeatureId AS VARCHAR(max))[text()]

                    FROM AdvertisementFeatures

                    WHERE AdvertisementFeatures.AdvertisementId = t.AdvertisementId AND
                    (@featureids is null or AdvertisementFeatures.FeatureId in (@featureids)) AND
                    (GETDATE() >= AdvertisementFeatures.StartDate and GETDATE() < AdvertisementFeatures.EndDate)

                    FOR XML PATH(''), TYPE)
                    .value('.', 'NVARCHAR(MAX)'), 1, 2, ' ') List_Output

                    FROM AdvertisementFeatures t

                    where t.AdvertisementId = Advertisements.Id

                    GROUP BY AdvertisementId) is not null) 

                    END");
        }
        
        public override void Down()
        {
        }
    }
}
