using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LocalizationConfigLoader
{
    static string Path = "Config";
    public static void LoadText(string region, string tag)
    {
        if (LocalizationSystem.TextIsContains(region, tag))
        {
            return;
        }
        var cfg = Resources.Load<TextLocalizationConfig>($"{Path}/Text/{region}/{tag}");
        cfg.BuildBuffer();
        LocalizationSystem.RegisterTextConfig(region, tag, cfg);
    }
    public static void LoadSprite(string region, string tag)
    {
        if (LocalizationSystem.SpriteIsContains(region, tag))
        {
            return;
        }
        var cfg = Resources.Load<SpriteLocalizationConfig>($"{Path}/Sprite/{region}/{tag}");
        cfg.BuildBuffer();
        LocalizationSystem.RegisterSpriteConfig(region, tag, cfg);
    }

    public static void LoadWithCode(string region, string tag)
    {
        if (LocalizationSystem.TextIsContains(region, tag))
        {
            return;
        }
        var cfg = ScriptableObject.CreateInstance<TextLocalizationConfig>();
        cfg.Dictionary.Add(new TextLocalizationConfig.Node() { Key = "test", Value = "%Player% load with code!" });
        cfg.BuildBuffer();
        LocalizationSystem.RegisterTextConfig(region, tag, cfg);
    }
}
