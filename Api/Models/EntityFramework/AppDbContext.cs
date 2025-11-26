using Microsoft.EntityFrameworkCore;

namespace Api.Models.EntityFramework
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Animal> Animaux { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Server=localhost; port=5432; Database=R508_DS; uid=postgres; password=postgres;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<Animal>(entity =>
            {
                entity.HasKey(animal => animal.Id).HasName("pk_ani");

                entity.Property(animal => animal.Name)
                    .HasDefaultValue(1.0);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
