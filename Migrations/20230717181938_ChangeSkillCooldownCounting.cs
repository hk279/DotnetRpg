using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSkillCooldownCounting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterSkill");

            migrationBuilder.DropTable(
                name: "SkillCooldown");

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemainingCooldown",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CharacterId", "RemainingCooldown" },
                values: new object[] { null, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CharacterId",
                table: "Skills",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Characters_CharacterId",
                table: "Skills",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Characters_CharacterId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_CharacterId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "RemainingCooldown",
                table: "Skills");

            migrationBuilder.CreateTable(
                name: "CharacterSkill",
                columns: table => new
                {
                    CharactersId = table.Column<int>(type: "int", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSkill", x => new { x.CharactersId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_CharacterSkill_Characters_CharactersId",
                        column: x => x.CharactersId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSkill_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillCooldown",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: true),
                    RemainingCooldown = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillCooldown", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillCooldown_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSkill_SkillsId",
                table: "CharacterSkill",
                column: "SkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillCooldown_CharacterId",
                table: "SkillCooldown",
                column: "CharacterId");
        }
    }
}
