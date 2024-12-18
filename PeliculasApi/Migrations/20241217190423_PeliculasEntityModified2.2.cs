using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculasApi.Migrations
{
    /// <inheritdoc />
    public partial class PeliculasEntityModified22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeliculaGeneros_Generos_GeneroId",
                table: "PeliculaGeneros");

            migrationBuilder.DropForeignKey(
                name: "FK_PeliculaGeneros_Peliculas_PeliculaId",
                table: "PeliculaGeneros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculaGeneros",
                table: "PeliculaGeneros");

            migrationBuilder.RenameTable(
                name: "PeliculaGeneros",
                newName: "PeliculaGenero");

            migrationBuilder.RenameIndex(
                name: "IX_PeliculaGeneros_PeliculaId",
                table: "PeliculaGenero",
                newName: "IX_PeliculaGenero_PeliculaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculaGenero",
                table: "PeliculaGenero",
                columns: new[] { "GeneroId", "PeliculaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculaGenero_Generos_GeneroId",
                table: "PeliculaGenero",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculaGenero_Peliculas_PeliculaId",
                table: "PeliculaGenero",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeliculaGenero_Generos_GeneroId",
                table: "PeliculaGenero");

            migrationBuilder.DropForeignKey(
                name: "FK_PeliculaGenero_Peliculas_PeliculaId",
                table: "PeliculaGenero");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculaGenero",
                table: "PeliculaGenero");

            migrationBuilder.RenameTable(
                name: "PeliculaGenero",
                newName: "PeliculaGeneros");

            migrationBuilder.RenameIndex(
                name: "IX_PeliculaGenero_PeliculaId",
                table: "PeliculaGeneros",
                newName: "IX_PeliculaGeneros_PeliculaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculaGeneros",
                table: "PeliculaGeneros",
                columns: new[] { "GeneroId", "PeliculaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculaGeneros_Generos_GeneroId",
                table: "PeliculaGeneros",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculaGeneros_Peliculas_PeliculaId",
                table: "PeliculaGeneros",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
