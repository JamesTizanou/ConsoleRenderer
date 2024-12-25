using Main;

namespace Classes
{
    class Tile
    {
        public int numero;
        public Vector2D<int> pos;
        public Vector2D<int> size;
        public Colors color;
        public Tile(Vector2D<int> pos, Vector2D<int> size, int numero, Colors col = Colors.Yellow)
        {
            this.size = size;
            this.pos = pos;
            this.numero = numero;
            color = col;
        }

        public void Light()
        {
            Color act = Color.GetPencil();
            Color.Pencil(color);
            Program.DrawFullRect(new Rect(pos.x, pos.y, size.x, size.y));
            Color.Pencil(Colors.Black);
            Program.DrawRect(new Rect(pos.x, pos.y, size.x, size.y));
            Color.Pencil(act);
        }
    }

    class Grid
    {
        public Vector2D<int> pos;
        public int squaresPerColumn;
        public int squaresPerRow;
        public Vector2D<int> tileSize;
        public List<Tile> grille = new List<Tile>();

        public Grid(Vector2D<int> pos, Vector2D<int> tailleDUnTile, int spc, int spr, Colors col = Colors.Yellow)
        {
            squaresPerColumn = spc;
            squaresPerRow = spr;
            tileSize = tailleDUnTile;
            this.pos = pos;
            int i = 0;
            int x = pos.x;
            int y = pos.y;
            while (grille.Count < squaresPerColumn * squaresPerRow)
            {
                if (i % squaresPerColumn == 0 && i != 0)
                {
                    x = pos.x;
                    i = 0;
                    y += tailleDUnTile.y;
                }
                Vector2D<int> p = new Vector2D<int>(x + tailleDUnTile.x * i, y);
                grille.Add(new Tile(p, tailleDUnTile, grille.Count, col));
                i++;
            }
        }

        public void Display(bool drawContour = false)
        {
            for (int i = 0; i < grille.Count; i++)
            {
                grille[i].Light();
                //Program.DrawText(i.ToString(), grille[i].pos);  // To show the number of the tile
            }
            
            if (drawContour)
            {
                Color.Pencil(Colors.White);
                Program.DrawRect(new(pos, new(tileSize.x * squaresPerColumn, tileSize.y * squaresPerRow)));
            }
        }

        public void Personalize(int[] indx, Colors coul)
        {
            for (int i = 0; i < indx.Length; i++)
            {
                grille[indx[i]].color = coul;
            }
        }

        public void Personalize(List<int> indx, Colors coul)
        {
            for (int i = 0; i < indx.Count; i++)
            {
                grille[indx[i]].color = coul;
            }
        }

        public int GetRangee(int nb)
        {
            return nb / squaresPerColumn;
        }

        public int GetColonne(int nb)
        {
            return nb % squaresPerColumn;
        }

        public Vector2D<int> GetCoordinates(int posEncase)
        {
            return new(GetRangee(posEncase), GetColonne(posEncase));
        }
    }
}
