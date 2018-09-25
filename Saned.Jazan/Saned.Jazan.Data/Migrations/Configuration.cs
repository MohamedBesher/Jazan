namespace Saned.Jazan.Data.Migrations
{
    using Core.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Saned.Jazan.Data.Persistence.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistence\Migrations";
        }

        protected override void Seed(Saned.Jazan.Data.Persistence.ApplicationDbContext context)
        {
            if (!context.MobileSettings.Any())
            {
                context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[MobileSettings] ON 
                                                  
                                                    INSERT [dbo].[MobileSettings] ([Id], [SettingType], [Value]) VALUES (1, 1, N'عن التطبيق',N'من نحن')
                                                   
                                                    INSERT [dbo].[MobileSettings] ([Id], [SettingType], [Value]) VALUES (2, 2, N'0566678810',N'رقم الهاتف')
                                                   
                                                    INSERT [dbo].[MobileSettings] ([Id], [SettingType], [Value]) VALUES (3, 3, N'xgazanx',N'لاين')
                                                   
                                                    INSERT [dbo].[MobileSettings] ([Id], [SettingType], [Value]) VALUES (4, 4, N'@xgazanx',N'تويتر')
                                                    
                                                    INSERT [dbo].[MobileSettings] ([Id], [SettingType], [Value]) VALUES (5, 5, N'xgazanx',N'انستجرام')
                                                   
                                                    INSERT [dbo].[MobileSettings] ([Id], [SettingType], [Value]) VALUES (6, 6, N'xgazanx',N'سناب شات')
                                                    
                                                    INSERT [dbo].[MobileSettings] ([Id], [SettingType], [Value]) VALUES (7, 7, N'D6371692',N'بلاك بيرى')
                                                    
                                                    INSERT [dbo].[MobileSettings] ([Id], [SettingType], [Value]) VALUES (8, 8, N'xgazanx@gmail.com',N'البريد الالكترونى')
                                                    
                                                    SET IDENTITY_INSERT [dbo].[MobileSettings] OFF");
            }

            if (!context.Features.Any())
            {
                context.Features.AddOrUpdate(new Feature() { ArabicName = "إعلان بنر رئيسى" },
                                              new Feature() { ArabicName = "إعلان بنر بين الأقسام" },
                                              new Feature() { ArabicName = "الخريطة" },
                                              new Feature() { ArabicName = "تعديل على الإعلان" },
                                              new Feature() { ArabicName = "إرفاق صور بدون تحديد" },
                                              new Feature() { ArabicName = "ظهور الإعلان فى أعلى القسم" },
                                              new Feature() { ArabicName = "إشعارات عن الإعلان" },
                                              new Feature() { ArabicName = "سناب شات" },
                                              new Feature() { ArabicName = "انستجرام" },
                                              new Feature() { ArabicName = "فيسبوك" },
                                              new Feature() { ArabicName = "تويتر" },
                                              new Feature() { ArabicName = "الموقع الإلكترونى" },
                                              new Feature() { ArabicName = "البريد الإلكترونى" },
                                              new Feature() { ArabicName = "رقم الجوال" },
                                              new Feature() { ArabicName = "إرفاق صور" });
            }

            string adminRoleId, userRoleId;


            if (!context.Packages.Any())
            {
                context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Packages] ON 
                                                    INSERT [dbo].[Packages] ([Id], [ArabicName], [EnglishName], [Period], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [Price]) VALUES (1, N'الإعلان الماسى', N'الإعلان الماسى', 12, CAST(0x0000A5CE00000000 AS DateTime), N'66c5c2bb-c403-4314-98e2-d3f00f8a5e70', NULL, NULL, CAST(1500.00 AS Decimal(18, 2)))
                                                
                                                    INSERT [dbo].[Packages] ([Id], [ArabicName], [EnglishName], [Period], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [Price]) VALUES (2, N'الإعلان الذهبى', N'الإعلان الذهبى', NULL, CAST(0x0000A5CE00000000 AS DateTime), N'66c5c2bb-c403-4314-98e2-d3f00f8a5e70', NULL, NULL, CAST(850.00 AS Decimal(18, 2)))
                                               
                                                    INSERT [dbo].[Packages] ([Id], [ArabicName], [EnglishName], [Period], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [Price]) VALUES (3, N'الإعلان الفضى', N'الإعلان الفضى', NULL, CAST(0x0000A5CE00000000 AS DateTime), N'66c5c2bb-c403-4314-98e2-d3f00f8a5e70', NULL, NULL, CAST(150.00 AS Decimal(18, 2)))
                                                
                                                    INSERT [dbo].[Packages] ([Id], [ArabicName], [EnglishName], [Period], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [Price]) VALUES (4, N'الإعلان المجانى', N'الإعلان المجانى', NULL, CAST(0x0000A5CE00000000 AS DateTime), N'66c5c2bb-c403-4314-98e2-d3f00f8a5e70', NULL, NULL, CAST(0.00 AS Decimal(18, 2)))
                                                 
                                                    SET IDENTITY_INSERT [dbo].[Packages] OFF ");
            }

            //if (!context.PackageFeatures.Any())
            //{
            //    context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[PackageFeatures] ON 
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (1, 4, 3, NULL, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (2, 4, 15, NULL, 2)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (3, 3, 2, 30, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (4, 3, 3, NULL, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (5, 3, 4, 30, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (6, 3, 15, NULL, 10)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (8, 3, 6, NULL, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (9, 3, 7, 60, 3)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (10, 2, 1, 180, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (11, 2, 2, 180, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (12, 2, 3, NULL, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (13, 2, 4, 360, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (14, 2, 15, NULL, 20)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (15, 2, 6, NULL, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (16, 2, 7, 360, 12)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (17, 1, 1, 360, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (18, 1, 2, 360, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (19, 1, 3, NULL, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (20, 1, 4, 360, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (21, 1, 15, NULL, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (22, 1, 6, NULL, NULL)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (23, 1, 7, 360, 24)
            //                                            INSERT [dbo].[PackageFeatures] ([Id], [PackageId], [FeatureId], [Period], [Quantity]) VALUES (24, 1, 10, NULL, NULL)
            //                                            SET IDENTITY_INSERT [dbo].[PackageFeatures] OFF");
            //}

            if (!context.Roles.Any())
            {
                adminRoleId = context.Roles.Add(new IdentityRole("Administrator")).Id;
                userRoleId = context.Roles.Add(new IdentityRole("User")).Id;
                context.SaveChanges();
            }
            else
            {
                adminRoleId = context.Roles.First(c => c.Name == "Administrator").Id;
            }



            if (!context.Clients.Any())
            {
                context.Clients.AddRange(BuildClientsList());
                context.SaveChanges();
            }

            if (!context.MobileSettings.Any())
            {

            }

            if (!context.EmailSettings.Any())
            {
                context.EmailSettings.AddOrUpdate(
                    new EmailSetting()
                    {
                        Host = "relay-hosting.secureserver.net",
                        FromEmail = "confirm@saned.sa",
                        Password = "con@saned123#",
                        SubjectAr = "تأكيد البريد الإلكتروني",
                        SubjectEn = "",
                        MessageBodyAr = @"مرحبا @FullName
كود التفعيل هو
@code",
                        MessageBodyEn = "",
                        EmailSettingType = "1"
                    }, new EmailSetting()
                    {
                        Host = "relay-hosting.secureserver.net",
                        FromEmail = "confirm@saned.sa",
                        Password = "con@saned123#",
                        SubjectAr = "تغير كلمة السر",
                        SubjectEn = "",
                        MessageBodyAr = @"مرحبا @FullName
كود اعادة كلمة السر هو
@code
قم بادخالة وادخل كلمة سر جديدة",
                        MessageBodyEn = "",
                        EmailSettingType = "2"
                    }, new EmailSetting()
                    {
                        Host = "relay-hosting.secureserver.net",
                        FromEmail = "confirm@saned.sa",
                        Password = "con@saned123#",
                        SubjectAr = "رساله تنبيه",
                        SubjectEn = "",
                        MessageBodyAr = @"مرحبا @FullName
@code",
                        MessageBodyEn = "",
                        EmailSettingType = "3"
                    }
                    );
            }

        }

        private static List<Client> BuildClientsList()
        {

            List<Client> clientsList = new List<Client>
            {
                new Client
                { Id = "ngAuthApp",
                    Secret= Saned.Jazan.Data.Core.Tools.Helper.GetHash("abc@123"),
                    Name="AngularJS front-end Application",
                    ApplicationType =ApplicationTypes.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 7200,
                    AllowedOrigin = "http://localhost:32150/"
                },
                new Client
                {
                    Id = "consoleApp",
                    Secret= Saned.Jazan.Data.Core.Tools.Helper.GetHash("123@abc"),
                    Name="Console Application",
                    ApplicationType =ApplicationTypes.NativeConfidential,
                    Active = true,
                    RefreshTokenLifeTime = 14400,
                    AllowedOrigin = "*"
                }
            };

            return clientsList;
        }
    }
}
