using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetRpg.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusEffect",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    DamagePerTurnFactor = table.Column<int>(type: "int", nullable: false),
                    HealingPerTurnFactor = table.Column<int>(type: "int", nullable: false),
                    IncreasedDamagePercentage = table.Column<int>(type: "int", nullable: false),
                    DecreasedDamagePercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedDamageTakenPercentage = table.Column<int>(type: "int", nullable: false),
                    DecreasedDamageTakenPercentage = table.Column<int>(type: "int", nullable: false),
                    IsStunned = table.Column<bool>(type: "bit", nullable: false),
                    ReducedStrengthPercentage = table.Column<int>(type: "int", nullable: false),
                    ReducedIntelligencePercentage = table.Column<int>(type: "int", nullable: false),
                    ReducedArmorPercentage = table.Column<int>(type: "int", nullable: false),
                    ReducedResistancePercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedStrengthPercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedIntelligencePercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedArmorPercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedResistancePercentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusEffect", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterClass = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DamageType = table.Column<int>(type: "int", nullable: false),
                    TargetType = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    EnergyCost = table.Column<int>(type: "int", nullable: false),
                    Cooldown = table.Column<int>(type: "int", nullable: false),
                    WeaponDamagePercentage = table.Column<int>(type: "int", nullable: false),
                    MinBaseDamageFactor = table.Column<int>(type: "int", nullable: false),
                    MaxBaseDamageFactor = table.Column<int>(type: "int", nullable: false),
                    BaseDamageAttributeScalingFactor = table.Column<int>(type: "int", nullable: false),
                    Healing = table.Column<int>(type: "int", nullable: false),
                    StatusEffectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_StatusEffect_StatusEffectId",
                        column: x => x.StatusEffectId,
                        principalTable: "StatusEffect",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Fights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fights_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPlayerCharacter = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    UnassignedAttributePoints = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Stamina = table.Column<int>(type: "int", nullable: false),
                    Spirit = table.Column<int>(type: "int", nullable: false),
                    BaseArmor = table.Column<int>(type: "int", nullable: false),
                    BaseResistance = table.Column<int>(type: "int", nullable: false),
                    CurrentHitPoints = table.Column<int>(type: "int", nullable: false),
                    CurrentEnergy = table.Column<int>(type: "int", nullable: false),
                    Class = table.Column<int>(type: "int", nullable: false),
                    InventorySize = table.Column<int>(type: "int", nullable: false),
                    FightId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Fights_FightId",
                        column: x => x.FightId,
                        principalTable: "Fights",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Rarity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    IsEquipped = table.Column<bool>(type: "bit", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Stamina = table.Column<int>(type: "int", nullable: false),
                    Spirit = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: true),
                    Slot = table.Column<int>(type: "int", nullable: true),
                    Armor = table.Column<int>(type: "int", nullable: true),
                    Resistance = table.Column<int>(type: "int", nullable: true),
                    MinDamage = table.Column<int>(type: "int", nullable: true),
                    MaxDamage = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillInstance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    RemainingCooldown = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillInstance_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SkillInstance_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillInstance_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
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
                    RemainingDuration = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusEffectInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusEffectInstance_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StatusEffectInstance_StatusEffect_StatusEffectId",
                        column: x => x.StatusEffectId,
                        principalTable: "StatusEffect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatusEffectInstance_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_FightId",
                table: "Characters",
                column: "FightId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserId",
                table: "Characters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_UserId",
                table: "Fights",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_CharacterId",
                table: "Item",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_UserId",
                table: "Item",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillInstance_CharacterId",
                table: "SkillInstance",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillInstance_SkillId",
                table: "SkillInstance",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillInstance_UserId",
                table: "SkillInstance",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_StatusEffectId",
                table: "Skills",
                column: "StatusEffectId",
                unique: true,
                filter: "[StatusEffectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffectInstance_CharacterId",
                table: "StatusEffectInstance",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffectInstance_StatusEffectId",
                table: "StatusEffectInstance",
                column: "StatusEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffectInstance_UserId",
                table: "StatusEffectInstance",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "SkillInstance");

            migrationBuilder.DropTable(
                name: "StatusEffectInstance");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "StatusEffect");

            migrationBuilder.DropTable(
                name: "Fights");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
