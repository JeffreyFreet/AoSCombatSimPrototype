using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

/*Age of Sigmar Battle Simulator (Console Prototype)
 Written by GreenyRepublic
 Version 0.0.1 - June 2017
 */

namespace AoSConsole
{

    //Battle Class
    //Properties contain the units on both sides, a turn ticker, and other relevant metadata.
    //Methods included for adjusting the sides of the battle, starting the battle, getting battle data per-round, and other relevant output.
    static class Battle
    {
        private static int Turn = 0;
        public static Unit SideA;
        public static Unit SideB;
        

        //Order of battle: SideA takes first turn, then SideB, and so on.
        //During its turn to go, a unit generates a pool of wounds from one weapon, chucks it at the enemy (saves etc. calculated) and repeats this with its other weapons.
        //Continues until one (or both!) sides have no more models remaining.
        
        //Data output methods
        public static void PrintSides()
        {    
            Console.Out.WriteLine("\n|<>| The Sides |<>|\n");

            if (SideA != null) Console.Out.WriteLine(SideA.Models.First().Name + " (" + SideA.Models.Count + ")");
            else Console.Out.WriteLine("Side A Empty!");

            if (SideB!= null) Console.Out.WriteLine(SideB.Models.First().Name + " (" + SideB.Models.Count + ")");
            else Console.Out.WriteLine("Side B Empty!");
        }

        //Resets relevant numbers
        private static void Reset()
        {
            Turn = 0;
            
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
        public static void Play()
        {
            Console.WriteLine("\n\n||==|| -------------------- ||==||");
            Console.WriteLine("||==|| Let Battle Commence! ||==||");
            Console.WriteLine("||==|| -------------------- ||==||");
            //Each run of this loop is one round of combat.
            //We assume A takes turn 0, followed by B taking turn 1, etc.
            while (true)
            {
                if (CheckWin()) break;

                SideA.NewTurn();
                SideB.NewTurn();

                Console.WriteLine("\n\n|##| Turn " + Turn + " |##|\n");
                Console.Out.WriteLine(SideA.Name + " has " + SideA.LiveCount() + " models remaining.");
                Console.Out.WriteLine(SideB.Name + " has " + SideB.LiveCount() + " models remaining.\n");

                //Order of a turn: the sides attack one another, battleshock is then resolved.
                Unit firstTurn = (Turn % 2 == 0) ? SideA : SideB;
                Unit secondTurn = (firstTurn == SideA) ? SideB : SideA;
                
                //Fight

                firstTurn.MeleeAttack(secondTurn);
                if (CheckWin()) break;

                secondTurn.MeleeAttack(firstTurn);
                if (CheckWin()) break;



                //Battleshock
                if (SideA.LostUnits > SideB.LostUnits) SideA.Battleshock(0);
                if (SideB.LostUnits > SideA.LostUnits) SideB.Battleshock(0);
                Turn++;
                Console.Out.WriteLine("Next Turn?");
                Console.In.ReadLine();
            }
            Reset();
        }
    }

    class Program
    {
        //Change these numbers to toggle console output data.
        public static bool ShowDieRolls { get; } = true;
        public static bool ShowHitRolls { get; } = true;
        public static bool ShowWoundRolls { get; } = true;
        public static bool ShowSaveRolls { get; } = true;

        private static String appname = "Age of Sigmar Battle Simulator (Console Prototype)";
        private static String version = "0.0.1";

        private static Dictionary<String, Database> Factions = new Dictionary<string, Database>();

        public static void LoadFactions()
        {
            XElement facFile = XElement.Load("faction_list.xml");
            foreach (XElement entry in facFile.Descendants())
            {
                Factions.Add(entry.Value, new Database(entry.Value));
            }
        }

        public static Model GetModel(String name)
        {
            name = name.ToLower();
            Console.Out.WriteLine("Finding " + name);
            foreach (KeyValuePair<String, Database> pair in Factions)
            {   
                Console.Out.WriteLine("Searching in " + pair.Key);
                Dictionary<String, Model> mods = pair.Value.ModelData;
                Model m;
                if (mods.TryGetValue(name, out m)) return m;
            }
            return null;
        }

        /*Prints the main menu
        static void PrintMenu()
        {
            Console.Out.WriteLine("1. Run Battle");
            Console.Out.WriteLine("2. View Battle Setup");
            Console.Out.WriteLine("3. Modify Side A");
            Console.Out.WriteLine("4. Modify Side B");
            Console.Out.WriteLine("5. Quit");
        }
        */
        static void Main(string[] args)
        {
            Console.Out.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            Directory.SetCurrentDirectory(
                String.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\data"));

            //Initialisation
            Console.Out.WriteLine("Welcome to " + appname + "!\nv." + version);

            //FACTION DATABASES
            LoadFactions();

            //CHANGE THESE FOR PLAYTIMES//
            String NameA = "Saurus Warriors (Clubs)";
            int SizeA = 10;

            String NameB = "Liberators";
            int SizeB = 5;
            //--------------------------//

            Model UnitA = GetModel(NameA);
            if (UnitA == null)
            {
                Console.Out.WriteLine("Unit " + NameA + " not found!");
                //Environment.Exit(-1);
            }

            Model UnitB = GetModel(NameB);
            if (UnitB == null)
            {
                Console.Out.WriteLine("Unit " + NameB + " not found!");
                //Environment.Exit(-1);
            }

            while (true)
            {
                Battle.SideA = new Unit(UnitA, SizeA);
                Battle.SideB = new Unit(UnitB, SizeB);
                Battle.PrintSides();
                Console.Out.WriteLine("\nBegin?");
                Console.In.ReadLine();
                Battle.Play();
                Console.Out.WriteLine("\nPlay again? (Y/N)");
                String c = Console.In.ReadLine().ToLower();
                if (c == "n") break;
            }
        }
    }
}
