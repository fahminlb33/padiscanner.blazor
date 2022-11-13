using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PadiScanner.Migrations
{
    public partial class PredictionSeverity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Severity",
                table: "Predictions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GE24HFHHQZRRN024W32W8XF7",
                columns: new[] { "LastLoginAt", "Password" },
                values: new object[] { new DateTime(2022, 11, 13, 9, 10, 57, 483, DateTimeKind.Local).AddTicks(9429), "$2a$11$aGidx/nFict4j13pFKERFeyCT2I1ZU6jxRc35cFg2cFIWQmK0OPCq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GE24MT8165ZNXACDZYC8GMEQ",
                columns: new[] { "LastLoginAt", "Password" },
                values: new object[] { new DateTime(2022, 11, 13, 9, 10, 57, 924, DateTimeKind.Local).AddTicks(5234), "$2a$11$Tr5GaRzgWDBZ17bpZrlZI.dIo08clfKumh077rFErYTvLJZ0x.2ky" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GEBQMKK8SA2H2RSFXSCJFTMT",
                columns: new[] { "LastLoginAt", "Password" },
                values: new object[] { new DateTime(2022, 11, 13, 9, 10, 57, 630, DateTimeKind.Local).AddTicks(8613), "$2a$11$ac3yaZA0M/dE3EsWmvB0fOoqVCzAdX7K4z7qMT8Uk.n9qJp1RuFcu" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GEBQQ94E0Z8JBWGVJQNNH1N6",
                columns: new[] { "LastLoginAt", "Password" },
                values: new object[] { new DateTime(2022, 11, 13, 9, 10, 57, 777, DateTimeKind.Local).AddTicks(9765), "$2a$11$s2nii6iejHTuaNzaGbNIDuNbAQuBaBQAbcwa/ef/cYS8QSHUAdT22" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Severity",
                table: "Predictions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GE24HFHHQZRRN024W32W8XF7",
                columns: new[] { "LastLoginAt", "Password" },
                values: new object[] { new DateTime(2022, 10, 2, 14, 7, 1, 536, DateTimeKind.Local).AddTicks(11), "$2a$11$6Bih5edhY8tv5xkLxJyTbeC8bazNYYkCxFGljliYtV2pp.1nUNX9m" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GE24MT8165ZNXACDZYC8GMEQ",
                columns: new[] { "LastLoginAt", "Password" },
                values: new object[] { new DateTime(2022, 10, 2, 14, 7, 1, 987, DateTimeKind.Local).AddTicks(1043), "$2a$11$DUhAeefkh9djTb9dSyqnhuxJ3gjHZ6/BkGEfOoTkSi5cAsDr1E3ku" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GEBQMKK8SA2H2RSFXSCJFTMT",
                columns: new[] { "LastLoginAt", "Password" },
                values: new object[] { new DateTime(2022, 10, 2, 14, 7, 1, 685, DateTimeKind.Local).AddTicks(9089), "$2a$11$tYymUy7M5iYwyuCCbkOibuxT8yAJDS/Ct5bVds0CwV2/bwlcvK6je" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01GEBQQ94E0Z8JBWGVJQNNH1N6",
                columns: new[] { "LastLoginAt", "Password" },
                values: new object[] { new DateTime(2022, 10, 2, 14, 7, 1, 838, DateTimeKind.Local).AddTicks(6598), "$2a$11$3nC40lWoH9DXeBXFMg0MRujojdAio1uMBKapNoUVofj3UEe.lkpoy" });
        }
    }
}
