using UnityEngine;

public static class LocalizationHelper
{
    /// <summary>
    /// 获取通用的多语言
    /// </summary>
    public static string GetGlobalText(string id)
    {
        return LocalizationSystem.GetText(id, LocalizationSystem.TAG_DEFAULT);
    }

    /// <summary>
    /// 获取当前案件内的多语言
    /// </summary>
    public static string GetCurCaseText(string id)
    {
        return LocalizationSystem.GetText(id, LocalizationSystem.TAG_CURRENT_CASE);
    }
}