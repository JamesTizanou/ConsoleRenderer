using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace Classes
{
    class Rect
    {
        public Vector2D<int>? pos = new Vector2D<int>(0, 0);
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
    }

    class Circle
    {
        public Vector2D<int>? pos;
        public int rayon;

        public Circle(Vector2D<int> p, int r)
        {
            pos = p;
            rayon = r;
        }
    }
}
