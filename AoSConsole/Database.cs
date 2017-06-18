using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

/* Container class storing databases of weapons and models for a faction loaded from the relevant XML files.
 * Constructor takes a faction name and loads the weapon and model files for that faction in that order.
 */

namespace AoSConsole
{
    class Database
    {
        public Dictionary<String, Model> ModelData { get; }
        public Dictionary<String, Weapon> WeaponData { get; }
        public String FactionName { get; }

        public Database(String faction)
        {
            FactionName = faction;
            ModelData = new Dictionary<string, Model>();
            WeaponData = new Dictionary<string, Weapon>();

            XElement weaponFile;
            XElement modelFile;

            faction.ToLower();
            try { weaponFile = XElement.Load(String.Concat(faction, "_", "weapons.xml"));}
            catch(FileNotFoundException f) { Console.Out.Write("Error: Data files for " + faction + " not found! (" + f.Message + ")\n"); return; }

            modelFile = XElement.Load(String.Concat(faction, "_", "models.xml"));

            //Load melee weapons
            XElement weaponElement = weaponFile;
            foreach (var weapon in weaponElement.Elements())
            {
                String n = weapon.Attribute("name").Value;
                WeaponData.Add(n.ToLower(), new Weapon(
                        n,
                        int.Parse(weapon.Element("range").Value),
                        int.Parse(weapon.Element("attacks").Value),
                        int.Parse(weapon.Element("tohit").Value),
                        int.Parse(weapon.Element("towound").Value),
                        int.Parse(weapon.Element("rend").Value),
                        int.Parse(weapon.Element("damage").Value)
                    )
                );
                Console.Out.WriteLine("Weapon " + weapon.Attribute("name").Value + " added!");
            }
            

            //Load models
            foreach (var model in modelFile.Elements())
            {
                String n = model.Attribute("name").Value;
                XElement weaps = model.Element("weapons");
                XElement stats = model.Element("stats");

                List<Weapon> weapons = new List<Weapon>();
                foreach (var mw in weaps.Descendants())
                {
                    Weapon w;
                    if (WeaponData.TryGetValue(mw.Value.ToLower(), out w)) weapons.Add(w);
                    else Console.Out.WriteLine("Weapon profile " + mw.Value + " not found!");
                }

                ModelData.Add(n.ToLower(), new Model(
                        n,
                        int.Parse(stats.Element("move").Value),
                        int.Parse(stats.Element("wounds").Value),
                        int.Parse(stats.Element("bravery").Value),
                        int.Parse(stats.Element("save").Value),
                        int.Parse(model.Element("size").Value),
                        weapons
                    )
                );
            }

            String capname = String.Concat(faction.Substring(0,1).ToUpper(), faction.Substring(1));
            Console.Out.WriteLine("Successfully imported " + WeaponData.Count + " weapon profiles and " +
                                  ModelData.Count + " model profiles for the " + capname + " faction.");
        }

        public Model GetModel(String name)
        {
            Model m;
            if (ModelData.TryGetValue(name.ToLower(), out m)) return m;
            Console.Out.WriteLine("Model " + name + " not found!"); return null;
        }
    }
}


