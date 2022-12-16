namespace ArchrVSZombi_CSharpPort
{
    internal class Field
    {
        public List<List<List<string>>> StringField { get; set; }
        public Field()
        {
            StringField = new List<List<List<string>>>();
            foreach (List<List<string>> line in StringField)
            {
                foreach (List<string> row in line)
                {
                    row[0] = "";
                    row[1] = "";
                }
            }
        }
        public void DrawField()
        {
            char lettera = 'A';
            int a = lettera;
            int count = 0;
            for (int x = 1; x <= 3; x++)
            {
                Console.Write("\t");
                Console.Write(Convert.ToString(x).PadLeft(3).PadRight(3));
            }
            foreach (List<List<string>> line in StringField)
            {
                Console.WriteLine("\t" + String.Concat(Enumerable.Repeat("+-----",line.Count())));
            }
        }
    }
}
