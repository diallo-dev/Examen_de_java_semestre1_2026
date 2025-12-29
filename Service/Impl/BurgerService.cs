using BrasilBurger.Web.Entity;
using BrasilBurger.Web.Repository;

namespace BrasilBurger.Web.Service.Impl
{
    public class BurgerService : IBurgerService
    {
        private readonly IBurgerRepository _repository;

        public BurgerService(IBurgerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Burger> CreerAsync(Burger burger)
        {
            return await _repository.CreerAsync(burger);
        }

        public async Task<List<Burger>> ListerTousAsync()
        {
            return await _repository.ListerTousAsync();
        }

        public async Task<Burger?> TrouverParIdAsync(int id)
        {
            return await _repository.TrouverParIdAsync(id);
        }

        public async Task<List<Burger>> ListerParEtatAsync(string etat)
        {
            return await _repository.ListerParEtatAsync(etat);
        }
    }
}
