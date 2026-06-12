using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using Sts2RollingBoulderToPotatoMod.Services;

namespace Sts2RollingBoulderToPotatoMod.Patches;

[HarmonyPatch(typeof(PowerModel), "Icon", MethodType.Getter)]
internal static class PowerModelIconPatch
{
    [HarmonyPostfix]
    private static void Postfix(PowerModel __instance, ref Texture2D __result)
    {
        if (!PotatoAssetMatcher.IsRollingBoulderPower(__instance))
        {
            return;
        }

        __result = PotatoTextureLoader.GetCurrentOrFallback(__result)!;
    }
}

[HarmonyPatch(typeof(PowerModel), "BigIcon", MethodType.Getter)]
internal static class PowerModelBigIconPatch
{
    [HarmonyPostfix]
    private static void Postfix(PowerModel __instance, ref Texture2D __result)
    {
        if (!PotatoAssetMatcher.IsRollingBoulderPower(__instance))
        {
            return;
        }

        __result = PotatoTextureLoader.GetCurrentOrFallback(__result)!;
    }
}
