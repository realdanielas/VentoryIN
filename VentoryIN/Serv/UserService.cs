using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VentoryIN.Models;
using System.Text.Json;

namespace Sistema_de_tickets.Serv
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly VentoryInDbContext _context;

        public UserService(IHttpContextAccessor httpContextAccessor, VentoryInDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<Usuarios> GetCurrentUserAsync()
        {
            var userJson = _httpContextAccessor.HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                return null;
            }

            var usuarioSesion = JsonSerializer.Deserialize<Usuarios>(userJson);
            return await _context.usuarios.FindAsync(usuarioSesion.usuarioID);
        }
    }
}