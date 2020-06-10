using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[AddComponentMenu("UI/LocalizationText")]
public class LocalizationText : TMPro.TextMeshProUGUI, IOnLocalizationRegionChange
{
    public bool UseLocalization;
    public string LocalizationKey;
    public string LocalizationTag = "default";

    protected override void Awake()
    {
        base.Awake();
        LocalizationSystem.RegisterComponent(this);

        if (UseLocalization)
        {
            SetText(LocalizationKey);
        }
    }

    public new void SetText(string key)
    {
        SetText(key, LocalizationTag);
    }

    public void SetText(string key, string tag = "default")
    {
        if (UseLocalization)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            LocalizationKey = key;

            if (!string.IsNullOrEmpty(tag))
            {
                LocalizationTag = tag;
            }

            key = LocalizationSystem.GetText(LocalizationKey, LocalizationTag);
        }

        key = LocalizationSystem.ReplaceMarco(key);
        base.SetText(key);
    }

    public void OnLocalizationRegionChange()
    {
        if (UseLocalization)
        {
            SetText(LocalizationKey);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        LocalizationSystem.UnRegisterComponent(this);
    }
}
