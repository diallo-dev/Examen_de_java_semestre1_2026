using BrasilBurger.Web.Entity;
using BrasilBurger.Web.Repository;

namespace BrasilBurger.Web.Service.Impl
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Client> InscrireAsync(Client client, string motDePasse)
        {
            client.MotDePasse = BCrypt.Net.BCrypt.HashPassword(motDePasse);
            return await _repository.CreerAsync(client);
        }

        public async Task<Client?> ConnecterAsync(string email, string motDePasse)
        {
            var client = await _repository.TrouverParEmailAsync(email);
            
            if (client != null && BCrypt.Net.BCrypt.Verify(motDePasse, client.MotDePasse))
            {
                return client;
            }
            
            return null;
        }

        public async Task<Client?> TrouverParIdAsync(int id)
        {
            return await _repository.TrouverParIdAsync(id);
        }
    }
}
