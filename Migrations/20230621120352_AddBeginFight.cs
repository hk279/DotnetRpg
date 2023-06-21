using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class AddBeginFight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Defeats",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Fights",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Victories",
                table: "Characters");

            migrationBuilder.AddColumn<int>(
                name: "FightId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPlayerCharacter",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Fights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerCharacterId = table.Column<int>(type: "int", nullable: false),
                    MyProperty = table.Column<int>(type: "int", nullable: false),
                    FightStatus = table.Column<int>(type: "int", nullable: false),
                    IsPlayersTurn = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fights_Characters_PlayerCharacterId",
                        column: x => x.PlayerCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "Armor", "Avatar", "Class", "CurrentHitPoints", "FightId", "Intelligence", "IsPlayerCharacter", "MaxHitPoints", "Name", "Resistance", "Stamina", "Strength", "UserId" },
                values: new object[,]
                {
                    { 1, 5, "", null, 100, null, 0, false, 100, "Wild Boar", 5, 5, 10, null },
                    { 2, 10, "", null, 100, null, 0, false, 100, "Wolf", 5, 5, 5, null },
                    { 3, 15, "", null, 100, null, 0, false, 100, "Alpha Wolf", 10, 5, 10, null }
                });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                column: "DamageType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                column: "DamageType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                column: "DamageType",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_FightId",
                table: "Characters",
                column: "FightId");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_PlayerCharacterId",
                table: "Fights",
                column: "PlayerCharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Fights_FightId",
                table: "Characters",
                column: "FightId",
                principalTable: "Fights",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Fights_FightId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "Fights");

            migrationBuilder.DropIndex(
                name: "IX_Characters_FightId",
                table: "Characters");

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "FightId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IsPlayerCharacter",
                table: "Characters");

            migrationBuilder.AddColumn<int>(
                name: "Defeats",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Fights",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Victories",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                column: "DamageType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                column: "DamageType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                column: "DamageType",
                value: 0);
        }
    }
}
