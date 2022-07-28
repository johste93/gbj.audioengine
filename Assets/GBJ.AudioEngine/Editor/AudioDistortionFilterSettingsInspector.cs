using UnityEngine;
using UnityEditor;
using GBJ.AudioEngine.Settings;

namespace GBJ.AudioEngine.Editor
{
    public class AudioDistortionFilterSettingsInspector : UnityEditor.Editor
    {
        public static void Draw(AudioDistortionFilterSettings settings)
        {
            settings.Foldout = EditorGUILayout.Foldout(settings.Foldout, "Distortion Filter Settings");
            if(!settings.Foldout)
                return;
            
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            settings.Enabled = EditorGUILayout.Toggle("Enabled", settings.Enabled);
            GUI.enabled = settings.Enabled;

            GUILayout.FlexibleSpace();
            GUI.color = Color.red;
            if(GUILayout.Button("Reset", GUILayout.Width( EditorGUIUtility.fieldWidth )))
                Reset(settings);
            
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            settings.DistortionLevel = EditorGUILayout.Slider("Distortion Level", settings.DistortionLevel, 0f, 1f);

            GUI.enabled = true;
            EditorGUI.indentLevel--;
            GUILayout.Space(12f);
        }

        public static void Reset(AudioDistortionFilterSettings settings, bool displayDialog = true)
        {
            if(displayDialog && !EditorUtility.DisplayDialog("Reset?", "Are you sure?", "Reset", "Cancel"))
                return;

            GameObject go = new GameObject();
            go.hideFlags = HideFlags.DontSaveInEditor;
            go.AddComponent<AudioSource>();
            AudioDistortionFilter filter = go.AddComponent<AudioDistortionFilter>();

            settings.DistortionLevel = filter.distortionLevel;

            DestroyImmediate(go);
        }
    }
}