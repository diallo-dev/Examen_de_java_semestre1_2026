namespace BrasilBurger.Web.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreerAsync(T entity);
        Task<List<T>> ListerTousAsync();
        Task<T?> TrouverParIdAsync(int id);
        Task<List<T>> ListerParEtatAsync(string etat);
    }
}
