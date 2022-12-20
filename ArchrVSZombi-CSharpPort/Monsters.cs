using System.Data.Common;

namespace ArchrVSZombi_CSharpPort
{
    public class Monsters:Entities
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
            int col = f.StringField[0].Count() - 1;
            Monsters monchosen = mlist[monindex];
            f.StringField[rownum][col][0] = monchosen.ShortName;
            f.StringField[rownum][col][1] = $"{monchosen.MaxHP}/{monchosen.MaxHP}";
        }
        public void MonAdv(Field f, int row, int col, List<Defenders> deflist, List<Monsters> monlist)
        {
            if (ShortName == "ZOMBI" || ShortName == "SKELE")
            {
                if (f.StringField[row][col - 1][0].Length != 0)
                {
                    foreach (Defenders d in deflist)
                    {
                        if (f.StringField[row][col - 1][0] == d.ShortName)
                        {
                            string[] hlist = f.StringField[row][col - 1][1].Split('/');
                            int damage = new Random().Next(MinDamage, MaxDamage);
                            int remhealth = Convert.ToInt32(hlist[1]) - damage;
                            Console.WriteLine($"{Name} in lane {f.RowList[row]} hits {d.Name} for {damage} damage!");
                            if (remhealth <= 0)
                            {
                                f.StringField[row][col - 1][0] = "";
                                f.StringField[row][col - 1][1] = "";
                                return;
                            }
                            f.StringField[row][col - 1][1] = $"{remhealth}/{MaxHP}";
                        }
                        else
                        {
                            Console.WriteLine($"{Name} in lane {f.RowList[row]} is blocked from advancing.");
                            return;
                        }
                    }
                }
            }
            else if (ShortName == "WWOLF")
            {
                if (f.StringField[row][col - 1][0].Length != 0)
                {
                    foreach (Defenders d in deflist)
                    {
                        if (d.ShortName == f.StringField[row][col - 1][0])
                        {
                            string[] hlist = f.StringField[row][col - 1][1].Split('/');
                            int damage = new Random().Next(MinDamage, MaxDamage);
                            int remhealth = Convert.ToInt32(hlist[0]) - damage;
                            Console.WriteLine($"{Name} in lane {f.RowList[row]} hits {d.Name} for {damage} damage!");
                            if (remhealth <= 0)
                            {
                                f.StringField[row][col - 1][0] = "";
                                f.StringField[row][col - 1][1] = "";
                                return;
                            }
                            f.StringField[row][col - 1][1] = $"{remhealth}/{MaxHP}";
                            return;
                        }
                        else
                        {
                            Console.WriteLine($"{Name} in lane {f.RowList[row]} is blocked from advancing.'");
                            return;
                        }
                    }
                }
                else if (f.StringField[row][col - 2][0].Length != 0)
                {
                    f.StringField[row][col - 2][0] = f.StringField[row][col][0];
                    f.StringField[row][col - 2][1] = f.StringField[row][col][1];
                    f.StringField[row][col][0] = "";
                    f.StringField[row][col][1] = "";
                    return;
                }
            }
            int moves = col - Moves;
            if (moves < 0)
            {
                moves = 0;
            }
            f.StringField[row][moves][0] = f.StringField[row][col][0];
            f.StringField[row][moves][1] = f.StringField[row][col][1];
            f.StringField[row][col][0] = "";
            f.StringField[row][col][1] = "";
            Console.WriteLine($"{Name} in lane {f.RowList[row]} advances!");
            return;
        }

    }
}
