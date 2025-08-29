
using System;
using Verse;
using System.Collections.Generic;

namespace YanYu
{
    public class YanYu_ModSettings:ModSettings
    {   
        public List<String> swordList = new List<String>();
        public List<String> glovesList = new List<String>();
        public List<String> saberList = new List<String>();
        public List<String> staffList = new List<String>();


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref swordList, "swordList", LookMode.Value);
            Scribe_Collections.Look(ref glovesList, "glovesList", LookMode.Value);
            Scribe_Collections.Look(ref saberList, "saberList", LookMode.Value);
            Scribe_Collections.Look(ref staffList, "staffList", LookMode.Value);
        }

    }
}
