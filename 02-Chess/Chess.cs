using Main;

namespace Chess
{
    class Chess_
    {
        static int t = 75;
        static Colors couleur1 = Colors.White;
        static Colors couleur2 = Colors.Black;
        public static Grid board = new Grid(new(100, 100), new(t, t), 8, 8, couleur1);
        

        static List<int> casesNoires()
        {
            List<int> cn = new();
            for (int i = 0; i < board.grille.Count; i += board.squaresPerColumn)
            {
                int n = i;
                if (board.GetRangee(i) % 2 != 0)
                {
                    n++;
                }
                cn.Add(n);
                cn.Add(n + 2);
                cn.Add(n + 4);
                cn.Add(n + 6);
            }
            return cn;
        }
        public static void Chess()
        {
            board.Display(true);
            board.Personalize(casesNoires(), couleur2);
        }
    }
}
