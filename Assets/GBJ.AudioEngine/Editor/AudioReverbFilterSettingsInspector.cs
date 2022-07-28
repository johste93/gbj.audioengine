using UnityEngine;
using UnityEditor;
using GBJ.AudioEngine.Settings;

namespace GBJ.AudioEngine.Editor
{
    public class AudioReverbFilterSettingsInspector : UnityEditor.Editor
    {
        public static void Draw(AudioReverbFilterSettings settings)
        {
            settings.Foldout = EditorGUILayout.Foldout(settings.Foldout, "Reverb Filter Settings");
            if(!settings.Foldout)
                return;
            
            EditorGUI.indentLevel++;
            AudioReverbPreset selectedPreset = (AudioReverbPreset) EditorGUILayout.EnumPopup("Reverb Preset", settings.ReverbPreset);
            if(selectedPreset != settings.ReverbPreset)
            {
                settings.ReverbPreset = selectedPreset;
                if(settings.ReverbPreset != AudioReverbPreset.User)
                    LoadPreset(selectedPreset, settings);
            }
            

            GUI.enabled = settings.ReverbPreset == AudioReverbPreset.User;

            settings.DryLevel = EditorGUILayout.Slider("Dry Level", settings.DryLevel, -10000, 0f);
            settings.Room = EditorGUILayout.Slider("Room", settings.Room, -10000f, 0f);
            settings.RoomHF = EditorGUILayout.Slider("Room HF", settings.RoomHF, -10000f, 0f);
            settings.RoomLF = EditorGUILayout.Slider("Room LF", settings.RoomLF, -10000f, 0f);
            settings.DecayTime = EditorGUILayout.Slider("Decay Time", settings.DecayTime, 0.1f, 20f);
            settings.DecayHFRatio = EditorGUILayout.Slider("Decay HF Ratio", settings.DecayHFRatio, 0.1f, 2f);
            settings.ReflectionsLevel = EditorGUILayout.Slider("Reflections Level", settings.ReflectionsLevel, -10000f, 1000f);
            settings.ReflectionsDelay = EditorGUILayout.Slider("Reflections Delay", settings.ReflectionsDelay, 0f, 0.3f);
            settings.ReverbLevel = EditorGUILayout.Slider("Reverb Level", settings.ReverbLevel, -10000f, 2000f);
            settings.ReverbDelay = EditorGUILayout.Slider("Reverb Delay", settings.ReverbDelay, 0f, 0.1f);
            settings.HfReferance = EditorGUILayout.Slider("HF Referance", settings.HfReferance, 1000f, 20000f);
            settings.LfReferance = EditorGUILayout.Slider("LF Referance", settings.LfReferance, 20f, 1000f);
            settings.Diffusion = EditorGUILayout.Slider("Diffusion", settings.Diffusion, 0f, 100f);
            settings.Density = EditorGUILayout.Slider("Density", settings.Density, 0f, 100f);

            GUI.enabled = true;
            EditorGUI.indentLevel--;
            GUILayout.Space(12f);
        }

        private static void LoadPreset(AudioReverbPreset preset, AudioReverbFilterSettings settings)
        {
            GameObject go = new GameObject();
            go.hideFlags = HideFlags.DontSaveInEditor;
            go.AddComponent<AudioSource>();
            AudioReverbFilter filter = go.AddComponent<AudioReverbFilter>();
            filter.reverbPreset = preset;

            settings.DryLevel = filter.dryLevel;
            settings.Room = filter.room;
            settings.RoomHF = filter.roomHF;
            settings.RoomLF = filter.roomLF;
            settings.DecayTime = filter.decayTime;
            settings.DecayHFRatio = filter.decayHFRatio;
            settings.ReflectionsLevel = filter.reflectionsLevel;
            settings.ReflectionsDelay = filter.reflectionsDelay;
            settings.ReverbLevel = filter.reverbLevel;
            settings.HfReferance = filter.hfReference;
            settings.LfReferance = filter.lfReference;
            settings.Diffusion = filter.diffusion;
            settings.Density = filter.density;

            DestroyImmediate(go);
        }
    }
}

/*
public float DryLevel = 0f;
        public float Room = 0f;
        public float RoomHF = 0f;
        public float RoomLF = 0f;
        public float DecayTime = 1f;
        public float DecayHFRatio = 0.5f;
        public float ReflectionsLevel = -10000f;
        public float ReflectionsDelay = 0f;
        public float ReverbFilter = 0f;
        public float ReverbDelay = 0.04f;
        public float HfReferances = 5000f;
        public float LfReferances = 250f;
        public float Diffusion = 100f;
        public float Density = 100f;
*/