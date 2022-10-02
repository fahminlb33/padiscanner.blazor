using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PadiScanner.Migrations
{
    public partial class AddUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GE24HFHHQZRRN024W32W8XF7",
                columns: new[] { "FullName", "LastLoginAt", "Password", "Username" },
                values: new object[] { "Fahmi Noor Fiqri", new DateTime(2022, 10, 2, 14, 7, 1, 536, DateTimeKind.Local).AddTicks(11), "$2a$11$6Bih5edhY8tv5xkLxJyTbeC8bazNYYkCxFGljliYtV2pp.1nUNX9m", "fahmi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GE24MT8165ZNXACDZYC8GMEQ",
                columns: new[] { "LastLoginAt", "Password" },
                values: new object[] { new DateTime(2022, 10, 2, 14, 7, 1, 987, DateTimeKind.Local).AddTicks(1043), "$2a$11$DUhAeefkh9djTb9dSyqnhuxJ3gjHZ6/BkGEfOoTkSi5cAsDr1E3ku" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "LastLoginAt", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { "01GEBQMKK8SA2H2RSFXSCJFTMT", "Hanif Hanan Al-Jufri", new DateTime(2022, 10, 2, 14, 7, 1, 685, DateTimeKind.Local).AddTicks(9089), "$2a$11$tYymUy7M5iYwyuCCbkOibuxT8yAJDS/Ct5bVds0CwV2/bwlcvK6je", 1, "hanif" },
                    { "01GEBQQ94E0Z8JBWGVJQNNH1N6", "Abimanyu Okysaputra Rachman", new DateTime(2022, 10, 2, 14, 7, 1, 838, DateTimeKind.Local).AddTicks(6598), "$2a$11$3nC40lWoH9DXeBXFMg0MRujojdAio1uMBKapNoUVofj3UEe.lkpoy", 1, "abimanyu" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GEBQMKK8SA2H2RSFXSCJFTMT");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GEBQQ94E0Z8JBWGVJQNNH1N6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GE24HFHHQZRRN024W32W8XF7",
                columns: new[] { "FullName", "LastLoginAt", "Password", "Username" },
                values: new object[] { "NyankoAdmin", new DateTime(2022, 10, 2, 13, 42, 59, 74, DateTimeKind.Local).AddTicks(6877), "$2a$11$QvQADnPMFBKLgGQKCuxcNO8Ppvde0KO.6KQn/PmX8ypC4/rO3F/om", "nynanko" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GE24MT8165ZNXACDZYC8GMEQ",
                columns: new[] { "LastLoginAt", "Password" },
                values: new object[] { new DateTime(2022, 10, 2, 13, 42, 59, 236, DateTimeKind.Local).AddTicks(6398), "$2a$11$qJQdZBMIILVJvXNEEk4fT./LLmmCzt3yeXZUe5OXINe9Dh0duj/.q" });
        }
    }
}
