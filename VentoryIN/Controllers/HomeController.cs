using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_de_tickets.Serv;
using System.Diagnostics;
using VentoryIN.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;

namespace VentoryIN.Controllers
{
    public class HomeController : Controller
    {
        private readonly VentoryInDbContext _ventoryInDbContext;
        private readonly IConfiguration _configuration;
        //Descargar archivo
        private readonly IWebHostEnvironment _webHostEnvironment;
        //Subir fotos:
        private readonly IUserService _userService;


        public HomeController(VentoryInDbContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IUserService userService)
        {
            _ventoryInDbContext = context;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
        }

        public IActionResult Index()
        { //Agregar javascript de grafico 
            return View();
        }

        public IActionResult Productos()
        {
            // Recupera todas las categor�as desde la base de datos
            var categorias = _ventoryInDbContext.categorias.ToList();

            // P�salas a ViewData para ser utilizadas en la vista
            ViewData["TodasLasCategorias"] = categorias;

            return View();
        }

        public IActionResult ProductoStock()
        {
            var productos = _ventoryInDbContext.productos.ToList();
            ViewData["TodosLosProductos"] = productos;

            return View();
        }

        [HttpPost]
        public IActionResult EliminarCategoria(int id)
        {
            // Busca la categor�a en la base de datos
            var categoria = _ventoryInDbContext.categorias.FirstOrDefault(c => c.categoriaID == id);

            if (categoria != null)
            {
                // Elimina la categor�a si existe
                _ventoryInDbContext.categorias.Remove(categoria);
                _ventoryInDbContext.SaveChanges(); // Guarda los cambios en la base de datos
            }

            // Redirige nuevamente a la lista de productos
            return RedirectToAction("Productos");
        }
        [HttpPost]
        public IActionResult EliminarProducto(int id)
        {
            var producto = _ventoryInDbContext.productos.FirstOrDefault(p => p.productoID == id);

            if (producto != null)
            {
                _ventoryInDbContext.productos.Remove(producto);
                _ventoryInDbContext.SaveChanges();
            }

            return RedirectToAction("ProductoStock");
        }

        public IActionResult DetallesProducto(int id)
        {
            var producto = _ventoryInDbContext.productos.FirstOrDefault(p => p.productoID == id);

            if (producto == null)
            {
                return NotFound("Producto no encontrado.");
            }

            return PartialView("_DetallesProducto", producto);
        }


        public IActionResult Entradas()
        {
            return View();
        }

        public IActionResult EntradasPorMes()
        {
            // Realizar la agrupaci�n en la base de datos sin aplicar el formateo de fechas
            var entradasPorMes = _ventoryInDbContext.entradaVentas
                .GroupBy(e => new { e.fechaEntrada.Year, e.fechaEntrada.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalCompras = g.Sum(e => e.precioCompra * e.cantidad),  // Total de compras
                    TotalProductos = g.Sum(e => e.cantidad)  // Total de productos
                })
                .OrderBy(e => e.Year)  // Ordenar por a�o primero
                .ThenBy(e => e.Month)  // Luego ordenar por mes
                .ToList();  // Traer los datos a memoria

            // Crear el modelo final con los datos formateados
            var result = entradasPorMes.Select(e => new EntradaPorMesViewModel
            {
                // Ahora formateamos la fecha como Mes/A�o en memoria
                Mes = $"{e.Month}/{e.Year}",
                TotalCompras = e.TotalCompras,
                TotalProductos = e.TotalProductos
            }).ToList();

            // Pasar el resultado a la vista
            return View(result);
        }

        public IActionResult Salidas()
        {
            return View();
        }
        //Nuevo


        public IActionResult SalidasPorMes()
        {
            // Realizar la agrupaci�n de las salidas por mes/a�o sin aplicar el formateo de fechas
            var salidasPorMes = _ventoryInDbContext.salidaVentas
                .GroupBy(s => new { s.fechaSalida.Year, s.fechaSalida.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalVentas = g.Sum(s => s.precioVenta * s.cantidad),  // Total de ventas
                    TotalProductos = g.Sum(s => s.cantidad)  // Total de productos vendidos
                })
                .OrderBy(s => s.Year)  // Ordenar por a�o primero
                .ThenBy(s => s.Month)  // Luego ordenar por mes
                .ToList();  // Traer los datos a memoria

            // Crear el modelo final con los datos formateados
            var result = salidasPorMes.Select(s => new SalidaPorMesViewModel
            {
                // Formateamos el mes como "MM/yyyy"
                Mes = $"{s.Month}/{s.Year}",
                TotalVentas = s.TotalVentas,
                TotalProductos = s.TotalProductos
            }).ToList();

            // Pasar el resultado a la vista
            return View(result);
        }




        public IActionResult Proveedores()
        {
            var proveedores = _ventoryInDbContext.proveedores.ToList();
            ViewData["Proveedores"] = proveedores;
            return View();
        }
        public IActionResult CrearProveedores()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Crears(Proveedores nuevoProveedor)
        {
            if (string.IsNullOrEmpty(nuevoProveedor.nombreProveedor) || string.IsNullOrEmpty(nuevoProveedor.contacto))
            {
                TempData["Error"] = "Todos los campos son obligatorios.";
                return View();
            }

            _ventoryInDbContext.proveedores.Add(nuevoProveedor);
            _ventoryInDbContext.SaveChanges();

            return RedirectToAction("Proveedores");
        }

        public IActionResult ClasificacionProductos()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();

        }
        public IActionResult CrearCategoriaProducto()
        {
            return View();
        }
        
        public IActionResult CrearCategoriaProductos(Categorias nuevaCategoria)
        {
            //Validacion de campo lleno
            if (string.IsNullOrEmpty(nuevaCategoria.nombreCategoria))
            {
                TempData["Error"] = "Este campo es obligatorio";
                return RedirectToAction("CrearCategoriaProducto");
            }
            _ventoryInDbContext.Add(nuevaCategoria); // Agrega la nueva categor�a
            _ventoryInDbContext.SaveChanges(); // Guarda los cambios en la base de datos
            return RedirectToAction("Productos");
        }

       
        public IActionResult CrearProducto()
        {
            return View();
        }
        public IActionResult CrearProductos(Productos nuevoProducto)
        {
            // Validaci�n de campos requeridos
            if (string.IsNullOrEmpty(nuevoProducto.nombreProducto) ||
                string.IsNullOrEmpty(nuevoProducto.descripcion))
            {
                TempData["Error"] = "Todos los campos son obligatorios.";
                return RedirectToAction("CrearProducto");
            }

            // Guardar en la base de datos
            _ventoryInDbContext.Add(nuevoProducto);
            _ventoryInDbContext.SaveChanges();

            TempData["Success"] = "Producto creado exitosamente.";
            return RedirectToAction("ProductoStock");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
