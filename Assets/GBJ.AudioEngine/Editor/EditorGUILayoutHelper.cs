using UnityEngine;
using UnityEditor;

namespace GBJ.AudioEngine.Editor
{
    public class EditorGUILayoutHelper
    {
        public static void DrawLabeledSlider(ref float constant, GUIContent label, float minValue, float maxValue,
            string minLabel = "", string maxLabel = "", int indentLevel = 0)
        {
            Rect position = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);

            // Draw label
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.indentLevel -= indentLevel;

            // Draw slider
            constant = EditorGUI.Slider(position, constant, minValue, maxValue);

            float labelWidth = position.width;

            // Move to next line
            position.y += EditorGUIUtility.singleLineHeight;

            // Subtract the text field width thats drawn with slider
            position.width -= EditorGUIUtility.fieldWidth;

            GUI.color = Color.gray;
            GUIStyle style = GUI.skin.label;
            TextAnchor defaultAlignment = GUI.skin.label.alignment;
            style.alignment = TextAnchor.UpperLeft;
            EditorGUI.LabelField(position, minLabel, style);
            style.alignment = TextAnchor.UpperRight;
            EditorGUI.LabelField(position, maxLabel, style);
            GUI.skin.label.alignment = defaultAlignment;
            GUI.color = Color.white;
            EditorGUI.indentLevel += indentLevel;
            GUILayout.Space(10);
        }
    }
}