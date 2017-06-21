using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* A unit of a number of models.
 * As attacks in AoS are made unit-to-unit, well, you get the picture.
 * The unit class contains methods for attacking, battleshock tests, and any other actions that are performed for a unit as a whole.
 * Also includes nice helper things for tidying up lists so that dead units are appropriately removed.
 */

namespace AoSConsole
{
    class Unit
    {
        public List<Model> Models { get; private set;  }

        //How many times the unit size multiplier this unit is.
        public int Count { get; }
        private int Number;
        public int PointsValue;

        public String Name;
        //public String Faction;
        public Model TypeModel;
        public int HighestBravery;
        public int LostModels = 0;

        public Unit(Model model, int number, string Faction = "")
        {
            Models = new List<Model>();
            Count = (int) Math.Ceiling(number/(double) model.UnitSize);
            PointsValue = model.Cost * Count;
            Number = number;
            Name = model.Name;
            TypeModel = model;
            Debug.WriteLine("Unit costs:" + PointsValue);

            for (int i = 0; i < number; i++)
            {
                Models.Add(new Model(model));
                if (model.Bravery > HighestBravery) HighestBravery = model.Bravery;
            }
        }
        
        //Go through each model in the unit and get it to generate and send wounds to the opposing unit.
        //The size of the frontage determines the maximum number of models that can attack, however this is looped through for different ranges of weapon.
        //We're simplifying it to two ranges: 1" (most weapons) and 2" (spears/polearms).
        //This gives us a more accurate idea of how units with spears perform compared to those with bog-standard melee weapons.
        public void MeleeAttack(Unit target, int front)
        {
            int totalwounds = 0;

            //First 'rank'
            int i = 0;
            Model m;
            for (; i < front; i++)
            {
                try
                {
                    m = Models[i];
                    if (m == null) break;
                }
                catch(ArgumentOutOfRangeException) {break;}
                Debug.WriteLine("First rank!");
                totalwounds += Models[i].MeleeAttack(target.TypeModel, 1);
            }
            for (int j = 0; j < front; j++)
            {
                try
                {
                    m = Models[i + j];
                    if (m == null) break;
                }
                catch (ArgumentOutOfRangeException) { break; }
                Debug.WriteLine("Second rank!");
                totalwounds += Models[i + j].MeleeAttack(target.TypeModel, 2);
            }

            target.TakeWounds(totalwounds);
            Debug.WriteLine(Name + " deals " + totalwounds + " wounds to " + target.Name + " dealing " + target.LostModels + " casualties.");
        }

        //Counts the number of guys still alive in the unit.
        public int LiveCount()
        {
            return Models.Count(p => p != null);
        }

        public int DeadCount()
        {
            return Number - LiveCount();
        }

        public void NewTurn()
        {
            LostModels = 0;
        }

    //For resolving a number of wounds dealt to this unit 
    //Wounds are deducted from each model in the unit in sequence until there are no more wounds or the unit is destroyed.
        public void TakeWounds(int count)
        {   
            int c = count;
            foreach (Model m in Models)
            {
                if (m == null) continue;
                if (c >= m.Wounds)
                {
                    c -= m.Wounds;
                    m.Wounds = 0;
                    LostModels++;
                }
                else
                {
                    m.Wounds -= c;
                    break;
                }
            }
            if (TakeLosses()) Console.Out.WriteLine(""); 
        }

        //After taking wounds remove killed units and 'clean up'
        //Returns whether or not the unit has been destroyed entirely.
        private bool TakeLosses()
        {
            List<Model> newList = new List<Model>();
            foreach (Model m in Models)
            {
                if (m.Wounds > 0) newList.Add(m);
            }
            Models = newList;
            
            if (LiveCount() == 0)
            {
                return true;
            }
            Debug.WriteLine(Name + " has " + LiveCount() + " models left.");
            return false;
        }
        

        /*Evaluate battleshock test
        *From the Rules:
        *   To make a battleshock test, roll a dice and
            add the number of models from the unit
            that have been slain this turn. For each
            point by which the total exceeds the highest
            Bravery characteristic in the unit, one model
            in that unit must  flee and is removed from
            play. Add 1 to the Bravery characteristic
            being used for every 10 models that are in
            the unit when the test is taken.
        */

        public void Battleshock(int modifier)
        {
            //Console.Out.WriteLine("Battleshock test for " + Name);
            //Debug.WriteLine("Battleshock roll for " + Name + " (Bravery " + HighestBravery + "): ");

            int losses = Math.Max(0, (Die.Roll(modifier) + DeadCount()) - (HighestBravery + (int) Math.Floor((double) LiveCount() / 10)));
            //Console.Out.WriteLine(Math.Min(TypeModel.UnitSize, losses) + " models flee from " + Name + "!");
            if (LiveCount() <= losses) TakeWounds(500);
            else
            {
                int i = 0;
                foreach (Model m in Models)
                {
                    if (i == losses) break;
                    if (m != null) m.Wounds = 0;
                    i++;
                }
                TakeLosses();
            }
        }

        //Reset all models and wounds in unit.
        public void Reset()
        {
            LostModels = 0;
            Models.RemoveAll(m => m != null);
            for (int i = 0; i < Number; i++)
            {
                Models.Add(new Model(TypeModel));
            }
        }

    }
}
