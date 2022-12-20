using ArchrVSZombi_CSharpPort;
void DispMainMenu()
{
    Console.WriteLine("1. Start new game");
    Console.WriteLine("2. Load saved game");
    Console.WriteLine("3. Game settings");
    Console.WriteLine("4. Quit");
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
while (true)
{
    mainfield.DrawField();
    archer.BuyDef(mainfield, GameVariables, deflist);
}

// GameVariables.ShowCombMenu();