using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[AddComponentMenu("UI/LocalizationImage")]
public class LocalizationImage : Image, IOnLocalizationRegionChange
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
            SetSprite(LocalizationKey);
        }
    }

    

    public void OnLocalizationRegionChange()
    {
        if (UseLocalization)
        {
            SetSprite(LocalizationKey);
        }
    }

    public void SetSprite(string key)
    {
        SetSprite(key, LocalizationTag);
    }

    public void SetSprite(string key, string tag = "default")
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

            sprite = LocalizationSystem.GetSprite(LocalizationKey, LocalizationTag);
        }
    }

    public void SetSprite(Sprite _sprite)
    {
        sprite = _sprite;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        LocalizationSystem.UnRegisterComponent(this);
    }
}
