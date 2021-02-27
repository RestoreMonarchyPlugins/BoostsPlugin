using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RestoreMonarchy.BoostsPlugin.Models
{
    public interface IArmorsUI
    {
        void RefreshHatArmor();
        void RefreshPantsArmor();
        void RefreshShirtArmor();
        void RefreshVestArmor();
    }
}
