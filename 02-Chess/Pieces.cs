﻿using Main;

namespace Chess
{
    public enum pieces
    {
        PAWN,
        BISHOP,
        KNIGHT,
        ROOK,
        KING,
        QUEEN
    }
    class PiecesManager
    {
        public static Dictionary<pieces, Func<Pieces, Move>> moves = new Dictionary<pieces, Func<Pieces, Move>>
        {
            {pieces.PAWN, Pawn }
        };

        public static Move Pawn(Pieces p)
        {
            if (Chess_.tour == 0)
            {
                if (p.firstMove)
                {
                    return new(new int[] { p.pos - Chess_.board.squaresPerColumn, p.pos + (Chess_.board.squaresPerColumn) * 2 });
                }
                return new(new int[] { p.pos + Chess_.board.squaresPerColumn });
            }
            else if (Chess_.tour == 1)
            {
                if (p.firstMove)
                {
                    return new(new int[] { p.pos - Chess_.board.squaresPerColumn, p.pos - (Chess_.board.squaresPerColumn) * 2 });
                }
                return new(new int[] { p.pos - Chess_.board.squaresPerColumn });
            }
            throw new Exception();
        }
    }

    class Pieces
    {
        public int pos;

        public string image;

        public int player;

        public bool firstMove = true;

        static bool isShowingMoves = false;

        public Color couleur;

        public pieces nom { get; set; }

        public Pieces(pieces n, int p, int player, Color couleur) // player 0 = white, player 1 = black
        {
            nom = n;
            pos = p;
            image = nom.ToString().ToLower() + ".png"; // PAWN => pawn.png
            this.player = player;
            this.couleur = couleur;
        }

        public void Draw()
        {
            Program.DrawImage(image, Chess_.board.grille[pos].pos, Chess_.board.tileSize); // à refaire
        }

        public bool ClickSurPiece()
        {
            if (player == Chess_.tour)
            {
                if (Program.MouseLeftPressed())
                {
                    if (Program.PointInRect(Program.MousePosition(), new(Chess_.board.grille[pos].pos, Chess_.board.tileSize)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Move GetMoves()
        {
            return PiecesManager.moves[nom](this);
        }
    }

    class Move
    {
        public int[] casesPossibles;
        public Move(int[] casesPossibles)
        {
            this.casesPossibles = casesPossibles;
        }

        public void ShowMoves()
        {

            for (int i = 0; i < casesPossibles.Length; i++)
            {
                Color.Pencil(Colors.Red);
                Program.DrawFullCircle(new(Chess_.board.grille[casesPossibles[i]].pos + (Chess_.board.tileSize) / 2, Chess_.board.tileSize.x / 2 - 5));
            }
        }

        public void Apply(Pieces p, int pos)
        {
            p.pos = pos;
            p.firstMove = false;
        }

        public int isClicked()
        {
            for (int i = 0; i < casesPossibles.Length; i++)
            {
                if (Program.MouseLeftPressed())
                {
                    if (Program.PointInRect(Program.MousePosition(), new(Chess_.board.grille[casesPossibles[i]].pos, Chess_.board.tileSize)))
                    {
                        return casesPossibles[i];
                    }
                }
            }
            return -1;
            //throw new NotImplementedException();
        }
    }
}
