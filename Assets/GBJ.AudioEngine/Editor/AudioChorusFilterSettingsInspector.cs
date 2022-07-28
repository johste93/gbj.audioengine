using UnityEngine;
using UnityEditor;
using GBJ.AudioEngine.Settings;

namespace GBJ.AudioEngine.Editor
{
    public class AudioChorusFilterSettingsInspector : UnityEditor.Editor
    {
        public static void Draw(AudioChorusFilterSettings settings)
        {
            settings.Foldout = EditorGUILayout.Foldout(settings.Foldout, "Chorus Filter Settings");
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
           
            settings.DryMix = EditorGUILayout.Slider("Dry Mix", settings.DryMix, 0f, 1f);
            settings.WetMix1 = EditorGUILayout.Slider("Wet Mix 1", settings.WetMix1, 0f, 1f);
            settings.WetMix2 = EditorGUILayout.Slider("Wet Mix 2", settings.WetMix2, 0f, 1f);
            settings.WetMix3 = EditorGUILayout.Slider("Wet Mix 3", settings.WetMix3, 0f, 1f);
            settings.Delay = EditorGUILayout.Slider("Delay", settings.Delay, 0.1f, 100f);
            settings.Rate = EditorGUILayout.Slider("Rate", settings.Rate, 0, 20f);
            settings.Depth = EditorGUILayout.Slider("Depth", settings.Depth, 0f, 1f);

            GUI.enabled = true;
            EditorGUI.indentLevel--;
            GUILayout.Space(12f);
        }

        public static void Reset(AudioChorusFilterSettings settings, bool displayDialog = true)
        {
            if(displayDialog && !EditorUtility.DisplayDialog("Reset?", "Are you sure?", "Reset", "Cancel"))
                return;

            GameObject go = new GameObject();
            go.hideFlags = HideFlags.DontSaveInEditor;
            go.AddComponent<AudioSource>();
            AudioChorusFilter filter = go.AddComponent<AudioChorusFilter>();

            settings.DryMix = filter.dryMix;
            settings.WetMix1 = filter.wetMix1;
            settings.WetMix2 = filter.wetMix2;
            settings.WetMix3 = filter.wetMix3;
            settings.Delay = filter.delay;
            settings.Rate = filter.rate;
            settings.Depth = filter.depth;

            DestroyImmediate(go);
        }
    }
}

/*



public bool Enabled = false;
        public float DryMix = 0.5f;
        public float WetMix1 = 0.5f;
        public float WetMix2 = 0.5f;
        public float WetMix3 = 0.5f;
        public float Delay = 40;
        public float Rate = 0.8f;
        public float Depth = 0.03f;
        */