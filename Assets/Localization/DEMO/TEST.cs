using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public LocalizationText text;
    public void ChangeRegion()
    {
        if (LocalizationSystem.GetRegion() == "CN")
        {
            LocalizationSystem.ChangeRegion("EN");
        }
        else
        {
            LocalizationSystem.ChangeRegion("CN");
        }
    }

    public void LoadRegion()
    {
        LocalizationConfigLoader.LoadText(LocalizationSystem.GetRegion(), "default");
        LocalizationConfigLoader.LoadSprite(LocalizationSystem.GetRegion(), "default");
    }

    public void UnLoadRegion()
    {
        LocalizationSystem.UnRegisterTextConfig(LocalizationSystem.GetRegion(), "default");
        LocalizationSystem.UnRegisterSpriteConfig(LocalizationSystem.GetRegion(), "default");
    }

    public void SetText()
    {
        text.UseLocalization = false;
        text.SetText("%Player% is good boy");
    }
}
