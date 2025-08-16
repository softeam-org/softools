using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Softools.Auth.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "UserCredentials",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_UserCredentials_UserId",
                        column: x => x.UserId,
                        principalTable: "UserCredentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0198b2f8-f42e-736b-948e-380bd1c25618"), "Comercial" },
                    { new Guid("0198b2f8-f42e-7533-9bbe-ad7ae49cf19a"), "Diretor" },
                    { new Guid("0198b2f8-f42e-77f1-abad-3784e1e6401e"), "Projetos" },
                    { new Guid("0198b2f8-f42e-78aa-90ea-0ffc405e98ce"), "Financeiro" },
                    { new Guid("0198b2f8-f42e-7ce0-a796-4f7aa7f82849"), "Marketing" },
                    { new Guid("0198b2f8-f42e-7fb7-a46f-457d2738099b"), "Presidente" },
                    { new Guid("0198b2f8-f42e-7ff3-92d7-e0e105eafd39"), "RH" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentials_Email",
                table: "UserCredentials",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_UserCredentials_Email",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "UserCredentials");
        }
    }
}
