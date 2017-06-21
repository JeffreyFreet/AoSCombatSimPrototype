using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

/*Age of Sigmar Battle Simulator (Console Prototype)
 Written by GreenyRepublic
 Version 0.0.1 - June 2017
 */

//TODO Tidy up class structure.
//TODO Add unit upgrades and multiple weapon types.
//TODO Add more units and weapons!

namespace AoSConsole
{
    

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
            String NameA = "Kroxigor";
            int SizeA = 3;

            String NameB = "Protectors";
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
                Console.In.ReadLine();
                Environment.Exit(-1);
            }


            
            while (true)
            {

                Console.Out.WriteLine("How many battles?");
                int reps = int.Parse(Console.In.ReadLine());

                Stats[] BattleArray = new Stats[reps];

                Battle.SideA = new Unit(UnitA, SizeA);
                Battle.SideB = new Unit(UnitB, SizeB);

                Console.Out.WriteLine("Simulating " + reps + " battles.");
                for (int i = 0; i < reps; i++)
                {
                    Debug.WriteLine("Battle: " + i  + "/" + reps);
                    Console.Out.WriteLine("Battle: " + (i+1) + "/" + reps);
                    BattleArray[i] = Battle.Play(false);
                    Battle.Reset();
                }

                //Calculate averages.
                float AWins = 0;
                float BWins = 0;
                float ASurv = 0;
                float BSurv = 0;

                foreach (Stats stat in BattleArray)
                {
                    if (stat.Winner())
                    {
                        AWins++;
                        ASurv += stat.SurvivingModels(true);
                    }
                    else
                    {
                        BWins++;
                        BSurv += stat.SurvivingModels(false);
                    }
                }

                String mostWins = (AWins > BWins) ? Battle.SideA.Name : Battle.SideB.Name;
                float winNum = (AWins > BWins) ? AWins : BWins;
                
                Console.Out.WriteLine("Successfully ran " + reps + " battles.");
                Console.Out.WriteLine("NUMBERS TIME!\n");
                Console.Out.WriteLine("|Most wins|\n" + mostWins + " with " + winNum + "\n");
                Console.Out.WriteLine("|Percentage of games won|\n" + Battle.SideA.Name + ": " + 100*(AWins/reps) + "%\n" + Battle.SideB.Name + ": " + 100*(BWins/reps) + "%\n");
                float ASpercent = 100*(ASurv/reps) / SizeA;
                float BSpercent = 100*(BSurv/reps) / SizeB;
                Console.Out.WriteLine("|Average Surviving Models Per Win|\n" + Battle.SideA.Name + ": " + (ASurv/reps) + " (" + ASpercent + "%)\n" + Battle.SideB.Name + ": " + (BSurv/reps) + " (" + BSpercent + " %)\n");

                //Battle.PrintSides();
                //Console.Out.WriteLine("\nBegin?");
                //Console.In.ReadLine();


                Console.Out.WriteLine("\nPlay again? (Y/N)");
                String c = Console.In.ReadLine().ToLower();
                if (c == "n") break;
            }
        }
    }
}
