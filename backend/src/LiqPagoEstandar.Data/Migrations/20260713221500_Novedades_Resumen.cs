using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiqPagoEstandar.Data.Migrations
{
    /// <inheritdoc />
    public partial class Novedades_Resumen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Novedades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalId = table.Column<int>(type: "int", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    HorasNormales = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    HorasFeriado = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    HorasExtra = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Novedades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Novedades_Personal_PersonalId",
                        column: x => x.PersonalId,
                        principalTable: "Personal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResumenesMensuales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumenesMensuales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResumenPersonalDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumenMensualId = table.Column<int>(type: "int", nullable: false),
                    PersonalId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    ClienteNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PersonalNombreCompleto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Dni = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CategoriaNombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValorHora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SueldoBasicoNormal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalHorasNormales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemHorasExtras = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AniosAntiguedad = table.Column<int>(type: "int", nullable: false),
                    ItemAntiguedad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemFeriados = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemZonaDesfavorable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAPagar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PdfContenido = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PdfNombreArchivo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumenPersonalDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumenPersonalDetalles_ResumenesMensuales_ResumenMensualId",
                        column: x => x.ResumenMensualId,
                        principalTable: "ResumenesMensuales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Novedades_PersonalId_Anio_Mes",
                table: "Novedades",
                columns: new[] { "PersonalId", "Anio", "Mes" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumenesMensuales_Anio_Mes",
                table: "ResumenesMensuales",
                columns: new[] { "Anio", "Mes" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumenPersonalDetalles_ResumenMensualId",
                table: "ResumenPersonalDetalles",
                column: "ResumenMensualId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Novedades");

            migrationBuilder.DropTable(
                name: "ResumenPersonalDetalles");

            migrationBuilder.DropTable(
                name: "ResumenesMensuales");
        }
    }
}
