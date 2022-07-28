using UnityEngine;
using UnityEditor;
using GBJ.AudioEngine.Settings;

namespace GBJ.AudioEngine.Editor
{
    public class AudioHighPassFilterSettingsInspector : UnityEditor.Editor
    {
        public static void Draw(AudioHighPassFilterSettings settings)
        {
            settings.Foldout = EditorGUILayout.Foldout(settings.Foldout, "High Pass Filter Settings");
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

            settings.CutoffFrequency = EditorGUILayout.Slider("Cutoff Frequency", settings.CutoffFrequency, 10, 22000);
            settings.HighpassResonanceQ = EditorGUILayout.Slider("Highpass Resonance Q", settings.HighpassResonanceQ, 10, 22000);

            GUI.enabled = true;
            EditorGUI.indentLevel--;
            GUILayout.Space(12f);
        }

        public static void Reset(AudioHighPassFilterSettings settings, bool displayDialog = true)
        {
            if(displayDialog && !EditorUtility.DisplayDialog("Reset?", "Are you sure?", "Reset", "Cancel"))
                return;

            GameObject go = new GameObject();
            go.hideFlags = HideFlags.DontSaveInEditor;
            go.AddComponent<AudioSource>();
            AudioHighPassFilter filter = go.AddComponent<AudioHighPassFilter>();

            settings.CutoffFrequency = filter.cutoffFrequency;
            settings.HighpassResonanceQ = filter.highpassResonanceQ;

            DestroyImmediate(go);
        }
    }
}
