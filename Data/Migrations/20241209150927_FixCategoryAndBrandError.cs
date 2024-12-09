using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PI4Daan.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCategoryAndBrandError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Zet BrandId en CategoryId op een standaardwaarde of maak ze nullable
            migrationBuilder.Sql("UPDATE Collectibles SET BrandId = NULL WHERE BrandId NOT IN (SELECT Id FROM Brands)");
            migrationBuilder.Sql("UPDATE Collectibles SET CategoryId = NULL WHERE CategoryId NOT IN (SELECT Id FROM Categories)");

            migrationBuilder.DropForeignKey(
                name: "FK_Collectibles_Brands_BrandId",
                table: "Collectibles");

            migrationBuilder.DropForeignKey(
                name: "FK_Collectibles_Categories_CategoryId",
                table: "Collectibles");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Collectibles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "Collectibles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Collectibles_Brands_BrandId",
                table: "Collectibles",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Collectibles_Categories_CategoryId",
                table: "Collectibles",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Collectibles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "Collectibles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
