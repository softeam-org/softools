using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Softools.Auth.Migrations
{
    /// <inheritdoc />
    public partial class FixRoleSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0198b2f8-f42e-736b-948e-380bd1c25618"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0198b2f8-f42e-7533-9bbe-ad7ae49cf19a"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0198b2f8-f42e-77f1-abad-3784e1e6401e"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0198b2f8-f42e-78aa-90ea-0ffc405e98ce"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0198b2f8-f42e-7ce0-a796-4f7aa7f82849"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0198b2f8-f42e-7fb7-a46f-457d2738099b"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0198b2f8-f42e-7ff3-92d7-e0e105eafd39"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Presidente" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Diretor" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Projetos" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "RH" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Financeiro" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Marketing" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Comercial" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

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
        }
    }
}
