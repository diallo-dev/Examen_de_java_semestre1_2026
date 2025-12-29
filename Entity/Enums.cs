namespace BrasilBurger.Web.Entity
{
    public enum ModePaiement
    {
        Wave,
        Orange_Money
    }

    public enum LieuConsommation
    {
        SurPlace,
        Livraison,
        Emporter
    }

    public enum EtatCommande
    {
        Encours_Preparation,
        Terminer,
        Livrer,
        EnCours_Livraison,
        NonTraiter
    }
}
