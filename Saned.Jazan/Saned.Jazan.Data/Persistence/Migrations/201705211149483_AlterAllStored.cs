namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterAllStored : DbMigration
    {
        public override void Up()
        {
            Sql(@"USE [Saned_Jazan]
GO
/****** Object:  StoredProcedure [dbo].[Advertisement_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Advertisement_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[Advertisements]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[Advertisement_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Advertisement_Insert]
    @Name [nvarchar](250),
    @CityName [nvarchar](250),
    @CategoryId [int],
    @PackageId [int],
    @Description [nvarchar](1000),
    @ImageUrl [nvarchar](250),
    @Latitude [nvarchar](250),
    @Longitude [nvarchar](250),
    @WorkingHours [nvarchar](max),
    @Mobile [nvarchar](250),
    @Email [nvarchar](250),
    @WebSite [nvarchar](250),
    @Twitter [nvarchar](250),
    @FaceBook [nvarchar](250),
    @Instagram [nvarchar](250),
    @Snapchat [nvarchar](250),
    @IsApproved [bit],
    @IsActive [bit],
    @CreatedOn [datetime],
    @CreatedBy [nvarchar](128),
    @UpdatedOn [datetime],
    @UpdatedBy [nvarchar](128),
    @StartDate [datetime],
    @EndDate [datetime]
AS
BEGIN
    INSERT [dbo].[Advertisements]([Name], [CityName], [CategoryId], [PackageId], [Description], [ImageUrl], [Latitude], [Longitude], [WorkingHours], [Mobile], [Email], [WebSite], [Twitter], [FaceBook], [Instagram], [Snapchat], [IsApproved], [IsActive], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StartDate], [EndDate])
    VALUES (@Name, @CityName, @CategoryId, @PackageId, @Description, @ImageUrl, @Latitude, @Longitude, @WorkingHours, @Mobile, @Email, @WebSite, @Twitter, @FaceBook, @Instagram, @Snapchat, @IsApproved, @IsActive, @CreatedOn, @CreatedBy, @UpdatedOn, @UpdatedBy, @StartDate, @EndDate)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[Advertisements]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[Advertisements] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[Advertisement_SelectPaging]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Advertisement_SelectPaging]
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
    AspNetUsers.Name as 'CreatedByUserName',
    Advertisements.CategoryId,
	Categories.CategoryNameAr as 'CategoryName',
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
	Advertisements.StartDate,
	Advertisements.EndDate,
    Packages.ArabicName as 'PackageName',
        (SELECT AVG(TotalDegree)  FROM[dbo].[RatingUsers]
    WHERE RelatedId = Advertisements.Id AND RelatedType = 1) AS Rating,

   (SELECT ISNULL(SUM([Views].[Count]), 0)  FROM [dbo].[Views]
    WHERE RelatedId = Advertisements.Id AND [RelatedTypeId] = 1 ) AS [Views],
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
    ROW_NUMBER() OVER(ORDER BY Advertisements.packageId , Advertisements.createdOn  DESC)  AS RowNumber
    From Advertisements
    --INNER JOIN AdvertisementFeatures ON Advertisements.Id = AdvertisementFeatures.AdvertisementId
    --inner join Features on AdvertisementFeatures.FeatureId = Features.Id
    inner join Packages on Advertisements.PackageId = Packages.Id
    inner join AspNetUsers on AspNetUsers.Id = Advertisements.CreatedBy
	inner join Categories on Categories.CategoryId = Advertisements.CategoryId
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


END
GO
/****** Object:  StoredProcedure [dbo].[Advertisement_SelectSummaryCount]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Advertisement_SelectSummaryCount] 
    @CategoryId [int]
AS
BEGIN
 DECLARE @OverallCount INT 
 DECLARE @BetweenSections INT 

 SET @OverallCount = (SELECT COUNT(DISTINCT Advertisements.Id) 
    	From Advertisements INNER JOIN AdvertisementFeatures
    ON Advertisements.Id = AdvertisementFeatures.AdvertisementId
    inner join Features on AdvertisementFeatures.FeatureId = Features.Id
    inner join Packages on Advertisements.PackageId = Packages.Id
    inner join AspNetUsers on AspNetUsers.Id = Advertisements.CreatedBy
    WHERE  ((Advertisements.IsActive = 1 and Advertisements.IsApproved = 1)) AND
    (@CategoryId IS NULL OR Advertisements.CategoryId  = @CategoryId ) AND 
    ((GETDATE() >= AdvertisementFeatures.StartDate and GETDATE() < AdvertisementFeatures.EndDate)))


SET @BetweenSections = (SELECT COUNT(DISTINCT Advertisements.Id) 
    	From Advertisements INNER JOIN AdvertisementFeatures
    ON Advertisements.Id = AdvertisementFeatures.AdvertisementId
    inner join Features on AdvertisementFeatures.FeatureId = Features.Id
    inner join Packages on Advertisements.PackageId = Packages.Id
    inner join AspNetUsers on AspNetUsers.Id = Advertisements.CreatedBy
    WHERE  ((Advertisements.IsActive = 1 and Advertisements.IsApproved = 1)) AND
    (@CategoryId IS NULL OR Advertisements.CategoryId  = @CategoryId ) AND 
    (AdvertisementFeatures.FeatureId in (2)) AND
    ((GETDATE() >= AdvertisementFeatures.StartDate and GETDATE() < AdvertisementFeatures.EndDate)))


	SELECT @OverallCount OverallCount , @BetweenSections BetweenSectionsOverAllCount
END
GO
/****** Object:  StoredProcedure [dbo].[Advertisement_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Advertisement_Update]
    @Id [int],
    @Name [nvarchar](250),
    @CityName [nvarchar](250),
    @CategoryId [int],
    @PackageId [int],
    @Description [nvarchar](1000),
    @ImageUrl [nvarchar](250),
    @Latitude [nvarchar](250),
    @Longitude [nvarchar](250),
    @WorkingHours [nvarchar](max),
    @Mobile [nvarchar](250),
    @Email [nvarchar](250),
    @WebSite [nvarchar](250),
    @Twitter [nvarchar](250),
    @FaceBook [nvarchar](250),
    @Instagram [nvarchar](250),
    @Snapchat [nvarchar](250),
    @IsApproved [bit],
    @IsActive [bit],
    @CreatedOn [datetime],
    @CreatedBy [nvarchar](128),
    @UpdatedOn [datetime],
    @UpdatedBy [nvarchar](128),
    @StartDate [datetime],
    @EndDate [datetime]
AS
BEGIN
    UPDATE [dbo].[Advertisements]
    SET [Name] = @Name, [CityName] = @CityName, [CategoryId] = @CategoryId, [PackageId] = @PackageId, [Description] = @Description, [ImageUrl] = @ImageUrl, [Latitude] = @Latitude, [Longitude] = @Longitude, [WorkingHours] = @WorkingHours, [Mobile] = @Mobile, [Email] = @Email, [WebSite] = @WebSite, [Twitter] = @Twitter, [FaceBook] = @FaceBook, [Instagram] = @Instagram, [Snapchat] = @Snapchat, [IsApproved] = @IsApproved, [IsActive] = @IsActive, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy, [StartDate] = @StartDate, [EndDate] = @EndDate
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[AdvertisementFeature_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[AdvertisementFeature_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[AdvertisementFeatures]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[AdvertisementFeature_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[AdvertisementFeature_Insert]
    @AdvertisementId [int],
    @FeatureId [int],
    @Period [int],
    @Quantity [int],
    @StartDate [datetime],
    @EndDate [datetime]
AS
BEGIN
    INSERT [dbo].[AdvertisementFeatures]([AdvertisementId], [FeatureId], [Period], [Quantity], [StartDate], [EndDate])
    VALUES (@AdvertisementId, @FeatureId, @Period, @Quantity, @StartDate, @EndDate)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[AdvertisementFeatures]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[AdvertisementFeatures] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[AdvertisementFeature_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[AdvertisementFeature_Update]
    @Id [int],
    @AdvertisementId [int],
    @FeatureId [int],
    @Period [int],
    @Quantity [int],
    @StartDate [datetime],
    @EndDate [datetime]
AS
BEGIN
    UPDATE [dbo].[AdvertisementFeatures]
    SET [AdvertisementId] = @AdvertisementId, [FeatureId] = @FeatureId, [Period] = @Period, [Quantity] = @Quantity, [StartDate] = @StartDate, [EndDate] = @EndDate
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[AdvertisementImage_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[AdvertisementImage_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[AdvertisementImages]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[AdvertisementImage_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[AdvertisementImage_Insert]
    @ImageUrl [nvarchar](max),
    @AdvertisementId [int]
AS
BEGIN
    INSERT [dbo].[AdvertisementImages]([ImageUrl], [AdvertisementId])
    VALUES (@ImageUrl, @AdvertisementId)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[AdvertisementImages]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[AdvertisementImages] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[AdvertisementImage_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[AdvertisementImage_Update]
    @Id [int],
    @ImageUrl [nvarchar](max),
    @AdvertisementId [int]
AS
BEGIN
    UPDATE [dbo].[AdvertisementImages]
    SET [ImageUrl] = @ImageUrl, [AdvertisementId] = @AdvertisementId
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[Categories_Select_Paged]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE
[dbo].[Categories_Select_Paged] ( 

@NameFilter nvarchar(400) = null , @PageSize int = 10 ,@PageNumber int = 1 , @LanguageId int = 0)
                             As
                             Begin 
                            Declare @result Table (CategoryId int ,CategoryName nvarchar(400), ImageUrl nvarchar(Max))
                            Declare @tempresult Table (CategoryId int ,CategoryName nvarchar(400), ImageUrl nvarchar(MAx))
  
                            insert into @result 
                            SELECT  ca.CategoryId , CASE    
                            WHEN  @LanguageId = 0 THEN ISNULL(ca.CategoryNameAr,ca.CategoryNameEn)
                            WHEN  @LanguageId = 1 THEN ISNULL(ca.CategoryNameEn,ca.CategoryNameAr)
                            END  as 'CategoryName' ,ca.CategoryImage
                            FROM  dbo.Categories ca  
                              where ca.ParentId = 0 order by ca.CreateDate desc
 
                            IF not NULLIF(@NameFilter, '') IS NULL
                            begin 
	                            insert into @tempresult
                                    Select * from @result where  CategoryName Like '%'+ @NameFilter+ '%'
	                            delete from @result where 1 = 1
	                            insert into @result 
	                                select * from @tempresult
	                            delete from @tempresult where 1 = 1
                            end
                            select * from @result order by CategoryName asc 
                                  OFFSET @PageSize *(@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)

                            End
GO
/****** Object:  StoredProcedure [dbo].[Categories_Select_Single]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Categories_Select_Single](@CategoryId int , @LanguageId int = 0)
                                    As 
                                    Begin 
                                    Select DISTINCT top 1  ca.CategoryId , CASE    
                                    WHEN  @LanguageId = 0 THEN ISNULL(ca.CategoryNameAr,ca.CategoryNameEn)
                                    WHEN  @LanguageId = 1 THEN ISNULL(ca.CategoryNameEn,ca.CategoryNameAr)
                                    END  as 'CategoryName',ca.CategoryImage as ImageUrl from dbo.Categories ca where CategoryId = @CategoryId
                                    end
GO
/****** Object:  StoredProcedure [dbo].[Category_Chields_Select_Paged]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Category_Chields_Select_Paged] ( @NameFilter nvarchar(400) = null , @PageSize int = 10 ,@PageNumber int = 1 ,@ParentId int,@LanguageId int = 0)
                     As
                     Begin 
                    Declare @result Table (CategoryId int ,CategoryName nvarchar(400), ImageUrl nvarchar(MAX))
                    Declare @tempresult Table (CategoryId int ,CategoryName nvarchar(400), ImageUrl nvarchar(MAX))
                     insert into @result 
                     SELECT  ca.CategoryId , CASE    
                    WHEN  @LanguageId = 0 THEN ISNULL(ca.CategoryNameAr,ca.CategoryNameEn)
                    WHEN  @LanguageId = 1 THEN ISNULL(ca.CategoryNameEn,ca.CategoryNameAr)
                    END  as 'CategoryName',ca.CategoryImage
                    FROM  dbo.Categories ca  
                      where ca.ParentId = @ParentId order by CreateDate desc
                     IF not NULLIF(@NameFilter, '') IS NULL
                    begin 
	                    insert into @tempresult
                            Select * from @result where  CategoryName Like '%'+ @NameFilter+ '%'
	                    delete from @result where 1 = 1
	                    insert into @result 
	                        select * from @tempresult
	                    delete from @tempresult where 1 = 1
                    end
                    select * from @result order by CategoryName asc
                          OFFSET @PageSize *(@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)
                    End

           
GO
/****** Object:  StoredProcedure [dbo].[Category_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Category_Delete]
    @CategoryId [int]
AS
BEGIN
    DELETE [dbo].[Categories]
    WHERE ([CategoryId] = @CategoryId)
END
GO
/****** Object:  StoredProcedure [dbo].[Category_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Category_Insert]
    @CategoryNameAr [nvarchar](400),
    @CategoryNameEn [nvarchar](400),
    @CategoryImage [nvarchar](max),
    @ParentId [int],
    @Status [tinyint],
    @CreateDate [datetime]
AS
BEGIN
    INSERT [dbo].[Categories]([CategoryNameAr], [CategoryNameEn], [CategoryImage], [ParentId], [Status], [CreateDate])
    VALUES (@CategoryNameAr, @CategoryNameEn, @CategoryImage, @ParentId, @Status, @CreateDate)
    
    DECLARE @CategoryId int
    SELECT @CategoryId = [CategoryId]
    FROM [dbo].[Categories]
    WHERE @@ROWCOUNT > 0 AND [CategoryId] = scope_identity()
    
    SELECT t0.[CategoryId]
    FROM [dbo].[Categories] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[CategoryId] = @CategoryId
END
GO
/****** Object:  StoredProcedure [dbo].[Category_Name_Exist]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Category_Name_Exist](@CategoryName nvarchar(400) , @CategoryId int = Null,@LanguageId int = 0)
	                As
	                Begin 
	                    IF not NULLIF(@CategoryId, '') IS NULL
	                    Begin
	                    --CASE WHEN  @LanguageId = '0' THEN 0 ELSE 1 END
	                    if @LanguageId = 0 
	                    begin 
		                    select Count(1) from dbo.Categories where CategoryId <> @CategoryId and CategoryNameAr = @CategoryName 
	                    end
	                    else
	                    begin
		                    select Count(1) from dbo.C ategories where CategoryId <> @CategoryId and CategoryNameEn = @CategoryName 
	                    end
	                    End
	                    Else 
	                    begin
	                    if @LanguageId = 0
	                    begin 
		                    select Count(1) from dbo.Categories where CategoryNameAr = @CategoryName 
	                    end
	                    else
	                    begin
		                    select Count(1) from dbo.Categories where CategoryNameEn = @CategoryName 
	                    end
	                    end
	                        End 
GO
/****** Object:  StoredProcedure [dbo].[Category_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Category_Update]
    @CategoryId [int],
    @CategoryNameAr [nvarchar](400),
    @CategoryNameEn [nvarchar](400),
    @CategoryImage [nvarchar](max),
    @ParentId [int],
    @Status [tinyint],
    @CreateDate [datetime]
AS
BEGIN
    UPDATE [dbo].[Categories]
    SET [CategoryNameAr] = @CategoryNameAr, [CategoryNameEn] = @CategoryNameEn, [CategoryImage] = @CategoryImage, [ParentId] = @ParentId, [Status] = @Status, [CreateDate] = @CreateDate
    WHERE ([CategoryId] = @CategoryId)
END
GO
/****** Object:  StoredProcedure [dbo].[CulturalCompetitionAnswer_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[CulturalCompetitionAnswer_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[CulturalCompetitionAnswers]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[CulturalCompetitionAnswer_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[CulturalCompetitionAnswer_Insert]
    @Value [nvarchar](500),
    @CreatedOn [datetime],
    @CreatedBy [nvarchar](128),
    @CulturalCompetitionQuestionId [int],
    @IsWinner [bit]
AS
BEGIN
    INSERT [dbo].[CulturalCompetitionAnswers]([Value], [CreatedOn], [CreatedBy], [CulturalCompetitionQuestionId], [IsWinner])
    VALUES (@Value, @CreatedOn, @CreatedBy, @CulturalCompetitionQuestionId, @IsWinner)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[CulturalCompetitionAnswers]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[CulturalCompetitionAnswers] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[CulturalCompetitionAnswer_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[CulturalCompetitionAnswer_Update]
    @Id [int],
    @Value [nvarchar](500),
    @CreatedOn [datetime],
    @CreatedBy [nvarchar](128),
    @CulturalCompetitionQuestionId [int],
    @IsWinner [bit]
AS
BEGIN
    UPDATE [dbo].[CulturalCompetitionAnswers]
    SET [Value] = @Value, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [CulturalCompetitionQuestionId] = @CulturalCompetitionQuestionId, [IsWinner] = @IsWinner
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[CulturalCompetitionQuestion_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[CulturalCompetitionQuestion_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[CulturalCompetitionQuestions]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[CulturalCompetitionQuestion_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[CulturalCompetitionQuestion_Insert]
    @Title [nvarchar](250),
    @Question [nvarchar](800),
    @CreatedOn [datetime],
    @CreatedBy [nvarchar](128),
    @UpdatedOn [datetime],
    @UpdatedBy [nvarchar](128),
    @IsPublished [bit]
AS
BEGIN
    INSERT [dbo].[CulturalCompetitionQuestions]([Title], [Question], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [IsPublished])
    VALUES (@Title, @Question, @CreatedOn, @CreatedBy, @UpdatedOn, @UpdatedBy, @IsPublished)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[CulturalCompetitionQuestions]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[CulturalCompetitionQuestions] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[CulturalCompetitionQuestion_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[CulturalCompetitionQuestion_Update]
    @Id [int],
    @Title [nvarchar](250),
    @Question [nvarchar](800),
    @CreatedOn [datetime],
    @CreatedBy [nvarchar](128),
    @UpdatedOn [datetime],
    @UpdatedBy [nvarchar](128),
    @IsPublished [bit]
AS
BEGIN
    UPDATE [dbo].[CulturalCompetitionQuestions]
    SET [Title] = @Title, [Question] = @Question, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy, [IsPublished] = @IsPublished
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[CulturalCompetitionQuestionSponsor_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[CulturalCompetitionQuestionSponsor_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[CulturalCompetitionQuestionSponsors]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[CulturalCompetitionQuestionSponsor_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[CulturalCompetitionQuestionSponsor_Insert]
    @CulturalCompetitionQuestionId [int],
    @AdvertisementId [int],
    @CreatedOn [datetime],
    @CreatedBy [nvarchar](128)
AS
BEGIN
    INSERT [dbo].[CulturalCompetitionQuestionSponsors]([CulturalCompetitionQuestionId], [AdvertisementId], [CreatedOn], [CreatedBy])
    VALUES (@CulturalCompetitionQuestionId, @AdvertisementId, @CreatedOn, @CreatedBy)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[CulturalCompetitionQuestionSponsors]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[CulturalCompetitionQuestionSponsors] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[CulturalCompetitionQuestionSponsor_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[CulturalCompetitionQuestionSponsor_Update]
    @Id [int],
    @CulturalCompetitionQuestionId [int],
    @AdvertisementId [int],
    @CreatedOn [datetime],
    @CreatedBy [nvarchar](128)
AS
BEGIN
    UPDATE [dbo].[CulturalCompetitionQuestionSponsors]
    SET [CulturalCompetitionQuestionId] = @CulturalCompetitionQuestionId, [AdvertisementId] = @AdvertisementId, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[Devices_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Devices_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[Devices]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[Devices_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Devices_Insert]
    @DeviceId [nvarchar](max),
    @UserId [nvarchar](max),
    @IsOnline [bit]
AS
BEGIN
    INSERT [dbo].[Devices]([DeviceId], [UserId], [IsOnline])
    VALUES (@DeviceId, @UserId, @IsOnline)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[Devices]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[Devices] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[Devices_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Devices_Update]
    @Id [int],
    @DeviceId [nvarchar](max),
    @UserId [nvarchar](max),
    @IsOnline [bit]
AS
BEGIN
    UPDATE [dbo].[Devices]
    SET [DeviceId] = @DeviceId, [UserId] = @UserId, [IsOnline] = @IsOnline
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[MobileSetting_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[MobileSetting_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[MobileSettings]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[MobileSetting_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[MobileSetting_Insert]
    @SettingType [int],
    @Value [nvarchar](250)
AS
BEGIN
    INSERT [dbo].[MobileSettings]([SettingType], [Value])
    VALUES (@SettingType, @Value)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[MobileSettings]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[MobileSettings] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[MobileSetting_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[MobileSetting_Update]
    @Id [int],
    @SettingType [int],
    @Value [nvarchar](250)
AS
BEGIN
    UPDATE [dbo].[MobileSettings]
    SET [SettingType] = @SettingType, [Value] = @Value
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[News_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[News_Delete]
                    (@NewsId int)
                    As 
                    begin 
                    begin tran
                    delete from dbo.News where Id = @NewsId ;
                    Delete from dbo.NewsImages where NewsId = @NewsId
                    commit
                end
GO
/****** Object:  StoredProcedure [dbo].[News_Image_SetAsDefault]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[News_Image_SetAsDefault]
                    (@ImageID int)
                    As
                    begin 
                      begin tran
                      declare @oldId int 
                      Set @oldId = (Select top 1 ImageId from dbo.NewsImages where IsDefault = 1)
                      update dbo.NewsImages set IsDefault = 0 where ImageId = @oldId
                      update dbo.NewsImages set IsDefault = 1 where ImageId = @ImageID
                      commit 
                    end
GO
/****** Object:  StoredProcedure [dbo].[News_Select_Image]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                ALTER PROCEDURE [dbo].[News_Select_Image]
                    @NewsId int
                    As
                    begin
                    select im.ImageId, im.ImagePath, im.IsDefault from dbo.NewsImages im where NewsId = @NewsId;
                    end
                
GO
/****** Object:  StoredProcedure [dbo].[News_Select_Paged]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[News_Select_Paged]
    @PageNumber [int] = 1,
    @PageSize [int] = 10,
    @DateFilter [nvarchar](8) = NULL,
    @TitleFilter [nvarchar](300) = NULL,
    @DetailFilter [nvarchar](max) = NULL
AS
BEGIN
     Declare @result Table (NewsId int ,Title nvarchar(300), Details nvarchar(max) , DefaultImage nvarchar(60) , PublishingDate datetime)
    Declare @tempresult Table (NewsId int ,Title nvarchar(300), Details nvarchar(max) , DefaultImage nvarchar(60) , PublishingDate datetime)
    insert into @result 
    SELECT  cm.Id ,  cm.Title ,cm.Details , nm.ImagePath , cm.PublishingDate
    FROM  dbo.News cm inner join dbo.NewsImages nm on cm.Id = nm.NewsId
    where nm.IsDefault = 1 
    			                    order by cm.PublishingDate desc
    OFFSET @PageSize *(@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)
    IF not NULLIF(@DateFilter, '') IS NULL
    begin 
    	                    insert into @tempresult
    Select * from @result where PublishingDate between @DateFilter and @DateFilter + ' 23:59:59'
    	                    delete from @result where 1 = 1
    	                    insert into @result 
    	                       select * from @tempresult
    	                    delete from @tempresult where 1 = 1
    end
    
    IF not NULLIF(@TitleFilter, '') IS NULL
    begin 
    insert into @tempresult
    Select * from @result where Title Like '%'+ @TitleFilter+ '%'
    delete from @result where 1 = 1
    	                    insert into @result 
    	                       select * from @tempresult
    	                    delete from @tempresult where 1 = 1
    end
    
    IF not NULLIF(@TitleFilter, '') IS NULL
    begin 
    insert into @tempresult
    Select * from @result where Details Like '%'+ @DetailFilter+ '%'
    delete from @result where 1 = 1
    	                    insert into @result 
    	                       select * from @tempresult
    	                    delete from @tempresult where 1 = 1
    end
    select * from @result ;
END
GO
/****** Object:  StoredProcedure [dbo].[News_Select_Single]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[News_Select_Single] 
                            (
                                @NewsId int 
                            )
                            As
                            begin 
                            select top 1 * from dbo.News where Id = @NewsId
                            end 
GO
/****** Object:  StoredProcedure [dbo].[News_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                ALTER PROCEDURE[dbo].[News_Update] 
                        (
                          @NewsId int ,
                          @Title nvarchar(300),
                          @Description nvarchar(max),
                          @concatenatedNewImages nvarchar(max) = Null ,
                          @deletedImages nvarchar(max) = Null
                        ) 
                        As 
                        begin 
                        Set NoCount ON ;
                        begin transaction 
                        Update [dbo].[News] set [Title] =@Title, [Details] = @Description where Id = @NewsId
                        IF not NULLIF(@deletedImages, '') IS NULL
                        begin 
                        -- Deleting Images 
                        Declare @currentDeleted nvarchar(max)
                        Declare @DeletedbatchCount int 
                        Declare @batchesDeleted Table (batch nvarchar(max) , isProcessed bit default 0)
                        Insert into @batchesDeleted (batch) 
                           Select * from dbo.Split(',' , @deletedImages)
                        set @DeletedbatchCount = (select count (*) from @batchesDeleted)

                        While @DeletedbatchCount > 0 
                        begin 
                          Select top 1  @currentDeleted = batch from @batchesDeleted where isProcessed = 0
                          delete from dbo.NewsImages where ImageId = CONVERT(int, @currentDeleted) 
                          update @batchesDeleted set isProcessed = 1 where batch = @currentDeleted
                          set @DeletedbatchCount = @DeletedbatchCount -1
                        end
                        end

                        IF not NULLIF(@concatenatedNewImages, '') IS NULL
                        begin 
                        -- adding new images 
                        Declare @currentLine nvarchar(max)
                        Declare @batchCount int 
                        Declare @batches Table (batch nvarchar(max) , isConcatinated bit default 0);
                        Insert into @batches (batch) 
                           Select * from dbo.Split(',' , @concatenatedNewImages)
                        set @batchCount = (select Count(*) from @batches)

                        While @batchCount > 0
                        begin
                          Select top 1  @currentLine = batch from @batches where isConcatinated = 0
                           Insert into dbo.NewsImages (ImagePath,IsDefault,NewsId)
                             select dbo.Wordparser(@currentLine , 1) ,  CASE WHEN dbo.Wordparser(@currentLine ,2) = '0' THEN 0 ELSE 1 END ,@NewsId
                          update @batches set isConcatinated = 1 where batch = @currentLine
                          set @batchCount = @batchCount - 1
                        end
                        end
                        commit
                        end 
            
GO
/****** Object:  StoredProcedure [dbo].[NotificationLog_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[NotificationLog_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[NotificationLogs]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[NotificationLog_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[NotificationLog_Insert]
    @NotificationMessageId [int],
    @RecieverId [nvarchar](max),
    @isSeen [bit]
AS
BEGIN
    INSERT [dbo].[NotificationLogs]([NotificationMessageId], [RecieverId], [isSeen])
    VALUES (@NotificationMessageId, @RecieverId, @isSeen)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[NotificationLogs]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[NotificationLogs] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[NotificationLog_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[NotificationLog_Update]
    @Id [int],
    @NotificationMessageId [int],
    @RecieverId [nvarchar](max),
    @isSeen [bit]
AS
BEGIN
    UPDATE [dbo].[NotificationLogs]
    SET [NotificationMessageId] = @NotificationMessageId, [RecieverId] = @RecieverId, [isSeen] = @isSeen
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[Notifications_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Notifications_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[Notifications]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[Notifications_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Notifications_Insert]
    @ArabicMessage [nvarchar](max),
    @EnglishMessage [nvarchar](max),
    @CreationDate [datetime],
    @RelatedId [nvarchar](max),
    @RelatedType [nvarchar](max)
AS
BEGIN
    INSERT [dbo].[Notifications]([ArabicMessage], [EnglishMessage], [CreationDate], [RelatedId], [RelatedType])
    VALUES (@ArabicMessage, @EnglishMessage, @CreationDate, @RelatedId, @RelatedType)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[Notifications]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[Notifications] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[Notifications_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[Notifications_Update]
    @Id [int],
    @ArabicMessage [nvarchar](max),
    @EnglishMessage [nvarchar](max),
    @CreationDate [datetime],
    @RelatedId [nvarchar](max),
    @RelatedType [nvarchar](max)
AS
BEGIN
    UPDATE [dbo].[Notifications]
    SET [ArabicMessage] = @ArabicMessage, [EnglishMessage] = @EnglishMessage, [CreationDate] = @CreationDate, [RelatedId] = @RelatedId, [RelatedType] = @RelatedType
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[TouristVisit_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[TouristVisit_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[TouristVisits]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[TouristVisit_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[TouristVisit_Insert]
    @Name [nvarchar](250),
    @CityName [nvarchar](250),
    @VisitDate [datetime],
    @CreatedOn [datetime],
    @CreatedBy [nvarchar](128),
    @Latitude [nvarchar](max),
    @Longitude [nvarchar](max),
    @Description [nvarchar](1000),
    @UpdatedOn [datetime],
    @UpdatedBy [nvarchar](128),
    @ImageUrl [nvarchar](max),
    @IsApproved [bit]
AS
BEGIN
    INSERT [dbo].[TouristVisits]([Name], [CityName], [VisitDate], [CreatedOn], [CreatedBy], [Latitude], [Longitude], [Description], [UpdatedOn], [UpdatedBy], [ImageUrl], [IsApproved])
    VALUES (@Name, @CityName, @VisitDate, @CreatedOn, @CreatedBy, @Latitude, @Longitude, @Description, @UpdatedOn, @UpdatedBy, @ImageUrl, @IsApproved)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[TouristVisits]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[TouristVisits] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[TouristVisit_SelectById]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[TouristVisit_SelectById]
    @Id [int]
AS
BEGIN
    SELECT 
    (SELECT  ISNULL(AVG(TotalDegree),0)  FROM [dbo].[RatingUsers] 
    WHERE RelatedId = TouristVisits.Id AND RelatedType = 2 ) AS Rating,
	TouristVisits.Id,
    TouristVisits.Name,
    TouristVisits.VisitDate,
    TouristVisits.ImageUrl,
    TouristVisits.CityName,
    CreatedOn,
    CreatedBy,
    AspNetUsers.Name AS UserName,
    Latitude,
    Longitude,
    Description,
    TouristVisitImages.ImageUrl as MediaUrl,
    	                TouristVisitImages.MediaType,
    	                TouristVisitImages.Id as TouristVisitImageId
    FROM TouristVisits INNER JOIN AspNetUsers ON AspNetUsers.Id = TouristVisits.CreatedBy
    	                left join TouristVisitImages on TouristVisits.Id = TouristVisitImages.TouristVisitId
    WHERE  IsApproved=1 and TouristVisits.Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[TouristVisit_SelectPaged]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[TouristVisit_SelectPaged]
    @PageNumber [int],
    @PageSize [int],
    @UserId [nvarchar](128) = NULL
AS
BEGIN
     DECLARE @PageLowerBound INT DECLARE @PageUpperBound INT 
    DECLARE @OverallCount INT 
    SET @PageLowerBound = @PageSize * (@PageNumber-1) + 1
    SET @PageUpperBound = @PageLowerBound + @PageSize 
    
    SET @OverallCount = (SELECT COUNT(Id) FROM TouristVisits WHERE 
    	                        (@UserId IS NULL OR TouristVisits.CreatedBy  = @UserId ) )
    
    
    SELECT *, @OverallCount AS OverallCount  
    	             FROM (SELECT TouristVisits.Id ,
    	                          TouristVisits.Name ,
	(SELECT  ISNULL(AVG(TotalDegree),0)  FROM [dbo].[RatingUsers] 
    WHERE RelatedId = TouristVisits.Id AND RelatedType = 2 ) AS Rating,
	(SELECT ISNULL(SUM([Views].[Count]),0)  FROM [dbo].[Views] 
    WHERE RelatedId = TouristVisits.Id AND [RelatedTypeId] = 2  ) AS [Views],
								  TouristVisits.Description , 
    								  TouristVisits.CityName,
    								  TouristVisits.VisitDate,
    								  TouristVisits.ImageUrl,
    				              ROW_NUMBER() OVER(ORDER BY TouristVisits.CreatedOn DESC) AS RowNumber 
    	             FROM TouristVisits 
    	             WHERE IsApproved=1 and (@UserId IS NULL OR TouristVisits.CreatedBy  = @UserId) ) TV
    WHERE TV.RowNumber >= @PageLowerBound AND TV.RowNumber < @PageUpperBound
END
GO
/****** Object:  StoredProcedure [dbo].[TouristVisit_SelectSummaryCount]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[TouristVisit_SelectSummaryCount] 

AS
BEGIN
 DECLARE @OverallCount INT 
 DECLARE @BetweenSections INT 

 SET @OverallCount = (select COUNT(TouristVisits.Id) from TouristVisits)


SET @BetweenSections = (SELECT COUNT(DISTINCT Advertisements.Id) 
    	From Advertisements INNER JOIN AdvertisementFeatures
    ON Advertisements.Id = AdvertisementFeatures.AdvertisementId
    inner join Features on AdvertisementFeatures.FeatureId = Features.Id
    inner join Packages on Advertisements.PackageId = Packages.Id
    inner join AspNetUsers on AspNetUsers.Id = Advertisements.CreatedBy
    WHERE  ((Advertisements.IsActive = 1 and Advertisements.IsApproved = 1)) AND 
    (AdvertisementFeatures.FeatureId in (2)) AND
    ((GETDATE() >= AdvertisementFeatures.StartDate and GETDATE() < AdvertisementFeatures.EndDate)))


	SELECT @OverallCount OverallCount , @BetweenSections BetweenSectionsOverAllCount
END
GO
/****** Object:  StoredProcedure [dbo].[TouristVisit_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[TouristVisit_Update]
    @Id [int],
    @Name [nvarchar](250),
    @CityName [nvarchar](250),
    @VisitDate [datetime],
    @CreatedOn [datetime],
    @CreatedBy [nvarchar](128),
    @Latitude [nvarchar](max),
    @Longitude [nvarchar](max),
    @Description [nvarchar](1000),
    @UpdatedOn [datetime],
    @UpdatedBy [nvarchar](128),
    @ImageUrl [nvarchar](max),
    @IsApproved [bit]
AS
BEGIN
    UPDATE [dbo].[TouristVisits]
    SET [Name] = @Name, [CityName] = @CityName, [VisitDate] = @VisitDate, [CreatedOn] = @CreatedOn, [CreatedBy] = @CreatedBy, [Latitude] = @Latitude, [Longitude] = @Longitude, [Description] = @Description, [UpdatedOn] = @UpdatedOn, [UpdatedBy] = @UpdatedBy, [ImageUrl] = @ImageUrl, [IsApproved] = @IsApproved
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[TouristVisitImage_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[TouristVisitImage_Delete]
    @Id [int]
AS
BEGIN
    DELETE [dbo].[TouristVisitImages]
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [dbo].[TouristVisitImage_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[TouristVisitImage_Insert]
    @ImageUrl [nvarchar](max),
    @MediaType [int],
    @TouristVisitId [int]
AS
BEGIN
    INSERT [dbo].[TouristVisitImages]([ImageUrl], [MediaType], [TouristVisitId])
    VALUES (@ImageUrl, @MediaType, @TouristVisitId)
    
    DECLARE @Id int
    SELECT @Id = [Id]
    FROM [dbo].[TouristVisitImages]
    WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
    
    SELECT t0.[Id]
    FROM [dbo].[TouristVisitImages] AS t0
    WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[TouristVisitImage_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[TouristVisitImage_Update]
    @Id [int],
    @ImageUrl [nvarchar](max),
    @MediaType [int],
    @TouristVisitId [int]
AS
BEGIN
    UPDATE [dbo].[TouristVisitImages]
    SET [ImageUrl] = @ImageUrl, [MediaType] = @MediaType, [TouristVisitId] = @TouristVisitId
    WHERE ([Id] = @Id)
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Advertisement_SelectById]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Advertisement_SelectById]
    @Id [int]
AS
BEGIN
    SELECT Advertisements.* ,
    	                 AdvertisementImages.ImageUrl AS AdvertisementImageUrl , 
    	                 AdvertisementImages.Id AS  AdvertisementImageId
    	                 FROM Advertisements LEFT JOIN AdvertisementImages 
    	                 ON Advertisements.Id = AdvertisementImages.AdvertisementId WHERE Advertisements.Id = @Id
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Categories_SelectParentAndChild]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Saned_Jazan].[Categories_SelectParentAndChild]
 @ParentId INT=NULL
 AS
 
  Create Table #TemCat
 (
CategoryID INT , 
CategoryNameAr NVARCHAR(400),
CategoryNameEn NVARCHAR(400),
CategoryImage NVARCHAR(MAX),
ParentId INT,

Status INT,
 CreateDate Datetime,
)
if	(@ParentId IS NULL)
BEGIN
 INSERT INTO #TemCat
SELECT * FROM  Categories 
UPDATE #TemCat SET ParentId =NULL WHERE ParentId=0

SELECT c2.CategoryId, c1.CategoryNameAr as ParentName , c2.CategoryNameAr, c2.CategoryImage,c2.ParentId
FROM #TemCat c1
right JOIN #TemCat c2
ON c1.CategoryId = c2.parentid ORDER BY c2.ParentId ASC

END
ELSE
BEGIN
 INSERT INTO #TemCat
SELECT * FROM  Categories 
UPDATE #TemCat SET ParentId =NULL WHERE ParentId=0

SELECT c2.CategoryId, c1.CategoryNameAr as ParentName , c2.CategoryNameAr, c2.CategoryImage,c2.ParentId
FROM #TemCat c1
right JOIN #TemCat c2
ON c1.CategoryId = c2.parentid 
WHERE c2.ParentId=@ParentId
ORDER BY c2.ParentId ASC
END

GO
/****** Object:  StoredProcedure [Saned_Jazan].[Comments_Add]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Comments_Add]
    @ParentId [int],
    @RelatedId [nvarchar](max),
    @CommentTypeId [int],
    @CreatedDate [datetime],
    @CommentText [nvarchar](max),
    @UserId [nvarchar](max)
AS
BEGIN
       INSERT INTO[dbo].[Comments](      [CommentTypeId],
    [CreatedDate],
    [ParentId],
    [RelatedId],
    [CommentText],
    [UserId]
    )
    VALUES
    (
    @CommentTypeId,
    @CreatedDate,
    @ParentId,
    @RelatedId,
    @CommentText,
    @UserId
    )
    
    
    SELECT c.Id, c.ParentId, c.RelatedId, c.CommentTypeId, c.CreatedDate, c.UpdatedDate, c.CommentText,
    c.UserId, OverallCount = COUNT(1) OVER() ,anu.Name AS 'FullName' , anu.UserName
    FROM dbo.Comments c
    JOIN dbo.AspNetUsers anu
    ON c.UserId= anu.Id
    
    WHERE c.Id= SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Comments_Delete]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Comments_Delete]
    @CommentId [int]
AS
BEGIN
    DELETE FROM Comments WHERE Id = @CommentId DELETE FROM Comments WHERE ParentId = @CommentId
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Comments_SelectAll]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Comments_SelectAll]
AS
BEGIN
    SELECT * , AspNetUsers.Name as 'FullName' , AspNetUsers.UserName  as 'UserName' FROM Comments
    	                                                                   INNER JOIN AspNetUsers ON AspNetUsers.Id = Comments.UserId
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Comments_SelectByPaging]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Comments_SelectByPaging]
    @PageNumber [int],
    @PageSize [int],
    @RelatedId [nvarchar](max),
    @CommentTypeId [int]
AS
BEGIN
    SELECT  c.Id, c.ParentId, c.RelatedId, c.CommentTypeId, c.CreatedDate, c.UpdatedDate, c.CommentText,
    c.UserId, OverallCount = COUNT(1) OVER() ,anu.Name AS 'FullName' ,anu.UserName , anu.PhotoUrl
    ,(select count(id) from comments where comments.parentid = c.id) as ChildrenCommentsCount
	FROM dbo.Comments c
    JOIN dbo.AspNetUsers anu
    ON c.UserId = anu.Id
    WHERE  (@RelatedId IS NULL OR c.RelatedId = @RelatedId ) 
    AND (@CommentTypeId IS  NULL OR c.CommentTypeId = @CommentTypeId)
    ORDER BY
    c.CreatedDate  DESC	
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY OPTION (RECOMPILE);
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Comments_SelectByParentId]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Comments_SelectByParentId]
    @ParentId [int]
AS
BEGIN
    SELECT Comments.* , AspNetUsers.Name as 'FullName' , AspNetUsers.UserName  as 'UserName' ,  AspNetUsers.PhotoUrl as PhotoUrl FROM Comments
    	             INNER JOIN AspNetUsers ON AspNetUsers.Id = Comments.UserId WHERE ParentId = @ParentId
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Comments_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Comments_Update]
    @CommentId [int],
    @ParentId [int],
    @RelatedId [nvarchar](max),
    @CommentTypeId [int],
    @UpdatedDate [datetime],
    @CommentText [nvarchar](max),
    @UserId [nvarchar](max)
AS
BEGIN
     UPDATE [dbo].[Comments]
    SET [ParentId] = @ParentId
    ,[RelatedId] = @RelatedId
    ,[CommentTypeId] = @CommentTypeId
    ,[UpdatedDate] = @UpdatedDate
    ,[CommentText] = @CommentText
    ,[UserId] = @UserId
    WHERE  Id = @CommentId
    
    	                                SELECT * FROM Comments 
    	                                WHERE Comments.Id = @CommentId
    
    SELECT @@ROWCOUNT
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[CulturalCompetitionAnswers_SelectWinnerUsers]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[CulturalCompetitionAnswers_SelectWinnerUsers]
AS
BEGIN
    SELECT AspNetUsers.Id , AspNetUsers.Name , AspNetUsers.PhotoUrl FROM  AspNetUsers 
    INNER JOIN CulturalCompetitionAnswers ON AspNetUsers.Id = CulturalCompetitionAnswers.CreatedBy
    INNER JOIN CulturalCompetitionQuestions ON CulturalCompetitionQuestions.Id =  CulturalCompetitionAnswers.CulturalCompetitionQuestionId
    WHERE CulturalCompetitionAnswers.IsWinner = 1 
    AND CulturalCompetitionQuestions.IsPublished = 0
    AND CulturalCompetitionQuestions.Id = (SELECT TOP 1 Id FROM CulturalCompetitionQuestions WHERE IsPublished = 0 ORDER BY CreatedOn DESC)
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[CulturalCompetitionQuestion_SelectActiveQuestion]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[CulturalCompetitionQuestion_SelectActiveQuestion]
AS
BEGIN
    SELECT TOP 1 Id ,  Title , Question   FROM CulturalCompetitionQuestions 
    WHERE CulturalCompetitionQuestions.IsPublished = 1
	order by CreatedOn desc
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[CulturalCompetitionQuestionSponsors_SelectSponsors]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[CulturalCompetitionQuestionSponsors_SelectSponsors]
AS
BEGIN
    SELECT Advertisements.Id , Advertisements.Name , Advertisements.ImageUrl FROM CulturalCompetitionQuestionSponsors INNER JOIN Advertisements ON 
    CulturalCompetitionQuestionSponsors.AdvertisementId = Advertisements.Id WHERE CulturalCompetitionQuestionId  = 
    (
    SELECT TOP 1 Id  FROM CulturalCompetitionQuestions 
    WHERE CulturalCompetitionQuestions.IsPublished = 1
	order by createdon desc 
    )
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Features_SelectAll]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Features_SelectAll]
AS
BEGIN
    SELECT * FROM Features
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Features_SelectByPackageId]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Features_SelectByPackageId]
    @Id [int]
AS
BEGIN
     SELECT Packages.Id AS PackageId , Packages.ArabicName AS PackageArabicName , Packages.EnglishName AS PackageEnglishName ,Packages.Period , Packages.Price , 
    Features.Id AS FeatureId , Features.ArabicName AS FeatureArabicName ,Features.EnglishName AS FeatureEnglishName ,PackageFeatures.Period AS PackageFeaturePeriod ,
    PackageFeatures.Quantity AS PackageFeatureQuantity FROM PackageFeatures 
    INNER JOIN Packages ON PackageFeatures.PackageId = Packages.Id 
    INNER JOIN Features ON Features.Id = PackageFeatures.FeatureId
    WHERE PackageFeatures.PackageId = @Id 
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Features_Update]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Features_Update]
    @Id [int],
    @ArabicName [nvarchar](250),
    @EnglishName [nvarchar](250)
AS
BEGIN
    UPDATE [dbo].[Features] SET [ArabicName] = @ArabicName , [EnglishName]  = @EnglishName WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[MobileSetting_SelectAll]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[MobileSetting_SelectAll]
AS
BEGIN
    SELECT [Id] ,[SettingType] ,[Value] FROM [MobileSettings]
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[MobileSetting_SelectBySettingType]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[MobileSetting_SelectBySettingType]
    @SettingType [int]
AS
BEGIN
    SELECT [Id] ,[SettingType] ,[Value] FROM [MobileSettings] WHERE SettingType = @SettingType
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[News_Insert]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[News_Insert]
                    @Title nvarchar(300),
                    @Description nvarchar(max),
                    @concatenatedImages nvarchar(max)
                    As
                    Begin 
                    Begin Transaction
                    Declare @NewsId int
                    Declare @currentLine nvarchar(max)
                    Declare @batchCount int
                    Declare @batches Table(batch nvarchar(max), isConcatinated bit default 0)
                                INSERT[dbo].[News]([Title], [Details], [PublishingDate])
                        VALUES(@Title, @Description, GetDate())
                    Set @NewsId = SCOPE_IDENTITY()
                    Insert into @batches(batch)
                       Select* from dbo.Split(',' , @concatenatedImages)
                    set @batchCount = (select Count(*) from @batches)
                    While @batchCount > 0
                    begin
                      Select top 1  @currentLine = batch from @batches where isConcatinated = 0
                       Insert into dbo.NewsImages (ImagePath,IsDefault,NewsId)
                         select dbo.Wordparser(@currentLine , 1) ,  CASE WHEN dbo.Wordparser(@currentLine ,2) = '0' THEN 0 ELSE 1 END ,@NewsId
                      update @batches set isConcatinated = 1 where batch = @currentLine
                      set @batchCount = @batchCount - 1
                    end
                    --SELECT CASE WHEN @PREPRO = 'False' THEN 0 ELSE 1 END
                    commit
                    end 
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Packages_SelectAll]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Packages_SelectAll]
AS
BEGIN
    SELECT * FROM Packages
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Packages_SelectPaging]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Packages_SelectPaging]
    @PageNumber [int],
    @PageSize [int],
    @Name [nvarchar](max) = NULL,
    @Period [int] = NULL,
    @Price [decimal](18, 2) = NULL
AS
BEGIN
    DECLARE @PageLowerBound INT  DECLARE @PageUpperBound INT 
    DECLARE @OverallCount INT 
    SET @PageLowerBound = @PageSize * (@PageNumber-1) + 1
    SET @PageUpperBound = @PageLowerBound + @PageSize 
    
    	            SET @OverallCount = (SELECT COUNT(Id) FROM Packages WHERE  (@Name IS NULL OR Packages.ArabicName LIKE N'%'+ @Name + N'%' ) 
    AND (@Period IS  NULL OR Packages.Period = @Period)
    	            AND (@Price IS  NULL OR Packages.Price = @Price))
    
    
    	            SELECT * , @OverallCount AS OverallCount  FROM (SELECT * , ROW_NUMBER() OVER(ORDER BY Packages.Id ASC) AS RowNumber FROM Packages WHERE (@Name IS NULL OR Packages.ArabicName LIKE N'%'+ @Name + N'%' ) 
    AND (@Period IS  NULL OR Packages.Period = @Period)
    	            AND (@Price IS  NULL OR Packages.Price = @Price)) PAK
    WHERE PAK.RowNumber >= @PageLowerBound AND PAK.RowNumber < @PageUpperBound
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Ratings_GetAll]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Ratings_GetAll]
AS
BEGIN
    Select * From RatingElements
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Ratings_Save]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Ratings_Save]
    @valueList [nvarchar](max),
    @UserId [nvarchar](128),
    @RelatedId [nvarchar](128),
    @RatingDate [datetime],
    @Description [nvarchar](max),
    @RelatedType [int]
AS
BEGIN
    
    BEGIN TRY BEGIN TRANSACTION 
    DECLARE @UserRatingId BIGINT , @Id BIGINT , @rowpos INT , @rowvalue VARCHAR(8000),@RatingElementId BIGINT , @Degree INT , @rowlen INT;
    
    SET @rowpos = 0 
    SET @rowlen = 0;       
    SET @UserRatingId = (SELECT Id FROM RatingUsers WHERE UserId = @UserId AND RelatedId = @RelatedId and RelatedType = @RelatedType);
    
    -- INSERT Header
    IF (@UserRatingId IS NULL) BEGIN
    
    	   -- INSERT INTO RatingUsers
    		INSERT INTO dbo.RatingUsers (TotalDegree, UserId, RelatedId, RatingDate, Description, RelatedType )
    		VALUES (0,
    		@UserId,
    		@RelatedId,
    		@RatingDate,
    		@Description,
    		@RelatedType);
    -- READ LAS PK OF TABLE RatingUsers
    		SELECT @Id = [Id]
    		FROM RatingUsers
    		WHERE @@ROWCOUNT > 0
    		AND [Id] = SCOPE_IDENTITY();
    
    		SET @UserRatingId =
    		(SELECT t0.[Id]
    		FROM RatingUsers AS t0
    		WHERE @@ROWCOUNT > 0
    		AND t0.[Id] = @Id );
    
    	   DECLARE @charIndex INT=CHARINDEX(',', @valueList, @rowpos + 1);
    	   
    		WHILE @charIndex > 0 
    		BEGIN
    				SET @rowlen = CHARINDEX(',', @valueList, @rowpos + 1) - @rowpos;
    
    				SET @rowvalue = SUBSTRING(@valueList, @rowpos, @rowlen);
    
    				-- SET @RatingElementId = LEFT(@rowvalue,
    				--  CHARINDEX(':', @rowvalue) - 1);
    
    				SET @RatingElementId = LEFT(@rowvalue,CHARINDEX(':',@rowvalue)-1)
    				SET @Degree = RIGHT(@rowvalue,LEN(@rowvalue)-CHARINDEX(':',@rowvalue)) -- SET @Degree = RIGHT(@rowvalue, CHARINDEX(':', @rowvalue) - 1);
    
    	
    
    				INSERT INTO dbo.RatingUserDetails (Degree, RatingElementId, RatingUserId)
    				VALUES (@Degree , -- Degree - int
    				@RatingElementId , -- RatingElementId - bigint
    				@UserRatingId);
    
    				SET @rowpos = CHARINDEX(',', @valueList, @rowpos + @rowlen) + 1;
    SET  @charIndex=CHARINDEX(',', @valueList, @rowpos + 1);
    			  PRINT @charIndex
    			if(@charIndex=0)
    				SET @valueList = SUBSTRING(@valueList, @rowpos, @rowlen);	
    
    		END;
    
    		IF (@charIndex = 0)
    				BEGIN
    						SET @rowvalue = @valueList;
    						
    
    						SET @RatingElementId = LEFT(@rowvalue, CHARINDEX(':', @rowvalue) - 1);
    					SET @Degree = RIGHT(@rowvalue,LEN(@rowvalue)-CHARINDEX(':',@rowvalue))        
    						INSERT INTO dbo.RatingUserDetails (Degree, RatingElementId, RatingUserId)
    						VALUES (@Degree , -- Degree - int
    						@RatingElementId , -- RatingElementId - bigint
    						@UserRatingId);
    
    		END;
    		UPDATE [dbo].[RatingUsers]
    		SET [TotalDegree] =
    		(SELECT SUM([Degree])
    		FROM dbo.RatingUserDetails
    		WHERE RatingUserId = @UserRatingId )
    		WHERE RatingUsers.Id = @UserRatingId
		
    
    END;
    
    
    ELSE 
    		BEGIN 
    
    		SET @charIndex =CHARINDEX(',', @valueList, @rowpos + 1)
    		WHILE  @charIndex> 0
    	  
    	   BEGIN
    
    		SET @rowlen = CHARINDEX(',', @valueList, @rowpos + 1) - @rowpos;
    	SET @rowvalue = SUBSTRING(@valueList, @rowpos, @rowlen);
    	    
    
    		SET @RatingElementId = LEFT(@rowvalue,CHARINDEX(':',@rowvalue)-1)
    		SET @Degree = RIGHT(@rowvalue,LEN(@rowvalue)-CHARINDEX(':',@rowvalue)) 
    
    		UPDATE dbo.RatingUserDetails
    		SET [Degree] = @Degree,
    		[RatingElementId] = @RatingElementId
    		WHERE [RatingUserId] = @UserRatingId
    		AND [RatingElementId] = @RatingElementId;
    
    
    		SET @rowpos = CHARINDEX(',', @valueList, @rowpos + @rowlen) + 1;
    		SET  @charIndex =CHARINDEX(',', @valueList, @rowpos + 1)
    		IF (@charIndex = 0) 
    		SET @valueList = SUBSTRING(@valueList, @rowpos, @rowlen);
    	END;
    
    		    
    	 
    	IF (@charIndex = 0) 
    		BEGIN
    		SET @rowvalue =@valueList
    
    	SET @RatingElementId = LEFT(@rowvalue, CHARINDEX(':', @rowvalue) - 1);
    	SET @Degree = RIGHT(@rowvalue,LEN(@rowvalue)-CHARINDEX(':',@rowvalue))   
    		--SET @RatingElementId = LEFT(@rowvalue, CHARINDEX(':', @rowvalue) - 1);
    
    
    		--SET @Degree = RIGHT(@rowvalue, CHARINDEX(':', @rowvalue) - 1);
    
    
    		UPDATE dbo.RatingUserDetails
    		SET[Degree] = @Degree,
    		[RatingElementId] = @RatingElementId
    		WHERE [RatingUserId] = @UserRatingId
    		AND [RatingElementId] = @RatingElementId;
    
    	
    		END;
    
    UPDATE [dbo].[RatingUsers]
    		SET [TotalDegree] =
    		(SELECT SUM([Degree])
    		FROM dbo.RatingUserDetails
    		WHERE RatingUserId = @UserRatingId )
    		WHERE RatingUsers.Id = @UserRatingId;
    		END;
    
    
    
    COMMIT TRANSACTION
    SELECT 1 END TRY BEGIN CATCH IF @@TRANCOUNT > 0
    ROLLBACK;
    
    
    SELECT 0 END CATCH
    
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Views_ClearHistoy]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Views_ClearHistoy]
    @UserId [nvarchar](max),
    @DeviceId [nvarchar](max)
AS
BEGIN
    if(@UserId is not null)
    begin
    delete from Views
    Where (UserId = @UserId)
    Select @@ROWCOUNT
    end
    else
    begin
    delete from Views
    Where (DeviceId = @DeviceId)
    Select @@ROWCOUNT
    end
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Views_GetAll]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Views_GetAll]
AS
BEGIN
    SELECT * FROM Views
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Views_GetByPaging]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Views_GetByPaging]
    @PageNumber [int],
    @PageSize [int],
    @UserId [nvarchar](max),
    @DeviceId [nvarchar](max)
AS
BEGIN
    if(@UserId is not null)
    begin
    select * from Views where UserId = @UserId
    Order by CreatedDate DESC
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY OPTION (RECOMPILE);
    	end
    else
    select * from Views where DeviceId = @DeviceId
    Order by CreatedDate DESC
    OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY OPTION (RECOMPILE);
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Views_GetItemViews]    Script Date: 5/21/2017 1:39:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Views_GetItemViews]
    @RelatedId [nvarchar](max),
    @RelatedTypeId [int]
AS
BEGIN
    Select ViewCount from ItemViews where (RelatedId = @RelatedId and RelatedTypeId = RelatedTypeId)
END
GO
/****** Object:  StoredProcedure [Saned_Jazan].[Views_Save]    Script Date: 5/21/2017 1:39:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[Saned_Jazan].[Views_Save]
    @UserId [nvarchar](max),
    @DeviceId [nvarchar](max),
    @RelatedTypeId [int],
    @RelatedId [nvarchar](max),
    @Count [int],
    @CreatedDate [datetime]
AS
BEGIN
Declare @TempId BIGINT;
Declare @TempViewId BIGINT;
SET @TempViewId =  (select Id from Views where RelatedId = @RelatedId and RelatedTypeId = @RelatedTypeId);
If (@DeviceId IS NULL) 
Begin
Set @TempId = (Select Id from Views Where UserId = @UserId and RelatedId = @RelatedId and RelatedTypeId = @RelatedTypeId);
if (@TempId IS null) 
begin
Insert into dbo.Views ( UserId , DeviceId, RelatedId, RelatedTypeId, Count, CreatedDate) Values(@UserId , @DeviceId, @RelatedId, @RelatedTypeId, 1 ,@CreatedDate);
end 
END
if (@UserId is null ) 
    	begin 
    	set @TempId =  ( select Id from Views where DeviceId = @DeviceId and RelatedId = @RelatedId and RelatedTypeId = @RelatedTypeId );
    	
		if(@TempId is  null)
    		begin
    	Insert into dbo.Views ( UserId , DeviceId, RelatedId, RelatedTypeId, Count, CreatedDate) Values(@UserId , @DeviceId, @RelatedId, @RelatedTypeId, 1 ,@CreatedDate); 

    	end 
    END
    	if (@TempId is null)
        select * from Views where Id = SCOPE_IDENTITY();
    	else
    	select* from Views where Id = @TempId;
    		
END
GO
");
        }
        
        public override void Down()
        {
        }
    }
}
