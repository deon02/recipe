using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recipe.Migrations
{
    /// <inheritdoc />
    public partial class thirdmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Recipes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoData",
                table: "Recipes",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "PhotoData",
                table: "Recipes");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Recipes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
