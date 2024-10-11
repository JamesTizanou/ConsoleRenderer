namespace Classes
{
    class Vector2D<Numeric>
    {
        public Numeric? x;
        public Numeric? y;

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
