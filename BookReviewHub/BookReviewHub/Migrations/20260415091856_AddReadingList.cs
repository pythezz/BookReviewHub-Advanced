using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReviewHub.Migrations
{
    /// <inheritdoc />
    public partial class AddReadingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadingListItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingListItems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadingListItems_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c3d4e5f6-a7b8-9012-cdef-123456789012",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bf9e9b47-3abe-4312-a234-8f6c8fbaf03b", "AQAAAAIAAYagAAAAECPpFMnp89nBtMCTPktXvj7D7qQg7aHuwNMayElGpt2uiVpMt/29pdcvjVpvUIw0UA==" });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingListItems_BookId",
                table: "ReadingListItems",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingListItems_UserId_BookId",
                table: "ReadingListItems",
                columns: new[] { "UserId", "BookId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadingListItems");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c3d4e5f6-a7b8-9012-cdef-123456789012",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a4c781b0-5cbc-424d-9f89-91ff4a906989", "AQAAAAIAAYagAAAAEGlUc45EGvtYapYSW1AbZtnaDpc/l1QS8foJY2INyidY0WkYkN7qNixvSrgWidTbKg==" });
        }
    }
}
