using Main;

namespace Chess
{

    class Pieces
    {
        public Vector2D<int> pos;
        public enum pieces
        {
            PAWN,
            BISHOP,
            KNIGHT,
            ROOK,
            KING,
            QUEEN
        }
        public pieces? nom { get; set; }

        public Pieces(pieces n, Vector2D<int> p)
        {
            nom = n;
            pos = p;
        }

        public void Draw()
        {

        }
    }
}
