using System.Data;

namespace ArchrVSZombi_CSharpPort
{
    public class Field
    {
        public int LastChar { get; set; }
        public List<List<List<string>>> StringField { get; set; }
        public List<char> RowList { get; set; }
        public Field()
        {
            char lettera = 'A';
            int a = lettera;
            int row = 5;
            int col = 7;
            StringField = new List<List<List<string>>>();
            for (int x = 0; x < row; x++)
            {
                StringField.Add(new List<List<string>>());
                for (int y = 0; y < col; y++)
                {
                    StringField[x].Add(new List<String>());
                    StringField[x][y].Add("");
                    StringField[x][y].Add("");
                }
            }
            LastChar = a + row;
            RowList = new List<char>();
            for (int x = (int)'A'; x < LastChar; x++)
            {
                RowList.Add((char)x);
            }
        }
        public Field(int row, int col)
        {
            StringField = new List<List<List<string>>>();
            for (int x = 0; x < row; x++)
            {
                StringField.Add(new List<List<string>>());
                for (int y = 0; y < col; y++)
                {
                    StringField[x].Add(new List<String>());
                    StringField[x][y].Add("");
                    StringField[x][y].Add("");
                }
            }
        }
        public void DrawField()
        {
            char lettera = 'A';
            int a = lettera;
            for (int x = 1; x <= 3; x++)
            {
                Console.Write("   ");
                Console.Write(Convert.ToString(x).PadLeft(3).PadRight(3));
            }
            Console.WriteLine();
            foreach (var line in StringField)
            {
                Console.WriteLine("  " + String.Concat(Enumerable.Repeat("+-----", line.Count())) + "+");
                Console.Write((char)a + " ");
                a++;
                foreach (var row in line)
                {
                    Console.Write($"|{row[0].PadBoth(5)}");
                }
                Console.WriteLine("|");
                Console.Write("  ");
                foreach (var row in line)
                {
                    Console.Write($"|{row[1].PadBoth(5)}");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("  " + String.Concat(Enumerable.Repeat("+-----", StringField[0].Count())) + "+");
        }
        public bool FieldCheck(List<Monsters> monlist)
        {
            foreach (var row in StringField)
            {
                foreach (var col in row)
                {
                    foreach (Monsters m in monlist)
                    {
                        if (col[0] == m.ShortName)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
