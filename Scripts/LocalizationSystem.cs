using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class LocalizationSystem
{
    static Dictionary<string, Dictionary<string, TextLocalizationConfig>> TextDict = new Dictionary<string, Dictionary<string, TextLocalizationConfig>>();
    static Dictionary<string, Dictionary<string, SpriteLocalizationConfig>> SpriteDict = new Dictionary<string, Dictionary<string, SpriteLocalizationConfig>>();
    static List<IOnLocalizationRegionChange> AliveComponents = new List<IOnLocalizationRegionChange>();
    static string Region = "CN";

    public static Action OnRegionChange;
    public static Func<string, string> OnAfterTreatment;

    static LocalizationSystem()
    {
        OnRegionChange += () =>
        {
            foreach (var text in AliveComponents)
            {
                text.OnLocalizationRegionChange();
            }
        };

        OnAfterTreatment += (string text) =>
        {            
            return text.Replace("%Player%", "KeXiang");
        };
    }

    public static bool TextIsContains(string region, string tag)
    {
        return TextDict.TryGetValue(region, out var tag_dict) && tag_dict.ContainsKey(tag);
    }

    public static bool SpriteIsContains(string region, string tag)
    {
        return SpriteDict.TryGetValue(region, out var tag_dict) && tag_dict.ContainsKey(tag);
    }

    public static void ChangeRegion(string region)
    {
        Region = region;
        OnRegionChange?.Invoke();
    }

    public static string GetRegion()
    {
        return Region;
    }

    public static void RegisterTextConfig(string region, string tag, TextLocalizationConfig config)
    {
        if (string.IsNullOrEmpty(region) || string.IsNullOrEmpty(tag) || config==null)
        {
            Debug.LogError("Localization Config is null.");
            return;
        }

        if (!TextDict.ContainsKey(region))
        {
            TextDict[region] = new Dictionary<string, TextLocalizationConfig>();
        }
        if (TextDict[region].ContainsKey(tag))
        {
            Debug.LogError($"This tag [{tag}] is exist.");
            return;
        }
        TextDict[region].Add(tag, config);
        OnRegionChange?.Invoke();
    }

    public static void UnRegisterTextConfig(string region, string tag = "")
    {
        if (TextDict.ContainsKey(region))
        {
            if (string.IsNullOrEmpty(tag))
            {
                if (TextDict.TryGetValue(region, out var tag_dict))
                {
                    foreach (var item in tag_dict)
                    {
                        item.Value.Discard();
                    }
                    TextDict.Remove(region);
                }
            }
            else
            {
                if (TextDict[region].TryGetValue(tag, out var cfg))
                {
                    cfg.Discard();
                    TextDict[region].Remove(tag);
                }
            }
            OnRegionChange?.Invoke();
        }
    }

    public static void RegisterSpriteConfig(string region, string tag, SpriteLocalizationConfig config)
    {
        if (string.IsNullOrEmpty(region) || string.IsNullOrEmpty(tag) || config == null)
        {
            Debug.LogError("Localization Config is null.");
            return;
        }

        if (!SpriteDict.ContainsKey(region))
        {
            SpriteDict[region] = new Dictionary<string, SpriteLocalizationConfig>();
        }
        if (SpriteDict[region].ContainsKey(tag))
        {
            Debug.LogError($"This tag [{tag}] is exist.");
            return;
        }
        SpriteDict[region].Add(tag, config);
        OnRegionChange?.Invoke();
    }

    public static void UnRegisterSpriteConfig(string region, string tag = "")
    {
        if (SpriteDict.ContainsKey(region))
        {
            if (string.IsNullOrEmpty(tag))
            {
                if (SpriteDict.TryGetValue(region, out var tag_dict))
                {
                    foreach (var item in tag_dict)
                    {
                        item.Value.Discard();
                    }
                    SpriteDict.Remove(region);
                }
            }
            else
            {
                if (SpriteDict[region].TryGetValue(tag, out var cfg))
                {
                    cfg.Discard();
                    SpriteDict[region].Remove(tag);
                }
            }
            OnRegionChange?.Invoke();
        }
    }

    public static void RegisterComponent(IOnLocalizationRegionChange text)
    {
        AliveComponents.Add(text);
    }

    public static void UnRegisterComponent(IOnLocalizationRegionChange text)
    {
        AliveComponents.Remove(text);
    }

    public static string GetText(string key, string tag = "default", string region = "")
    {
        if (string.IsNullOrEmpty(region))
        {
            region = Region;
        }
        var text = $"{region}_{tag}_{key}";

        if (TextDict.TryGetValue(region, out var tag_dict) && tag_dict.TryGetValue(tag, out var cfg))
        {
            if (cfg.GetText(key, out var t))
            {
                text = t;
            }
        }

        return text;
    }

    public static string ReplaceMarco(string text)
    {
        if (OnAfterTreatment != null)
        {
            return OnAfterTreatment(text);
        }
        return text;
    }

    public static Sprite GetSprite(string key, string tag = "default", string region = "")
    {
        if (string.IsNullOrEmpty(region))
        {
            region = Region;
        }

        if (SpriteDict.TryGetValue(region, out var tag_dict) && tag_dict.TryGetValue(tag, out var cfg))
        {
            if (cfg.GetText(key, out var sprite))
            {
                return sprite;
            }
        }
        return null;
    }
}
