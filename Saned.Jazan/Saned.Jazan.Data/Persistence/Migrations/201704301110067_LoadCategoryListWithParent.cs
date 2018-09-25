namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoadCategoryListWithParent : DbMigration
    {
        public override void Up()
        {
            Sql(@"

 CREATE PROC Categories_SelectParentAndChild
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


--WHERE c2.ParentId=ISNULL(@ParentId,c2.ParentId)


");
        }
        
        public override void Down()
        {
        }
    }
}
