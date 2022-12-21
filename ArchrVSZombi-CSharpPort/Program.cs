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
        Console.WriteLine("The evil grows stronger!");
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
            if (row[0][0] == m.ShortName)
            {
                return m.Name;
            }
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
void SaveGame(Field f, List<Defenders> deflist, List<Monsters> monlist, GameVars g)
{
    XmlSerialization.WriteToXmlFile("./field.xml", f);
    XmlSerialization.WriteToXmlFile("./deflist.xml", deflist);
    XmlSerialization.WriteToXmlFile("./monlist.xml", monlist);
    XmlSerialization.WriteToXmlFile("./gamevars.xml", g);
    Console.WriteLine("Game Saved.");
}
void LoadGame(Field f, GameVars g, List<Monsters> mlist, List<Defenders> deflist)
{
    f = XmlSerialization.ReadFromXmlFile<Field>("./field.xml");
    deflist = XmlSerialization.ReadFromXmlFile<List<Defenders>>("./deflist.xml");
    mlist = XmlSerialization.ReadFromXmlFile<List<Monsters>>("./monlist.xml");
    g = XmlSerialization.ReadFromXmlFile<GameVars>("./gamevars.xml");
    return;
}
bool FileCheck()
{
    try
    {
        Field f = XmlSerialization.ReadFromXmlFile<Field>("./field.xml");
        List<Defenders> deflist = XmlSerialization.ReadFromXmlFile<List<Defenders>>("./deflist.xml");
        List<Monsters> mlist = XmlSerialization.ReadFromXmlFile<List<Monsters>>("./monlist.xml");
        GameVars g = XmlSerialization.ReadFromXmlFile<GameVars>("./gamevars.xml");
    }
    catch
    {
        return false;
    }
    return true;
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
bool exitflag = false;
bool continueflag = false;
int sel = 0;
int count = 0;
Console.WriteLine("Desperate Defenders");
Console.WriteLine("-------------------");
Console.WriteLine("Defend the city from undead monsters!");
Console.WriteLine();
while (true)
{
    if (exitflag)
    {
        break;
    }
    if (!continueflag)
    {
        DispMainMenu();
        try
        {
            Console.Write("Your choice? ");
            sel = Convert.ToInt32(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Invalid input, enter from selection above.");
            continue;
        }
    }
    if (sel == 1)
    {
        sel = 0;
        while (true)
        {
            if (exitflag)
            {
                break;
            }
            if (mainfield.FieldCheck(monlist))
            {
                zombie.SpawnMon(mainfield, monlist);
            }
            mainfield.DrawField();
            GameVariables.ShowCombMenu();
            while (true)
            {
                try
                {
                    Console.Write("Your choice? ");
                    sel = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid choixe, choose a number instead.");
                    continue;
                }
                if (sel == 1)
                {
                    archer.BuyDef(mainfield, GameVariables, deflist);
                    EndTurn(mainfield, GameVariables, monlist, deflist);
                    break;
                }
                else if (sel == 2)
                {
                    EndTurn(mainfield,GameVariables,monlist, deflist);
                    break;
                }
                else if (sel == 3)
                {
                    if (FileCheck())
                    {
                        Console.Write("Are you sure you want to override your save file? (Y/N) ");
                        string selection = Console.ReadLine();
                        selection = selection.ToUpper();
                        if (selection == "Y")
                        {
                            SaveGame(mainfield, deflist, monlist, GameVariables);
                            exitflag = true;
                            break;
                        }
                        else if (selection == "N")
                        {
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        SaveGame(mainfield, deflist, monlist, GameVariables);
                        exitflag= true;
                        break;
                    }
                }
                else if (sel == 4)
                {
                    exitflag= true;
                    break;
                }
                else if (sel == 5)
                {
                    if (UpgradeUnit(GameVariables, deflist, count))
                    {
                        count++;
                        EndTurn(mainfield,GameVariables,monlist, deflist);
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option, please select from option above");
                }
            }
            if (KillCount(GameVariables))
            {
                Console.WriteLine("You have protected the city! You win!");
                exitflag= true;
            }
            
            else if (MonCheck(mainfield, monlist) is not null)
            {
                Console.WriteLine($"A {MonCheck(mainfield, monlist)} has reached the city! All is lost!");
                Console.WriteLine("You have lost the game. :(");
                exitflag= true;
            }
        }
    }
    else if (sel == 2)
    {
        if (!FileCheck())
        {
            Console.WriteLine("Savefile not present, save your file before loading.");
            continue;
        }
        else
        {
            LoadGame(mainfield, GameVariables, monlist, deflist);
            continueflag= true;
            sel = 1;
            continue;
        }
    }
    else if (sel == 3)
    {
        sel = 0;
        while (true)
        {
            Console.WriteLine("Type the option number below to change the following options.");
            Console.WriteLine("1. Board Size, eg number of lanes and length");
            Console.WriteLine("2. Kill Target");
            Console.WriteLine("3. Threat meter length");
            Console.WriteLine("4. Enter a new game");
            Console.WriteLine("5. Return to main menu");
            try
            {
                Console.Write("What is your choice? ");
                sel = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("You have inputted an invalid option, please choose form options above.");
                continue;
            }
            if (sel == 1)
            {
                bool loop = true;
                int baserow = mainfield.StringField.Count();
                int basecol = mainfield.StringField[0].Count();
                while (loop)
                {
                    Console.WriteLine($"The current board size is {baserow} by {basecol}");
                    Console.Write("Do you wish to change it? (Y/N) ");
                    string selection = Console.ReadLine();
                    selection = selection.ToUpper();
                    if (selection == "Y")
                    {
                        while (true)
                        {
                            try
                            {
                                Console.Write("How many rows do you want? ");
                                int newrow = Convert.ToInt32(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("You have entered an invalid option");
                                continue;
                            }
                            try
                            {
                                Console.Write("How many columns do you want? ");
                                int newcol = Convert.ToInt32(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("You have entered an invalid option");
                                continue;
                            }
                            mainfield = new Field(baserow, basecol);
                            loop = false;
                            break;
                        }
                    }
                    else if (selection == "N")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option.");
                        continue;
                    }
                }
            }
            else if (sel == 2)
            {
                bool loop = true;
                while (loop)
                {
                    Console.WriteLine($"The current kill target is {GameVariables.GVariables["monster_kill_target"]}");
                    Console.Write("Do you wish to change it? (Y/N) ");
                    string selection = Console.ReadLine();
                    selection = selection.ToUpper();
                    int option;
                    if (selection == "Y")
                    {
                        while (true)
                        {
                            try
                            {
                                Console.Write("What do you want to change it to? ");
                                option = Convert.ToInt32(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("You have entered an invalid option");
                                continue;
                            }
                            GameVariables.GVariables["monster_kill_target"] = option;
                            loop = false;
                            break;
                        }
                    }
                    else if (selection == "N")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option");
                        continue;
                    }
                }
            }
            else if (sel == 3)
            {
                bool loop = true;
                while (loop)
                {
                    Console.WriteLine($"The current max threat is {GameVariables.GVariables["max_threat"]}");
                    Console.Write("Do you wish to change it? (Y/N) ");
                    string selection = Console.ReadLine();
                    selection = selection.ToUpper();
                    int option;
                    if (selection == "Y")
                    {
                        while (true)
                        {
                            try
                            {
                                Console.Write("What do you want to change it to? ");
                                option = Convert.ToInt32(Console.ReadLine());
                            }
                            catch
                            {
                                Console.WriteLine("You have entered an invalid option");
                                continue;
                            }
                            GameVariables.GVariables["max_threat"] = option;
                            loop = false;
                            break;
                        }
                    }
                    else if (selection == "N")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option");
                        continue;
                    }
                }
            }
            else if (sel == 4)
            {
                sel = 1;
                continueflag = true;
                break;
            }
            else if (sel == 5)
            {
                sel = 0;
                break;
            }
            else
            {
                Console.WriteLine("You have inputted an invalid option, please choose from option above.");
            }
        }
    }
    else if (sel == 4)
    {
        break;
    }
    else
    {
        Console.WriteLine("Invalid option, select from numbers above.");
    }
}
Console.WriteLine("See you next time!");