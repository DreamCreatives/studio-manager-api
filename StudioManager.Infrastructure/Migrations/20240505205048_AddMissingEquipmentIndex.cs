using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudioManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingEquipmentIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_equipments_equipment_types_equipment_type_id",
                table: "equipments");

            migrationBuilder.AddForeignKey(
                name: "fk_equipments_equipment_types_equipment_type_id",
                table: "equipments",
                column: "equipment_type_id",
                principalTable: "equipment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_equipments_equipment_types_equipment_type_id",
                table: "equipments");

            migrationBuilder.AddForeignKey(
                name: "fk_equipments_equipment_types_equipment_type_id",
                table: "equipments",
                column: "equipment_type_id",
                principalTable: "equipment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
