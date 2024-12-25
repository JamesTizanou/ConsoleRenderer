using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;

namespace Ludo
{
    internal abstract class Debugger
    {
        /*internal DebugMode Mode = DebugMode.MENU;
        internal Thread Thread;
        internal enum DebugMode
        {
            MENU,
        }*/
        public static void Show()
        {
            /*Console.WriteLine("What do you want to do?: ");
            Console.WriteLine("A. List all players");*/
            Console.Clear();
            Console.WriteLine($"Token 1: ({Player.Actual().token1.pos.x}) , ({Player.Actual().token1.pos.x})");
            Console.WriteLine($"Token 2: ({Player.Actual().token2.pos.x}) , ({Player.Actual().token2.pos.x})");
            Console.WriteLine($"Token 3: ({Player.Actual().token3.pos.x}) , ({Player.Actual().token3.pos.x})");
            Console.WriteLine($"Token 4: ({Player.Actual().token4.pos.x}) , ({Player.Actual().token4.pos.x})");
            Console.WriteLine($"Cursor: ({Program.MousePosition().x}) , ({Program.MousePosition().y})");
        }

        /*public void ListPlayers()
        {
            Console.WriteLine("What do you want to do?: ");
            Console.WriteLine("R. List all of red's token");
        }*/
    }
}
