using Chess;
using SDL2;
using System.Globalization;
using System.Runtime.InteropServices;
using static SDL2.SDL;

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
    #region classes

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


        public Vector2D(Numeric? x, Numeric? y)
        {
            this.x = x;
            this.y = y;
        }
    }

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

    class SoundManager
    {

    }

    class Sound
    {
        public string? file;
        IntPtr sonData;

        public Sound(string file)
        {
            file = "../../../Sounds/" + file;
            //pointeur vers le son loadé (idealement tu le garde dans une liste pour eviter de le loader a chaque frame lol)
            this.file = file;
            if (file[file.Length - 1] == 'v')
            {
                sonData = SDL_mixer.Mix_LoadWAV(file);
            }
            else
            {
                sonData = SDL_mixer.Mix_LoadMUS(file);
            }
            //sonData = SDL_mixer.Mix_LoadWAV(file);
            Console.WriteLine(SDL_mixer.Mix_GetError());
        }

        ~Sound()
        {
            SDL_mixer.Mix_FreeChunk(sonData);
        }

        public void Play()
        {
            if (file[file.Length - 1] == 'v')
            {
                SDL_mixer.Mix_PlayChannel(-1, sonData, 1);
            }
            else
            {
                SDL_mixer.Mix_PlayMusic(sonData, -1);
            }

        }

        public void Stop()
        {
            if (file[file.Length - 1] == 'v')
            {
                SDL_mixer.Mix_HaltChannel(-1);
            }
            else
            {
                SDL_mixer.Mix_HaltMusic();
            }
        }
    }

    /*class Text
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
            font = SDL_ttf.TTF_OpenFont(this.fontPath, this.size);
            if (font == IntPtr.Zero)
            {
                Console.WriteLine(SDL_GetError());
            }
            textSurface = SDL_ttf.TTF_RenderText_Solid(font, text, col);
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
    }*/

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

        public int GetCoordinates(int x, int y)
        {
            return y * squaresPerColumn + x;
        }
    }

    class Menu
    {
        public List<MenuItem> Actions { get; set; } = new();
        string? Nom { get; set; }
        public Menu(string nom, List<MenuItem> m)
        {
            // Actions.
        }
        public void Show()
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Display();
            }
        }
    }

    /*public enum MenuItemBox
    {
        Circle,
        Square,
        Rectangle
    }*/

    class MenuItem
    {
        public string Name { get; set; }

        public Rect box;

        bool isInvoked = false;

        Action action; // revenir sur ceci

        public MenuItem(string name, Rect b, Action action)
        {
            Name = name;
            box = b;
            this.action = action;
        }

        public void Display(Colors color = Colors.White, bool hover = true, Colors hoverColor = Colors.Lime, Colors hoverTextColor = Colors.Black)
        {
            if (!isInvoked)
            {
                if (Program.PointInRect(Program.MousePosition(), box) && hover)
                {
                    Color.Pencil(hoverColor);
                    Program.DrawFullRect(box);
                    Color.Pencil(hoverTextColor);
                    Program.DrawText(Name, new(box.pos.x + 10, box.pos.y + 10));
                }
                else
                {
                    Color.Pencil(color);
                    Program.DrawRect(box);
                    Program.DrawText(Name, new(box.pos.x + 10, box.pos.y + 10));
                }
                if (Program.MouseLeftPressed())
                {
                    isInvoked = true;
                }
                return;
            }
            action.Invoke();
        }
    }

    class Time
    {
        public uint Temps { get; set; }
        public TempsFormat Format { init; get; }

        public Time(uint temps, TempsFormat format = TempsFormat.MilliSecondes)
        {
            Temps = temps;
            if (format == TempsFormat.MilliSecondes || format == TempsFormat.Secondes || format == TempsFormat.Minutes || format == TempsFormat.Heures)
            {
                Format = format;
            }
        }

        // Retourne le temps écoulé depuis le début de l'exécution du programme dans le format demandé
        public static uint GetTime(TempsFormat format = TempsFormat.Secondes)
        {
            switch(format)
            {
                case TempsFormat.MilliSecondes:
                    return SDL_GetTicks();
                case TempsFormat.Secondes:
                    return SDL_GetTicks() / 1000;
                case TempsFormat.Minutes:
                    return SDL_GetTicks() / 60000;
                case TempsFormat.Heures:
                    return SDL_GetTicks() / (60000 * 60);
            }
            throw new NotImplementedException("Le format ne peut être que des millisecondes, des secondes, des minutes ou des heures");
        }
    }

    public enum TempsFormat
    {
        MilliSecondes, // A
        Secondes, // S
        Minutes, // M
        Heures, // H
        SA, // secondes et millisecondes
        MS, // minutes et secondes
        MSA, // Minutes, secondes et millisecondes
        HM, // Heures et minutes
        HS, // Heures et secondes
        HMS, // Heures et minutes et secondes
        HMSA, // Heures et minutes et secondes et millisecondes
    }

    class Minuteur
    {
        public Time Debut;
        public Time Duree;

        public Minuteur(uint duree)
        {
            Debut = new Time(SDL_GetTicks());
            Duree = new Time(duree);
        }

        public bool isFinished()
        {
            return Duree.Temps + Debut.Temps > SDL_GetTicks();
        }

        public uint TimeLeft()
        {
            if (SDL_GetTicks() < Duree.Temps)
            {
                return Duree.Temps - (SDL_GetTicks() - Debut.Temps);
            }
            Console.WriteLine($"Le minuteur de {Duree.Temps} {Duree.Format.ToString().ToLower()} est terminé");
            return 0;
        }

        public uint TimeNow()
        {
            if (Debut.Temps + (SDL_GetTicks() - Debut.Temps) < Duree.Temps)
            {
                return SDL_GetTicks() - Debut.Temps;
            }
            Console.WriteLine($"Le minuteur actuel est terminé");
            return 0;
        }

        public void Ecrire(Vector2D<int> pos,string format = "mm:ss")
        {
            string texte = "";
            DateTime tempsRestant = new DateTime(TimeLeft());
            texte = tempsRestant.ToString(format,CultureInfo.InvariantCulture); 
            /*switch (format)
            {
                case TempsFormat.MilliSecondes:
                    texte = $"{tempsRestant}";
                    break;
                case TempsFormat.Secondes:
                    texte = $"{tempsRestant / 1000}";
                    break;
                case TempsFormat.Minutes:
                    texte = $"{tempsRestant / 60000}";
                    break;
                case TempsFormat.Heures:
                    texte = $"{tempsRestant / 3600000}";
                    break;
                case TempsFormat.SA:
                    texte = $"{tempsRestant / 1000}:{tempsRestant - (tempsRestant / 1000) * 1000}";
                    break;
                case TempsFormat.MSA:
                    //texte = $"{tempsRestant / 60000}:{tempsRestant % 1000}:{tempsRestant - (tempsRestant / 1000) * 1000}";
                    break;
                    Program.DrawText(texte,pos);
            }*/
        }
    }


    #endregion

    #region couleurs
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
    #endregion

    abstract class Program
    {
        #region variables globales et main
        public static IntPtr window;
        public static IntPtr renderer;
        public static bool running = true;
        static byte[] old_key_state;
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
            return ((p.x >= r.pos.x) &&
                    (p.x < (r.pos.x + r.size.x)) &&
                    (p.y >= r.pos.y) &&
                    (p.y < (r.pos.y + r.size.y)));
        }

        public static void DrawFullRect(Rect rect)
        {
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
            for (int i = 0; i < 360; i++)
            {
                Vector2D<int> vect = new Vector2D<int>((int)(Math.Cos(i) * cercle.rayon) + cercle.pos.x, (int)(Math.Sin(i) * cercle.rayon) + cercle.pos.y);
                DrawPix(vect);
            }
        }

        public static void DrawFullCircle(Circle cercle)
        {
            Rect delim = new Rect(new Vector2D<int>(cercle.pos.x - cercle.rayon, cercle.pos.y - cercle.rayon),
                                  new Vector2D<int>(cercle.rayon * 2, cercle.rayon * 2));

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
        //static Menu
        static MenuItem c = new("Chess", new Rect(100, 100, 100, 100), Chess_.Chess);
        static void Render()
        {
            Color.Pencil(Colors.Black);
            // Clears the current render surface.
            Clear();

            // update the key state at every frame a la fin

            c.Display();

            //Ludo_.Ludo();

            //Chess_.Chess();

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
