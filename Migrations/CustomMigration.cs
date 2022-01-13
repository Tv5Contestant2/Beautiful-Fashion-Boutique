﻿using ECommerce1.Data;
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

            var sqlResult = "INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [IsAdmin], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [LastLoggedIn],[IsFirstTimeLogin],[Image]) " +
                            "VALUES (N'9c41ddb6-e488-4060-b9fe-0daece2c0bca', N'Administrator', NULL, 1, N'admin@beautifultime.com', N'ADMIN@BEAUTIFULTIME.COM', N'admin@beautifultime.com', N'ADMIN@BEAUTIFULTIME.COM', 1, N'AQAAAAEAACcQAAAAEHowEgP4/BcoIiCIEuNiUJVd0/uwve4NG3MUeUi0mMYkfg4VCNer9um8GWjOPsjy5A==', N'D3IUGT7Y3SENN66UOTBSZY3X4ZAPX7XL', N'a3b7e9c4-77c3-4e6c-9d34-214f48709c7d', NULL, 0, 0, NULL, 1, 0, '" + DateTime.Now + "',1," + "N'/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAAnAAD/4QMraHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjMtYzAxMSA2Ni4xNDU2NjEsIDIwMTIvMDIvMDYtMTQ6NTY6MjcgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjE3RTg1MDE2RDQxMDExRTZBMjU0OTg2QjA1RDAxQTExIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjE3RTg1MDE3RDQxMDExRTZBMjU0OTg2QjA1RDAxQTExIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MTdFODUwMTRENDEwMTFFNkEyNTQ5ODZCMDVEMDFBMTEiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MTdFODUwMTVENDEwMTFFNkEyNTQ5ODZCMDVEMDFBMTEiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7/7gAhQWRvYmUAZMAAAAABAwAQAwIDBgAAEXAAACOmAABAzP/bAIQA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');";
            migrationBuilder.Sql(sqlResult);
        }
    }
}