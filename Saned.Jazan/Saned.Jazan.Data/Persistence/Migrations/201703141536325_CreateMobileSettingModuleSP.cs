namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMobileSettingModuleSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("MobileSetting_SelectAll", @"SELECT [Id] ,[SettingType] ,[Value] FROM [MobileSettings]");
            CreateStoredProcedure("MobileSetting_SelectBySettingType", p => new { SettingType = p.Int() }, @"SELECT [Id] ,[SettingType] ,[Value] FROM [MobileSettings] WHERE SettingType = @SettingType");
        }
        
        public override void Down()
        {
            DropStoredProcedure("MobileSetting_SelectAll");
            DropStoredProcedure("MobileSetting_SelectBySettingType");
        }
    }
}
