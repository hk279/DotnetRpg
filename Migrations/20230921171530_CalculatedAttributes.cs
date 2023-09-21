using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class CalculatedAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Armor",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "MaxEnergy",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Resistance",
                table: "Characters",
                newName: "BaseResistance");

            migrationBuilder.RenameColumn(
                name: "MaxHitPoints",
                table: "Characters",
                newName: "BaseArmor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaseResistance",
                table: "Characters",
                newName: "Resistance");

            migrationBuilder.RenameColumn(
                name: "BaseArmor",
                table: "Characters",
                newName: "MaxHitPoints");

            migrationBuilder.AddColumn<int>(
                name: "Armor",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxEnergy",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
