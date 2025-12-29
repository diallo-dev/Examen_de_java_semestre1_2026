using BrasilBurger.Web.Entity;
using BrasilBurger.Web.Repository;

namespace BrasilBurger.Web.Service.Impl
{
    public class ComplementService : IComplementService
    {
        private readonly IComplementRepository _repository;

        public ComplementService(IComplementRepository repository)
        {
            _repository = repository;
        }

        public async Task<Complement> CreerAsync(Complement complement)
        {
            return await _repository.CreerAsync(complement);
        }

        public async Task<List<Complement>> ListerTousAsync()
        {
            return await _repository.ListerTousAsync();
        }

        public async Task<Complement?> TrouverParIdAsync(int id)
        {
            return await _repository.TrouverParIdAsync(id);
        }

        public async Task<List<Complement>> ListerParEtatAsync(string etat)
        {
            return await _repository.ListerParEtatAsync(etat);
        }
    }
}
