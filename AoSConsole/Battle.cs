using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoSConsole
{
    //Battle Class
    //Properties contain the units on both sides, a turn ticker, and other relevant metadata.
    //Methods included for adjusting the sides of the battle, starting the battle, getting battle data per-round, and other relevant output.
    static class Battle
    {
        private static int Turn = 1;
        public static Unit SideA;
        public static Unit SideB;

        //This is used as a *very* rough approximation of model positioning and base contact.
        //In simple terms: this is how many models from either side can fight in melee at any given time.
        //For 2"-range weapons this gets doubled for obvious reasons (PHALANX YEAH BOII)
        private static int Frontage = 8;


        //Order of battle: SideA takes first turn, then SideB, and so on.
        //During its turn to go, a unit generates a pool of wounds from one weapon, chucks it at the enemy (saves etc. calculated) and repeats this with its other weapons.
        //Continues until one (or both!) sides have no more models remaining.

        //Data output methods
        public static void PrintSides()
        {
            Console.Out.WriteLine("\n|<>| The Sides |<>|\n");

            if (SideA != null) Console.Out.WriteLine(SideA.Models.First().Name + " (" + SideA.Models.Count + ")");
            else Console.Out.WriteLine("Side A Empty!");

            if (SideB != null) Console.Out.WriteLine(SideB.Models.First().Name + " (" + SideB.Models.Count + ")");
            else Console.Out.WriteLine("Side B Empty!");
        }

        //Resets relevant numbers
        public static void Reset()
        {
            Turn = 1;
            SideA.Reset();
            SideB.Reset();
        }

        //Check to see if one side has destroyed the other entirely.
        private static bool CheckWin()
        {
            if (SideA.LiveCount() <= 0)
            {
                Console.Out.WriteLine("Side A (" + SideA.Name + ") destroyed!\nSide B (" + SideB.Name + ") is victorious with " + SideB.LiveCount() + " models remaining!");
                return true;
            }
            if (SideB.LiveCount() <= 0)
            {
                Console.Out.WriteLine("Side B (" + SideB.Name + ") destroyed!\nSide B (" + SideA.Name + ") is victorious with " + SideA.LiveCount() + " models remaining!");
                return true;
            }
            return false;
        }

        //Run the battle round-by-round until one side is destroyed.
        public static Stats Play(bool output)
        {
            Stats battleStats = new Stats();
            battleStats.AddTurn(SideA.LiveCount(), SideB.LiveCount());

            if (output)
            {
                Console.WriteLine("\n\n||==|| -------------------- ||==||");
                Console.WriteLine("||==|| Let Battle Commence! ||==||");
                Console.WriteLine("||==|| -------------------- ||==||");
            }

            //Each run of this loop is one round of combat.
            //We assume A takes turn 0, followed by B taking turn 1, etc.
            while (true)
            {
                Debug.WriteLine("Turn: " + Turn);
                Debug.WriteLine(SideA.Name + " has " + SideA.LiveCount() + " models left!");
                Debug.WriteLine(SideB.Name + " has " + SideB.LiveCount() + " models left!");
                if (CheckWin()) break;

                if (output)
                {
                    Console.WriteLine("\n\n|##| Turn " + Turn + " |##|\n");
                    Console.Out.WriteLine(SideA.Name + " has " + SideA.LiveCount() + " models remaining.");
                    Console.Out.WriteLine(SideB.Name + " has " + SideB.LiveCount() + " models remaining.");
                }

                //Thread.Sleep(250);

                //Order of a turn: the sides attack one another, battleshock is then resolved.
                Unit firstTurn = (Turn % 2 == 0) ? SideB : SideA;
                Unit secondTurn = (firstTurn == SideB) ? SideA : SideB;

                //Fight
                if (output) Console.WriteLine("\n||Fight Phase||\n");

                firstTurn.MeleeAttack(secondTurn, Frontage);
                if (CheckWin())
                {
                    battleStats.AddTurn(SideA.LiveCount(), SideB.LiveCount());
                    break;
                }
                
                secondTurn.MeleeAttack(firstTurn, Frontage);
                if (CheckWin())
                {
                    battleStats.AddTurn(SideA.LiveCount(), SideB.LiveCount());
                    break;
                }

                //Battleshock
                if (output) Console.WriteLine("\n\n||Battleshock Phase||\n");
                if (SideA.LostModels > SideB.LostModels) SideA.Battleshock(0);
                if (SideB.LostModels > SideA.LostModels) SideB.Battleshock(0);
                if (output) Console.WriteLine("\n|##| End of Turn " + Turn + " |##|\n");

                //Record stats.
                battleStats.AddTurn(SideA.LiveCount(), SideB.LiveCount());

                Turn++;
                if (output)
                {
                    Console.Out.WriteLine("Press ENTER to begin the next turn!");
                    Console.In.ReadLine();
                    Console.WriteLine("\n----------------------------------------------------------------\n");
                }

                SideA.NewTurn();
                SideB.NewTurn();
            }
            return battleStats;
        }
    }
}
