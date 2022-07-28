using UnityEngine;
using UnityEditor;
using GBJ.AudioEngine.Settings;

namespace GBJ.AudioEngine.Editor
{
    public class AudioLowPassFilterSettingsInspector : UnityEditor.Editor
    {
        public static void Draw(AudioLowPassFilterSettings settings)
        {
            settings.Foldout = EditorGUILayout.Foldout(settings.Foldout, "Low Pass Filter Settings");
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
            settings.LowpassResonanceQ = EditorGUILayout.Slider("Lowpass Resonance Q", settings.LowpassResonanceQ, 1f, 10f);

            GUI.enabled = true;
            EditorGUI.indentLevel--;
            GUILayout.Space(12f);
        }

        public static void Reset(AudioLowPassFilterSettings settings, bool displayDialog = true)
        {
            if(displayDialog && !EditorUtility.DisplayDialog("Reset?", "Are you sure?", "Reset", "Cancel"))
                return;

            GameObject go = new GameObject();
            go.hideFlags = HideFlags.DontSaveInEditor;
            go.AddComponent<AudioSource>();
            AudioLowPassFilter filter = go.AddComponent<AudioLowPassFilter>();

            settings.CutoffFrequency = filter.cutoffFrequency;
            settings.LowpassResonanceQ = filter.lowpassResonanceQ;

            DestroyImmediate(go);
        }
    }
}
