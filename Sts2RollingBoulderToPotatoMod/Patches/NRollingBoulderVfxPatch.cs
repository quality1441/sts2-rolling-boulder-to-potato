using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using Sts2RollingBoulderToPotatoMod.Services;

namespace Sts2RollingBoulderToPotatoMod.Patches;

[HarmonyPatch(typeof(NRollingBoulderVfx), "_Ready")]
internal static class NRollingBoulderVfxPatch
{
    private static readonly FieldInfo BoulderField =
        AccessTools.Field(typeof(NRollingBoulderVfx), "_boulder")!;

    [HarmonyPostfix]
    private static void Postfix(NRollingBoulderVfx __instance)
    {
        if (BoulderField.GetValue(__instance) is not Sprite2D boulder)
        {
            return;
        }

        PotatoTextureLoader.ApplyToSprite(boulder);
    }
}
