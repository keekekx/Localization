using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Localization/TextLocalizationConfig")]
public class TextLocalizationConfig : ScriptableObject
{
    [Serializable]
    public class Node
    {
        public string Key;
        public string Value;
    }
    public List<Node> Dictionary = new List<Node>();

    Dictionary<string, string> Buffer;

    public bool GetText(string key, out string text)
    {
        if (Buffer != null)
        {
            if (Buffer.TryGetValue(key, out text))
            {
                return true;
            }
            return false;
        }

        Debug.LogWarning("not build buffer, you can call BuildBuffer() in runtime.");
        var node = Dictionary.Find((n) => n.Key == key);
        if (node != null)
        {
            text = node.Value;
            return true;
        }
        text = "";
        return false;
    }

    /// <summary>
    /// 初始化缓冲区，空间换时间
    /// </summary>
    public void BuildBuffer()
    {
        if (Buffer!=null)
        {
            Debug.LogError("buffer exist,why rebuild that?");
            return;
        }
        Buffer = new Dictionary<string, string>();
        foreach (var item in Dictionary)
        {
            Buffer.Add(item.Key, item.Value);
        }
    }

    public void Discard()
    {
        Buffer?.Clear();
        Buffer = null;
    }
}
