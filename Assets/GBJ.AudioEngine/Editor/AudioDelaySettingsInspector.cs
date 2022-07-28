using UnityEngine;
using UnityEditor;
using GBJ.AudioEngine.Settings;

namespace GBJ.AudioEngine.Editor
{
    public class AudioDelaySettingsInspector : UnityEditor.Editor
    {
        public static void Draw(AudioDelaySettings settings)
        {
            if(settings.RandomDelay)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Delay", GUILayout.Width(EditorGUIUtility.labelWidth));
                
                settings.MinDelay = EditorGUILayout.FloatField("Min", settings.MinDelay);
                settings.MaxDelay = EditorGUILayout.FloatField("Max", settings.MaxDelay);

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                settings.Delay = EditorGUILayout.FloatField("Delay", settings.Delay);
            }

            settings.RandomDelay = EditorGUILayout.Toggle($"Random Delay", settings.RandomDelay);
            GUILayout.Space(12f);
        }

        public static void Reset(AudioDelaySettings settings, bool displayDialog = true)
        {
            if(displayDialog && !EditorUtility.DisplayDialog("Reset?", "Are you sure?", "Reset", "Cancel"))
                return;

            settings.Delay = 0;
            settings.RandomDelay = false; 
            settings.MinDelay = 0f;
            settings.MaxDelay = 1f;
        }
    }
}