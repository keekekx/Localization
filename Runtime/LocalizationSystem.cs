using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class LocalizationSystem
{
    #region Const
    /// <summary>
    /// 约定该tag为global text
    /// </summary>
    public const string TAG_DEFAULT = "default";
    
    /// <summary>
    /// 约定默认的当前案件的text
    /// </summary>
    public const string TAG_CURRENT_CASE = "current_case";

    public const string CASE_PATH_PREFIX = "Assets/Res/Cases";
    #endregion

    #region Variables
    static Dictionary<string, Dictionary<string, TextLocalizationConfig>> TextDict = new Dictionary<string, Dictionary<string, TextLocalizationConfig>>();
    static Dictionary<string, Dictionary<string, SpriteLocalizationConfig>> SpriteDict = new Dictionary<string, Dictionary<string, SpriteLocalizationConfig>>();
    static List<IOnLocalizationRegionChange> AliveComponents = new List<IOnLocalizationRegionChange>();
    
    /// <summary>
    /// 默认region
    /// </summary>
    public static string Region = "CN";

    public static Action OnRegionChange;
    public static Func<string, string> OnAfterTreatment;
    #endregion

    static LocalizationSystem()
    {
        OnRegionChange += () =>
        {
            foreach (var text in AliveComponents)
            {
                text.OnLocalizationRegionChange();
            }
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

#if UNITY_EDITOR
    [MenuItem("Tools/Localization System/Convert CSV to SO Config")]
    public static void TestCSV()
    {
        var path = EditorUtility.OpenFilePanel("Open", Application.dataPath, "csv");
        GenerateTextConfigWithCSV(path);
    }
    public static void GenerateTextConfigWithCSV(string path, string save_path = "")
    {
        if (!File.Exists(path))
        {
            Debug.LogError("CSV is not found!");
            return;
        }

        var configDict = new Dictionary<string, TextLocalizationConfig>();
        var regionDict = new Dictionary<int, string>();
        var rows = File.ReadAllLines(path);
        {
            var cols = new List<string>(rows[0].Trim().Split(','));
            for (var i = 1; i < cols.Count; i++)
            {
                var region = cols[i];
                if (string.IsNullOrEmpty(region)) continue;
                if (configDict.ContainsKey(region)) continue;
                configDict[region] = ScriptableObject.CreateInstance<TextLocalizationConfig>();
                regionDict[i] = region;
            }
        }
        
        for (var i = 2; i < rows.Length; i++)
        {
            var cols = new List<string>(rows[i].Trim().Split(','));
            var key = cols[0];
            for (var j = 1; j < cols.Count; j++)
            {
                if (regionDict.TryGetValue(j, out var region))
                {
                    configDict[region].Dictionary.Add(new TextLocalizationConfig.Node()
                    {
                        Key = key,
                        Value = cols[j],
                    });
                }
            }
        }

        if (string.IsNullOrEmpty(save_path))
        {
            save_path = EditorUtility.SaveFolderPanel("Save", Application.dataPath, "LocalizationConfig");
        }
        
        if (string.IsNullOrEmpty(save_path))
        {
            Debug.LogError("save path error!");
            return;
        }

        save_path = save_path.Replace("\\", "/");
        if (!save_path.Contains(Application.dataPath))
        {
            Debug.LogError($"only save inside project: {Application.dataPath}");
            return;
        }
        
        save_path = save_path.Replace(Application.dataPath, "Assets/");
        
        if (!Directory.Exists(save_path))
        {
            Directory.CreateDirectory(save_path);
        }
        foreach (var item in configDict)
        {
            AssetDatabase.CreateAsset(item.Value, Path.Combine(save_path, $"{item.Key}.asset"));
        }
        AssetDatabase.Refresh();
    }
#endif

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

    public static string GetText(string key, string tag = TAG_DEFAULT, string region = "")
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

    public static Sprite GetSprite(string key, string tag = TAG_DEFAULT, string region = "")
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
