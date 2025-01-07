using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Entidades;

namespace PeliculasApi
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var generos = new[]
{
                new Genero{ Id=1,Nombre  = "Acción" },
                new Genero{ Id=2,Nombre  = "Aventura" },
                new Genero{ Id=3,Nombre  = "Ciencia Ficción" },
                new Genero{ Id=4,Nombre  = "Comedia" },
                new Genero{ Id=5,Nombre  = "Drama" },
                new Genero{ Id=6,Nombre  = "Terror" },
                new Genero{ Id=7,Nombre  = "Thriller" },
                new Genero{ Id=8,Nombre  = "Fantasía" },
                new Genero{ Id=9,Nombre  = "Animación" },
                new Genero{ Id=10,Nombre  = "Romance" },
                new Genero{ Id=11,Nombre  = "Musical" },
                new Genero{ Id=12,Nombre  = "Documental" },
                new Genero{ Id=13,Nombre  = "Crimen" },
                new Genero{ Id=14,Nombre  = "Bélico" },
                new Genero{ Id=15,Nombre  = "Western" },
                new Genero{ Id=16,Nombre  = "Biografía" },
                new Genero{ Id=17,Nombre  = "Deportes" },
                new Genero{ Id=18,Nombre  = "Superhéroes" },
                new Genero{ Id=19,Nombre  = "Policial" },
                new Genero{ Id=20,Nombre  = "Infantil" },


            };
            modelBuilder.Entity<Genero>().HasData(generos);


            base.OnModelCreating(modelBuilder);

            //Configurando llaves para las relaciones de entidades
            modelBuilder.Entity<PeliculaActor>().HasKey(e => new { e.ActorId, e.PeliculaId });
            modelBuilder.Entity<PeliculaCine>().HasKey(e => new {  e.CineId, e.PeliculaId });
            modelBuilder.Entity<PeliculaGenero>().HasKey(e => new {  e.GeneroId, e.PeliculaId, });
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculaActor> PeliculaActor { get; set; }
        public DbSet<PeliculaCine> PeliculaCine { get; set; }
        public DbSet<PeliculaGenero> PeliculaGenero { get; set; }
    }
}
