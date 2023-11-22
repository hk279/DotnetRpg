using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnetrpg.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fights", x => x.Id);
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
                    Healing = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
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
                    IsStunned = table.Column<bool>(type: "bit", nullable: false),
                    ReducedStrengthPercentage = table.Column<int>(type: "int", nullable: false),
                    ReducedIntelligencePercentage = table.Column<int>(type: "int", nullable: false),
                    ReducedArmorPercentage = table.Column<int>(type: "int", nullable: false),
                    ReducedResistancePercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedStrengthPercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedIntelligencePercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedArmorPercentage = table.Column<int>(type: "int", nullable: false),
                    IncreasedResistancePercentage = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusEffect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusEffect_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPlayerCharacter = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<long>(type: "bigint", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Stamina = table.Column<int>(type: "int", nullable: false),
                    Spirit = table.Column<int>(type: "int", nullable: false),
                    BaseArmor = table.Column<int>(type: "int", nullable: false),
                    BaseResistance = table.Column<int>(type: "int", nullable: false),
                    CurrentHitPoints = table.Column<int>(type: "int", nullable: false),
                    CurrentEnergy = table.Column<int>(type: "int", nullable: false),
                    Class = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    InventorySize = table.Column<int>(type: "int", nullable: false),
                    FightId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Fights_FightId",
                        column: x => x.FightId,
                        principalTable: "Fights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Characters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                    MaxDamage = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

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
                    { 1, 90, 1, 5, 1, "Violently charge the enemy.", 15, 0, 40, 30, "Charge", 1, 3, 10 },
                    { 2, 90, 1, 5, 1, "Violently charge the enemy.", 15, 0, 50, 40, "Charge", 2, 3, 20 },
                    { 3, 90, 1, 5, 1, "Violently charge the enemy.", 15, 0, 50, 40, "Charge", 3, 3, 30 }
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
                name: "IX_Characters_FightId",
                table: "Characters",
                column: "FightId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserId",
                table: "Characters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_CharacterId",
                table: "Item",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillInstance_CharacterId",
                table: "SkillInstance",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillInstance_SkillId",
                table: "SkillInstance",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffect_SkillId",
                table: "StatusEffect",
                column: "SkillId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffectInstance_CharacterId",
                table: "StatusEffectInstance",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusEffectInstance_StatusEffectId",
                table: "StatusEffectInstance",
                column: "StatusEffectId");
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
                name: "Characters");

            migrationBuilder.DropTable(
                name: "StatusEffect");

            migrationBuilder.DropTable(
                name: "Fights");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
