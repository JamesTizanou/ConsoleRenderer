using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
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
            if (file == null) return;
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
            if (file == null) return;
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
}
