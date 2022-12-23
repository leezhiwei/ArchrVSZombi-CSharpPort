using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ArchrVSZombi_CSharpPort
{
    public class Defenders : Entities
    {
        public int Price { get; set; }
        public Defenders() : base() { }
        public Defenders(string n, string sn, int mhp, int mind, int maxd, int p) : base("Defenders", n, sn, mhp, mind, maxd)
        {
            Price = p;
        }
        public bool PlaceDef(Field f, string pos)
        {
            List<int> collist = new List<int> { 1, 2, 3 };
            char row;
            char col;
            int rowindex;
            int colindex;
            if (pos.Length != 2)
            {
                return false;
            }
            try
            {
                pos = pos.ToUpper();
                row = pos[0];
                col = pos[1];
            }
            catch
            {
                return false;
            }
            if (f.RowList.Contains(row))
            {
                rowindex = f.RowList.IndexOf(row); 
                if (collist.Contains(Convert.ToInt32(char.GetNumericValue(col))))
                {
                    colindex = collist.IndexOf(Convert.ToInt32(char.GetNumericValue((col))));
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            if (f.StringField[rowindex][colindex][0].Length == 0)
            {
                f.StringField[rowindex][colindex][0] = ShortName;
                f.StringField[rowindex][colindex][1] = $"{MaxHP}/{MaxHP}";
            }
            else
            {
                return false;
            }
            return true;
        }
        public void BuyDef(Field f, GameVars g, List<Defenders> dlist)
        {
            Console.WriteLine("What do you wish to buy?");
            foreach (Defenders d in dlist)
            {
                Console.WriteLine($"{dlist.IndexOf(d) + 1}. {d.Name} {d.Price} gold");
            }
            Console.WriteLine($"{dlist.Count() + 1}. Don't buy");
            while (true)
            {
                int choice;
                try
                {
                    Console.Write("Your choice? ");
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Please select from the choice above.");
                    continue;
                }
                if (choice != (dlist.Count() + 1))
                {
                    int index = choice - 1;
                    Defenders defchosen = dlist[index];
                    if (g.GVariables["gold"] < defchosen.Price)
                    {
                        Console.WriteLine("You do not have enough gold.");
                    }
                    else
                    {
                        g.GVariables["gold"] -= defchosen.Price;
                        while (true)
                        {
                            Console.Write("Where do you want to place your defender? ");
                            string pos = Console.ReadLine();
                            if (defchosen.PlaceDef(f, pos))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Positioning unsuccessful.");
                                continue;
                            }
                        }
                    }
                }
                else if (choice == dlist.Count() + 1)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please select from the choice above");
                }
                return;
            }
        }
        public void DefAttack(Field f, int row, int col, List<Monsters> monlist, GameVars g)
        {
            if (ShortName == "ARCHR")
            {
                foreach (var line in f.StringField[row])
                {
                    foreach (Monsters monster in monlist)
                    {
                        if (line[0] == monster.ShortName)
                        {
                            string[] healthlist = line[1].Split('/');
                            int damage = new Random().Next(MinDamage,MaxDamage);
                            if (monster.ShortName == "SKELE")
                            {
                                double mondamage = damage;
                                damage = Convert.ToInt32(Math.Round(mondamage / 2));
                            }
                            int remhealth = Convert.ToInt32(healthlist[0]) - damage;
                            Console.WriteLine($"{Name} in lane {f.RowList[row]} shoots {monster.Name} for {damage} damage!");
                            if (remhealth <= 0)
                            {
                                Console.WriteLine($"{monster.Name} dies!");
                                Console.WriteLine($"You gained {monster.Reward} gold as a reward.");
                                g.GVariables["gold"] += monster.Reward;
                                g.GVariables["monsters_killed"]++;
                                line[0] = "";
                                line[1] = "";
                                g.GVariables["threat"] += monster.Reward;
                                return;
                            }
                            line[1] = $"{remhealth}/{healthlist[1]}";
                        }
                    }
                }
            }
            else if (ShortName == "MINE")
            {
                if (f.StringField[row][col + 1][0].Length != 0)
                {
                    int rownum = 0;
                    int colnum = 0;
                    foreach (var line in f.StringField)
                    {
                        foreach (var column in line)
                        {
                            if (column[0] == "MINE")
                            {
                                rownum = f.StringField.IndexOf(line);
                                colnum = line.IndexOf(column);
                            }
                        }
                    }
                    List<int> indexlist = new List<int> { -1, 0, 1 };
                    try
                    {
                        for (int index = 0; index < 3; index++)
                        {
                            int rowindex = rownum + indexlist[index];
                            for (int index2 = 0; index2 < 3; index2++)
                            {
                                int columnindex = colnum + indexlist[index2];
                                foreach (Monsters mon in monlist)
                                {
                                    if (f.StringField[rowindex][columnindex][0] == mon.ShortName)
                                    {
                                        string[] healthlist = f.StringField[rowindex][columnindex][1].Split("/");
                                        int damage = 9;
                                        int remhealth = Convert.ToInt32(healthlist[0]) - damage;
                                        Console.WriteLine($"{Name} in lane {f.RowList[rowindex]} explodes and damages {mon.ShortName[0]} for {damage} damage!");
                                        if (remhealth <= 0)
                                        {
                                            Console.WriteLine($"{mon.Name} dies!");
                                            Console.WriteLine($"You gained {mon.Reward} gold as a reward.");
                                            g.GVariables["gold"] += mon.Reward;
                                            g.GVariables["monsters_killed"]++;
                                            f.StringField[rowindex][columnindex][0] = "";
                                            f.StringField[rowindex][columnindex][1] = "";
                                            f.StringField[rownum][colnum][0] = "";
                                            f.StringField[rownum][colnum][1] = "";
                                            g.GVariables["threat"] += mon.Reward;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        return;
                    }
                }
            }
            return;
        }
    }
}
