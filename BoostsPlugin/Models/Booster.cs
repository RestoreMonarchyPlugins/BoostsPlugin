namespace RestoreMonarchy.Boosts.Models
{
    public class Booster
    {
        public Booster(BoostItem boostItem)
        {
            BoostItem = boostItem;
        }

        public BoostItem BoostItem { get; set; }
    }
}
