using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.BoostsPlugin.Models
{
    public class BoostItem
    {
        public ushort ItemId { get; set; }
        public float SpeedBoost { get; set; }
        public float JumpBoost { get; set; }
        public bool RequireEquip { get; set; }
    }
}
