using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderer._02_Chess
{
    class Chess_
    {
        static int t = 75;
        static Grid board = new Grid(new(100,100),new(t,t), 8, 8, Colors.White);

        static List<int> casesNoires()
        {
            List<int> cn = new();
            for(int i = 0; i < board.grille.Count; i += board.squaresPerColumn)
            {
                if (board.GetRangee(i) % 2 == 0)
                {
                    cn.Add(i);
                    cn.Add(i + 2);
                    cn.Add(i + 4);
                    cn.Add(i + 6);
                }
                else
                {

                }
            }
            return cn;
        }
        public static void Chess()
        {
            board.Display();
            board.Personalize(casesNoires(), Colors.Black);
        }
    }
}
