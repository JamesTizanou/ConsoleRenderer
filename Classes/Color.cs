using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace Classes
{
    public enum Colors
    {
        White,
        Red,
        Green,
        Blue,
        Black,
        Yellow,
        Orange,
        Cyan,
        Purple,
        Pink,
        SkyBlue,
        Lime
    }
    class Color
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
        public static explicit operator Color(SDL_Color coul)
        {
            return new Color(coul.r, coul.g, coul.b, coul.a);
        }

        public static explicit operator Color(Colors coul)
        {
            if (Colors.Red == coul)
            {
                return new Color(255, 0, 0, 255);
            }
            else if (Colors.Blue == coul)
            {
                return new Color(0, 0, 255, 255);
            }
            else if (Colors.Green == coul)
            {
                return new Color(0, 255, 0, 255);
            }
            else if (Colors.Yellow == coul)
            {
                return new Color(255, 255, 0, 255);
            }
            else if (Colors.White == coul)
            {
                return new Color(255, 255, 255, 255);
            }
            else if (Colors.Black == coul)
            {
                return new Color(0, 0, 0, 255);
            }
            else if (Colors.Orange == coul)
            {
                return new Color(255, 165, 0, 255);
            }
            else if (Colors.Pink == coul)
            {
                return new Color(255, 192, 203, 255);
            }
            else if (Colors.Purple == coul)
            {
                return new Color(160, 32, 240, 255);
            }
            else if (Colors.Cyan == coul)
            {
                return new Color(0, 255, 255, 255);
            }
            else if (Colors.SkyBlue == coul)
            {
                return new Color(135, 206, 235, 255);
            }
            else if (Colors.Lime == coul)
            {
                return new(191, 255, 0);
            }
            throw new Exception("Couleur introuvale");
        }

        public static Color GetPencil()
        {
            byte r;
            byte g;
            byte b;
            byte a;
            SDL_GetRenderDrawColor(Program.renderer, out r, out g, out b, out a);
            return new Color(r, g, b, a);
        }

        public static void Pencil(Color coul)
        {
            SDL_SetRenderDrawColor(Program.renderer, coul.r, coul.g, coul.b, coul.a);
        }

        public static void Pencil(byte r, byte g, byte b, byte a = 255)
        {
            SDL_SetRenderDrawColor(Program.renderer, r, g, b, a);
        }

        public static void Pencil(Colors coul)
        {
            Color c = (Color)coul;
            SDL_SetRenderDrawColor(Program.renderer, c.r, c.g, c.b, c.a);
        }


        public Color(byte _r, byte _g, byte _b, byte _a = 255)
        {
            r = _r;
            g = _g;
            b = _b;
            a = _a;
        }
    }
}
