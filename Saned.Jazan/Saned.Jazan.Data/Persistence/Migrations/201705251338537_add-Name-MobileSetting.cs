namespace Saned.Jazan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNameMobileSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MobileSettings", "Name", c => c.String());
            AlterStoredProcedure(
                "dbo.MobileSetting_Insert",
                p => new
                    {
                        SettingType = p.Int(),
                        Value = p.String(maxLength: 250),
                        Name = p.String(),
                    },
                body:
                    @"INSERT [dbo].[MobileSettings]([SettingType], [Value], [Name])
                      VALUES (@SettingType, @Value, @Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[MobileSettings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[MobileSettings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.MobileSetting_Update",
                p => new
                    {
                        Id = p.Int(),
                        SettingType = p.Int(),
                        Value = p.String(maxLength: 250),
                        Name = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[MobileSettings]
                      SET [SettingType] = @SettingType, [Value] = @Value, [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.MobileSettings", "Name");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
