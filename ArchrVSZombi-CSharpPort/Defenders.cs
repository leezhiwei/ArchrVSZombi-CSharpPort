using System.Data;
using System.Linq;

namespace ArchrVSZombi_CSharpPort
{
    internal class Defenders:Entities
    {
        public int Price { get; set; }
        public Defenders():base() { }
        public Defenders(string n, string sn, int mhp, int mind, int maxd, int p) : base("Defenders", n, sn, mhp, mind, maxd)
        {
            Price = p;
        }
        public bool PlaceDef(Field f, string pos)
        {
            List<char> alpha = new List<char>();
            List<int> collist = new List<int> { 1, 2, 3 };
            char row;
            char col;
            int rowindex;
            int colindex;
            for (int x = (int) 'A'; x < f.LastChar; x++)
            {
                alpha.Add((char) x);
            }
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
            if (alpha.Contains(row))
            {
                rowindex = alpha.IndexOf(row);
                Console.WriteLine(char.GetNumericValue(col));
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
    }
}
