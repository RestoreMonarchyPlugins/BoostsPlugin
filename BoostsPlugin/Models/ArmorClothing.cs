using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestoreMonarchy.BoostsPlugin.Models
{
    public class ArmorClothing
    {
        [XmlAttribute]
        public ushort ItemId { get; set; }
        [XmlAttribute]
        public float Armor { get; set; } 
        [XmlAttribute]
        public float ExplosionArmor { get; set; }   
    }
}
