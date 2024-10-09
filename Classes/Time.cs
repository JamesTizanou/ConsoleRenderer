using Main;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace Classes
{
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
            switch (format)
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

        public void Ecrire(Vector2D<int> pos, string format = "mm:ss")
        {
            string texte = "";
            DateTime tempsRestant = new DateTime(TimeLeft());
            texte = tempsRestant.ToString(format, CultureInfo.InvariantCulture);
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
}
