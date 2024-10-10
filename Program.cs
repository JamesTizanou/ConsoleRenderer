using static Chess.Chess;
using static Ludo.Ludo;
using SDL2;
using System.Runtime.InteropServices;
using static SDL2.SDL;
using Classes;

/*
 * Petite librairie chill développée par James Tizanou à partir du 04/06/2024 (04 juin 2024)
 * Il n'est pas encore possible de détecter si une touche est pressée ou pas. On peut toutefois savoir si elle est maintenue
 * Composantes manquantes:
 * - sound
 * - keypressed
 * - camera
 * -
 */

namespace Main
{
    abstract class Program
    {
        #region variables globales et main
        public static IntPtr window;
        public static IntPtr renderer;
        public static bool running = true;
        static byte[]? old_key_state;
        static uint old_m_state;

        static void Main()
        {
            SDL_LogSetAllPriority(SDL_LogPriority.SDL_LOG_PRIORITY_WARN);
            SDL_LogSetPriority((int)SDL_LogCategory.SDL_LOG_CATEGORY_ERROR, SDL_LogPriority.SDL_LOG_PRIORITY_DEBUG);
            Setup();
            //SDL_RenderSetLogicalSize(renderer, 800, 800);
            while (running)
            {
                PollEvents();
                Render();
            }
            CleanUp();
        }
        #endregion

        #region shape

        public static Vector2D<int> MousePosition()
        {
            Vector2D<int> mpos = new Vector2D<int>(0, 0);
            int x;
            int y;
            SDL_GetMouseState(out x, out y);
            mpos.x = x;
            mpos.y = y;
            Console.WriteLine("Cordonnées: " + mpos.x + "," + mpos.y);
            return mpos;
        }


        public static void DrawPix(Vector2D<int> pos)
        {
            SDL_RenderDrawPoint(renderer, pos.x, pos.y);
            SDL_GetCursor();
        }

        public static void DrawLine(Vector2D<int> beg, Vector2D<int> end)
        {
            SDL_RenderDrawLine(renderer, beg.x, beg.y, end.x, end.y);
        }

        public static void DrawRect(Rect rect)
        {
            if (rect.pos == null || rect.size == null) return;
            var rectangle = new SDL_Rect
            {
                x = rect.pos.x,
                y = rect.pos.y,
                w = rect.size.x,
                h = rect.size.y
            };
            SDL_RenderDrawRect(renderer, ref rectangle);
        }

        public static bool PointInRect(Vector2D<int> p, Rect r)
        {
            if (r.pos == null || r.size == null) return false;
            return ((p.x >= r.pos.x) &&
                    (p.x < (r.pos.x + r.size.x)) &&
                    (p.y >= r.pos.y) &&
                    (p.y < (r.pos.y + r.size.y)));
        }

        public static bool PointInCircle(Vector2D<int> p, Circle r)
        {
            if (r.pos == null) return false;
            return Math.Sqrt(Math.Pow(r.pos.x - p.x,2) + Math.Pow(r.pos.y - p.y, 2)) <= r.rayon;
        }

        public static void DrawFullRect(Rect rect)
        {
            if (rect.pos == null || rect.size == null) return;
            var rectangle = new SDL_Rect
            {
                x = rect.pos.x,
                y = rect.pos.y,
                w = rect.size.x,
                h = rect.size.y
            };
            SDL_RenderFillRect(renderer, ref rectangle);
        }

        public static void DrawCircle(Circle cercle)
        {
            if (cercle.pos == null) return;
            for (int i = 0; i < 360; i++)
            {
                Vector2D<int> vect = new Vector2D<int>((int)(Math.Cos(i) * cercle.rayon) + cercle.pos.x, (int)(Math.Sin(i) * cercle.rayon) + cercle.pos.y);
                DrawPix(vect);
            }
        }

        public static void DrawFullCircle(Circle cercle)
        {
            if (cercle == null || cercle.pos == null) return;
            Rect delim = new Rect(new Vector2D<int>(cercle.pos.x - cercle.rayon, cercle.pos.y - cercle.rayon),
                                  new Vector2D<int>(cercle.rayon * 2, cercle.rayon * 2));
            if (delim.pos == null || delim.size == null) return;
            for (int n = delim.pos.y; n <= delim.pos.y + delim.size.y; n++)
            {
                for (int i = delim.pos.x; i <= delim.pos.x + delim.size.x; i++)
                {
                    Vector2D<int> point = new Vector2D<int>(i, n);
                    if (Math.Pow(point.x - cercle.pos.x, 2) + Math.Pow(point.y - cercle.pos.y, 2) < Math.Pow(cercle.rayon, 2))
                    {
                        DrawPix(point);
                    }
                }
            }
        }

        static float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        public static Vector2D<float> Lerp(Vector2D<int> finalPos, Vector2D<float> actualPos, float by)
        {
            Lerp(finalPos.x, actualPos.x, by);
            Lerp(finalPos.y, actualPos.y, by);
            return new Vector2D<float>(Lerp(finalPos.x, actualPos.x, by), Lerp(finalPos.y, actualPos.y, by));
        }

        static double Rise(Vector2D<int> p1, Vector2D<int> p2)
        {
            return ((double)p1.y - p2.y) / ((double)p1.x - p2.x);
        }

        static void DrawFlatTriangle(Vector2D<int> p1, int flat_y, int x1, int x2)
        {
            int miny = Math.Min(p1.y, flat_y);
            int maxy = Math.Max(p1.y, flat_y);

            for (int y = miny; y < maxy; y++)
            {
                double xx1 = 0;
                double xx2 = 0;

                if (p1.x == x1)
                {
                    xx1 = x1;

                    double a2 = Rise(p1, new Vector2D<int>(x2, flat_y));
                    double b2 = p1.y - a2 * p1.x;
                    xx2 = (y - b2) / a2;

                }
                else if (p1.x == x2)
                {
                    double a1 = Rise(p1, new Vector2D<int>(x1, flat_y));
                    double b1 = p1.y - a1 * p1.x;

                    xx1 = (y - b1) / a1;

                    xx2 = x2;
                }
                else
                {
                    double a1 = Rise(p1, new Vector2D<int>(x1, flat_y));
                    double a2 = Rise(p1, new Vector2D<int>(x2, flat_y));

                    double b1 = p1.y - a1 * p1.x;
                    double b2 = p1.y - a2 * p1.x;

                    xx1 = (y - b1) / a1;
                    xx2 = (y - b2) / a2;
                }

                double minx = Math.Min(xx1, xx2);
                double maxx = Math.Max(xx1, xx2);

                for (double x = minx; x < maxx; x++)
                {
                    DrawPix(new Vector2D<int>((int)x, y));
                }
            }
        }

        public static void DrawTriangle(Vector2D<int> p1, Vector2D<int> p2, Vector2D<int> p3)
        {
            DrawLine(p1, p2);
            DrawLine(p1, p3);
            DrawLine(p2, p3);
        }

        public static void DrawFullTriangle(Vector2D<int> p1, Vector2D<int> p2, Vector2D<int> p3)
        {
            Vector2D<int> top;
            Vector2D<int> mid = new Vector2D<int>(0, 0);
            Vector2D<int> bot;

            if (p1.y > p2.y) top = p1;
            else top = p2;
            if (p3.y > top.y) top = p3;

            if (p1.y < p2.y) bot = p1;
            else bot = p2;
            if (p3.y < bot.y) bot = p3;

            if (top == p1 && bot == p2) mid = p3;
            if (top == p3 && bot == p2) mid = p1;
            if (top == p1 && bot == p3) mid = p2;

            double flat_x = 0;

            if (top.x == bot.x)
            {
                flat_x = top.x;
            }
            else
            {

                double a = Rise(top, bot);
                double b = top.y - top.x * a;
                flat_x = (mid.y - b) / a;
            }

            DrawFlatTriangle(top, mid.y, mid.x, (int)flat_x);
            DrawFlatTriangle(bot, mid.y, mid.x, (int)flat_x);
        }

        public static void DrawText(string text, Vector2D<int> pos, int size = 30, string fontPath = "../../../Fonts/Makeup.otf")
        {
            Color coul = Color.GetPencil();
            SDL_Color c = new SDL_Color();
            c.r = coul.r;
            c.g = coul.g;
            c.b = coul.b;
            c.a = coul.a;
            IntPtr font = SDL_ttf.TTF_OpenFont(fontPath, size);
            if (font == IntPtr.Zero)
            {
                Console.WriteLine(SDL_GetError());
            }
            IntPtr textSurface = SDL_ttf.TTF_RenderText_Solid(font, text, c);
            IntPtr textTexture = SDL_CreateTextureFromSurface(renderer, textSurface);
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
            src.w = 100;
            src.h = 100;
            SDL_FreeSurface(textSurface);
            SDL_ttf.TTF_CloseFont(font);
            SDL_QueryTexture(textTexture, out f, out g, out src.w, out src.h);
            SDL_QueryTexture(textTexture, out f, out g, out dest.w, out dest.h);
            SDL_RenderCopy(renderer, textTexture, ref src, ref dest);
            SDL_DestroyTexture(textTexture);

        }

        public static void DrawImage(string path, Vector2D<int> pos, Vector2D<int> size)
        {
            path = "../../../" + path;

            if (!File.Exists(path))
            {
                Console.WriteLine($"Le fichier \"{path}\" n'existe pas");
            }

            IntPtr texture = SDL_image.IMG_LoadTexture(renderer, path);

            SDL_Rect dest = new SDL_Rect();
            dest.x = pos.x;
            dest.y = pos.y;
            dest.w = size.x;
            dest.h = size.y;
            SDL_RenderCopy(renderer, texture, IntPtr.Zero, ref dest);
            SDL_DestroyTexture(texture);
        }

        #endregion

        #region Keyboard Handling

        public static bool KeyHeld(SDL_Scancode code)
        {
            IntPtr keyboardState = SDL_GetKeyboardState(out int numkeys);
            byte[] state = new byte[numkeys];
            Marshal.Copy(keyboardState, state, 0, numkeys);
            if (state[(int)code] != 0)
            {
                return true;
            }
            return false;
        }

        public static bool KeyPressed(SDL_Scancode code)
        {
            if (old_key_state == null) return false;
            if (KeyHeld(code) && old_key_state[(int)code] == 0) //pour savoir si le bouton est pressé on regarde si il est tenu et si il l'était pas la frame d'avant avec old key state
            {
                return true;
            }
            return false;
        }

        public static bool MouseLeftHeld()
        {
            var state = SDL_GetMouseState(out int x, out int y);
            return (state & SDL_BUTTON_LMASK) != 0;
        }

        public static bool MouseRightHeld()
        {
            var state = SDL_GetMouseState(out int x, out int y);
            return (state & SDL_BUTTON_RMASK) != 0;
        }

        public static bool MouseLeftPressed()
        {
            bool l_cur = MouseLeftHeld();
            bool l_old = ((old_m_state & SDL_BUTTON_LMASK) != 0);
            return (l_cur && !l_old);
        }

        public static bool MouseRightPressed()
        {
            bool l_cur = MouseRightHeld();
            bool l_old = ((old_m_state & SDL_BUTTON_RMASK) != 0);
            return (l_cur && !l_old);
        }

        public static bool MouseLeftReleased()
        {
            bool l_cur = MouseLeftHeld();
            bool l_old = ((old_m_state & SDL_BUTTON_LMASK) != 0);

            return (!l_cur && l_old);
        }

        public static bool MouseRightReleased()
        {
            bool l_cur = MouseRightHeld();
            bool l_old = ((old_m_state & SDL_BUTTON_LMASK) != 0);

            return (!l_cur && l_old);
        }

        #endregion

        #region render
        static void Clear()
        {
            SDL_RenderClear(renderer);
        }
        #endregion

        #region graphic
        /// <summary>
        /// Renders to the window.
        /// </summary> 
        
        
        static MenuItemBox chessMenu = new("Chess", ChessMain, new Rect(100, 100, 100, 100));
        static MenuItemBox ludoMenu = new("Ludo", LudoMain, new Rect(300, 100, 100, 100));
        static void Render()
        {
            Color.Pencil(Colors.Black);
            // Clears the current render surface.
            Clear();

            // update the key state at every frame a la fin

            //chessMenu.Display();
            //ludoMenu.Display();
            Console.WriteLine("Server or client?: ");
            string rep = Console.ReadLine();
            if (rep == "s")
            {
                Server.ServerMain();
            }
            else if (rep == "c")
            {
                Client.ClientMain();
            }
            

            UpdateKeyInfo();

            // Switches out the currently presented render surface with the one we just did work on.
            SDL_RenderPresent(renderer);
        }
        static void UpdateKeyInfo()
        {
            IntPtr keyboardState = SDL_GetKeyboardState(out int numkeys);
            old_key_state = new byte[numkeys];
            Marshal.Copy(keyboardState, old_key_state, 0, numkeys); //ca ca copie le tableau qui se trouve au pointer keyboardState dans un vrai tableau byte[]
            old_m_state = SDL_GetMouseState(out int i, out int j);
        }
        static void Setup()
        {
            // Initilizes 
            if (SDL_Init(SDL_INIT_VIDEO) < 0 || SDL_Init(SDL_INIT_AUDIO) < 0 || SDL_Init(SDL_INIT_EVERYTHING) < 0)
            {
                Console.WriteLine($"There was an issue initializing  {SDL_GetError()}");
            }

            //Ici c'est ce que j'ai ajouté pour faire marcher ttf et mix

            SDL_ttf.TTF_Init();
            SDL_mixer.Mix_Init(SDL_mixer.MIX_InitFlags.MIX_INIT_MP3);
            SDL_mixer.Mix_Init(SDL_mixer.MIX_InitFlags.MIX_INIT_MP3);

            if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, SDL_mixer.MIX_DEFAULT_CHANNELS, 1024) == -1) //Initialisation de l'API Mixer
                Console.WriteLine("%s", SDL_mixer.Mix_GetError());

            SDL_mixer.Mix_AllocateChannels(8);
            SDL_mixer.Mix_Volume(1, SDL_mixer.MIX_MAX_VOLUME / 2);

            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG);
            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_JPG);

            // Create a new window given a title, size, and passes it a flag indicating it should be shown.
            window = SDL_CreateWindow(
                "SDL .NET 6 Tutorial",
                SDL_WINDOWPOS_UNDEFINED,
                SDL_WINDOWPOS_UNDEFINED,
                800,
                800,
                SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            if (window == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the window. {SDL_GetError()}");
            }

            // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
            renderer = SDL_CreateRenderer(
                window,
                -1,
                SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (renderer == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the renderer. {SDL_GetError()}");
            }

            SDL_SetRenderDrawBlendMode(renderer, SDL_BlendMode.SDL_BLENDMODE_BLEND);
        }
        static void PollEvents()
        {
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                }
            }
        }
        static void CleanUp()
        {
            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);
            SDL_Quit();
        }
        #endregion
    }
}
