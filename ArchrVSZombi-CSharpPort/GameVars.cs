namespace ArchrVSZombi_CSharpPort
{
    public class GameVars
    {
        public SerializableDictionary<string, int> GVariables { get; set; }
        public GameVars()
        {
            GVariables = new SerializableDictionary<string, int>();
            GVariables.Add("turn", 0);
            GVariables.Add("monster_kill_target", 20);
            GVariables.Add("monsters_killed", 0);
            GVariables.Add("num_monsters", 0);
            GVariables.Add("gold", 10);
            GVariables.Add("threat", 0);
            GVariables.Add("max_threat", 10);
            GVariables.Add("danger_level", 1);

        }
        public void ShowCombMenu()
        {
            GVariables["turn"]++;
            GVariables["threat"]++;
            string dashes = new string('-', GVariables["threat"]);
            Console.WriteLine($"Turn {GVariables["turn"]}     Threat = [{dashes.ToString().PadRight(GVariables["max_threat"])}]     Danger Level = {GVariables["danger_level"]}");
            Console.WriteLine($"Gold = {GVariables["gold"]}   Monsters killed = {GVariables["monsters_killed"]}/{GVariables["monster_kill_target"]}");
            Console.WriteLine("1. Buy unit     2. End turn");
            Console.WriteLine("3. Save game    4. Quit     5. Upgrade unit");
        }
    }
}
