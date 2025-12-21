using System.Text.Json;
using BrasilBurger.Web.ViewModels;

namespace BrasilBurger.Web.Helpers
{
    public static class PanierHelper
    {
        private const string PANIER_KEY = "Panier";

        public static List<ItemPanier> GetPanier(ISession session)
        {
            var json = session.GetString(PANIER_KEY);
            if (string.IsNullOrEmpty(json))
                return new List<ItemPanier>();
            
            return JsonSerializer.Deserialize<List<ItemPanier>>(json) ?? new List<ItemPanier>();
        }

        public static void SavePanier(ISession session, List<ItemPanier> panier)
        {
            var json = JsonSerializer.Serialize(panier);
            session.SetString(PANIER_KEY, json);
        }

        public static void AjouterItem(ISession session, ItemPanier item)
        {
            var panier = GetPanier(session);
            
            var existant = panier.FirstOrDefault(i => i.Id == item.Id && i.Type == item.Type);
            if (existant != null)
            {
                existant.Quantite += item.Quantite;
            }
            else
            {
                panier.Add(item);
            }
            
            SavePanier(session, panier);
        }

        public static void RetirerItem(ISession session, int id, string type)
        {
            var panier = GetPanier(session);
            panier.RemoveAll(i => i.Id == id && i.Type == type);
            SavePanier(session, panier);
        }

        public static void ViderPanier(ISession session)
        {
            session.Remove(PANIER_KEY);
        }
    }
}
