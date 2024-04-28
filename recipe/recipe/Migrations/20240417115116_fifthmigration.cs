using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recipe.Migrations
{
    /// <inheritdoc />
    public partial class fifthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Recipes");

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoData",
                table: "Recipes",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
