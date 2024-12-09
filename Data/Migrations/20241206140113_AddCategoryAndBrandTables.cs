using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PI4Daan.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryAndBrandTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Collectibles");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Collectibles");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Collectibles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Collectibles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collectibles_BrandId",
                table: "Collectibles",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Collectibles_CategoryId",
                table: "Collectibles",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collectibles_Brands_BrandId",
                table: "Collectibles",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Collectibles_Categories_CategoryId",
                table: "Collectibles",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collectibles_Brands_BrandId",
                table: "Collectibles");

            migrationBuilder.DropForeignKey(
                name: "FK_Collectibles_Categories_CategoryId",
                table: "Collectibles");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Collectibles_BrandId",
                table: "Collectibles");

            migrationBuilder.DropIndex(
                name: "IX_Collectibles_CategoryId",
                table: "Collectibles");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Collectibles");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Collectibles");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Collectibles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Collectibles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
