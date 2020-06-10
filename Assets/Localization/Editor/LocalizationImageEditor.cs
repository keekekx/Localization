using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;

namespace TMPro.EditorUtilities
{
    [CustomEditor(typeof(LocalizationImage), true), CanEditMultipleObjects]
    public class LocalizationImageEditor : ImageEditor
    {
        private SerializedProperty m_UseLocalization;
        private SerializedProperty m_LocalizationKey;
        private SerializedProperty m_LocalizationTag;
        private SerializedProperty m_LocalizationRegion;
        private bool m_HavePropertiesChanged;
        protected struct Foldout
        {
            public static bool localizationSettings;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_UseLocalization = serializedObject.FindProperty("UseLocalization");
            m_LocalizationKey = serializedObject.FindProperty("LocalizationKey");
            m_LocalizationTag = serializedObject.FindProperty("LocalizationTag");
        }

        protected void DrawLocalization()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_UseLocalization, new GUIContent("Use Localization"));
            if (m_UseLocalization.boolValue)
            {
                EditorGUILayout.PropertyField(m_LocalizationTag, new GUIContent("Localization Tag", "所属标签"));
                EditorGUILayout.PropertyField(m_LocalizationKey, new GUIContent("Localization Key", "本地化索引"));
            }
            if (EditorGUI.EndChangeCheck())
            {
                m_HavePropertiesChanged = true;
            }
        }
        protected void DrawLocalizationSettings()
        {
            Foldout.localizationSettings = EditorGUILayout.Foldout(Foldout.localizationSettings, "Localization Settings", true, TMP_UIStyleManager.boldFoldout);
            if (Foldout.localizationSettings)
            {
                EditorGUI.indentLevel += 1;

                DrawLocalization();

                EditorGUI.indentLevel -= 1;
            }
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            DrawLocalizationSettings();

            EditorGUILayout.Space();

            if (m_HavePropertiesChanged)
            {
                m_HavePropertiesChanged = false;
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();

                foreach (var t in targets)
                {
                    var ltmp = t as LocalizationImage;
                    if (ltmp.UseLocalization)
                    {
                        ltmp.SetSprite(m_LocalizationKey.stringValue);
                    }
                }
            }
            else
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
