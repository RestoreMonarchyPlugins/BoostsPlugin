using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RestoreMonarchy.BoostsPlugin.Services
{
    public class ArmorsDurabilityService : MonoBehaviour
    {
        private BoostsPlugin pluginInstance => BoostsPlugin.Instance;

        void Start()
        {
            DamageTool.damagePlayerRequested += OnDamagePlayerRequested;
        }

        void OnDestroy()
        {
            DamageTool.damagePlayerRequested -= OnDamagePlayerRequested;
        }

        private void OnDamagePlayerRequested(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            if (Provider.modeConfigData.Items.Has_Durability)
                return;

            var limb = parameters.limb;

            if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG)
            {
                DamagePants(parameters.player.clothing);
            } else if (limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM)
            {
                DamageShirt(parameters.player.clothing);
            } else if (limb == ELimb.SPINE)
            {
                DamageVest(parameters.player.clothing);
            } else if (limb == ELimb.SKULL)
            {
                DamageHat(parameters.player.clothing);
            }
        }

        private void DamagePants(PlayerClothing clothing)
        {
            if (clothing.pants != 0)
            {
                clothing.pantsQuality--;
                clothing.sendUpdatePantsQuality();
            }
        }

        private void DamageShirt(PlayerClothing clothing)
        {
            if (clothing.shirt != 0)
            {
                clothing.shirtQuality--;
                clothing.sendUpdateShirtQuality();
            }
        }

        private void DamageVest(PlayerClothing clothing)
        {
            if (clothing.vest != 0)
            {
                clothing.vestQuality--;
                clothing.sendUpdateVestQuality();
            }
        }

        private void DamageHat(PlayerClothing clothing)
        {
            if (clothing.hat != 0)
            {
                clothing.hatQuality--;
                clothing.sendUpdateHatQuality();
            }
        }
    }
}
