using Microsoft.EntityFrameworkCore;

namespace VentoryIN.Models
{
    public class VentoryInDbContext : DbContext
    {
        public VentoryInDbContext(DbContextOptions options) : base(options) { }

        //Aquí poner los contextos de las tablas a utilizar:
       
        public DbSet<Categorias> categorias { get; set; }

        public DbSet<Productos> productos { get; set; }
        public DbSet<Proveedores> proveedores { get; set; }
        public DbSet<ProductoProveedores> productoProveedores { get; set; }
        public DbSet<TipoUsuario> tipoUsuario { get; set; }
        public DbSet<Usuarios> usuarios { get; set; }

        public DbSet<Inventario> inventario { get; set; }

        public DbSet<EntradaVentas> entradaVentas { get; set; }

        public DbSet<SalidaVentas> salidaVentas { get; set; }

    }
}
