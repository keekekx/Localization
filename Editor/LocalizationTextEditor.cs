using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace TMPro.EditorUtilities
{
    [CustomEditor(typeof(LocalizationText), true), CanEditMultipleObjects]
    public class LocalizationTextEditor : TMP_BaseEditorPanel
    {
        private SerializedProperty m_UseLocalization;
        private SerializedProperty m_LocalizationKey;
        private SerializedProperty m_LocalizationTag;

        static readonly GUIContent k_RaycastTargetLabel = new GUIContent("Raycast Target", "Whether the text blocks raycasts from the Graphic Raycaster.");

        SerializedProperty m_RaycastTargetProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_RaycastTargetProp = serializedObject.FindProperty("m_RaycastTarget");
            m_UseLocalization = serializedObject.FindProperty("UseLocalization");
            m_LocalizationKey = serializedObject.FindProperty("LocalizationKey");
            m_LocalizationTag = serializedObject.FindProperty("LocalizationTag");
        }

        protected override void DrawExtraSettings()
        {
            Foldout.extraSettings = EditorGUILayout.Foldout(Foldout.extraSettings, k_ExtraSettingsLabel, true, TMP_UIStyleManager.boldFoldout);
            if (Foldout.extraSettings)
            {
                EditorGUI.indentLevel += 1;

                DrawMargins();

                DrawGeometrySorting();

                DrawRichText();

                DrawRaycastTarget();

                DrawParsing();

                DrawKerning();

                DrawPadding();

                EditorGUI.indentLevel -= 1;
            }
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
            EditorGUILayout.LabelField("Localization Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            DrawLocalization();
            EditorGUI.indentLevel -= 1;
        }

        protected void DrawRaycastTarget()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_RaycastTargetProp, k_RaycastTargetLabel);
            if (EditorGUI.EndChangeCheck())
            {
                // Change needs to propagate to the child sub objects.
                Graphic[] graphicComponents = m_TextComponent.GetComponentsInChildren<Graphic>();
                for (int i = 1; i < graphicComponents.Length; i++)
                    graphicComponents[i].raycastTarget = m_RaycastTargetProp.boolValue;

                m_HavePropertiesChanged = true;
            }
        }

        // Method to handle multi object selection
        protected override bool IsMixSelectionTypes()
        {
            GameObject[] objects = Selection.gameObjects;
            if (objects.Length > 1)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i].GetComponent<LocalizationText>() == null)
                        return true;
                }
            }
            return false;
        }
        protected override void OnUndoRedo()
        {
            int undoEventId = Undo.GetCurrentGroup();
            int lastUndoEventId = s_EventId;

            if (undoEventId != lastUndoEventId)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    //Debug.Log("Undo & Redo Performed detected in Editor Panel. Event ID:" + Undo.GetCurrentGroup());
                    TMPro_EventManager.ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED(true, targets[i] as LocalizationText);
                    s_EventId = undoEventId;
                }
            }
        }

        public override void OnInspectorGUI()
        {

            // Make sure Multi selection only includes TMP Text objects.
            if (IsMixSelectionTypes()) return;

            serializedObject.Update();

            DrawLocalizationSettings();

            DrawTextInput();

            DrawMainSettings();

            DrawExtraSettings();

            EditorGUILayout.Space();

            if (m_HavePropertiesChanged)
            {
                m_HavePropertiesChanged = false;
                m_TextComponent.havePropertiesChanged = true;
                m_TextComponent.ComputeMarginSize();
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();

                foreach (var t in targets)
                {
                    var ltmp = t as LocalizationText;
                    if (ltmp.UseLocalization)
                    {
                        ltmp.SetText(m_LocalizationKey.stringValue);
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
