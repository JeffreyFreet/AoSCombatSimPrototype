using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            catch(FileNotFoundException f) { Debug.Write("Error: Data files for " + faction + " not found! (" + f.Message + ")\n"); return; }

            modelFile = XElement.Load(String.Concat(faction, "_", "models.xml"));

            //Load weapons
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
                Debug.WriteLine("Weapon " + weapon.Attribute("name").Value + " added!");
            }
            

            //Load models
            foreach (var model in modelFile.Elements())
            {
                String n = model.Attribute("name").Value;
                XElement weaps = model.Element("weapons");
                XElement stats = model.Element("stats");

                List<Weapon> mweapons = new List<Weapon>();
                List<Weapon> rweapons = new List<Weapon>();
                foreach (var mw in weaps.Descendants())
                {
                    Weapon w;
                    if (WeaponData.TryGetValue(mw.Value.ToLower(), out w))
                    {
                        if (mw.Attribute("type").Value == "melee") mweapons.Add(w);
                        else rweapons.Add(w);
                    }
                    else Debug.WriteLine("Weapon profile " + mw.Value + " not found!");
                }

                ModelData.Add(n.ToLower(), new Model(
                        n,
                        int.Parse(stats.Element("move").Value),
                        int.Parse(stats.Element("wounds").Value),
                        int.Parse(stats.Element("bravery").Value),
                        int.Parse(stats.Element("save").Value),
                        int.Parse(model.Element("size").Value),
                        mweapons,
                        rweapons
                    )
                );
                Debug.WriteLine("Model " + model.Attribute("name").Value + " added!");
            }

            String capname = String.Concat(faction.Substring(0,1).ToUpper(), faction.Substring(1));
            Debug.WriteLine("Successfully imported " + WeaponData.Count + " weapon profiles and " +
                                  ModelData.Count + " model profiles for the " + capname + " faction.");
        }

        public Model GetModel(String name)
        {
            Model m;
            if (ModelData.TryGetValue(name.ToLower(), out m)) return m;
            Debug.WriteLine("Model " + name + " not found!"); return null;
        }
    }
}


