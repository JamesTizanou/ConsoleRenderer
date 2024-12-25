using Classes;
using Main;

namespace Ludo
{
    internal class Pion
    {
        public int numero;
        public int spawn;
        public int caseActuelle;
        public bool outOfHome = false;
        unsafe public Player* joueur;
        public Vector2D<int> pos = new(0, 0);

        unsafe Player* GetPlayerByName(string nomDuJoueur)
        {
            if (nomDuJoueur == "rouge")
            {
                fixed (Player* j = &Player.rouge) { return j; };
            }
            else if (nomDuJoueur == "bleu")
            {
                fixed (Player* j = &Player.bleu) { return j; };
            }
            else if (nomDuJoueur == "jaune")
            {
                fixed (Player* j = &Player.jaune) { return j; };
            }
            else if (nomDuJoueur == "vert")
            {
                fixed (Player* j = &Player.vert) { return j; };
            }
            throw new Exception("No player found");
        }

        public unsafe Pion(int n, int s, string nomDuJoueur)
        {
            numero = n;
            spawn = s;
            joueur = GetPlayerByName(nomDuJoueur);
            caseActuelle = s;
            pos.x = Ludo.board.grille[s].pos.x;
            pos.y = Ludo.board.grille[s].pos.y;
        }

        public void ShowNumber()
        {
            Color.Pencil(Colors.White);
        }

        public unsafe void Display()
        {
            pos = Ludo.board.grille[caseActuelle].pos;
            if (outOfHome)
            {
                pos = Ludo.board.grille[joueur->chemin_p[caseActuelle]].pos;
            }
            Color col = Color.GetPencil();
            Color.Pencil(joueur->couleur);
            Circle circ = new Circle(new(pos.x + Ludo.board.tileSize.x / 2, pos.y + Ludo.board.tileSize.x / 2), (Ludo.board.tileSize.x - 10) / 2);
            Program.DrawFullCircle(circ);
            Color.Pencil(Colors.Black);
            Program.DrawCircle(circ);
            Color.Pencil(col);
        }

        public unsafe bool isFinishing()
        {
            List<int> finishTiles = joueur->chemin_p.GetRange(joueur->chemin_p.Count - 6, 6);
            return finishTiles.Contains(caseActuelle);
        }

        public unsafe void Move()
        {
            if (outOfHome)
            {
                if (caseActuelle + De.val < joueur->chemin_p.Last())
                {
                    caseActuelle += De.val;
                }
                else if (caseActuelle + De.val == joueur->chemin_p.Last())
                {
                    Ludo.Winner = joueur->couleur;
                }
            }
        }
    }
}
