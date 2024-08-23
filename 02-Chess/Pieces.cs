using Main;
using static Chess.Pieces;

namespace Chess
{
    class PiecesManager
    {
        public static Dictionary<pieces, Func<Pieces, int[]>> moves;

        static int[] PawnMove(Pieces p)
        {
            if (Chess_.tour == 0)
            {
                if (p.firstMove)
                {
                    return new int[] { p.pos - Chess_.board.squaresPerColumn, p.pos - (Chess_.board.squaresPerColumn) * 2 };
                }
                return new int[] { p.pos - Chess_.board.squaresPerColumn };
            }
            else if (Chess_.tour == 1)
            {
                if (p.firstMove)
                {
                    return new int[] { p.pos - Chess_.board.squaresPerColumn, p.pos + (Chess_.board.squaresPerColumn) * 2 };
                }
                return new int[] { p.pos + Chess_.board.squaresPerColumn };
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

        string pawn = "pawn.";
        string bishop = "bishop.";
        string knight = "knight.";
        string rook = "rook.";
        string king = "king.";
        string queen = "queen.";
        public enum pieces
        {
            PAWN,
            BISHOP,
            KNIGHT,
            ROOK,
            KING,
            QUEEN
        }
        public pieces nom { get; set; }

        public Pieces(pieces n, int p, int player) // player 0 = white, player 1 = black
        {
            nom = n;
            pos = p;
            image = nom.ToString().ToLower() + ".png";
            this.player = player;
        }

        public void Draw()
        {
            Program.DrawImage(image, Chess_.board.grille[pos].pos, Chess_.board.tileSize); // à refaire
        }

        bool ClickSurPiece()
        {
            if (player == Chess_.tour)
            {
                if (true)
                {

                }
            }
            return true;
        }

        public void ShowMoves()
        {
            if (ClickSurPiece())
            {
                if (!isShowingMoves)
                {
                    int[] moves = PiecesManager.moves[nom](this);
                    for (int i = 0; i < moves.Length; i++)
                    {
                        Program.DrawFullCircle(new(Chess_.board.grille[i].pos + (Chess_.board.grille[i].pos) / 2, Chess_.board.tileSize.x / 2 - 5));
                    }
                }
            }
        }

        public void Move()
        {
            /*switch (nom)
            {
                case pieces.PAWN:
                    PawnMove();
                    break;
            }*/
        }
    }
}
