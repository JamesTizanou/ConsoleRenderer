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

        public static List<Pieces> _Pieces = new() { new(pieces.PAWN, 50, 1, (Color)Colors.White), new(pieces.PAWN, 18, 1, (Color)Colors.White) , new(pieces.PAWN, 19, 0, (Color)Colors.Black), new(pieces.PAWN, 17, 0, (Color)Colors.Black) };


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
        public static Pieces? lastClicked = null;
        public static Move? lastMoves = null;
        static void ChoixPion()
        {
            if (Program.MouseLeftPressed())
            {
                bool found = false;
                if (lastMoves != null && lastClicked != null)
                {
                    int p = lastMoves.isClicked();
                    if (p >= 0)
                    {
                        found = true;
                        lastMoves.Apply(lastClicked, lastMoves.isClicked());
                        lastClicked = null;
                        lastMoves = null;
                        return;
                    }
                }
                for (int i = 0; i < _Pieces.Count; i++)
                {
                    if (_Pieces[i].ClickSurPiece())
                    {
                        found = true;
                        lastClicked = _Pieces[i];
                        lastMoves = lastClicked.GetMoves();
                    }
                }
                if (!found)
                {
                    lastClicked = null;
                    lastMoves = null;
                }
            }
            if (lastClicked != null)
            {
                lastMoves = lastClicked.GetMoves();
                lastMoves.ShowMoves();
            }
            return;
        }

        static Vector2D<int> posImage = new(100, 100);
        static Pieces whitePawn = new(pieces.PAWN, 50, 1, (Color)Colors.White);
        static Pieces BlackPawn = new(pieces.PAWN, 3, 0, (Color)Colors.Black);
        public static void Chess()
        {
            board.Display(true);
            board.Personalize(casesNoires(), couleur2);

            for (int i = 0; i < _Pieces.Count; i++)
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

        public static void ChangeTurn()
        {
            if (tour == 0) { tour = 1; return; }
            if (tour == 1) { tour = 0; }
        }
    }
}
