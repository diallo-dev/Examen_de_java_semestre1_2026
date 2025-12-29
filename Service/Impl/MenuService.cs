using BrasilBurger.Web.Entity;
using BrasilBurger.Web.Repository;

namespace BrasilBurger.Web.Service.Impl
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repository;

        public MenuService(IMenuRepository repository)
        {
            _repository = repository;
        }

        public async Task<Menu> CreerAsync(Menu menu)
        {
            return await _repository.CreerAsync(menu);
        }

        public async Task<List<Menu>> ListerTousAsync()
        {
            return await _repository.ListerTousAsync();
        }

        public async Task<Menu?> TrouverParIdAsync(int id)
        {
            return await _repository.TrouverParIdAsync(id);
        }

        public async Task<List<Menu>> ListerParEtatAsync(string etat)
        {
            return await _repository.ListerParEtatAsync(etat);
        }
    }
}
