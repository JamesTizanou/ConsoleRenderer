﻿using Main;

// TODO:
// Fix l'algorithme du pawn pour manger, car de la façon dont il est fait maintenant, un pawn pourrait manger une pièce qui 'est pas directement à sa diagonale 

namespace Chess
{
    class Chess_
    {
        static int t = 100;
        static Colors couleur1 = Colors.White;
        static Colors couleur2 = Colors.Black;
        public static Grid board = new Grid(new(0, 0), new(t, t), 8, 8, couleur1);
        public static int tour = 1;
        static bool FirstExec = true;

        public static List<Pieces> _Pieces = new()
        {
            new(pieces.PAWN, 8, 0),
            new(pieces.PAWN, 9, 0),
            new(pieces.PAWN, 10, 0),
            new(pieces.PAWN, 11, 0),
            new(pieces.PAWN, 12, 0),
            new(pieces.PAWN, 13, 0),
            new(pieces.PAWN, 14, 0),
            new(pieces.PAWN, 15, 0),

            new(pieces.ROOK, 0, 0),
            new(pieces.KNIGHT, 1, 0),
            new(pieces.BISHOP, 2, 0),
            new(pieces.QUEEN, 3, 0),
            new(pieces.KING, 4, 0),
            new(pieces.BISHOP, 5, 0),
            new(pieces.KNIGHT, 6, 0),
            new(pieces.ROOK, 7, 0),



            new(pieces.PAWN, 48, 1),
            new(pieces.PAWN, 49, 1),
            new(pieces.PAWN, 50, 1),
            new(pieces.PAWN, 51, 1),
            new(pieces.PAWN, 52, 1),
            new(pieces.PAWN, 53, 1),
            new(pieces.PAWN, 54, 1),
            new(pieces.PAWN, 55, 1),

            new(pieces.ROOK, 56, 1),
            new(pieces.KNIGHT, 57, 1),
            new(pieces.BISHOP, 58, 1),
            new(pieces.QUEEN, 59, 1),
            new(pieces.KING, 60, 1),
            new(pieces.BISHOP, 61, 1),
            new(pieces.KNIGHT, 62, 1),
            new(pieces.ROOK, 63, 1)
        };

        public static bool CaseVide(int c)
        {
            for (int i = 0; i < _Pieces.Count; i++)
            {
                if (c == _Pieces[i].pos)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CaseEnnemie(int j, int c)
        {
            for (int i = 0; i < _Pieces.Count; i++)
            {
                if (c == _Pieces[i].pos && tour != j)
                {
                    return false;
                }
            }
            return true;
        }


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
        static Sound music = new Sound("music.mp3");
        public static void Chess()
        {
            board.Display(true);
            board.Personalize(casesNoires(), couleur2);
            ChoixPion();
            for (int i = 0; i < _Pieces.Count; i++)
            {
                _Pieces[i].Draw();
            }
            music.Play();
        }

        public static void ChangeTurn()
        {
            if (tour == 0) { tour = 1; return; }
            if (tour == 1) { tour = 0; }
        }
    }
}
