using System;
using System.Collections.Generic;
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
            //Console.Out.Write("Hit Roll: ");
            return Die.Roll(modifier) >= ToHit;
        }

        public bool WoundRoll(int modifier)
        {
            //Console.Out.Write("Wound Roll: ");
            return Die.Roll(modifier) >= ToWound;
        }
    }
}
