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
    private const string ConfigFileName = "config.json";
    private const string ExampleFileName = "config.example.json";

    public static PotatoConfig Load(string modDirectory)
    {
        string configPath = Path.Combine(modDirectory, ConfigFileName);
        string examplePath = Path.Combine(modDirectory, ExampleFileName);

        if (!File.Exists(configPath) && File.Exists(examplePath))
        {
            try
            {
                File.Copy(examplePath, configPath);
                RollingBoulderToPotatoMod.Logger.Info(
                    $"Created {ConfigFileName} from {ExampleFileName}.");
            }
            catch (Exception ex)
            {
                RollingBoulderToPotatoMod.Logger.Warn(
                    $"Could not create {ConfigFileName}: {ex.Message}");
            }
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
                    $"Missing \"potato\" in {ConfigFileName}. Defaulting to random.");
                return PotatoConfig.Default();
            }

            return ParsePotatoElement(potatoElement);
        }
        catch (Exception ex)
        {
            RollingBoulderToPotatoMod.Logger.Warn(
                $"Failed to read {ConfigFileName}: {ex.Message}. Defaulting to random.");
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
