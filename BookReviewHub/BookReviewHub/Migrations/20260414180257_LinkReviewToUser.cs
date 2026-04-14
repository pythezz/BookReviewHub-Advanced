using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReviewHub.Migrations
{
    /// <inheritdoc />
    public partial class LinkReviewToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewerName",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c3d4e5f6-a7b8-9012-cdef-123456789012",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a4c781b0-5cbc-424d-9f89-91ff4a906989", "AQAAAAIAAYagAAAAEGlUc45EGvtYapYSW1AbZtnaDpc/l1QS8foJY2INyidY0WkYkN7qNixvSrgWidTbKg==" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reviews");

            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "Reviews",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewerName",
                table: "Reviews",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c3d4e5f6-a7b8-9012-cdef-123456789012",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "863fce3d-8c26-4c65-b7a8-9dd130af2c61", "AQAAAAIAAYagAAAAEBkc49s2SztECDUIRgel8G062udIN2l7rI1+fDARDc3G+P2g2pPrwsY2byDSbmSADg==" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
