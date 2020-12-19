using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostsPlugin.Models
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
