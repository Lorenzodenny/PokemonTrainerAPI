using Microsoft.EntityFrameworkCore;
using PokemonTrainerAPI.Model;

namespace PokemonTrainerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura la relazione uno-a-molti tra Trainer e Pokemon
            modelBuilder.Entity<Trainer>()
                .HasMany(t => t.Pokemons)
                .WithOne(p => p.Trainer)
                .HasForeignKey(p => p.TrainerId);
        }
    }
}
