using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Sts2RollingBoulderToPotatoMod.Config;
using Sts2RollingBoulderToPotatoMod.Services;

namespace Sts2RollingBoulderToPotatoMod;

[ModInitializer(nameof(Initialize))]
public static class RollingBoulderToPotatoMod
{
    public const string ModId = "Sts2RollingBoulderToPotato";

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        string modDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? AppContext.BaseDirectory;

        PotatoConfig config = PotatoConfigLoader.Load(modDir);
        PotatoSelector.Initialize(config);

        Harmony harmony = new(ModId);
        harmony.PatchAll(Assembly.GetExecutingAssembly());

        Logger.Info(
            $"Rolling Boulder to Potato initialized. Using potato #{PotatoSelector.CurrentIndex} " +
            $"(config: {config.RawValue}).");
    }
}
