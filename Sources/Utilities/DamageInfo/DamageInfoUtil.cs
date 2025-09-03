using HarmonyLib;
using Verse;
using System.Reflection;

public static class DamageInfoUtil
{
    private static readonly FieldInfo armorPenField = typeof(DamageInfo).GetField("armorPenetrationInt", BindingFlags.NonPublic | BindingFlags.Instance);

    public static void AddArmorPenetration(ref DamageInfo dinfo, float ap)
    {
        if (armorPenField != null)
        {
            float currentAP = (float)armorPenField.GetValue(dinfo);
            armorPenField.SetValueDirect(__makeref(dinfo), currentAP + ap);
        }
    }
}
