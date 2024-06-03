using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudioManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "reservations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    keycloak_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.Sql("""
                                 INSERT INTO "users" ("id", "first_name", "last_name", "email", "keycloak_id")
                                    VALUES ('ee96af24-137b-4b68-8555-dfe42feaab89', 'Kamil', 'Oberaj', 'qaqoil122@gmail.com', 'ee96af24-137b-4b68-8555-dfe42feaab89')
                                 """);
            
            migrationBuilder.Sql("""
                                 UPDATE "reservations" SET "user_id" = 'ee96af24-137b-4b68-8555-dfe42feaab89'
                                 """);

            migrationBuilder.CreateIndex(
                name: "ix_reservations_user_id",
                table: "reservations",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_reservations_users_user_id",
                table: "reservations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reservations_users_user_id",
                table: "reservations");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropIndex(
                name: "ix_reservations_user_id",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "reservations");
        }
    }
}
