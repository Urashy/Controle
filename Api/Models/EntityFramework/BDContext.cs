using Microsoft.EntityFrameworkCore;

namespace Api.Models.EntityFramework
{
    public partial class BDContext : DbContext
    {
    //public DbSet<Animal> Animaux { get; set; }

    public BDContext()
        {
        }

    public BDContext(DbContextOptions<BDContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        //modelBuilder.Entity<Animal>(entity =>
        //{
        //    entity.ToTable("T_E_ANIMAL_ANI");

        //    entity.HasKey(e => e.IdAnimal);

        //    entity.Property(e => e.IdAnimal)
        //          .ValueGeneratedOnAdd()
        //          .UseIdentityColumn();
        //});

        //modelBuilder.HasDefaultSchema("public");
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
}
