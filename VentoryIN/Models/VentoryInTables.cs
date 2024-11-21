using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VentoryIN.Models
{
    public class VentoryInTables
    {
    }

    public class Categorias
    {
        [Key]
        public int categoriaID { get; set; }
        public string? nombreCategoria { get; set; }
        public int? parentCategoriaID { get; set; }
    }

    public class Productos
    {
        [Key]
        public int productoID { get; set; }
        public string? nombreProducto { get; set; }
        public string? descripcion { get; set; }
        public int categoriaID { get; set; }
    }

    public class Proveedores
    {
        [Key]
        public int proveedorID { get; set; }
        public string? nombreProveedor{ get; set; }
        public string? contacto { get; set; }
    }

    public class ProductoProveedores
    {
        [Key]
        public int productoID { get; set; }
        public int proveedorID { get; set; }
    }

    public class TipoUsuario
    {
        [Key]
        public int tipoUsuarioID { get; set; }
        public string? nombreTipoUsuario { get; set; }
    }

    public class Usuarios
    {
        [Key]
        public int usuarioID { get; set; }
        public string? nombreUsuario { get; set; }
        public string? email { get; set; }
        public int tipoUsuarioID { get; set; }

        [JsonIgnore]
        public string? contrasena { get; set; }
        public byte[]? Foto { get; set; } // Añadido para la foto

        [NotMapped]
        public IFormFile? PhotoUpload { get; set; }

    }

    public class Inventario
    {
        [Key]
        public int productoID { get; set; }
        public int stockActual { get; set; }

    }

    public class EntradaVentas
    {
        [Key]
        public int entradaID { get; set; }
        public int productoID { get; set; }
        public int cantidad { get; set; }
        public DateTime fechaEntrada { get; set; } = DateTime.Now;
        public decimal precioCompra { get; set; }
        public int proveedorID { get; set; }

    }

    public class EntradaPorMesViewModel
    {
        public string Mes { get; set; }
        public decimal TotalCompras { get; set; }
        public int TotalProductos { get; set; }
    }

 


    public class SalidaVentas
    {
        [Key]
        public int salidaID { get; set; }
        public int productoID { get; set; }
        public int cantidad { get; set; }
        public DateTime fechaSalida { get; set; } = DateTime.Now;
        public decimal precioVenta { get; set; }
        public int usuarioID { get; set; }

    }
    public class SalidaPorMesViewModel
    {
        public string Mes { get; set; } // Mes/Año en formato "MM/yyyy"
        public decimal TotalVentas { get; set; } // Total de ventas en ese mes
        public int TotalProductos { get; set; } // Total de productos vendidos en ese mes
    }

}
