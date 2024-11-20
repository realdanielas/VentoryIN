
using VentoryIN.Models;

namespace Sistema_de_tickets.Serv
{
    public interface IUserService
    {
        Task<Usuarios> GetCurrentUserAsync();
    }
}
