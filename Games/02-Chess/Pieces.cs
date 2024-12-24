using Classes;
using Main;

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
            List<int> possibilities = new();
            int ind = p.player == 0 ? 1 : -1;
            if (p.firstMove && Chess.CaseVide(p.pos + (Chess.board.squaresPerColumn) * 2 * ind) && Chess.CaseVide(p.pos + (Chess.board.squaresPerColumn) * ind))
            {
                possibilities.Add(p.pos + (Chess.board.squaresPerColumn) * 2 * ind);
            }

            if (Chess.CaseVide(p.pos + Chess.board.squaresPerColumn * ind))
            {
                possibilities.Add(p.pos + Chess.board.squaresPerColumn * ind);
            }
            for (int i = 0; i < Chess._Pieces.Count; i++)
            {
                // Ne marche pas. Dépendamment de la valeur de mon index, le getColle change
                if (p.pos + 7 * ind == Chess._Pieces[i].pos && Chess.board.GetColonne(p.pos) >= 1 && Chess.board.GetColonne(p.pos) < 7)
                {
                    if (Program.KeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_A))
                    {
                        Console.WriteLine("   " + Chess.board.GetColonne(p.pos));
                    }
                    possibilities.Add(p.pos + 7 * ind);
                }
                else if (p.pos + 9 * ind == Chess._Pieces[i].pos && Chess.board.GetColonne(p.pos) >= 1 && Chess.board.GetColonne(p.pos) < 7)
                {
                    if (Program.KeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_A))
                    {
                        Console.WriteLine("   " + Chess.board.GetColonne(p.pos));
                    }
                    possibilities.Add(p.pos + 9 * ind);
                }
            }
            possibilities = FiltrerMoves(p, possibilities);
            return new(possibilities);
        }



        public static Move Knight(Pieces p)
        {
            List<int> poss = new List<int>();
            if (Chess.board.GetColonne(p.pos) >= 2 && Chess.board.GetRangee(p.pos) >= 1) { poss.Add(p.pos - 10); }
            if (Chess.board.GetColonne(p.pos) >= 1 && Chess.board.GetRangee(p.pos) >= 2) { poss.Add(p.pos - 17); }
            if (Chess.board.GetColonne(p.pos) >= 2 && Chess.board.GetRangee(p.pos) <= 6) { poss.Add(p.pos + 6); }
            if (Chess.board.GetColonne(p.pos) >= 1 && Chess.board.GetRangee(p.pos) <= 5) { poss.Add(p.pos + 15); }
            if (Chess.board.GetColonne(p.pos) <= 6 && Chess.board.GetRangee(p.pos) <= 5) { poss.Add(p.pos + 17); }
            if (Chess.board.GetColonne(p.pos) <= 5 && Chess.board.GetRangee(p.pos) <= 6) { poss.Add(p.pos + 10); }
            if (Chess.board.GetColonne(p.pos) <= 5 && Chess.board.GetRangee(p.pos) >= 1) { poss.Add(p.pos - 6); }
            if (Chess.board.GetColonne(p.pos) <= 6 && Chess.board.GetRangee(p.pos) >= 2) { poss.Add(p.pos - 15); }
            poss = FiltrerMoves(p, poss);
            return new(poss);
        }

        public static Move Rook(Pieces p)
        {
            List<int> poss = new();
            for (int i = p.pos - 8; i >= 0; i -= 8)
            {
                if (Chess.CaseVide(i)) { poss.Add(i); }
                else if (Chess.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            for (int i = p.pos + 8; i <= 63; i += 8)
            {
                if (Chess.CaseVide(i)) { poss.Add(i); }
                else if (Chess.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            for (int i = p.pos + 1; i <= 8 * Chess.board.GetRangee(p.pos) + 7; i++)
            {
                if (Chess.CaseVide(i)) { poss.Add(i); }
                else if (Chess.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            for (int i = p.pos - 1; i >= 8 * Chess.board.GetRangee(p.pos); i--)
            {
                if (Chess.CaseVide(i)) { poss.Add(i); }
                else if (Chess.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            poss = FiltrerMoves(p, poss);
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
                if (Chess.CaseVide(i) && i % 8 == 0) { poss.Add(i); break; }
                if (Chess.CaseVide(i) && i >= 0) { poss.Add(i); }
                else if (Chess.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            return poss;
        }

        public static List<int> DownLeft(Pieces p)
        {
            List<int> poss = new();
            for (int i = p.pos + 7; i < 1000; i += 7)
            {
                if (Chess.CaseVide(i) && i % 8 == 0) { poss.Add(i); break; }
                if (Chess.CaseVide(i) && i <= 63) { poss.Add(i); }
                else if (Chess.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            return poss;
        }

        public static List<int> UpRight(Pieces p)
        {
            List<int> poss = new();
            for (int i = p.pos - 7; i < 1000; i -= 7)
            {
                if (Chess.CaseVide(i) && i % 8 == 7) { poss.Add(i); break; }
                if (Chess.CaseVide(i) && i >= 0) { poss.Add(i); }
                else if (Chess.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
                else { break; }
            }
            return poss;
        }

        public static List<int> DownRight(Pieces p)
        {
            List<int> poss = new();
            for (int i = p.pos + 9; i < 1000; i += 9)
            {
                if (Chess.CaseVide(i) && i % 8 == 7) { poss.Add(i); break; }
                if (Chess.CaseVide(i) && i <= 63) { poss.Add(i); }
                else if (Chess.CaseEnnemie(p.player, i)) { poss.Add(i); break; }
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

        public static Move King(Pieces p) // Il manque retirer les moves qui mettent le roi en Checkmate
        {
            List<int> poss = new() { p.pos - 9, p.pos - 8, p.pos - 7, p.pos - 1, p.pos + 1, p.pos + 7, p.pos + 8, p.pos + 9 };

            // CODE POUR CASTLE

            List<int> enemi = new();
            for (int i = 0; i < Chess._Pieces.Count; i++)
            {
                if (Chess._Pieces[i].player != Chess.tour && Chess._Pieces[i].nom != pieces.KING) // Évite que roi aille à une pos où il se fera checkmate, sauf si l'autre roi peut aller à cette pos
                {
                    enemi.AddRange(Chess._Pieces[i].GetMoves().casesPossibles);
                }
            }
            for (int n = 0; n < poss.Count; n++)
            {
                for (int i = 0; i < enemi.Count; i++)
                {
                    if (poss[n] == enemi[i])
                    {
                        poss.RemoveAt(n);
                        if (n > 0)
                        {
                            n--;
                        }
                    }
                }
            }
            poss = FiltrerMoves(p, poss);
            return new(poss);
        }

        static List<int> FiltrerMoves(Pieces p, List<int> poss) // retourne toutes les cases de mes moves possibles qui ne sont pas occupées par mes autres pions
        {
            for (int i = 0; i < Chess._Pieces.Count; i++)
            {
                if (Chess._Pieces[i].player == p.player)
                {
                    for (int j = 0; j < poss.Count; j++)
                    {
                        if (poss[j] < 0 || poss[j] > 63 || Chess._Pieces[i].pos == poss[j])
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

        public Pieces(pieces n, int p, int player) // player 0 = white, player 1 = black
        {
            nom = n;
            pos = p;
            this.player = player;
            if (player == 0)
            {
                couleur = (Color)Colors.Black;
            }
            else
            {
                couleur = (Color)Colors.White;
            }
            truePos.x = Chess.board.grille[pos].pos.x;
            truePos.y = Chess.board.grille[pos].pos.y;
            string arg = "WHITE";
            if (player == 0)
            {
                arg = "BLACK";
            }
            image = "Images/" + nom.ToString() + "_" + arg + ".png";
        }

        public void Draw()
        {
            Vector2D<int> posF = Chess.board.grille[pos].pos;
            truePos = Program.Lerp(posF, truePos, 0.9F);
            Vector2D<int> a = new((int)truePos.x, (int)truePos.y);
            Program.DrawImage(image, new(a.x + 5, a.y + 5), new(Chess.board.tileSize.x - 10, Chess.board.tileSize.y - 10));
        }

        public bool ClickSurPiece()
        {
            if (player == Chess.tour)
            {
                if (Program.MouseLeftPressed())
                {
                    if (Program.PointInRect(Program.MousePosition(), new(Chess.board.grille[pos].pos, Chess.board.tileSize)))
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
                    Program.DrawFullCircle(new(Chess.board.grille[casesPossibles[i]].pos + (Chess.board.tileSize) / 2, Chess.board.tileSize.x / 2 - 5));
                }
            }
        }

        public void Apply(Pieces p, int pos)
        {
            p.pos = pos;
            p.firstMove = false;
            for (int i = 0; i < Chess._Pieces.Count; i++)
            {
                if (Chess._Pieces[i].player != Chess.tour)
                {
                    if (Chess._Pieces[i].pos == pos)
                    {
                        if (Chess._Pieces[i].nom == pieces.KING)
                        {
                            Chess.ChangeTurn();
                            Chess.gameFinished = true;
                        }
                        Chess._Pieces.RemoveAt(i);
                    }
                }
            }
            Chess.ChangeTurn();
        }

        public int isClicked()
        {
            for (int i = 0; i < casesPossibles.Count; i++)
            {
                if (Program.MouseLeftPressed())
                {
                    if (Program.PointInRect(Program.MousePosition(), new(Chess.board.grille[casesPossibles[i]].pos, Chess.board.tileSize)))
                    {
                        return casesPossibles[i];
                    }
                }
            }
            return -1;
        }
    }
}
