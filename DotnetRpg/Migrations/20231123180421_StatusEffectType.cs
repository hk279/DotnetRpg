using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class StatusEffectType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HealingPerTurn",
                table: "StatusEffect",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "DamagePerTurn",
                table: "StatusEffect",
                newName: "HealingPerTurnFactor");

            migrationBuilder.AddColumn<int>(
                name: "DamagePerTurnFactor",
                table: "StatusEffect",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "BaseDamageAttributeScalingFactor", "CharacterClass", "Cooldown", "DamageType", "Description", "EnergyCost", "Healing", "MaxBaseDamageFactor", "MinBaseDamageFactor", "Name", "Rank", "TargetType", "WeaponDamagePercentage" },
                values: new object[,]
                {
                    { 4, 50, 1, 4, 1, "Slash at your opponent, causing a grievous wound.", 20, 0, 50, 40, "Rend", 1, 3, 30 },
                    { 5, 50, 1, 4, 1, "Slash at your opponent, causing a grievous wound.", 20, 0, 50, 40, "Rend", 2, 3, 50 },
                    { 6, 50, 1, 4, 1, "Slash at your opponent, causing a grievous wound.", 20, 0, 50, 40, "Rend", 3, 3, 50 }
                });

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DamagePerTurnFactor", "Type" },
                values: new object[] { 0, 1 });

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DamagePerTurnFactor", "Type" },
                values: new object[] { 0, 1 });

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DamagePerTurnFactor", "Type" },
                values: new object[] { 0, 1 });

            migrationBuilder.InsertData(
                table: "StatusEffect",
                columns: new[] { "Id", "DamagePerTurnFactor", "DecreasedDamagePercentage", "DecreasedDamageTakenPercentage", "Duration", "HealingPerTurnFactor", "IncreasedArmorPercentage", "IncreasedDamagePercentage", "IncreasedDamageTakenPercentage", "IncreasedIntelligencePercentage", "IncreasedResistancePercentage", "IncreasedStrengthPercentage", "IsStunned", "Name", "ReducedArmorPercentage", "ReducedIntelligencePercentage", "ReducedResistancePercentage", "ReducedStrengthPercentage", "SkillId", "Type" },
                values: new object[,]
                {
                    { 4, 30, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, false, "Bleed", 0, 0, 0, 0, 4, 1 },
                    { 5, 30, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, false, "Bleed", 0, 0, 0, 0, 5, 1 },
                    { 6, 30, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, false, "Bleed", 0, 0, 0, 0, 6, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "DamagePerTurnFactor",
                table: "StatusEffect");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "StatusEffect",
                newName: "HealingPerTurn");

            migrationBuilder.RenameColumn(
                name: "HealingPerTurnFactor",
                table: "StatusEffect",
                newName: "DamagePerTurn");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 1,
                column: "HealingPerTurn",
                value: 0);

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 2,
                column: "HealingPerTurn",
                value: 0);

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 3,
                column: "HealingPerTurn",
                value: 0);
        }
    }
}
