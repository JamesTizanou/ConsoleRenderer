using Classes;
using Main;

namespace Ludo
{
    abstract class De
    {
        public static int val = Random.Shared.Next(1, 7);
        public static bool obtenu = false;
        public static void Renew()
        {
            obtenu = true;
            val = Random.Shared.Next(1, 7);
        }

    }
    class Ludo
    {
        public static Grid board = new Grid(new Vector2D<int>(50, 50), new Vector2D<int>(45, 45), 15, 15, Colors.White);
        public static int[] chemin = new int[]{ 6,7,8,23,38,53,68, 83,99,100,101,102,103,104,119,134,133,
                                                132,131,130,129,143,158,173,188,203,218,217,216, 201,186,
                                                171,156,141,125,124,123,122,121,120,105,90,91,92,93,94,95,81,66,51,36,21 };
        static bool FirstExec = true;


        public static void LudoMain()
        {
            if (FirstExec)
            {
                Player.rouge.IsPlaying = true;
                FirstExec = false;
            }
            DisplayBoard();
            ObtenirDe();
            JouerSonTour();
            DisplayTokens();
            Console.WriteLine(De.val);
        }

        static void ObtenirDe()
        {
            string txt;
            /*if (De.val == 6)
            {*/
            txt = "Valeur du cube : " + Player.Actual().name + " : " + De.val.ToString();
            /*}
            else
            {
                txt = "Valeur du cube : " + Player.Previous().name + " : " + De.val.ToString();
            }*/
            Color.Pencil(Colors.Orange);
            Program.DrawText(txt, new Vector2D<int>(200, 15), 20);
            if (!De.obtenu)
            {
                Color.Pencil(Player.Actual().couleur);
                Rect rect = new Rect(new Vector2D<int>(500, 5), new Vector2D<int>(30, 30));
                Program.DrawFullRect(rect);
                if (Program.MouseLeftPressed())
                {
                    if (Program.PointInRect(Program.MousePosition(), rect))
                    {
                        De.Renew();
                    }
                }
            }
        }

        static void DisplayTokens()
        {
            Player.rouge.DisplayTokens();
            Player.vert.DisplayTokens();
            Player.bleu.DisplayTokens();
            Player.jaune.DisplayTokens();
        }

        static void DisplayBoard()
        {
            board.Personalize(new int[] { 91, 106, 107, 108, 109, 110 }, Player.rouge.couleur);
            board.Personalize(new int[] { 22, 23, 37, 52, 67, 82 }, Player.jaune.couleur);
            board.Personalize(new int[] { 201, 202, 187, 172, 157, 142 }, Player.bleu.couleur);
            board.Personalize(new int[] { 133, 118, 117, 116, 115, 114 }, Player.vert.couleur);
            for (int i = 0; i < board.grille.Count; i++)
            {
                if (Util.TrouverColonne(i) < 6 && Util.TrouverRangee(i) < 6)
                {
                    board.grille[i].color = Player.rouge.couleur;
                }
                else if (Util.TrouverColonne(i) < 6 && Util.TrouverRangee(i) >= 9)
                {
                    board.grille[i].color = Player.bleu.couleur;
                }
                else if (Util.TrouverColonne(i) >= 9 && Util.TrouverRangee(i) < 6)
                {
                    board.grille[i].color = Player.jaune.couleur;
                }
                else if (Util.TrouverColonne(i) >= 9 && Util.TrouverRangee(i) >= 9)
                {

                    board.grille[i].color = Player.vert.couleur;
                }
            }
            board.Display();
            DrawBoardCenter();
        }

        static void DrawBoardCenter()
        {
            Vector2D<int> centre = new(board.grille[112].pos.x + board.tileSize.x / 2, board.grille[112].pos.y + board.tileSize.y / 2);
            Vector2D<int> topleft = new(board.grille[96].pos.x, board.grille[96].pos.y);
            Vector2D<int> bottomleft = new(board.grille[141].pos.x, board.grille[141].pos.y);
            Vector2D<int> topright = new(board.grille[99].pos.x, board.grille[99].pos.y);
            Vector2D<int> bottomright = new(board.grille[144].pos.x, board.grille[144].pos.y);
            Color.Pencil(Player.rouge.couleur);
            Program.DrawFullTriangle(bottomleft, topleft, centre);
            Color.Pencil(Player.jaune.couleur);
            Program.DrawFullTriangle(topright, topleft, centre);
            Color.Pencil(Player.vert.couleur);
            Program.DrawFullTriangle(bottomright, topright, centre);
            Color.Pencil(Player.bleu.couleur);
            Program.DrawFullTriangle(bottomright, centre, bottomleft);
            Color.Pencil(Colors.Black);
            Program.DrawRect(new Rect(topleft, new Vector2D<int>(bottomright.x - topleft.x, bottomright.y - topleft.y)));
            Program.DrawLine(topleft, bottomright);
            Program.DrawLine(bottomleft, topright);
            Program.DrawLine(topleft, bottomright);
            Program.DrawLine(bottomleft, topright);
        }

        static void SwitchTurns()
        {
            Player.Actual().pionJoue = false;
            De.obtenu = false;

            if (Player.rouge.IsPlaying)
            {
                Player.rouge.IsPlaying = false;
                Player.bleu.IsPlaying = true;
            }
            else if (Player.bleu.IsPlaying)
            {
                Player.bleu.IsPlaying = false;
                Player.vert.IsPlaying = true;
            }
            else if (Player.vert.IsPlaying)
            {
                Player.vert.IsPlaying = false;
                Player.jaune.IsPlaying = true;
            }
            else if (Player.jaune.IsPlaying)
            {
                Player.jaune.IsPlaying = false;
                Player.rouge.IsPlaying = true;
            }

            /*//Player a = new Player(Player.Actual());
            Player.Next().IsPlaying = true;
            //a.IsPlaying = false;
            Player.Previous().IsPlaying = false;*/
        }

        static unsafe void ClickSurCase(Tile zone)
        {
            if (Program.MouseLeftPressed())
            {
                if (Program.PointInRect(Program.MousePosition(), new Rect(zone.pos, board.tileSize)))
                {
                    Pion pi = Player.Actual().PionClique(zone.pos);
                    Player.Actual().pionsEnMaison--;
                    De.obtenu = false;
                    pi.outOfHome = true;
                    pi.caseActuelle = 0;
                    pi.pos = board.grille[pi.joueur->chemin_p[pi.caseActuelle]].pos;
                }
            }
        }

        static void SortirDeMaison()
        {
            Tile token1 = board.grille[Player.Actual().token1.caseActuelle];
            Tile token2 = board.grille[Player.Actual().token2.caseActuelle];
            Tile token3 = board.grille[Player.Actual().token3.caseActuelle];
            Tile token4 = board.grille[Player.Actual().token4.caseActuelle];

            ClickSurCase(token1);
            ClickSurCase(token2);
            ClickSurCase(token3);
            ClickSurCase(token4);
        }

        static void BougerUnPion()
        {
            Vector2D<int>[] p = { Player.Actual().token1.pos, Player.Actual().token2.pos, Player.Actual().token3.pos, Player.Actual().token4.pos };
            for (int i = 0; i < p.Length; i++)
            {
                if (Program.MouseLeftPressed())
                {
                    if (Program.PointInRect(Program.MousePosition(), new Rect(p[i], board.tileSize)))
                    {
                        Pion pi = Player.Actual().PionClique(p[i]);
                        if (pi.outOfHome)
                        {
                            pi.Move();
                            if (De.val != 6)
                            {
                                //SwitchTurns();
                                break;
                            }
                            else
                            {
                                De.obtenu = false;
                            }
                        }
                    }
                }
            }
        }

        static void JouerSonTour()
        {
            if (De.obtenu && !Player.Actual().pionJoue)
            {
                if (Player.Actual().pionsEnMaison < 4)
                {
                    BougerUnPion();
                }
                if (De.val == 6 && Player.Actual().pionsEnMaison > 0)
                {
                    SortirDeMaison();
                }
                if (Player.Actual().pionsEnMaison == 4 && De.val != 6)
                {
                    SwitchTurns();
                }
            }
        }
    }
}
