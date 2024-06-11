using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;
using Main;

namespace Ludo
{
    internal class Util
    {
        public static int at(int index)
        {
            int a = index % Ludo_.chemin.Length;
            return Ludo_.chemin[a];
        }

        public static int IndexOf(int el, List<int> vect)
        {
            for (int i = 0; i < vect.Count; i++)
            {
                if (el == vect[i])
                {
                    return i;
                }
            }
            throw new Exception("Element not found");
        }

        public static int IndexOf(int el, int[] vect)
        {
            for (int i = 0; i < vect.Length; i++)
            {
                if (el == vect[i])
                {
                    return i;
                }
            }
            throw new Exception("Element not found");
        }
        public static List<int> RearangerVecteur(int nombre, List<int> vect)
        {
            List<int> rep = new List<int>();
            int ind = IndexOf(nombre, vect);
            for (int i = ind; i < vect.Count; i++)
            {
                rep.Add(vect[i]);
            }
            for (int i = 0; i < ind; i++)
            {
                rep.Add(vect[i]);
            }
            return rep;
        }

        public static List<int> RearangerVecteur(int nombre, int[] vect)
        {
            List<int> rep = new List<int>();
            int ind = IndexOf(nombre, vect);
            for (int i = ind; i < vect.Length; i++)
            {
                rep.Add(vect[i]);
            }
            for (int i = 0; i < ind; i++)
            {
                rep.Add(vect[i]);
            }
            return rep;
        }

        public static int TrouverRangee(int index)
        {
            return index / 15;
        }

        public static int TrouverColonne(int index)
        {
            return index % 15;
        }
    }
}
