using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using GBJ.AudioEngine.Settings;

namespace GBJ.AudioEngine.Editor
{
    public class AudioSourceSettingsInspector : UnityEditor.Editor
    {
        public static void Draw(AudioSourceSettings settings)
        {
            settings.Foldout = EditorGUILayout.Foldout(settings.Foldout, "Audio Source Settings");
            if(!settings.Foldout)
                return;

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = Color.red;
            if(GUILayout.Button("Reset", GUILayout.Width( EditorGUIUtility.fieldWidth )))
                Reset(settings);
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;

            settings.Output = (AudioMixerGroup) EditorGUILayout.ObjectField("Output", settings.Output, typeof(AudioMixerGroup));
            settings.BypassEffects = EditorGUILayout.Toggle("Bypass Effects", settings.BypassEffects);
            settings.BypassListenerEffects = EditorGUILayout.Toggle("Bypass Listener Effects", settings.BypassListenerEffects);
            settings.BypassReverbZones = EditorGUILayout.Toggle("Bypass Reverb Zones", settings.BypassReverbZones);
            settings.Loop = EditorGUILayout.Toggle("Loop", settings.Loop);   

            GUILayout.Space(12f);

            RandomSlider("Volume", ref settings.RandomVolume, ref settings.Volume, ref settings.MinVolume, ref settings.MaxVolume, 0f, 1f);
            OverTimeEffect("Volume Over Time", ref settings.VolumeOverLifetime, ref settings.VolumeOverLifetimeType, ref settings.VolumeFadeInPosition, ref settings.VolumeFadeOutPosition, ref settings.VolumeOverLifetimeCurve);
            GUILayout.Space(12f);

            RandomSlider("Pitch", ref settings.RandomPitch, ref settings.Pitch, ref settings.MinPitch, ref settings.MaxPitch, -3f, 3f);
            OverTimeEffect("Pitch Over Time", ref settings.PitchOverLifetime, ref settings.PitchOverLifetimeType, ref settings.PitchFadeInPosition, ref settings.PitchFadeOutPosition, ref settings.PitchOverLifetimeCurve);
            GUILayout.Space(12f);

            RandomSlider("Stereo Pan", ref settings.RandomStereoPan, ref settings.StereoPan, ref settings.MinStereoPan, ref settings.MaxStereoPan, -1f, 1f);
            OverTimeEffect("Stereo Pan Over Time", ref settings.StereoPanOverLifetime, ref settings.stereoPanOverLifetimeType, ref settings.StereoPanFadeInPosition, ref settings.StereoPanFadeOutPosition, ref settings.StereoPanOverLifetimeCurve);
            GUILayout.Space(12f);
            
            settings.Priority = EditorGUILayout.IntSlider("Priority", settings.Priority, 0, 256);
            GUILayout.Space(12f);

            if(settings.RandomDelay)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Delay", GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUI.indentLevel--;
                GUILayout.Label("Min");
                settings.MinDelay = Mathf.Max(0, EditorGUILayout.FloatField(settings.MinDelay));
                GUILayout.Label("Max");
                settings.MaxDelay = Mathf.Max(0, EditorGUILayout.FloatField(settings.MaxDelay));
                EditorGUI.indentLevel++;
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                settings.Delay = EditorGUILayout.FloatField("Delay", settings.Delay);
            }

            settings.RandomDelay = EditorGUILayout.Toggle($"Random Delay", settings.RandomDelay);
            GUILayout.Space(12f);
            
            Draw3DSoundSettings(settings);
            
 
            EditorGUI.indentLevel--;
            GUILayout.Space(12f);
        }

        public static void Reset(AudioSourceSettings settings, bool displayDialog = true)
        {
            if(displayDialog && !EditorUtility.DisplayDialog("Reset?", "Are you sure?", "Reset", "Cancel"))
                return;

            GameObject go = new GameObject();
            go.hideFlags = HideFlags.DontSaveInEditor;
            AudioSource source = go.AddComponent<AudioSource>();

            settings.Output = source.outputAudioMixerGroup;
            settings.BypassEffects = source.bypassEffects;
            settings.BypassListenerEffects = source.bypassListenerEffects;
            settings.BypassReverbZones = source.bypassReverbZones;
            settings.Loop = source.loop;

            settings.Volume = source.volume;
            settings.RandomVolume = false; 
            settings.MinVolume = 0f;
            settings.MaxVolume = 1f;
            settings.VolumeOverLifetime = false;
            settings.VolumeOverLifetimeType = AudioOverLifetimeType.Curve;
            settings.VolumeFadeInPosition = 0f;
            settings.VolumeFadeOutPosition = 0f;
            settings.VolumeOverLifetimeCurve = new AnimationCurve(new Keyframe[]{ new Keyframe(0f, 0f), new Keyframe(0.1f, 1f), new Keyframe(0.9f, 1f), new Keyframe(1f, 0f)});

            settings.Pitch = source.pitch;
            settings.RandomPitch = false;
            settings.MinPitch = -3f;
            settings.MaxPitch = 3f;
            settings.PitchOverLifetime = false;
            settings.PitchOverLifetimeType = AudioOverLifetimeType.Curve;
            settings.PitchFadeInPosition = 0f;
            settings.PitchFadeOutPosition = 0f;
            settings.PitchOverLifetimeCurve = new AnimationCurve(new Keyframe[]{ new Keyframe(0f, 0f), new Keyframe(0.1f, 1f), new Keyframe(0.9f, 1f), new Keyframe(1f, 0f)});

            settings.StereoPan = source.panStereo;
            settings.RandomStereoPan = false;
            settings.MinStereoPan = -1f;
            settings.MaxStereoPan = 1f;
            settings.StereoPanOverLifetime = false;
            settings.stereoPanOverLifetimeType = AudioOverLifetimeType.Curve;
            settings.StereoPanFadeInPosition = 0f;
            settings.StereoPanFadeOutPosition = 0f;
            settings.StereoPanOverLifetimeCurve = new AnimationCurve(new Keyframe[]{ new Keyframe(0f, 0f), new Keyframe(0.1f, 1f), new Keyframe(0.9f, 1f), new Keyframe(1f, 0f)});

            settings.Priority = 128;

            settings.Delay = 0;
            settings.RandomDelay = false; 
            settings.MinDelay = 0f;
            settings.MaxDelay = 1f;

            settings.SpatialBlend = source.spatialBlend;
            settings.DopplerLevel = source.dopplerLevel;
            settings.Spread = source.spread;
            settings.MinDistance = source.minDistance;
            settings.MaxDistance = source.maxDistance;
            settings.AudioRolloffMode = source.rolloffMode;
            settings.CustomRolloff = source.GetCustomCurve(AudioSourceCurveType.CustomRolloff);

            DestroyImmediate(go);
        }

        private static void Draw3DSoundSettings(AudioSourceSettings settings)
        {
            settings.Foldout3DSoundSettings = EditorGUILayout.Foldout(settings.Foldout3DSoundSettings, "3D Sound Settings");
            if(settings.Foldout3DSoundSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUILayoutHelper.DrawLabeledSlider(ref settings.SpatialBlend, new GUIContent("Spatial Blend", "Sets how much this AudioSource is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D."), 0f, 1f, "2D", "3D", 2);
                GUILayout.Space(12f);
                settings.DopplerLevel = EditorGUILayout.Slider(new GUIContent("Doppler Level"), settings.DopplerLevel, 0, 5f);
                settings.Spread = EditorGUILayout.Slider(new GUIContent("Spread"), settings.Spread, 0, 360);
                settings.MinDistance = EditorGUILayout.FloatField(new GUIContent("Min Distance"), settings.MinDistance);
                settings.MaxDistance = EditorGUILayout.FloatField(new GUIContent("Max Distance"), settings.MaxDistance);
                settings.AudioRolloffMode = (AudioRolloffMode) EditorGUILayout.EnumPopup(new GUIContent("Rolloff Mode"), settings.AudioRolloffMode);
                GUI.enabled = settings.AudioRolloffMode == AudioRolloffMode.Custom;
                settings.CustomRolloff = EditorGUILayout.CurveField(new GUIContent("Rolloff Curve"), settings.CustomRolloff);
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
        }

        private static void RandomSlider(string label, ref bool isRandom, ref float constant, ref float minValue, ref float maxValue, float minLimit, float maxLimit)
        {
            if(isRandom)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUI.indentLevel--;
                minValue = Mathf.Clamp(EditorGUILayout.FloatField(minValue, GUILayout.Width(EditorGUIUtility.fieldWidth)), minLimit, maxLimit);
                EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, minLimit, maxLimit);
                maxValue = Mathf.Clamp(EditorGUILayout.FloatField(maxValue, GUILayout.Width(EditorGUIUtility.fieldWidth)), minLimit, maxLimit);

                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel++;
            }
            else
            {
                constant = EditorGUILayout.Slider(label, constant, minLimit, maxLimit);
            }

            isRandom = EditorGUILayout.Toggle($"Random {label}", isRandom);
        }

        private static void OverTimeEffect(string label, ref bool enabled, ref AudioOverLifetimeType type, ref float fadeInTime, ref float fadeOutTime, ref AnimationCurve curve)
        {
            enabled = EditorGUILayout.Toggle(label, enabled);
            if(!enabled)
                return;
                
            type = (AudioOverLifetimeType)EditorGUILayout.EnumPopup("Type", type);

            switch(type)
            {
                case AudioOverLifetimeType.Precise:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Precise",  GUILayout.Width(EditorGUIUtility.labelWidth));
                    EditorGUI.indentLevel--;
                    EditorGUILayout.LabelField("Inn:", GUILayout.Width(30));
                    fadeInTime = EditorGUILayout.FloatField(fadeInTime);
                    EditorGUILayout.LabelField("Out:", GUILayout.Width(30));
                    fadeOutTime = EditorGUILayout.FloatField(fadeOutTime);
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                break;
                case AudioOverLifetimeType.Curve:
                    curve = EditorGUILayout.CurveField("Curve", curve, Color.yellow, new Rect(0, 0, 1, 1));
                break;
            }
        }
    }
}