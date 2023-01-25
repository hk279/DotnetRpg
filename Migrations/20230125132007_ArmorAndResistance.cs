using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class ArmorAndResistance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Defense",
                table: "Characters",
                newName: "Resistance");

            migrationBuilder.AddColumn<int>(
                name: "Armor",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Armor",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Resistance",
                table: "Characters",
                newName: "Defense");
        }
    }
}
