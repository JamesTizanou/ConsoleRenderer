using Main;
using static Chess.Pieces;

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
        public static Dictionary<pieces, Func<Pieces, int[]>> moves = new Dictionary<pieces, Func<Pieces, int[]>> 
        {
            {pieces.PAWN, PawnMove }
        };

        public static int[] PawnMove(Pieces p)
        {
            if (Chess_.tour == 0)
            {
                if (p.firstMove)
                {
                    return new int[] { p.pos - Chess_.board.squaresPerColumn, p.pos + (Chess_.board.squaresPerColumn) * 2 };
                }
                return new int[] { p.pos + Chess_.board.squaresPerColumn };
            }
            else if (Chess_.tour == 1)
            {
                if (p.firstMove)
                {
                    return new int[] { p.pos - Chess_.board.squaresPerColumn, p.pos - (Chess_.board.squaresPerColumn) * 2 };
                }
                return new int[] { p.pos - Chess_.board.squaresPerColumn };
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

        public void ShowMoves()
        {  
            int[] moves = PiecesManager.moves[nom](this);
            for (int i = 0; i < moves.Length; i++)
            {
                Color.Pencil(Colors.Red);
                Program.DrawFullCircle(new(Chess_.board.grille[moves[i]].pos + (Chess_.board.tileSize) / 2, Chess_.board.tileSize.x / 2 - 5));
            }
        }

        public void Move()
        {
            int[] moves = PiecesManager.moves[nom](this);
            for (int i = 0; i < moves.Length; i++)
            {
                if (Program.MouseLeftPressed())
                {
                    if (Program.PointInRect(Program.MousePosition(), new(Chess_.board.grille[moves[i]].pos, Chess_.board.tileSize)))
                    {
                        pos = moves[i];
                        firstMove = false;
                        break;
                    }
                }
            }
        }
    }

    class Moves
    {
        static Pieces? dernierPionClique;

    }
}
