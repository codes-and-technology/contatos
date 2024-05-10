using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegionalContacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoNoCampo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contato_TelefoneRegiao_PhoneRegionId",
                table: "Contato");

            migrationBuilder.DropColumn(
                name: "IdRegiao",
                table: "Contato");

            migrationBuilder.AddForeignKey(
                name: "IdArea",
                table: "Contato",
                column: "PhoneRegionId",
                principalTable: "TelefoneRegiao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "IdArea",
                table: "Contato");

            migrationBuilder.AddColumn<Guid>(
                name: "IdRegiao",
                table: "Contato",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Contato_TelefoneRegiao_PhoneRegionId",
                table: "Contato",
                column: "PhoneRegionId",
                principalTable: "TelefoneRegiao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
