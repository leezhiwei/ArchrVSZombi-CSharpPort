namespace ArchrVSZombi_CSharpPort
{
    public abstract class Entities
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int MaxHP { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public Entities() { }
        public Entities(string t,string n, string sn, int mhp, int mind, int maxd)
        {
            Type = t;
            Name = n;
            ShortName = sn;
            MaxHP = mhp;
            MinDamage = mind;
            MaxDamage = maxd;
        }
    }
}
