using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class SkillDamageTypeRenaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SkillTargetType",
                table: "Skills",
                newName: "TargetType");

            migrationBuilder.RenameColumn(
                name: "SkillDamageType",
                table: "Skills",
                newName: "DamageType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetType",
                table: "Skills",
                newName: "SkillTargetType");

            migrationBuilder.RenameColumn(
                name: "DamageType",
                table: "Skills",
                newName: "SkillDamageType");
        }
    }
}
