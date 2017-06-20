using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Weapon profile base class
 * Contains stats as well as methods for rolling dice for hitting/wounding etc.
 */

namespace AoSConsole
{
    class Weapon
    {
        public string Name { get; }
        public int Range { get; }
        public int Attacks { get; }
        public int ToHit { get; }   
        public int ToWound { get; } 
        public int Rend { get; }
        public int Damage { get; }
        //True = Melee, False = Ranged
        public bool Type { get; }

        public Weapon(string name, int range, int attacks, int toHit, int toWound, int rend, int damage)
        {
            Name = name;
            Range = range;
            Attacks = attacks;
            ToHit = toHit;
            ToWound = toWound;
            Rend = rend;
            Damage = damage;
        }

        //Make rolls for hits and wounds and return the total.
        public int GenerateWounds(int hitmod, int woundmod)
        {
            int w = 0;
            for (int i = 0; i < Attacks; i++){ if (HitRoll(hitmod) && WoundRoll(woundmod)) w++;}
            //Console.Out.WriteLine("Weapon " + Name + " does " + w * Damage + " wounds.");
            return w * Damage;
        }

        public bool HitRoll(int modifier)
        {
            if (Program.ShowHitRolls) Debug.Write(Name + ": ");
            int r = Die.Roll(modifier);
            if (r >= ToHit)
            {
                if (Program.ShowHitRolls) Debug.Write("(Hits!) => ");
                return true;
            }
            if (Program.ShowHitRolls) Debug.Write("(Misses!)\n");
            return false;
        }

        public bool WoundRoll(int modifier)
        {
            int r = Die.Roll(modifier);
            if (r >= ToHit)
            {
                if (Program.ShowWoundRolls) Debug.Write("(Wounds!)\n");
                return true;
            }
            if (Program.ShowWoundRolls) Debug.Write("(Fails!)\n");
            return false;
        }
    }
}
