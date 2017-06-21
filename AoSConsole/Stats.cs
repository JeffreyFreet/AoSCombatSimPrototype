using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoSConsole
{
    //Container for storing end results of a single battle.
    class Stats
    {
        //Stores turn-by-turn unit sizes.
        //NOTE number in each index is the number of models at the end of a turn, except for index 0 which is starting unit sizes.
        private List<int> SideAModels;
        private List<int> SideBModels;

        public Stats()
        {
            SideAModels = new List<int>();
            SideBModels = new List<int>();
        }

        public void AddTurn(int sidea, int sideb)
        {
            SideAModels.Add(sidea);
            SideBModels.Add(sideb);
        }

        public int NumberOfTurns()
        {
            return SideAModels.Count - 1;
        }

        //True for SideA, False for SideB.
        public bool Winner()
        {
            return SideAModels[SideAModels.Count - 1] != 0;
        }

        public int SurvivingModels(bool side)
        {
            //Debug.WriteLine("SideA has " + SideAModels.Last());
            //Debug.WriteLine("SideB has " + SideBModels.Last());
            if (side)
            {
                return SideAModels.Last();
            }
            return SideBModels.Last();
        }
    }
}
