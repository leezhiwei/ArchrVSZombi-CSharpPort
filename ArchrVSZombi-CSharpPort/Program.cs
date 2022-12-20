using ArchrVSZombi_CSharpPort;
void DispMainMenu()
{
    Console.WriteLine("1. Start new game");
    Console.WriteLine("2. Load saved game");
    Console.WriteLine("3. Game settings");
    Console.WriteLine("4. Quit");
}
DispMainMenu();
Field mainfield = new Field();
GameVars GameVariables = new GameVars();
Defenders archer = new Defenders("Archer", "ARCHR", 5, 1, 4, 5);
Defenders wall = new Defenders("Wall", "WALL", 20, 0, 0, 3);
Defenders mine = new Defenders("Mine", "MINE", 1, 0, 0, 8);
List<Defenders> deflist = new List<Defenders> { archer, wall, mine };
while (true)
{
    mainfield.DrawField();
    int choice = Convert.ToInt32(Console.ReadLine());
    if (choice == 0)
    {
        break;
    }
    archer.PlaceDef(mainfield, "A2");
}

// GameVariables.ShowCombMenu();