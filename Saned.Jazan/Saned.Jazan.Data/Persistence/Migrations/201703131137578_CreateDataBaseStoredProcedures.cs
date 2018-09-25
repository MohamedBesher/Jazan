namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDataBaseStoredProcedures : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION dbo.Split (@sep char(1), @s varchar(8000))
                    RETURNS table
                    AS
                    RETURN (
                        WITH splitter_cte AS (
                          SELECT CHARINDEX(@sep, @s) as pos, 0 as lastPos
                          UNION ALL
                          SELECT CHARINDEX(@sep, @s, pos + 1), pos
                          FROM splitter_cte
                          WHERE pos > 0
                        )
                        SELECT SUBSTRING(@s, lastPos + 1,
                                         case when pos = 0 then 80000
                                         else pos - lastPos -1 end) as chunk
                        FROM splitter_cte
                      )
                    GO
                    Create FUNCTION dbo.Wordparser
                    (
                      @multiwordstring VARCHAR(255),
                      @wordnumber      NUMERIC
                    )
                    returns VARCHAR(255)
                    AS
                      BEGIN
                          DECLARE @remainingstring VARCHAR(255)
                          SET @remainingstring=@multiwordstring
                          DECLARE @numberofwords NUMERIC
                          SET @numberofwords=(LEN(@remainingstring) - LEN(REPLACE(@remainingstring, '-', '')) + 1)
                          DECLARE @word VARCHAR(50)
                          DECLARE @parsedwords TABLE
                          (
                             line NUMERIC IDENTITY(1, 1),
                             word VARCHAR(255)
                          )
                          WHILE @numberofwords > 1
                            BEGIN
                                SET @word=LEFT(@remainingstring, CHARINDEX('-', @remainingstring) - 1)
                                INSERT INTO @parsedwords(word)
                                SELECT @word
                                SET @remainingstring= REPLACE(@remainingstring, Concat(@word, '-'), '')
                                SET @numberofwords=(LEN(@remainingstring) - LEN(REPLACE(@remainingstring, '-', '')) + 1)
                                IF @numberofwords = 1
                                  BREAK
                                ELSE
                                  CONTINUE
                            END
                          IF @numberofwords = 1
                            SELECT @word = @remainingstring
                          INSERT INTO @parsedwords(word)
                          SELECT @word
                          RETURN
                            (SELECT word
                             FROM   @parsedwords
                             WHERE  line = @wordnumber)
                      END
                    ");

            // Insert News 
            Sql(@"create procedure News_Insert
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
                    end ");

            Sql(@"
                CREATE procedure[dbo].[News_Select_Image]
                    @NewsId int
                    As
                    begin
                    select im.ImageId, im.ImagePath, im.IsDefault from dbo.NewsImages im where NewsId = @NewsId;
                    end
                ");
            //Sql(@"create procedure dbo.News_Insert_Image
            //        @Url nvarchar(36),
            //        @isDefault bit ,
            //        @NewsId int 
            //        As 
            //        begin 
            //        Insert into dbo.NewsImages (ImagePath,IsDefault,NewsId) Values(@Url,@isDefault,@NewsId)
            //        end");

            Sql(@"CREATE PROCEDURE [dbo].[News_Select_Paged]
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
    FROM  dbo.News cm left outer join dbo.NewsImages nm on cm.Id = nm.NewsId
    where  (nm.IsDefault is null or  nm.IsDefault = 1) 
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
                 ");

            Sql(@"create procedure dbo.News_Select_Single 
                            (
                                @NewsId int 
                            )
                            As
                            begin 
                            select top 1 * from dbo.News where Id = @NewsId
                            end ");

            Sql(@"
                Create procedure [dbo].[News_Update] 
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
            ");

            Sql(@"create procedure dbo.News_Delete
                    (@NewsId int)
                    As 
                    begin 
                    begin tran
                    delete from dbo.News where Id = @NewsId ;
                    Delete from dbo.NewsImages where NewsId = @NewsId
                    commit
                end");

            Sql(@"create procedure dbo.News_Image_SetAsDefault
                    (@ImageID int)
                    As
                    begin 
                      begin tran
                      declare @oldId int 
                      Set @oldId = (Select top 1 ImageId from dbo.NewsImages where IsDefault = 1)
                      update dbo.NewsImages set IsDefault = 0 where ImageId = @oldId
                      update dbo.NewsImages set IsDefault = 1 where ImageId = @ImageID
                      commit 
                    end");

            Sql(@"Create Procedure dbo.Category_Name_Exist(@CategoryName nvarchar(400) , @CategoryId int = Null,@LanguageId int = 0)
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
	                        End ");

            Sql(@"Create procedure [dbo].[Categories_Select_Paged] ( @NameFilter nvarchar(400) = null , @PageSize int = 10 ,@PageNumber int = 1 , @LanguageId int = 0)
                             As
                             Begin 
                            Declare @result Table (CategoryId int ,CategoryName nvarchar(400), ImageUrl nvarchar(MAX))
                            Declare @tempresult Table (CategoryId int ,CategoryName nvarchar(400), ImageUrl nvarchar(MAX))
  
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

                            End");

            Sql(@"Create Procedure [dbo].[Category_Chields_Select_Paged] ( @NameFilter nvarchar(400) = null , @PageSize int = 10 ,@PageNumber int = 1 ,@ParentId int,@LanguageId int = 0)
                     As
                     Begin 
                    Declare @result Table (CategoryId int ,CategoryName nvarchar(400), ImageUrl nvarchar(36))
                    Declare @tempresult Table (CategoryId int ,CategoryName nvarchar(400), ImageUrl nvarchar(36))
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
                    End");

            Sql(@"create procedure [dbo].[Categories_Select_Single](@CategoryId int , @LanguageId int = 0)
                                    As 
                                    Begin 
                                    Select DISTINCT top 1  ca.CategoryId , CASE    
                                    WHEN  @LanguageId = 0 THEN ISNULL(ca.CategoryNameAr,ca.CategoryNameEn)
                                    WHEN  @LanguageId = 1 THEN ISNULL(ca.CategoryNameEn,ca.CategoryNameAr)
                                    END  as 'CategoryName',ca.CategoryImage as ImageUrl from dbo.Categories ca where CategoryId = @CategoryId
                                    end");


            CreateStoredProcedure("Features_SelectAll", "SELECT * FROM Features");

            CreateStoredProcedure("Features_Update",
                     p => new
                     {
                         Id = p.Int(),
                         ArabicName = p.String(maxLength: 250),
                         EnglishName = p.String(maxLength: 250)
                     },
                     @"UPDATE [dbo].[Features] SET [ArabicName] = @ArabicName , [EnglishName]  = @EnglishName WHERE Id = @Id");

            CreateStoredProcedure("Packages_SelectAll", @"SELECT * FROM Packages");

            CreateStoredProcedure("Features_SelectByPackageId", p => new
            {
                Id = p.Int()
            }, @" SELECT Packages.Id AS PackageId , Packages.ArabicName AS PackageArabicName , Packages.EnglishName AS PackageEnglishName ,Packages.Period , Packages.Price , 
                  Features.Id AS FeatureId , Features.ArabicName AS FeatureArabicName ,Features.EnglishName AS FeatureEnglishName ,PackageFeatures.Period AS PackageFeaturePeriod ,
                  PackageFeatures.Quantity AS PackageFeatureQuantity FROM PackageFeatures 
                  INNER JOIN Packages ON PackageFeatures.PackageId = Packages.Id 
                  INNER JOIN Features ON Features.Id = PackageFeatures.FeatureId
                  WHERE PackageFeatures.PackageId = @Id ");


            CreateStoredProcedure("Packages_SelectPaging", P => new
            {
                PageNumber = P.Int(),
                PageSize = P.Int(),
                Name = P.String(defaultValueSql: "NULL"),
                Period = P.Int(defaultValueSql: "NULL"),
                Price = P.Decimal(defaultValueSql: "NULL", precision: 18, scale: 2)
            }, @"DECLARE @PageLowerBound INT  DECLARE @PageUpperBound INT 
                DECLARE @OverallCount INT 
                SET @PageLowerBound = @PageSize * (@PageNumber-1) + 1
                SET @PageUpperBound = @PageLowerBound + @PageSize 

	            SET @OverallCount = (SELECT COUNT(Id) FROM Packages WHERE  (@Name IS NULL OR Packages.ArabicName LIKE N'%'+ @Name + N'%' ) 
                AND (@Period IS  NULL OR Packages.Period = @Period)
	            AND (@Price IS  NULL OR Packages.Price = @Price))


	            SELECT * , @OverallCount AS OverallCount  FROM (SELECT * , ROW_NUMBER() OVER(ORDER BY Packages.Id ASC) AS RowNumber FROM Packages WHERE (@Name IS NULL OR Packages.ArabicName LIKE N'%'+ @Name + N'%' ) 
                AND (@Period IS  NULL OR Packages.Period = @Period)
	            AND (@Price IS  NULL OR Packages.Price = @Price)) PAK
                WHERE PAK.RowNumber >= @PageLowerBound AND PAK.RowNumber < @PageUpperBound");


        }

        public override void Down()
        {
        }
    }
}
