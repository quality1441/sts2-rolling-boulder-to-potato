using System.Text.Json;

namespace Sts2RollingBoulderToPotatoMod.Config;

public sealed class PotatoConfig
{
    public required string RawValue { get; init; }

    public required int SelectedIndex { get; init; }

    public static PotatoConfig Default() => new()
    {
        RawValue = "random",
        SelectedIndex = Random.Shared.Next(1, 4),
    };

    public static PotatoConfig Fixed(int index) => new()
    {
        RawValue = index.ToString(),
        SelectedIndex = index,
    };

    public static PotatoConfig RandomAtLaunch() => new()
    {
        RawValue = "random",
        SelectedIndex = Random.Shared.Next(1, 4),
    };
}

public static class PotatoConfigLoader
{
    private const string ConfigFileName = "potato.cfg";
    private const string LegacyConfigFileName = "config.json";

    public static PotatoConfig Load(string modDirectory)
    {
        string configPath = Path.Combine(modDirectory, ConfigFileName);
        if (!File.Exists(configPath) && File.Exists(Path.Combine(modDirectory, LegacyConfigFileName)))
        {
            configPath = Path.Combine(modDirectory, LegacyConfigFileName);
            RollingBoulderToPotatoMod.Logger.Warn(
                $"Using legacy {LegacyConfigFileName}. Rename it to {ConfigFileName} and remove " +
                $"any *.json config files from the mod folder to avoid STS2 manifest errors.");
        }

        if (!File.Exists(configPath))
        {
            RollingBoulderToPotatoMod.Logger.Info(
                $"No {ConfigFileName} found. Defaulting to random potato.");
            return PotatoConfig.Default();
        }

        try
        {
            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(configPath));
            if (!document.RootElement.TryGetProperty("potato", out JsonElement potatoElement))
            {
                RollingBoulderToPotatoMod.Logger.Warn(
                    $"Missing \"potato\" in {Path.GetFileName(configPath)}. Defaulting to random.");
                return PotatoConfig.Default();
            }

            return ParsePotatoElement(potatoElement);
        }
        catch (Exception ex)
        {
            RollingBoulderToPotatoMod.Logger.Warn(
                $"Failed to read {Path.GetFileName(configPath)}: {ex.Message}. Defaulting to random.");
            return PotatoConfig.Default();
        }
    }

    private static PotatoConfig ParsePotatoElement(JsonElement potatoElement)
    {
        if (potatoElement.ValueKind == JsonValueKind.String)
        {
            string? value = potatoElement.GetString()?.Trim();
            if (string.Equals(value, "random", StringComparison.OrdinalIgnoreCase))
            {
                return PotatoConfig.RandomAtLaunch();
            }

            if (int.TryParse(value, out int parsed) && parsed is >= 1 and <= 3)
            {
                return PotatoConfig.Fixed(parsed);
            }
        }

        if (potatoElement.ValueKind == JsonValueKind.Number
            && potatoElement.TryGetInt32(out int number)
            && number is >= 1 and <= 3)
        {
            return PotatoConfig.Fixed(number);
        }

        RollingBoulderToPotatoMod.Logger.Warn(
            "Invalid \"potato\" value. Use \"random\", 1, 2, or 3. Defaulting to random.");
        return PotatoConfig.Default();
    }
}
