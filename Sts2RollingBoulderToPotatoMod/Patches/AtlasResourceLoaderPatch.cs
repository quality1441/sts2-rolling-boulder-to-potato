using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using Sts2RollingBoulderToPotatoMod.Services;

namespace Sts2RollingBoulderToPotatoMod.Patches;

[HarmonyPatch(typeof(AtlasResourceLoader), "_Load")]
internal static class AtlasResourceLoaderPatch
{
    [HarmonyPostfix]
    private static void Postfix(string path, ref Variant __result)
    {
        if (__result.VariantType == Variant.Type.Nil)
        {
            return;
        }

        (string? atlasName, string? spriteName) = AtlasResourceLoader.ParsePath(path);
        if (!ShouldReplaceAtlasSprite(atlasName, spriteName))
        {
            return;
        }

        Texture2D? potato = PotatoSelector.Current;
        if (potato is null)
        {
            return;
        }

        __result = potato;
    }

    private static bool ShouldReplaceAtlasSprite(string? atlasName, string? spriteName)
    {
        if (string.Equals(atlasName, "card_atlas", StringComparison.OrdinalIgnoreCase))
        {
            return PotatoAssetMatcher.IsRollingBoulderCardSprite(spriteName);
        }

        if (string.Equals(atlasName, "power_atlas", StringComparison.OrdinalIgnoreCase))
        {
            return PotatoAssetMatcher.IsRollingBoulderPowerSprite(spriteName);
        }

        return false;
    }
}
