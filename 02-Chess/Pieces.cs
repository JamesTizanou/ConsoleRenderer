using Main;

namespace Chess
{

    class Pieces
    {
        public int pos;

        public string image;

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

        public Pieces(pieces n, int p)
        {
            nom = n;
            pos = p;
            image = AssocierType();
        }

        string AssocierType()
        {
            if (nom == pieces.PAWN)
            {
                return pawn;
            }
            else if (nom == pieces.BISHOP)
            {
                return bishop;
            }
            else if (nom == pieces.KNIGHT)
            {
                return knight;
            }
            else if (nom == pieces.KING)
            {
                return king;
            }
            else if (nom == pieces.ROOK)
            {
                return rook;
            }
            else if (nom == pieces.QUEEN)
            {
                return queen;
            }
            throw new Exception("Pièce inexistante");
        }

        public void Draw()
        {
            //Program.DrawImage(image, Chess_.board.grille.[pos], Chess_.board.tileSize); // à refaire
        }

        public void ShowMoves()
        {

        }

        public void Move()
        {
            switch (nom)
            {
                case pieces.PAWN:
                    PawnMove();
                    break;
            }
        }

        void PawnMove()
        {

        }
    }
}
