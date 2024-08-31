using Main;
using System.Runtime.CompilerServices;

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
            {pieces.PAWN, Pawn },
            {pieces.KNIGHT, Knight },
            {pieces.ROOK, Rook },
            {pieces.BISHOP, Bishop }
        };

        public static Move Pawn(Pieces p)
        {
            int ind = 0;
            if (p.player == 0) { ind = 1; } else { ind = -1; }
            if (p.firstMove)
            {
                return new(new List<int> { p.pos + Chess_.board.squaresPerColumn * ind, p.pos + (Chess_.board.squaresPerColumn) * 2 * ind });
            }
            List<int> possibilities = new();
            if (Chess_.CaseVide(p.pos + Chess_.board.squaresPerColumn * ind))
            { 
                possibilities.Add(p.pos + Chess_.board.squaresPerColumn * ind);
            }
            for (int i = 0; i < Chess_._Pieces.Count; i++)
            {
                if (p.pos + 7 * ind == Chess_._Pieces[i].pos)
                {
                    possibilities.Add(p.pos + 7 * ind);
                }
                else if (p.pos + 9 * ind == Chess_._Pieces[i].pos)
                {
                    possibilities.Add(p.pos + 9 * ind);
                }
            }
            possibilities = FiltrerMoves(p, possibilities);
            return new(possibilities);
        }



        public static Move Knight(Pieces p)
        {
            /*int m1 = 6;
            int m2 = 10;
            int m3 = 15;
            int m4 = 17;*/
            List<int> poss = new List<int>() { p.pos - 6, p.pos - 15, p.pos - 17, p.pos - 10, p.pos + 6, p.pos + 15, p.pos + 17, p.pos + 10 };
            poss = FiltrerMoves(p, poss);
            for (int i = 0; i < poss.Count; i++)
            {
                if (Math.Abs(Chess_.board.GetColonne(p.pos) - Chess_.board.GetColonne(poss[i])) == 2 || Math.Abs(Chess_.board.GetRangee(p.pos) - Chess_.board.GetRangee(poss[i])) == 2) // mauvais algorithme, c'est pour tester
                {

                }
                else
                {
                    poss.RemoveAt(i);
                }
            }

            return new(poss);
        }

        public static Move Rook(Pieces p)
        {
            List<int> poss = new();
            for (int i = p.pos - 8; i >= 0; i -= 8)
            {
                if (Chess_.CaseVide(i)) { poss.Add(i);}
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break;}
            }
            for (int i = p.pos + 8; i <= 63; i += 8)
            {
                if (Chess_.CaseVide(i)) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; } 
                else { break; }
            }
            for (int i = p.pos + 1; i <= 8 * Chess_.board.GetRangee(p.pos) + 7; i++)
            {
                if (Chess_.CaseVide(i)) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            for (int i = p.pos - 1; i >= 8 * Chess_.board.GetRangee(p.pos); i--)
            {
                if (Chess_.CaseVide(i)) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            poss = FiltrerMoves(p,poss);
            return new(poss);
        }

        public static Move Bishop(Pieces p)
        {
            List<int> poss = new();
            for (int i = p.pos - 9; i < 1000; i-= 9)
            {
                if (Chess_.CaseVide(i) && i % 8 != 7 && i > 7) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            for (int i = p.pos - 7; i < 1000; i -= 7)
            {
                if (Chess_.CaseVide(i) && i % 8 != 0 && i > 7) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            for (int i = p.pos + 7; i < 1000; i += 7)
            {
                if (Chess_.CaseVide(i) && i % 8 != 0 && i < 56) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            for (int i = p.pos + 9; i < 1000; i += 9)
            {
                if (Chess_.CaseVide(i) && i % 8 != 7 && i < 56) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            poss = FiltrerMoves(p, poss);
            return new(poss);
        }

        static List<int> FiltrerMoves(Pieces p, List<int> poss) // retourne toutes les cases de mes moves possibles qui ne sont pas occupées par mes autres pions
        {
            for (int i = 0; i < Chess_._Pieces.Count; i++)
            {
                if (Chess_._Pieces[i].player == p.player)
                {
                    for (int j = 0; j < poss.Count; j++)
                    {
                        if (poss[j] < 0 || poss[j] > 63 || Chess_._Pieces[i].pos == poss[j])
                        {
                            poss.RemoveAt(j);
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

        public Color couleur;

        public Vector2D<float> truePos { get; set; } = new(0, 0);

        public pieces nom { get; set; }

        public Pieces(pieces n, int p, int player, Color couleur) // player 0 = white, player 1 = black
        {
            nom = n;
            pos = p;
            this.player = player;
            this.couleur = couleur;
            truePos.x = Chess_.board.grille[pos].pos.x;
            truePos.y = Chess_.board.grille[pos].pos.y;
            string arg = "WHITE";
            if (player == 0)
            {
                arg = "BLACK";
            }
            image = "Images/" + nom.ToString() + "_" + arg + ".png";
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
                if (casesPossibles[i] >= 0 && casesPossibles[i] <= 63)
                {
                    Program.DrawFullCircle(new(Chess_.board.grille[casesPossibles[i]].pos + (Chess_.board.tileSize) / 2, Chess_.board.tileSize.x / 2 - 5));
                }
            }
        }

        public void Apply(Pieces p, int pos)
        {
            p.pos = pos;
            p.firstMove = false;
            for (int i = 0; i < Chess_._Pieces.Count; i++)
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
