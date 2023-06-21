using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class Refinement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Fights_FightId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IsPlayersTurn",
                table: "Fights");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Fights_FightId",
                table: "Characters",
                column: "FightId",
                principalTable: "Fights",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Fights_FightId",
                table: "Characters");

            migrationBuilder.AddColumn<bool>(
                name: "IsPlayersTurn",
                table: "Fights",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Fights_FightId",
                table: "Characters",
                column: "FightId",
                principalTable: "Fights",
                principalColumn: "Id");
        }
    }
}
