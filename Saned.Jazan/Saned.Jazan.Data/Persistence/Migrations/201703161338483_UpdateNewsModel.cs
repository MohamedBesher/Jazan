namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateNewsModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NewsImages", "ImagePath", c => c.String(maxLength: 60));
            AlterStoredProcedure("News_Select_Paged", p => new
            {
                PageNumber = p.Int(defaultValueSql: "1"),
                PageSize = p.Int(defaultValueSql: "10"),
                DateFilter = p.String(maxLength: 8, defaultValueSql: "NULL"),
                TitleFilter = p.String(maxLength: 300, defaultValueSql: "NULL"),
                DetailFilter = p.String(defaultValueSql: "NULL")

            }, @" Declare @result Table (NewsId int ,Title nvarchar(300), Details nvarchar(max) , DefaultImage nvarchar(60) , PublishingDate datetime)
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
                    select * from @result ;");
        }

        public override void Down()
        {
            AlterColumn("dbo.NewsImages", "ImagePath", c => c.String(maxLength: 36));
        }
    }
}
