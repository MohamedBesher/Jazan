namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStoredProecdures : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER PROCEDURE [dbo].[Advertisement_SelectPaging]
    @PageNumber[int],
    @PageSize[int],
    @CategoryId[int],
    @UserId[nvarchar](128),
    @FeatureIds[nvarchar](max),
    @AdvertisementId[int]
AS
BEGIN
    -- calcaulate page lowerbound and pageupperbound


    DECLARE @PageLowerBound INT  DECLARE @PageUpperBound INT
    DECLARE @OverallCount INT
    SET @PageLowerBound = @PageSize * (@PageNumber - 1) + 1
    SET @PageUpperBound = @PageLowerBound + @PageSize

    -- set overallcount


    SET @OverallCount = (SELECT COUNT(DISTINCT Advertisements.Id)

        From Advertisements INNER JOIN AdvertisementFeatures
    ON Advertisements.Id = AdvertisementFeatures.AdvertisementId
    inner join Features on AdvertisementFeatures.FeatureId = Features.Id
    inner join Packages on Advertisements.PackageId = Packages.Id
    inner join AspNetUsers on AspNetUsers.Id = Advertisements.CreatedBy
    WHERE(@UserId IS NOT NULL OR(Advertisements.IsActive = 1 and Advertisements.IsApproved = 1)) AND
    (@CategoryId IS NULL OR Advertisements.CategoryId = @CategoryId) AND
    (@UserId IS NULL OR Advertisements.CreatedBy = @UserId) AND
    (@featureids IS NULL OR AdvertisementFeatures.FeatureId in (@featureids)) AND
    (@AdvertisementId IS NULL OR  Advertisements.Id = @AdvertisementId) AND
    (@UserId IS NOT NULL OR(GETDATE() >= AdvertisementFeatures.StartDate and GETDATE() < AdvertisementFeatures.EndDate))--AND
    --(@UserId IS NOT NULL OR(GETDATE() >= Advertisements.StartDate and GETDATE() < Advertisements.EndDate))
    )


    SELECT *, @OverallCount AS OverallCount
    FROM(
    SELECT
    DISTINCT Advertisements.Id as 'AdvertisementId',
    Advertisements.Name as 'AdvertisementName',
    Advertisements.ImageUrl as 'AdvertisementImageUrl',
    Advertisements.CityName 'CityName',
    Advertisements.Description as 'Description',
    Advertisements.CreatedBy as 'CreatedById',
    AspNetUsers.UserName as 'CreatedByUserName',
    Advertisements.CategoryId,
    Advertisements.IsApproved,
    Advertisements.Email as Email,
    Advertisements.FaceBook,
    Advertisements.Instagram,
    Advertisements.Latitude,
    Advertisements.Longitude,
    Advertisements.Mobile,
    Advertisements.Snapchat,
    Advertisements.Twitter,
    Advertisements.WebSite,
    Advertisements.WorkingHours,
    Advertisements.PackageId,
    Packages.ArabicName as 'PackageName',
        (SELECT AVG(TotalDegree)  FROM[dbo].[RatingUsers]
    WHERE RelatedId = Advertisements.Id AND RelatedType = 1) AS Rating,

   (SELECT ISNULL(SUM([Views].[Count]), 0)  FROM[dbo].[Views]
    WHERE RelatedId = Advertisements.Id AND[RelatedTypeId] = 1 ) AS[Views],
    (SELECT
    STUFF((SELECT ', ' + CAST(AdvertisementFeatures.FeatureId AS VARCHAR(max))[text()]
    FROM AdvertisementFeatures
    WHERE AdvertisementFeatures.AdvertisementId = t.AdvertisementId AND
    (@featureids is null or AdvertisementFeatures.FeatureId in (@featureids)) AND
    (@UserId IS NOT NULL OR(GETDATE() >= AdvertisementFeatures.StartDate and GETDATE() < AdvertisementFeatures.EndDate))
    FOR XML PATH(''), TYPE)
    .value('.', 'NVARCHAR(MAX)'),1,2,' ') List_Output
     FROM AdvertisementFeatures t
    where t.AdvertisementId = Advertisements.Id
    GROUP BY AdvertisementId) Features ,
	(SELECT
    STUFF((SELECT ', ' + CAST(AdvertisementImages.ImageUrl + N':' + Cast(AdvertisementImages.Id as varchar(max)) AS VARCHAR(max))[text()]
    FROM AdvertisementImages
    WHERE AdvertisementImages.AdvertisementId = t.AdvertisementId
    FOR XML PATH(''), TYPE)
    .value('.', 'NVARCHAR(MAX)'),1,2,'') List_Output
     FROM AdvertisementImages t
    where t.AdvertisementId = Advertisements.Id
    GROUP BY AdvertisementId) Images,
    --Features.Id as'FeatureId' ,
    --Features.ArabicName as 'FeaturesArabicName' ,
    ROW_NUMBER() OVER(ORDER BY Advertisements.CreatedOn DESC)  AS RowNumber
    From Advertisements
    --INNER JOIN AdvertisementFeatures ON Advertisements.Id = AdvertisementFeatures.AdvertisementId
    --inner join Features on AdvertisementFeatures.FeatureId = Features.Id
    inner join Packages on Advertisements.PackageId = Packages.Id
    inner join AspNetUsers on AspNetUsers.Id = Advertisements.CreatedBy
    where(@UserId IS NOT NULL  OR(Advertisements.IsActive = 1 and IsApproved = 1)) AND
(@featureids is null or(SELECT

STUFF((SELECT ', ' + CAST(AdvertisementFeatures.FeatureId AS VARCHAR(max))[text()]

FROM AdvertisementFeatures

WHERE AdvertisementFeatures.AdvertisementId = t.AdvertisementId AND
(@featureids is null or AdvertisementFeatures.FeatureId in (@featureids)) AND
(@UserId IS NOT NULL OR(GETDATE() >= AdvertisementFeatures.StartDate and GETDATE() < AdvertisementFeatures.EndDate))

FOR XML PATH(''), TYPE)
.value('.', 'NVARCHAR(MAX)'), 1, 2, ' ') List_Output

FROM AdvertisementFeatures t

where t.AdvertisementId = Advertisements.Id

GROUP BY AdvertisementId) is not null) and
(@CategoryId IS NULL OR Advertisements.CategoryId = @CategoryId) AND
(@UserId IS NULL OR Advertisements.CreatedBy = @UserId) AND
--(@featureids is null or AdvertisementFeatures.FeatureId in (@featureids)) AND
(@AdvertisementId IS NULL OR  Advertisements.Id = @AdvertisementId)--AND
--GETDATE() >= AdvertisementFeatures.StartDate and GETDATE() < AdvertisementFeatures.EndDate AND
--(@UserId IS NOT NULL OR(GETDATE() >= Advertisements.StartDate and GETDATE() < Advertisements.EndDate))
	) ADS
    WHERE

    ADS.RowNumber >= @PageLowerBound AND ADS.RowNumber < @PageUpperBound


END");
        }
        
        public override void Down()
        {
        }
    }
}
