using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Centralised die roller class so other classes don't have their own Random() objects.
 * Just a DRY thing here.
 */

namespace AoSConsole

{
    static class Die
    {
        private static Random rand = new Random();

        public static int Roll(int modifier)
        {
            int r = rand.Next(1, 7);
            //Console.Out.WriteLine(r);
            return r;
        }
    }
}
