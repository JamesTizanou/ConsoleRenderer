using Classes;

namespace Ludo
{
    internal class Player
    {
        public bool IsPlaying = false;
        public int[] tokenSpawns;
        public string name;
        public Colors couleur;
        public int spawnTile;
        public int pionsEnMaison = 4;
        public bool pionJoue = false;
        public List<int> chemin_p;

        public Pion token1;
        public Pion token2;
        public Pion token3;
        public Pion token4;

        public Player(Colors coul, string n, int[] spawns, int st)
        {
            couleur = coul;
            name = n;
            tokenSpawns = spawns;
            spawnTile = st;
            chemin_p = Util.RearangerVecteur(spawnTile, Ludo.chemin);
            chemin_p.Remove(chemin_p.Last());
            chemin_p.AddRange(FinChemin(coul));
            token1 = new Pion(1, tokenSpawns[0], name);
            token2 = new Pion(2, tokenSpawns[1], name);
            token3 = new Pion(3, tokenSpawns[2], name);
            token4 = new Pion(4, tokenSpawns[3], name);
        }

        private IEnumerable<int> FinChemin(Colors player)
        {
            List<int> casesFinales = new List<int>();
            switch (player)
            {
                case Colors.Red:
                    return new int[] { 106, 107, 108, 109, 110, 111 };
                case Colors.Yellow:
                    return new int[] { 22, 37, 52, 67, 82, 97 };
                case Colors.Blue:
                    return new int[] { 202, 187, 172, 157, 142, 127 };
                case Colors.Green:
                    return new int[] { 118, 117, 116, 115, 114, 113 };
                default:
                    throw new Exception("Couleur inexistante");
            }
        }

        

        public Player(Player x)
        {
            couleur = x.couleur;
            name = x.name;
            tokenSpawns = x.tokenSpawns;
            spawnTile = x.spawnTile;
            chemin_p = Util.RearangerVecteur(x.spawnTile, Ludo.chemin);
            token1 = new Pion(1, x.tokenSpawns[0], x.name);
            token2 = new Pion(2, x.tokenSpawns[1], x.name);
            token3 = new Pion(3, x.tokenSpawns[2], x.name);
            token4 = new Pion(4, x.tokenSpawns[3], x.name);
        }

        public void DisplayTokens()
        {
            Color col = Color.GetPencil();
            Color.Pencil(couleur);
            token1.Display();
            token2.Display();
            token3.Display();
            token4.Display();
            Color.Pencil(col);
        }

        // si le joueur a 3 pions en maison, cette fonction retourne le pion qui est sorti
        public int PionsSortis()
        {
            int nb = 0;
            if (token1.outOfHome)
            {
                nb++;
            }
            if (token2.outOfHome)
            {
                nb++;
            }
            if (token3.outOfHome)
            {
                nb++;
            }
            if (token4.outOfHome)
            {
                nb++; 
            }
            return nb;
        }

        public Pion PionClique(Vector2D<int> pos)
        {
            if (pos == Ludo.board.grille[token1.caseActuelle].pos)
            {
                return token1;
            }
            else if (pos == Ludo.board.grille[token2.caseActuelle].pos)
            {
                return token2;
            }
            else if (pos == Ludo.board.grille[token3.caseActuelle].pos)
            {
                return token3; 
            }
            else if (pos == Ludo.board.grille[token4.caseActuelle].pos)
            {
                return token4;
            }
            else if ( token1.caseActuelle < 52 && pos == Ludo.board.grille[chemin_p[token1.caseActuelle]].pos)
            {
                return token1;
            }
            else if ( token2.caseActuelle < 52 && pos == Ludo.board.grille[chemin_p[token2.caseActuelle]].pos)
            {
                return token2;
            }
            else if ( token3.caseActuelle < 52 && pos == Ludo.board.grille[chemin_p[token3.caseActuelle]].pos)
            {
                return token3;
            }
            else if ( token4.caseActuelle < 52 && pos == Ludo.board.grille[chemin_p[token4.caseActuelle]].pos)
            {
                return token4;
            }
            throw new Exception($"Aucun pion trouvé pour {Actual().name}"); 
        }

        public static Player rouge = new Player(Colors.Red, "rouge", new int[] { 16, 19, 61, 64 }, 91);
        public static Player bleu = new Player(Colors.Blue, "bleu", new int[] { 151, 154, 196, 199 }, 201);
        public static Player jaune = new Player(Colors.Yellow, "jaune", new int[] { 25, 28, 70, 73 }, 23);
        public static Player vert = new Player(Colors.Green, "vert", new int[] { 160, 163, 205, 208 }, 133);

        public static Player Actual()
        {
            if (rouge.IsPlaying)
            {
                return rouge;
            }
            if (bleu.IsPlaying)
            {
                return bleu;
            }
            if (vert.IsPlaying)
            {
                return vert;
            }
            if (jaune.IsPlaying)
            {
                return jaune;
            }
            throw new Exception("Joueur non trouvé");
        }

        public static Player Previous()
        {
            if (rouge.IsPlaying)
            {
                return jaune;
            }
            if (bleu.IsPlaying)
            {
                return rouge;
            }
            if (vert.IsPlaying)
            {
                return bleu;
            }
            if (jaune.IsPlaying)
            {
                return vert;
            }
            throw new Exception("Joueur non trouvé");
        }

        public static Player Next()
        {
            if (rouge.IsPlaying)
            {
                return bleu;
            }
            if (bleu.IsPlaying)
            {
                return vert;
            }
            if (vert.IsPlaying)
            {
                return jaune;
            }
            if (jaune.IsPlaying)
            {
                return rouge;
            }
            throw new Exception("Joueur non trouvé");
        }
    }
}
