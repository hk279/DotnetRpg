using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class SkillDamageRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.RenameColumn(
                name: "Damage",
                table: "Skills",
                newName: "MinDamage");

            migrationBuilder.AddColumn<int>(
                name: "MaxDamage",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxDamage",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "MinDamage",
                table: "Skills",
                newName: "Damage");

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "CharacterClass", "CharacterId", "Cooldown", "Damage", "DamageType", "EnergyCost", "Healing", "Name", "RemainingCooldown", "TargetType" },
                values: new object[,]
                {
                    { 1, 1, null, 5, 10, 1, 15, 0, "Charge", 0, 3 },
                    { 2, 1, null, 5, 5, 1, 10, 0, "Rend", 0, 3 },
                    { 3, 1, null, 10, 0, 1, 10, 0, "Enrage", 0, 1 },
                    { 4, 1, null, 2, 20, 1, 20, 0, "Skillful Strike", 0, 3 },
                    { 5, 2, null, 10, 0, 2, 15, 0, "Arcane Barrier", 0, 2 },
                    { 6, 2, null, 2, 20, 2, 20, 0, "Ice Lance", 0, 3 },
                    { 7, 2, null, 3, 5, 2, 10, 0, "Combustion", 0, 3 },
                    { 8, 2, null, 10, 10, 2, 30, 0, "Lightning Storm", 0, 3 },
                    { 9, 3, null, 10, 0, 2, 10, 0, "Battle Meditation", 0, 1 },
                    { 10, 3, null, 3, 0, 2, 15, 20, "Miraclous Touch", 0, 2 },
                    { 11, 3, null, 2, 20, 2, 20, 0, "Holy Smite", 0, 3 },
                    { 12, 3, null, 3, 5, 2, 10, 0, "Cleansing Pain", 0, 3 }
                });
        }
    }
}
