#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace MyFw
{
    [CustomEditor(typeof(MaterialDesignColorComponent))]
    public class MaterialDesignColorComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MaterialDesignColorComponent component = (MaterialDesignColorComponent)target;

            EditorGUI.BeginChangeCheck();

            DrawDefaultInspector();

            if (EditorGUI.EndChangeCheck())
            {
                component.ApplyMaterialColor();
            }
               
            GUILayout.Space(10);

            // 対象コンポーネント情報
            EditorGUILayout.LabelField("Target Component:", component.GetTargetComponentName(), EditorStyles.helpBox);

            // プレビュー表示
            GUILayout.Space(5);
            Color previewColor = component.GetCurrentMaterialColor();
            // 色情報表示
            EditorGUILayout.LabelField($"RGB: ({previewColor.r:F2}, {previewColor.g:F2}, {previewColor.b:F2})", EditorStyles.miniLabel);
            EditorGUILayout.LabelField($"Hex: #{ColorUtility.ToHtmlStringRGB(previewColor)}", EditorStyles.miniLabel);
        }
    }
}
#endif