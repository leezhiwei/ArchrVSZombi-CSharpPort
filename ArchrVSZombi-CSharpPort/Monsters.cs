namespace ArchrVSZombi_CSharpPort
{
    internal class Monsters:Entities
    {
        public int Reward { get; set; }
        public Monsters() : base() { }
        public Monsters(string n, string sn, int mhp, int mind, int maxd, int r) : base("Defenders", n, sn, mhp, mind, maxd)
        {
            Reward = r;
        }
    }
}
