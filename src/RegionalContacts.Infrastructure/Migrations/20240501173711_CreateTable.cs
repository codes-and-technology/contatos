using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegionalContacts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelefoneRegiao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoArea = table.Column<short>(type: "smallint", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelefoneRegiao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contato",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    NumeroTelefone = table.Column<string>(type: "VARCHAR(9)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    IdRegiao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneRegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contato_TelefoneRegiao_PhoneRegionId",
                        column: x => x.PhoneRegionId,
                        principalTable: "TelefoneRegiao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contato_PhoneRegionId",
                table: "Contato",
                column: "PhoneRegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contato");

            migrationBuilder.DropTable(
                name: "TelefoneRegiao");
        }
    }
}
