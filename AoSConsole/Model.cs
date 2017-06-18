using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Root class for a model in Age of Sigmar as defined by its Warscroll
 * All models will inherit from this.
 * Mostly just stats, since this is pure combat there's nothing really functional in terms of movement etc.
 */

namespace AoSConsole
{
    class Model
    {
        //Core Stats
        public string Name {get; private set; }
        public int Move { get; private set; }
        public int MaxWounds { get; private set; }
        public int Wounds { get; set; }
        public int Bravery { get; private set; } 
        public int Save { get; private set; }
        public int UnitSize { get; private set; }

        //Weapons
        public List<Weapon> Weapons { get; }
        
        public Model(string name, int move, int wounds, int bravery, int save, int unitsize, List<Weapon> weapons)
        {
            Name = name;
            Move = move;
            MaxWounds = Wounds = wounds;
            Bravery = bravery;
            Save = save;
            Weapons = weapons;
            UnitSize = unitsize;
        }

        public Model(Model source)
        {
            Name = source.Name;
            Move = source.Move;
            MaxWounds = Wounds = source.Wounds;
            Bravery = source.Bravery;
            Save = source.Save;
            Weapons = source.Weapons;
            UnitSize = source.UnitSize;
        }

        ~Model()
        {
            Console.Out.WriteLine("Model " + Name + " removed!");
        }
        
        //Takes a target model profiel and makes melee attacks against 'it' (assuming the target is just an abstract stat bag, actual wounds are sorted out later)
        //This takes care fo all lookups for saves and whatnot.
        public int MeleeAttack(Model targetModel)
        {
            int total = 0;
            foreach (Weapon w in Weapons)
            {
                for (int i = 0; i < w.GenerateWounds(0, 0); i++)
                {
                    if (!targetModel.TakeHit(w)) total += w.Damage;
                }
            }
            return total;
        }
        
        //Make a saving roll, 
        //A save value of 1 is treated as having no save.
        public bool TakeHit(Weapon weapon)
        {
            int roll = Die.Roll(-weapon.Rend);
            if (Save == 1) roll = 0;
            if (roll >= Save)
            {
                //Console.Out.Write(Name + " save made!\n");
                return true;
            }
            //Console.Out.WriteLine(Name + " save failed!");
            return false;
        }
    }
}
;