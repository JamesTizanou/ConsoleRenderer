using Main;
namespace Classes
{
    class Vector2D<Numeric>
    {
        public Numeric? x ;
        /*{ 
            get
            {
                try
                {
                    //SDL2.SDL.SDL_RenderGetScale(Program.renderer, out float _x, out float _y);
                    int _x = 1; 
                    dynamic? xEnInt = x;
                    return xEnInt * _x;
                } catch (Exception ex)
                {
                    throw new Exception();
                }
            } 
            set
            {
                //SDL2.SDL.SDL_RenderGetScale(Program.renderer, out float _x, out float _y);
                int _x = 1;
                dynamic? xEnInt = x;
                x = xEnInt * _x;
            }
        }*/
        public Numeric? y;
        /*{
            get
            {
                //SDL2.SDL.SDL_RenderGetScale(Program.renderer, out float _x, out float _y);
                int _y = 1;
                dynamic? yEnInt = y;
                return yEnInt * _y;
            }
            set
            {
                //SDL2.SDL.SDL_RenderGetScale(Program.renderer, out float _x, out float _y);
                int _y = 1;
                dynamic? yEnInt = x;
                y = yEnInt * _y;
            }
        }*/

        public static Vector2D<Numeric> operator +(Vector2D<Numeric> a, Vector2D<Numeric> b)
        {
            dynamic? ax = a.x;
            dynamic? bx = b.x;
            dynamic? ay = a.y;
            dynamic? by = b.y;
            return new Vector2D<Numeric>(ax + bx, ay + by);
        }

        public static Vector2D<Numeric> operator -(Vector2D<Numeric> a, Vector2D<Numeric> b)
        {
            dynamic? ax = a.x;
            dynamic? bx = b.x;
            dynamic? ay = a.y;
            dynamic? by = b.y;
            return new Vector2D<Numeric>(ax - bx, ay - by);
        }

        public static Vector2D<Numeric> operator /(Vector2D<Numeric> a, int n)
        {
            dynamic? ax = a.x;
            dynamic? ay = a.y;
            return new Vector2D<Numeric>(ax / n, ay / n);
        }

        public static Vector2D<Numeric> operator *(Vector2D<Numeric> a, int n)
        {
            dynamic? ax = a.x;
            dynamic? ay = a.y;
            return new Vector2D<Numeric>(ax * n, ay * n);
        }

        /*public static implicit operator Vector2D<float>(Vector2D<int> a)
        {
            return new Vector2D<float>(a.x, a.y);
        }

        public static implicit operator Vector2D<int>(Vector2D<float> a)
        {
            return new Vector2D<int>((int)a.x, (int)a.y);
        }*/

        public Vector2D(Numeric? x, Numeric? y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
