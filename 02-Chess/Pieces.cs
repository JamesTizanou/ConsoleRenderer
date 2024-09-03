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
            {pieces.BISHOP, Bishop },
            {pieces.QUEEN, Queen },
            {pieces.KING, King }
        };

        public static Move Pawn(Pieces p)
        {
            int ind = 0;
            List<int> possibilities = new();
            if (p.player == 0) { ind = 1; } else { ind = -1; }
            if (p.firstMove)
            {
                possibilities.Add( /*p.pos + Chess_.board.squaresPerColumn * ind,*/ p.pos + (Chess_.board.squaresPerColumn) * 2 * ind );
            }
            
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
            List<int> poss = new List<int>();
            if (Chess_.board.GetColonne(p.pos) >= 2 && Chess_.board.GetRangee(p.pos) >= 1){ poss.Add(p.pos - 10);}
            if (Chess_.board.GetColonne(p.pos) >= 1 && Chess_.board.GetRangee(p.pos) >= 2) { poss.Add(p.pos - 17); }
            if (Chess_.board.GetColonne(p.pos) >= 2 && Chess_.board.GetRangee(p.pos) <= 6) { poss.Add(p.pos + 6); }
            if (Chess_.board.GetColonne(p.pos) >= 1 && Chess_.board.GetRangee(p.pos) <= 5) { poss.Add(p.pos + 15); }
            if (Chess_.board.GetColonne(p.pos) <= 6 && Chess_.board.GetRangee(p.pos) <= 5) { poss.Add(p.pos + 17); }
            if (Chess_.board.GetColonne(p.pos) <= 5 && Chess_.board.GetRangee(p.pos) <= 6) { poss.Add(p.pos + 10); }
            if (Chess_.board.GetColonne(p.pos) <= 5 && Chess_.board.GetRangee(p.pos) >= 1) { poss.Add(p.pos - 6); }
            if (Chess_.board.GetColonne(p.pos) <= 6 && Chess_.board.GetRangee(p.pos) >= 2) { poss.Add(p.pos - 15); }
            poss = FiltrerMoves(p, poss);
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
            if (p.pos % 8 != 0)
            {
                poss.AddRange(UpLeft(p));
                poss.AddRange(DownLeft(p));
            }
            if (p.pos % 8 != 7)
            {
                poss.AddRange(UpRight(p));
                poss.AddRange(DownRight(p));
            }
            poss = FiltrerMoves(p, poss);
            return new(poss);
        }

        public static List<int> UpLeft(Pieces p)
        {
            List<int> poss = new();
            for (int i = p.pos - 9; i < 1000; i -= 9)
            {
                if (Chess_.CaseVide(i) && i % 8 == 0) { poss.Add(i); break; }
                if (Chess_.CaseVide(i) && i >= 0) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            return poss;
        }

        public static List<int> DownLeft(Pieces p)
        {
            List<int> poss = new();
            for (int i = p.pos + 7; i < 1000; i += 7)
            {
                if (Chess_.CaseVide(i) && i % 8 == 0) { poss.Add(i); break; }
                if (Chess_.CaseVide(i) && i <= 63) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            return poss;
        }

        public static List<int> UpRight(Pieces p)
        {
            List<int> poss = new();
            for (int i = p.pos - 7; i < 1000; i -= 7)
            {
                if (Chess_.CaseVide(i) && i % 8 == 7) { poss.Add(i); break; }
                if (Chess_.CaseVide(i) && i >= 0) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            return poss;
        }

        public static List<int> DownRight(Pieces p)
        {
            List<int> poss = new();
            for (int i = p.pos + 9; i < 1000; i += 9)
            {
                if (Chess_.CaseVide(i) && i % 8 == 7) { poss.Add(i); break; }
                if (Chess_.CaseVide(i) && i <= 63) { poss.Add(i); }
                else if (Chess_.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            return poss;
        }

        public static Move Queen(Pieces p)
        {
            List<int> poss = new();
            poss = Bishop(p).casesPossibles;
            poss.AddRange(Rook(p).casesPossibles);
            return new(poss);
        }

        public static Move King(Pieces p) // Il manque retirer les moves qui mettent le roi en Chech mate
        {
            List<int> poss = new() { p.pos - 9, p.pos - 8, p.pos - 7, p.pos - 1, p.pos + 1, p.pos + 7, p.pos + 8, p.pos + 9};
            poss = FiltrerMoves(p,poss);
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
