namespace ArchrVSZombi_CSharpPort
{
    internal class Monsters:Entities
    {
        public int Reward { get; set; }
        public int Moves { get; set; }
        public Monsters() : base() { }
        public Monsters(string n, string sn, int mhp, int mind, int maxd, int mo, int r) : base("Defenders", n, sn, mhp, mind, maxd)
        {
            Reward = r;
            Moves = mo;
        }
        public void SpawnMon(Field f, List<Monsters> mlist)
        {
            int monindex = new Random().Next(0,mlist.Count());
            int rownum = new Random().Next(0, f.StringField.Count());
            int col = -1;
            Monsters monchosen = mlist[monindex];
            f.StringField[rownum][col][0] = monchosen.ShortName;
            f.StringField[rownum][col][1] = $"{monchosen.MaxHP}/{monchosen.MaxHP}";
        }
    }
}
