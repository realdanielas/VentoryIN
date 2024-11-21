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
            // Recupera todas las categorías desde la base de datos
            var categorias = _ventoryInDbContext.categorias.ToList();

            // Pásalas a ViewData para ser utilizadas en la vista
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
            // Busca la categoría en la base de datos
            var categoria = _ventoryInDbContext.categorias.FirstOrDefault(c => c.categoriaID == id);

            if (categoria != null)
            {
                // Elimina la categoría si existe
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

        public IActionResult Entradas()
        {
            return View();
        }
        public IActionResult Salidas()
        {
            return View();
        }
        public IActionResult Proveedores()
        {
            return View();
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
            _ventoryInDbContext.Add(nuevaCategoria); // Agrega la nueva categoría
            _ventoryInDbContext.SaveChanges(); // Guarda los cambios en la base de datos
            return RedirectToAction("Productos");
        }

       
        public IActionResult CrearProducto()
        {
            return View();
        }
        public IActionResult CrearProductos(Productos nuevoProducto)
        {
            //Validacion de campo lleno
            if (string.IsNullOrEmpty(nuevoProducto.nombreProducto) ||
                string.IsNullOrEmpty(nuevoProducto.descripcion))
            /*int.Equals(nuevoProducto.categoriaID)*/
            {
                TempData["Error"] = "Todos los campos son obligatorios.";
                return RedirectToAction("CrearProducto");
            }
            _ventoryInDbContext.Add(nuevoProducto);
            _ventoryInDbContext.SaveChanges();

            return RedirectToAction("ProductoStock");
        }
        public IActionResult Success()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
