using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using System.Reflection;

namespace YanYu
{
    public class YanYu_Mod : Mod
    {
        public static YanYu_ModSettings settings;
        public YanYu_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<YanYu_ModSettings>();
        }
        public override string SettingsCategory()
        {
            return "YanYu".Translate();
        }
        public override void DoSettingsWindowContents(UnityEngine.Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.Label("SwordList".Translate());
            if (listing_Standard.ButtonText("Edit".Translate()))
            {
                Find.WindowStack.Add(new Dialog_EditList("swordList", settings));
            }
            listing_Standard.Gap(12f);
            listing_Standard.Label("GlovesList".Translate());
            if (listing_Standard.ButtonText("Edit".Translate()))
            {
                Find.WindowStack.Add(new Dialog_EditList("glovesList", settings));
            }
            listing_Standard.Gap(12f);
            listing_Standard.Label("SaberList".Translate());
            if (listing_Standard.ButtonText("Edit".Translate()))
            {
                Find.WindowStack.Add(new Dialog_EditList("saberList", settings));
            }
            listing_Standard.Gap(12f);
            listing_Standard.Label("StaffList".Translate());
            if (listing_Standard.ButtonText("Edit".Translate()))
            {
                Find.WindowStack.Add(new Dialog_EditList("staffList", settings));
            }
            listing_Standard.End();
            settings.Write();
        }
    }

    public class Dialog_EditList : Window
    {
        private Vector2 scrollPos;
        private string title;
        private YanYu_ModSettings settings;
        private List<ThingDef> allWeapons;
        public override Vector2 InitialSize => new Vector2(600f, 600f);

        public Dialog_EditList(string title, YanYu_ModSettings settings)
        {
            this.title = title;
            this.settings = settings;
            doCloseX = true;
            absorbInputAroundWindow = true;
            forcePause = true;
            allWeapons = DefDatabase<ThingDef>.AllDefs
            .Where(d => d.IsWeapon).ToList();
            if (allWeapons.Count == 0)
            {
                Log.Error("No weapons found in DefDatabase.");
            }
            else
            {
                Log.Message($"Found {allWeapons.Count} weapons in DefDatabase.");
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect outRect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height - 40f);
            Rect viewRect = new Rect(0, 0, outRect.width - 20f, allWeapons.Count * 28f);

            Widgets.BeginScrollView(outRect, ref scrollPos, viewRect);
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(viewRect);    
            var field = settings.GetType().GetField(title, BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                var currentList = field.GetValue(settings) as List<string>;
                foreach (var weapon in allWeapons)
                {
                    if (weapon == null) Log.Message("weapon is null");
                    Log.Message($"{weapon.defName}");
                    bool isSelected = currentList.Contains(weapon.defName);
                    bool newSelected = isSelected;
                    listing.CheckboxLabeled(weapon.defName, ref newSelected);
                    if (newSelected != isSelected)
                    {
                        if (newSelected)
                        {
                            currentList.Add(weapon.defName);
                        }
                        else
                        {
                            currentList.Remove(weapon.defName);
                        }
                        settings.Write();
                    }
                }
            }

            listing.End();
            Widgets.EndScrollView();

            if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - 35f, 100f, 30f), "close".Translate()))
            {
                Close();
            }

        }
    }
}
