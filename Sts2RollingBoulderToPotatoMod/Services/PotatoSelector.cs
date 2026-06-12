using System.Reflection;
using Godot;
using MegaCrit.Sts2.Core.Models;
using Sts2RollingBoulderToPotatoMod.Config;

namespace Sts2RollingBoulderToPotatoMod.Services;

public static class PotatoSelector
{
    private static readonly Texture2D?[] Textures = new Texture2D?[3];

    public static int CurrentIndex { get; private set; }

    public static Texture2D? Current => Textures[CurrentIndex - 1];

    public static void Initialize(PotatoConfig config)
    {
        for (int i = 1; i <= 3; i++)
        {
            Textures[i - 1] = PotatoTextureLoader.LoadEmbedded(i);
        }

        CurrentIndex = config.SelectedIndex;
        if (Current is null)
        {
            RollingBoulderToPotatoMod.Logger.Warn(
                "Selected potato texture failed to load. Falling back to potato 1.");
            CurrentIndex = 1;
        }
    }
}

public static class PotatoTextureLoader
{
    public static Texture2D? LoadEmbedded(int index)
    {
        if (index is < 1 or > 3)
        {
            return null;
        }

        string resourceName =
            $"Sts2RollingBoulderToPotatoMod.Assets.potato_{index}.png";

        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream(resourceName);
        if (stream is null)
        {
            RollingBoulderToPotatoMod.Logger.Warn(
                $"Embedded potato resource not found: {resourceName}");
            return null;
        }

        byte[] bytes = new byte[stream.Length];
        _ = stream.Read(bytes, 0, bytes.Length);

        Image image = new();
        Error error = image.LoadPngFromBuffer(bytes);
        if (error != Error.Ok || image.IsEmpty())
        {
            RollingBoulderToPotatoMod.Logger.Warn(
                $"Failed to decode embedded potato #{index}: {error}");
            return null;
        }

        return ImageTexture.CreateFromImage(image);
    }

    public static Texture2D? GetCurrentOrFallback(Texture2D? fallback) =>
        PotatoSelector.Current ?? fallback;

    public static void ApplyToSprite(Sprite2D sprite)
    {
        Texture2D? potato = PotatoSelector.Current;
        if (potato is null)
        {
            return;
        }

        Texture2D? original = sprite.Texture;
        sprite.Texture = potato;
        sprite.Centered = true;

        if (original is null)
        {
            return;
        }

        Vector2 originalSize = original.GetSize();
        Vector2 potatoSize = potato.GetSize();
        if (originalSize.X <= 0f || originalSize.Y <= 0f || potatoSize.X <= 0f || potatoSize.Y <= 0f)
        {
            return;
        }

        float factor = MathF.Min(originalSize.X / potatoSize.X, originalSize.Y / potatoSize.Y);
        sprite.Scale *= factor;
    }
}

public static class PotatoAssetMatcher
{
    public static bool IsRollingBoulderCardSprite(string? spriteName)
    {
        string fileName = GetFileName(spriteName);
        return fileName.Equals("rolling_boulder", StringComparison.OrdinalIgnoreCase)
            || fileName.StartsWith("rolling_boulder_u", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsRollingBoulderPowerSprite(string? spriteName)
    {
        string fileName = GetFileName(spriteName);
        return fileName.Equals("rolling_boulder_power", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsRollingBoulderPower(PowerModel model) =>
        model.GetType().Name.Equals("RollingBoulderPower", StringComparison.Ordinal);

    private static string GetFileName(string? spriteName)
    {
        if (string.IsNullOrWhiteSpace(spriteName))
        {
            return string.Empty;
        }

        string fileName = spriteName.Contains('/')
            ? spriteName[(spriteName.LastIndexOf('/') + 1)..]
            : spriteName;

        if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
        {
            fileName = fileName[..^4];
        }

        if (fileName.EndsWith(".tres", StringComparison.OrdinalIgnoreCase))
        {
            fileName = fileName[..^5];
        }

        return fileName;
    }
}
