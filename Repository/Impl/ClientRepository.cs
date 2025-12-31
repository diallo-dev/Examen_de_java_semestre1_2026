using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Entity;

namespace BrasilBurger.Web.Repository.Impl
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Client> CreerAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            Console.WriteLine($" Client créé avec l'ID : {client.Id}");
            return client;
        }

        public async Task<Client?> TrouverParEmailAsync(string email)
        {
            return await _context.Clients
                .Where(c => c.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<Client?> TrouverParIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }
    }
}
