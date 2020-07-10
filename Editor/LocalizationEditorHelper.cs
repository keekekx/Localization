using UnityEditor;
using UnityEngine;

/// <summary>
/// 仅提供Editor侧的多语言接口
/// </summary>
public static class LocalizationEditorHelper
{
    #region Interface
    /// <summary>
    /// Editor侧获取目标案件的多语言，需要显式重新load该案件的多语言资源
    /// </summary>
    public static string GetCaseText(string caseName, string id)
    {
        LoadCaseLocalization(caseName);
        return LocalizationSystem.GetText(id, LocalizationSystem.TAG_CURRENT_CASE);
    }
    #endregion
    
    #region Load

    public static TextLocalizationConfig LoadCaseLocalization(string caseName)
    {
        string caseAssetPath = $"{LocalizationSystem.CASE_PATH_PREFIX}/{caseName}/Localization/CN.asset";
        
        TextLocalizationConfig config = AssetDatabase.LoadAssetAtPath<TextLocalizationConfig>(caseAssetPath);
        if (config != null)
        {
            config.Discard();
            config.BuildBuffer();
        }
        else
        {
            Debug.LogError($"case localization config not exist => {caseAssetPath}");
        }

        LocalizationSystem.UnRegisterTextConfig(LocalizationSystem.Region, LocalizationSystem.TAG_CURRENT_CASE);
        LocalizationSystem.RegisterTextConfig(LocalizationSystem.Region, LocalizationSystem.TAG_CURRENT_CASE, config);

        return config;
    }
    #endregion
}