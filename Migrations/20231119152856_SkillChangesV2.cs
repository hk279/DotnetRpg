using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class SkillChangesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_StatusEffect_StatusEffectId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_StatusEffect_Characters_CharacterId",
                table: "StatusEffect");

            migrationBuilder.DropTable(
                name: "CharacterSkill");

            migrationBuilder.DropIndex(
                name: "IX_StatusEffect_CharacterId",
                table: "StatusEffect");

            migrationBuilder.DropIndex(
                name: "IX_Skills_StatusEffectId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "StatusEffect");

            migrationBuilder.DropColumn(
                name: "RemainingCooldown",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "StatusEffectId",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "Stunned",
                table: "StatusEffect",
                newName: "SkillId");

            migrationBuilder.AddColumn<bool>(
                name: "IsStunned",
                table: "StatusEffect",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SkillInstance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    RemainingCooldown = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillInstance_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillInstance_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatusEffectInstance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusEffectId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    RemainingDuration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusEffectInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusEffectInstance_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatusEffectInstance_StatusEffect_StatusEffectId",
                        column: x => x.StatusEffectId,
                        principalTable: "StatusEffect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "BaseDamageAttributeScalingFactor", "CharacterClass", "Cooldown", "DamageType", "Description", "EnergyCost", "Healing", "MaxBaseDamageFactor", "MinBaseDamageFactor", "Name", "Rank", "TargetType", "WeaponDamagePercentage" },
                values: new object[,]
                {
                    { 1, 90, 1, 0, 1, "Violently charge the enemy.", 5, 15, 40, 30, "Charge", 1, 3, 10 },
                    { 2, 90, 1, 0, 1, "Violently charge the enemy.", 5, 15, 50, 40, "Charge", 2, 3, 20 },
                    { 3, 90, 1, 0, 1, "Violently charge the enemy.", 5, 15, 50, 40, "Charge", 3, 3, 30 }
                });

            migrationBuilder.InsertData(
                table: "StatusEffect",
                columns: new[] { "Id", "DamagePerTurn", "DecreasedDamagePercentage", "DecreasedDamageTakenPercentage", "Duration", "HealingPerTurn", "IncreasedArmorPercentage", "IncreasedDamagePercentage", "IncreasedDamageTakenPercentage", "IncreasedIntelligencePercentage", "IncreasedResistancePercentage", "IncreasedStrengthPercentage", "IsStunned", "Name", "ReducedArmorPercentage", "ReducedIntelligencePercentage", "ReducedResistancePercentage", "ReducedStrengthPercentage", "SkillId" },
                values: new object[,]
                {
                    { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, true, "Charge Stun", 0, 0, 0, 0, 1 },
                    { 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, true, "Charge Stun", 0, 0, 0, 0, 2 },
                    { 3, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, true, "Charge Stun", 0, 0, 0, 0, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffect_SkillId",
                table: "StatusEffect",
                column: "SkillId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillInstance_CharacterId",
                table: "SkillInstance",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillInstance_SkillId",
                table: "SkillInstance",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffectInstance_CharacterId",
                table: "StatusEffectInstance",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffectInstance_StatusEffectId",
                table: "StatusEffectInstance",
                column: "StatusEffectId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusEffect_Skills_SkillId",
                table: "StatusEffect",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusEffect_Skills_SkillId",
                table: "StatusEffect");

            migrationBuilder.DropTable(
                name: "SkillInstance");

            migrationBuilder.DropTable(
                name: "StatusEffectInstance");

            migrationBuilder.DropIndex(
                name: "IX_StatusEffect_SkillId",
                table: "StatusEffect");

            migrationBuilder.DeleteData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "IsStunned",
                table: "StatusEffect");

            migrationBuilder.RenameColumn(
                name: "SkillId",
                table: "StatusEffect",
                newName: "Stunned");

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "StatusEffect",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemainingCooldown",
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

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffect_CharacterId",
                table: "StatusEffect",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_StatusEffectId",
                table: "Skills",
                column: "StatusEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSkill_SkillsId",
                table: "CharacterSkill",
                column: "SkillsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_StatusEffect_StatusEffectId",
                table: "Skills",
                column: "StatusEffectId",
                principalTable: "StatusEffect",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusEffect_Characters_CharacterId",
                table: "StatusEffect",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id");
        }
    }
}
