using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class StatusEffectNameChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Stunned");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Stunned");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Stunned");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Bleeding");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Bleeding");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Bleeding");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Charge Stun");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Charge Stun");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Charge Stun");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Bleed");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Bleed");

            migrationBuilder.UpdateData(
                table: "StatusEffect",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Bleed");
        }
    }
}
