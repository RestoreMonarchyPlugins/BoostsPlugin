using System.Xml.Serialization;

namespace RestoreMonarchy.Boosts.Models
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
