using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudioManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IndexAdjustationsAfterNamespaceUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_equipments_equipment_types_equipment_type_id",
                table: "equipments");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "equipments");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_keycloak_id",
                table: "users",
                column: "keycloak_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reservations_start_date_end_date",
                table: "reservations",
                columns: new[] { "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "ix_equipments_name",
                table: "equipments",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_equipments_name_equipment_type_id",
                table: "equipments",
                columns: new[] { "name", "equipment_type_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_equipment_types_name",
                table: "equipment_types",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_equipments_equipment_types_equipment_type_id",
                table: "equipments",
                column: "equipment_type_id",
                principalTable: "equipment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_equipments_equipment_types_equipment_type_id",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "ix_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_keycloak_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_reservations_start_date_end_date",
                table: "reservations");

            migrationBuilder.DropIndex(
                name: "ix_equipments_name",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "ix_equipments_name_equipment_type_id",
                table: "equipments");

            migrationBuilder.DropIndex(
                name: "ix_equipment_types_name",
                table: "equipment_types");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "equipments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "fk_equipments_equipment_types_equipment_type_id",
                table: "equipments",
                column: "equipment_type_id",
                principalTable: "equipment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
