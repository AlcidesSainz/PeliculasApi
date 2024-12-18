using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculasApi.Migrations
{
    /// <inheritdoc />
    public partial class PeliculasEntityModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculaGeneros",
                table: "PeliculaGeneros");

            migrationBuilder.DropIndex(
                name: "IX_PeliculaGeneros_GeneroId",
                table: "PeliculaGeneros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculaCines",
                table: "PeliculaCines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculaActor",
                table: "PeliculaActor");

            migrationBuilder.DropIndex(
                name: "IX_PeliculaActor_ActorId",
                table: "PeliculaActor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculaGeneros",
                table: "PeliculaGeneros",
                columns: new[] { "GeneroId", "PeliculaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculaCines",
                table: "PeliculaCines",
                columns: new[] { "CineId", "PeliculaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculaActor",
                table: "PeliculaActor",
                columns: new[] { "ActorId", "PeliculaId" });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaGeneros_PeliculaId",
                table: "PeliculaGeneros",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaCines_PeliculaId",
                table: "PeliculaCines",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaActor_PeliculaId",
                table: "PeliculaActor",
                column: "PeliculaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculaGeneros",
                table: "PeliculaGeneros");

            migrationBuilder.DropIndex(
                name: "IX_PeliculaGeneros_PeliculaId",
                table: "PeliculaGeneros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculaCines",
                table: "PeliculaCines");

            migrationBuilder.DropIndex(
                name: "IX_PeliculaCines_PeliculaId",
                table: "PeliculaCines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PeliculaActor",
                table: "PeliculaActor");

            migrationBuilder.DropIndex(
                name: "IX_PeliculaActor_PeliculaId",
                table: "PeliculaActor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculaGeneros",
                table: "PeliculaGeneros",
                columns: new[] { "PeliculaId", "GeneroId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculaCines",
                table: "PeliculaCines",
                columns: new[] { "PeliculaId", "CineId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PeliculaActor",
                table: "PeliculaActor",
                columns: new[] { "PeliculaId", "ActorId" });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaGeneros_GeneroId",
                table: "PeliculaGeneros",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaActor_ActorId",
                table: "PeliculaActor",
                column: "ActorId");
        }
    }
}
