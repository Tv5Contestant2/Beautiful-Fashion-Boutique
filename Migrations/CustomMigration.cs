using ECommerce1.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ECommerce1.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("CustomMigration_Scripts")]
    public class CustomMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /**
             * 
             * Insert default admin for the 1st migration
             * Email:       admin@beautifultime.com
             * Password:    Aa!123456
             *
             ***/

            var sqlResult = "INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [IsAdmin], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [LastLoggedIn]) " + 
                            "VALUES (N'9c41ddb6-e488-4060-b9fe-0daece2c0bca', NULL, NULL, 1, N'admin@beautifultime.com', N'ADMIN@BEAUTIFULTIME.COM', N'admin@beautifultime.com', N'ADMIN@BEAUTIFULTIME.COM', 1, N'AQAAAAEAACcQAAAAEHowEgP4/BcoIiCIEuNiUJVd0/uwve4NG3MUeUi0mMYkfg4VCNer9um8GWjOPsjy5A==', N'D3IUGT7Y3SENN66UOTBSZY3X4ZAPX7XL', N'a3b7e9c4-77c3-4e6c-9d34-214f48709c7d', NULL, 0, 0, NULL, 1, 0, '" + DateTime.Now + "');";
            migrationBuilder.Sql(sqlResult);
        }
    }
}
