namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMobileSettingModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MobileSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SettingType = c.Int(nullable: false),
                        Value = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.MobileSetting_Insert",
                p => new
                    {
                        SettingType = p.Int(),
                        Value = p.String(maxLength: 250),
                    },
                body:
                    @"INSERT [dbo].[MobileSettings]([SettingType], [Value])
                      VALUES (@SettingType, @Value)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[MobileSettings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[MobileSettings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.MobileSetting_Update",
                p => new
                    {
                        Id = p.Int(),
                        SettingType = p.Int(),
                        Value = p.String(maxLength: 250),
                    },
                body:
                    @"UPDATE [dbo].[MobileSettings]
                      SET [SettingType] = @SettingType, [Value] = @Value
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.MobileSetting_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[MobileSettings]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.MobileSetting_Delete");
            DropStoredProcedure("dbo.MobileSetting_Update");
            DropStoredProcedure("dbo.MobileSetting_Insert");
            DropTable("dbo.MobileSettings");
        }
    }
}
