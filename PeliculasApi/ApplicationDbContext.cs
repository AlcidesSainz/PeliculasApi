using Microsoft.EntityFrameworkCore;
using PeliculasApi.Entidades;

namespace PeliculasApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configurando llaves para las relaciones de entidades
            modelBuilder.Entity<PeliculaActor>().HasKey(e => new { e.ActorId, e.PeliculaId });
            modelBuilder.Entity<PeliculaCine>().HasKey(e => new {  e.CineId, e.PeliculaId });
            modelBuilder.Entity<PeliculaGenero>().HasKey(e => new {  e.GeneroId, e.PeliculaId, });
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cines> Cines { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculaActor> PeliculaActor { get; set; }
        public DbSet<PeliculaCine> PeliculaCine { get; set; }
        public DbSet<PeliculaGenero> PeliculaGenero { get; set; }
    }
}
