using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class LocalizationMenu
{
    [MenuItem("GameObject/UI/LocalizationText", false, 30)]
    static void CreateLocalizationTextGameObject(MenuCommand menuCommand)
    {
        var go = new GameObject("LocalizationText");
        var rt = go.AddComponent<RectTransform>();
        var txt = go.AddComponent<LocalizationText>();
        var parent = menuCommand.context as GameObject;
        if (parent == null)
        {
            parent = Object.FindObjectOfType(typeof(Canvas)) as GameObject;
            if (parent == null)
            {
                var root = new GameObject("Canvas");
                root.layer = LayerMask.NameToLayer("UI");
                Canvas canvas = root.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                root.AddComponent<CanvasScaler>();
                root.AddComponent<GraphicRaycaster>();
                Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);
                parent = root;
            }
        }
        rt.SetParent(parent.transform);
        rt.localPosition = Vector3.zero;
        rt.localScale = Vector3.one;
        txt.SetText("Hello World");
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeGameObject = go;
    }

    [MenuItem("GameObject/UI/LocalizationImage", false, 31)]
    static void CreateLocalizationImageGameObject(MenuCommand menuCommand)
    {
        var go = new GameObject("LocalizationImage");
        var rt = go.AddComponent<RectTransform>();
        var image = go.AddComponent<LocalizationImage>();
        var parent = menuCommand.context as GameObject;
        if (parent == null)
        {
            parent = Object.FindObjectOfType(typeof(Canvas)) as GameObject;
            if (parent == null)
            {
                var root = new GameObject("Canvas");
                root.layer = LayerMask.NameToLayer("UI");
                Canvas canvas = root.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                root.AddComponent<CanvasScaler>();
                root.AddComponent<GraphicRaycaster>();
                Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);
                parent = root;
            }
        }
        rt.SetParent(parent.transform);
        rt.localPosition = Vector3.zero;
        rt.localScale = Vector3.one;
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeGameObject = go;
    }
}
