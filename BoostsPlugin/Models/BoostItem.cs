namespace RestoreMonarchy.Boosts.Models
{
    public class BoostItem
    {
        public ushort ItemId { get; set; }
        public float SpeedBoost { get; set; }
        public float JumpBoost { get; set; }
        public bool RequireEquip { get; set; }
    }
}
