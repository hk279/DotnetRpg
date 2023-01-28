using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class MaxAndCurrentHP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HitPoints",
                table: "Characters",
                newName: "CurrentHitPoints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentHitPoints",
                table: "Characters",
                newName: "HitPoints");
        }
    }
}
