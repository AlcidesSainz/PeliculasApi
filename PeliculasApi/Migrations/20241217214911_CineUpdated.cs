using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculasApi.Migrations
{
    /// <inheritdoc />
    public partial class CineUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeliculaCines_Cines_CinesId",
                table: "PeliculaCines");

            migrationBuilder.DropForeignKey(
                name: "FK_PeliculaCines_Peliculas_PeliculaId",
                table: "PeliculaCines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculaCines",
                table: "PeliculaCines");

            migrationBuilder.RenameTable(
                name: "PeliculaCines",
                newName: "PeliculaCine");

            migrationBuilder.RenameIndex(
                name: "IX_PeliculaCines_PeliculaId",
                table: "PeliculaCine",
                newName: "IX_PeliculaCine_PeliculaId");

            migrationBuilder.RenameIndex(
                name: "IX_PeliculaCines_CinesId",
                table: "PeliculaCine",
                newName: "IX_PeliculaCine_CinesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculaCine",
                table: "PeliculaCine",
                columns: new[] { "CineId", "PeliculaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculaCine_Cines_CinesId",
                table: "PeliculaCine",
                column: "CinesId",
                principalTable: "Cines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculaCine_Peliculas_PeliculaId",
                table: "PeliculaCine",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeliculaCine_Cines_CinesId",
                table: "PeliculaCine");

            migrationBuilder.DropForeignKey(
                name: "FK_PeliculaCine_Peliculas_PeliculaId",
                table: "PeliculaCine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculaCine",
                table: "PeliculaCine");

            migrationBuilder.RenameTable(
                name: "PeliculaCine",
                newName: "PeliculaCines");

            migrationBuilder.RenameIndex(
                name: "IX_PeliculaCine_PeliculaId",
                table: "PeliculaCines",
                newName: "IX_PeliculaCines_PeliculaId");

            migrationBuilder.RenameIndex(
                name: "IX_PeliculaCine_CinesId",
                table: "PeliculaCines",
                newName: "IX_PeliculaCines_CinesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculaCines",
                table: "PeliculaCines",
                columns: new[] { "CineId", "PeliculaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculaCines_Cines_CinesId",
                table: "PeliculaCines",
                column: "CinesId",
                principalTable: "Cines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PeliculaCines_Peliculas_PeliculaId",
                table: "PeliculaCines",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
