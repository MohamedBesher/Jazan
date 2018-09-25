namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class News_UpdateImg : DbMigration
    {

        public override void Up()
        {
            AlterColumn("dbo.NewsImages", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NewsImages", "ImagePath", c => c.String(maxLength: 60));
        }
    }
}
