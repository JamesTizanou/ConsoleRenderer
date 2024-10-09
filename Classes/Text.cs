using SDL2;
using Main;
using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace Classes
{
    class Text
    {
        IntPtr font;
        IntPtr textTexture;
        string text;
        IntPtr textSurface;
        Vector2D<int> pos;
        string fontPath;
        int size;
        SDL_Color col;

        public Text(string text, Vector2D<int> pos, int size = 30, string fontPath = "Fonts/Minecraft.ttf")
        {
            fontPath = "../../../" + fontPath;
            Color coul = Color.GetPencil();
            SDL_Color c = new SDL_Color();
            c.r = coul.r;
            c.g = coul.g;
            c.b = coul.b;
            c.a = coul.a;
            col = c;
            this.text = text;
            this.pos = pos;
            this.fontPath = fontPath;
            this.size = size;
            font = TTF_OpenFont(this.fontPath, this.size);
            if (font == IntPtr.Zero)
            {
                Console.WriteLine(SDL_GetError());
            }
            textSurface = TTF_RenderText_Solid(font, text, col);
            textTexture = SDL_CreateTextureFromSurface(Program.renderer, textSurface);
        }

        public void Draw()
        {
            SDL_Rect dest = new SDL_Rect();
            dest.x = pos.x;
            dest.y = pos.y;
            dest.w = 100;
            dest.h = 100;
            uint f = 0;
            int g = 0;

            SDL_Rect src = new SDL_Rect();
            src.x = 0;
            src.y = 0;
            src.w = 100; // textSurface.w;
            src.h = 100;  // textSurface.h;

            SDL_QueryTexture(textTexture, out f, out g, out src.w, out src.h);
            SDL_QueryTexture(textTexture, out f, out g, out dest.w, out dest.h);

            SDL_RenderCopy(Program.renderer, textTexture, ref src, ref dest);
            SDL_DestroyTexture(textTexture);
        }

        public void ChangeText(string newText)
        {
            text = newText;
            font = SDL_ttf.TTF_OpenFont(fontPath, size);
            if (font == IntPtr.Zero)
            {
                Console.WriteLine(SDL_GetError());
            }
            textSurface = SDL_ttf.TTF_RenderText_Solid(font, text, col);
            textTexture = SDL_CreateTextureFromSurface(Program.renderer, textSurface);
        }
    }
}
