using GestionSucursalesAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionSucursalesAPI.Infraestructure.Repository
{
    public class SucursalesDbContext : DbContext
    {
        public SucursalesDbContext(DbContextOptions<SucursalesDbContext> options) : base(options) { }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Usuario> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sucursal>().ToTable("Sucursal");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");

            modelBuilder.Entity<Sucursal>().HasKey(e => e.Id);
            modelBuilder.Entity<Usuario>().HasKey(e => e.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
