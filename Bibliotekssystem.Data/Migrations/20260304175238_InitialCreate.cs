using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bibliotekssystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ISBN = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PublishedYear = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    MemberSince = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "ISBN", "IsAvailable", "PublishedYear", "Title" },
                values: new object[,]
                {
                    { 1, "J.R.R. Tolkien", "978-91-0-123456-7", false, 1954, "Sagan om ringen" },
                    { 2, "J.R.R. Tolkien", "978-91-0-654321-8", true, 1937, "Hobbiten" },
                    { 3, "George Orwell", "978-91-0-111111-9", false, 1949, "1984" },
                    { 4, "George Orwell", "978-91-0-222222-0", true, 1945, "Djurfarmen" },
                    { 5, "Astrid Lindgren", "978-91-0-333333-1", true, 1945, "Pippi Långstrump" },
                    { 6, "Astrid Lindgren", "978-91-0-444444-2", true, 1973, "Bröderna Lejonhjärta" },
                    { 7, "August Strindberg", "978-91-0-555555-3", true, 1879, "Röda rummet" },
                    { 8, "Hjalmar Söderberg", "978-91-0-666666-4", false, 1905, "Doktor Glas" },
                    { 9, "Selma Lagerlöf", "978-91-0-777777-5", true, 1904, "Herr Arnes penningar" },
                    { 10, "Selma Lagerlöf", "978-91-0-888888-6", true, 1891, "Gösta Berlings saga" },
                    { 11, "Karin Boye", "978-91-0-999999-7", true, 1940, "Kallocain" },
                    { 12, "Selma Lagerlöf", "978-91-0-101010-8", false, 1907, "Nils Holgerssons underbara resa" },
                    { 13, "Astrid Lindgren", "978-91-0-121212-9", true, 1954, "Mio, min Mio" },
                    { 14, "Astrid Lindgren", "978-91-0-131313-0", true, 1981, "Ronja Rövardotter" },
                    { 15, "Hjalmar Bergman", "978-91-0-141414-1", true, 1919, "Markurells i Wadköping" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Email", "MemberId", "MemberSince", "Name" },
                values: new object[,]
                {
                    { 1, "anna@example.com", "M001", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anna Svensson" },
                    { 2, "erik@example.com", "M002", new DateTime(2023, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Erik Johansson" },
                    { 3, "maria@example.com", "M003", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maria Andersson" },
                    { 4, "karl@example.com", "M004", new DateTime(2024, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Karl Pettersson" },
                    { 5, "lisa@example.com", "M005", new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lisa Nilsson" },
                    { 6, "oscar@example.com", "M006", new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oscar Lindqvist" },
                    { 7, "emma@example.com", "M007", new DateTime(2024, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emma Bergström" },
                    { 8, "gustav@example.com", "M008", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gustav Eriksson" }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "Id", "BookId", "DueDate", "LoanDate", "MemberId", "ReturnDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null },
                    { 2, 3, new DateTime(2025, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null },
                    { 3, 8, new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null },
                    { 4, 12, new DateTime(2025, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null },
                    { 5, 2, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 5, new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 7, new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 4, new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, new DateTime(2025, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BookId",
                table: "Loans",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_MemberId",
                table: "Loans",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Email",
                table: "Members",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberId",
                table: "Members",
                column: "MemberId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
