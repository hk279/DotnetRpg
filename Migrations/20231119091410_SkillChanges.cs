using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class SkillChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinDamage",
                table: "Skills",
                newName: "WeaponDamagePercentage");

            migrationBuilder.RenameColumn(
                name: "MaxDamage",
                table: "Skills",
                newName: "Rank");

            migrationBuilder.AddColumn<int>(
                name: "BaseDamageAttributeScalingFactor",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaxBaseDamageFactor",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinBaseDamageFactor",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusEffectId",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StatusEffect",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    DamagePerTurn = table.Column<int>(type: "int", nullable: false),
                    HealingPerTurn = table.Column<int>(type: "int", nullable: false),
                    IncreasedDamagePercentage = table.Column<int>(type: "int", nullable: false),
                    DecreasedDamagePercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedDamageTakenPercentage = table.Column<int>(type: "int", nullable: false),
                    DecreasedDamageTakenPercentage = table.Column<int>(type: "int", nullable: false),
                    Stunned = table.Column<int>(type: "int", nullable: false),
                    ReducedStrengthPercentage = table.Column<int>(type: "int", nullable: false),
                    ReducedIntelligencePercentage = table.Column<int>(type: "int", nullable: false),
                    ReducedArmorPercentage = table.Column<int>(type: "int", nullable: false),
                    ReducedResistancePercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedStrengthPercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedIntelligencePercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedArmorPercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedResistancePercentage = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusEffect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusEffect_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_StatusEffectId",
                table: "Skills",
                column: "StatusEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffect_CharacterId",
                table: "StatusEffect",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_StatusEffect_StatusEffectId",
                table: "Skills",
                column: "StatusEffectId",
                principalTable: "StatusEffect",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_StatusEffect_StatusEffectId",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "StatusEffect");

            migrationBuilder.DropIndex(
                name: "IX_Skills_StatusEffectId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "BaseDamageAttributeScalingFactor",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "MaxBaseDamageFactor",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "MinBaseDamageFactor",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "StatusEffectId",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "WeaponDamagePercentage",
                table: "Skills",
                newName: "MinDamage");

            migrationBuilder.RenameColumn(
                name: "Rank",
                table: "Skills",
                newName: "MaxDamage");
        }
    }
}
