using Main;

namespace Chess
{
    class Chess_
    {
        static int t = 75;
        static Colors couleur1 = Colors.White;
        static Colors couleur2 = Colors.Black;
        public static Grid board = new Grid(new(100, 100), new(t, t), 8, 8, couleur1);
        public static int tour = 1;
        static bool FirstExec = true;

        static List<Pieces> _Pieces = new() {new(pieces.PAWN, 50, 1, (Color)Colors.White) };
        

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
        static Pieces? lastClicked = null;
        static void ChoixPion()
        {
            if (Program.MouseLeftPressed())
            {
                bool found = false;
                for (int i = 0; i < _Pieces.Count; i++)
                {
                    if (_Pieces[i].ClickSurPiece())
                    {
                        lastClicked = _Pieces[i];
                        found = true;
                    }
                }
                if (!found)
                {
                    lastClicked = null;
                }
            }
            if (lastClicked != null)
            {
                lastClicked.ShowMoves();
            }
        }

        static Vector2D<int> posImage = new(100, 100);
        static Pieces whitePawn = new(pieces.PAWN, 50, 1, (Color)Colors.White);
        static Pieces BlackPawn = new(pieces.PAWN, 3, 0, (Color)Colors.Black);
        public static void Chess()
        {
            board.Display(true);
            board.Personalize(casesNoires(), couleur2);

            for (int i = 0;i < _Pieces.Count;i++)
            {
                _Pieces[i].Draw();
            }
            ChoixPion();
            if (tour == 0)
            {
                Console.WriteLine("allo");
            }
            //BlackPawn.Draw();
        }
    }
}
