using ArchrVSZombi_CSharpPort;
void DispMainMenu()
{
    Console.WriteLine("1. Start new game");
    Console.WriteLine("2. Load saved game");
    Console.WriteLine("3. Game settings");
    Console.WriteLine("4. Quit");
}
void EndTurn(Field f, GameVars g, List<Monsters> mlist, List<Defenders> deflist)
{
    foreach (var row in f.StringField)
    {
        foreach (var col in row)
        {
            if (col[0].Length != 0)
            {
                foreach (Monsters m in mlist)
                {
                    if (m.ShortName == col[0])
                    {
                        m.MonAdv(f, f.StringField.IndexOf(row), row.IndexOf(col), deflist, mlist);
                    }
                }
                foreach (Defenders d in deflist)
                {
                    if (d.ShortName == col[0])
                    {
                        d.DefAttack(f, f.StringField.IndexOf(row), row.IndexOf(col), mlist, g);
                    }
                }
            }
        }
    }
    g.GVariables["gold"]++;
    g.GVariables["threat"] += new Random().Next(1, g.GVariables["danger_level"]);
    if (g.GVariables["turn"] % 12 == 0)
    {
        g.GVariables["danger_level"]++;
        foreach (Monsters m in mlist)
        {
            m.MinDamage++;
            m.MaxDamage++;
            m.Reward++;
            m.MaxHP++;
        }
        ThreatChecker(g, f, mlist);
    }
}
bool KillCount(GameVars g)
{
    if (g.GVariables["monsters_killed"] >= g.GVariables["monster_kill_target"])
    {
        return true;
    }
    return false;
}
string MonCheck(Field f, List < Monsters > mlist)
{
    foreach (var row in f.StringField)
    {
        foreach (Monsters m in mlist)
        {
            return m.Name;
        }
    }
    return null;
}
void ThreatChecker(GameVars g, Field f, List< Monsters > mlist)
{
    if (g.GVariables["threat"] >= g.GVariables["max_threat"])
    {
        mlist[0].SpawnMon(f, mlist);
        g.GVariables["threat"] -= g.GVariables["max_threat"];
    }
}
bool UpgradeUnit(GameVars g, List<Defenders> deflist, int count)
{
    bool done = false;
    int basegold = 8;
    int addgold = basegold + (2 * count);
    Console.WriteLine($"{deflist[0].Name} Minimum Damage: {deflist[0].MinDamage} Maximum Damage: {deflist[0].MaxDamage} Maximum HealthPoints: {deflist[0].MaxHP}");
    while (true)
    {
        Console.Write($"Are you sure you want to upgrade your archer? Requires {addgold} gold. (Y/N) ");
        string sel = Console.ReadLine();
        sel = sel.ToUpper();
        if (sel == "Y")
        {
            if (g.GVariables["gold"] < addgold)
            {
                done = false;
                Console.WriteLine("You do not have enough gold");
                break;
            }
            else
            {
                done = true;
                g.GVariables["gold"] -= addgold;
                deflist[0].MinDamage++;
                deflist[0].MaxDamage++;
                deflist[0].MaxHP++;
                break;
            }
        }
        else if (sel == "N")
        {
            done = false;
            break;
        }
        else
        {
            Console.WriteLine("Please type in Y or N");
            continue;
        }
    }
    return done;
}
Field mainfield = new Field();
GameVars GameVariables = new GameVars();
Defenders archer = new Defenders("Archer", "ARCHR", 5, 1, 4, 5);
Defenders wall = new Defenders("Wall", "WALL", 20, 0, 0, 3);
Defenders mine = new Defenders("Mine", "MINE", 1, 0, 0, 8);
List<Defenders> deflist = new List<Defenders> { archer, wall, mine };
Monsters zombie = new Monsters("Zombie", "ZOMBI", 15, 3, 6, 1, 2);
Monsters werewolf = new Monsters("Werewolf", "WWOLF", 10, 1, 4, 2, 3);
Monsters skeleton = new Monsters("Skeleton", "SKELE", 10, 1, 3, 1, 4);
List<Monsters> monlist = new List<Monsters> { zombie, werewolf, skeleton };