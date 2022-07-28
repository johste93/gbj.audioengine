using UnityEngine;
using UnityEditor;
using GBJ.AudioEngine.Settings;

namespace GBJ.AudioEngine.Editor
{
    public class AudioEchoFilterSettingsInspector : UnityEditor.Editor
    {
        public static void Draw(AudioEchoFilterSettings settings)
        {
            settings.Foldout = EditorGUILayout.Foldout(settings.Foldout, "Echo Filter Settings");
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

            settings.Delay = Mathf.RoundToInt(EditorGUILayout.Slider("Distortion Level", settings.Delay, 10, 5000));
            settings.DecayRatio = EditorGUILayout.Slider("Decay Raio", settings.DecayRatio, 0f, 1f);
            settings.DryMix = EditorGUILayout.Slider("Dry Mix", settings.DryMix, 0f, 1f);
            settings.WetMix = EditorGUILayout.Slider("Wet Mix", settings.WetMix, 0f, 1f);

            GUI.enabled = true;
            EditorGUI.indentLevel--;
            GUILayout.Space(12f);
        }

        public static void Reset(AudioEchoFilterSettings settings, bool displayDialog = true)
        {
            if(displayDialog && !EditorUtility.DisplayDialog("Reset?", "Are you sure?", "Reset", "Cancel"))
                return;

            GameObject go = new GameObject();
            go.hideFlags = HideFlags.DontSaveInEditor;
            go.AddComponent<AudioSource>();
            AudioEchoFilter filter = go.AddComponent<AudioEchoFilter>();

            settings.Delay = filter.delay;
            settings.DecayRatio = filter.decayRatio;
            settings.DryMix = filter.dryMix;
            settings.WetMix = filter.wetMix;

            DestroyImmediate(go);
        }
    }
}
