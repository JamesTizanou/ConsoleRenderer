using Main;
using System.Xml;

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
                    return new(new List<int> { p.pos + Chess_.board.squaresPerColumn, p.pos + (Chess_.board.squaresPerColumn) * 2 });
                }
                List<int> possibilities = new() { p.pos + Chess_.board.squaresPerColumn };
                possibilities = FiltrerMoves(p, possibilities);
                return new(possibilities);
            }
            else if (Chess_.tour == 1)
            {
                if (p.firstMove)
                {
                    return new(new List<int> { p.pos - Chess_.board.squaresPerColumn, p.pos - (Chess_.board.squaresPerColumn) * 2 });
                }
                List<int> possibilities = new(){ p.pos - Chess_.board.squaresPerColumn };
                possibilities = FiltrerMoves(p, possibilities);
                return new(possibilities);
            }
            throw new Exception();
        }

        static List<int> FiltrerMoves(Pieces p, List<int> poss) // retourne toutes les cases de mes moves possibles qui ne sont pas occupées par mes autres pions
        {
            for (int i = 0; i < Chess_._Pieces.Count; i++)
            {
                if (Chess_._Pieces[i].player == p.player)
                {
                    for (int j = 0; j < poss.Count; j++)
                    {
                        if (Chess_._Pieces[i].pos == poss[j])
                        {
                            poss.RemoveAt(j);
                        }
                    }
                }
                else
                {
                    if (p.player == 1)
                    {
                        if (p.pos - 7 == Chess_._Pieces[i].pos)
                        {
                            poss.Add(p.pos - 7);
                        }
                        else if (p.pos - 9 == Chess_._Pieces[i].pos)
                        {
                            poss.Add(p.pos - 9);
                        }
                    }
                    else
                    {
                        if (p.pos + 7 == Chess_._Pieces[i].pos)
                        {
                            poss.Add(p.pos + 7);
                        }
                        else if (p.pos + 9 == Chess_._Pieces[i].pos)
                        {
                            poss.Add(p.pos + 9);
                        }
                    }
                }
            }
            return poss;
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

        public Vector2D<float> truePos { get; set; } = new(0, 0);

        public pieces nom { get; set; }

        public Pieces(pieces n, int p, int player, Color couleur) // player 0 = white, player 1 = black
        {
            nom = n;
            pos = p;
            this.player = player;
            this.couleur = couleur;
            truePos.x = (float)Chess_.board.grille[pos].pos.x;
            truePos.y = (float)Chess_.board.grille[pos].pos.y;
            string arg = "WHITE";
            if (player == 0)
            {
                arg = "BLACK";
            }
            image = "Images/" + nom.ToString() + "_" + arg + ".png"; // PAWN => pawn.png
        }

        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        Vector2D<float> Lerp(Vector2D<int> firstFloat, Vector2D<float> secondFloat, float by)
        {
            Lerp(firstFloat.x, secondFloat.x, by);
            Lerp(firstFloat.y, secondFloat.y, by);
            return new Vector2D<float>(Lerp(firstFloat.x, secondFloat.x, by), Lerp(firstFloat.y, secondFloat.y, by));
        }

        public void Draw()
        {
            Vector2D<int> posF = Chess_.board.grille[pos].pos;
            truePos = Lerp(posF, truePos, 0.9F);
            Vector2D<int> a = new((int)truePos.x, (int)truePos.y);
            Program.DrawImage(image, /*Chess_.board.grille[pos].pos*/ a, Chess_.board.tileSize); // à refaire
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
        public List<int> casesPossibles = new();
        public Move(List<int> casesPossibles)
        {
            this.casesPossibles = casesPossibles;
        }

        public void ShowMoves()
        {

            for (int i = 0; i < casesPossibles.Count; i++)
            {
                Color.Pencil(Colors.Red);
                Program.DrawFullCircle(new(Chess_.board.grille[casesPossibles[i]].pos + (Chess_.board.tileSize) / 2, Chess_.board.tileSize.x / 2 - 5));
            }
        }

        public void Apply(Pieces p, int pos)
        {
            p.pos = pos;
            p.firstMove = false;
            for (int i = 0;i < Chess_._Pieces.Count;i++)
            {
                if (Chess_._Pieces[i].player != Chess_.tour)
                {
                    if (Chess_._Pieces[i].pos == pos)
                    {
                        Chess_._Pieces.RemoveAt(i);
                    }
                }
            }
            Chess_.ChangeTurn();
        }

        public int isClicked()
        {
            for (int i = 0; i < casesPossibles.Count; i++)
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
        }
    }
}
