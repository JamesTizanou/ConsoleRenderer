using Main;
using static SDL2.SDL;

namespace Classes
{
    abstract class Shape
    {
        public Vector2D<int>? pos = new Vector2D<int>(0, 0);
    }
    class Rect : Shape
    {
        public Vector2D<int>? size = new Vector2D<int>(0, 0);

        public static explicit operator Rect(SDL_Rect rect)
        {
            return new Rect(new Vector2D<int>(rect.x, rect.y), new Vector2D<int>(rect.w, rect.h));
        }

        public Rect(Vector2D<int>? _pos, Vector2D<int>? _size)
        {
            pos = _pos;
            size = _size;
        }

        public Rect(int x, int y, int w, int h)
        {
            pos = new Vector2D<int>(x, y);
            size = new Vector2D<int>(w, h);
        }

        public bool Hover()
        {
            return Program.PointInRect(Program.MousePosition(), this);
        }
    }

    class Circle : Shape
    {
        public int rayon;

        public Circle(Vector2D<int> p, int r)
        {
            pos = p;
            rayon = r;
        }

        /*public Circle(Vector2D<float> p, int r)
        {
            if (pos == null) return;
            pos.x = (int)p.x;
            pos.y = (int)p.y;
            rayon = r;
        }*/
    }
}
